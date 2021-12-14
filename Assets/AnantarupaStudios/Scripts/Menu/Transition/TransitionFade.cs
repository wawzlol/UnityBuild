using System.Threading.Tasks;
using UnityEngine;

namespace AnantarupaStudios.Menu.Transition
{
	[RequireComponent(typeof(CanvasGroup))]
	public class TransitionFade : MonoBehaviour, ITransition
	{
		private CanvasGroup canvasGroup;

		private bool currentlyIn;
		private float transitionStartTime;
		private bool finishTransition;
		private float transitionStartValue;

		private void Awake()
		{
			canvasGroup = GetComponent<CanvasGroup>();
			canvasGroup.alpha = 0;
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
				canvasGroup.alpha = currentlyIn ? Mathf.Lerp(transitionStartValue, 1, progress == 0 ? 0 : Mathf.Pow(2, 15 * progress - 15)) : Mathf.Lerp(transitionStartValue, 0, progress == 1 ? 1 : 1 - Mathf.Pow(2, -15 * progress));
			}
		}

		async Task ITransition.In()
		{
			currentlyIn = true;
			transitionStartTime = Time.realtimeSinceStartup;
			finishTransition = false;
			transitionStartValue = canvasGroup.alpha;
			await new WaitForSeconds(0.5f);
		}

		async Task ITransition.Out()
		{
			currentlyIn = false;
			transitionStartTime = Time.realtimeSinceStartup;
			finishTransition = false;
			transitionStartValue = canvasGroup.alpha;
			await new WaitForSeconds(0.5f);
		}
	}
}