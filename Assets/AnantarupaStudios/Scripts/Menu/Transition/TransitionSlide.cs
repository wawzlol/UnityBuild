using System.Threading.Tasks;
using UnityEngine;

namespace AnantarupaStudios.Menu.Transition
{
	public class TransitionSlide : MonoBehaviour, ITransition
	{
		[SerializeField] private Vector3 startOffset;

		private Vector3 defaultPosition;

		private bool currentlyIn;
		private float transitionStartTime;
		private bool finishTransition;
		private Vector3 transitionStartValue;

		private void Awake()
		{
			if (transform is RectTransform rect)
			{
				defaultPosition = rect.anchoredPosition3D;
				rect.anchoredPosition3D = defaultPosition - startOffset;
			}
			else
			{
				defaultPosition = transform.localPosition;
				transform.localPosition = defaultPosition - startOffset;
			}
			currentlyIn = false;
			finishTransition = true;
		}

		private void Update()
		{
			if (!finishTransition)
			{
				float progress = (Time.realtimeSinceStartup - transitionStartTime) / 0.5f;
				if (progress >= 1)
				{
					progress = 1;
					finishTransition = true;
				}
				Vector3 newPosition = currentlyIn ? Vector3.Lerp(transitionStartValue, defaultPosition, Mathf.Sin(progress * Mathf.PI / 2)) : Vector3.Lerp(transitionStartValue, defaultPosition - startOffset, 1 - Mathf.Cos(progress * Mathf.PI / 2));
				if (transform is RectTransform rect)
				{
					rect.anchoredPosition3D = newPosition;
				}
				else
				{
					transform.localPosition = newPosition;
				}
			}
		}

		async Task ITransition.In()
		{
			currentlyIn = true;
			transitionStartTime = Time.realtimeSinceStartup;
			finishTransition = false;
			if (transform is RectTransform rect)
			{
				transitionStartValue = rect.anchoredPosition3D;
			}
			else
			{
				transitionStartValue = transform.localPosition;
			}
			await new WaitForSeconds(0.5f);
		}

		async Task ITransition.Out()
		{
			currentlyIn = false;
			transitionStartTime = Time.realtimeSinceStartup;
			finishTransition = false;
			if (transform is RectTransform rect)
			{
				transitionStartValue = rect.anchoredPosition3D;
			}
			else
			{
				transitionStartValue = transform.localPosition;
			}
			await new WaitForSeconds(0.5f);
		}
	}
}