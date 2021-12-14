using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace AnantarupaStudios.Input
{
	[RequireComponent(typeof(TabButton))]
    public class TabButtonView : MonoBehaviour, ITabButtonView
	{
        [Required, SerializeField, SceneObjectsOnly] private Image image;
		[Required, SerializeField, AssetsOnly] private Sprite normalSprite;
		[Required, SerializeField, AssetsOnly] private Sprite pressedSprite;
		[Required, SerializeField, AssetsOnly] private Sprite selectedSprite;
		[SerializeField, AssetsOnly] private Sprite disabledSprite;

		//[Required, SerializeField] private bool downFeedback = false;
		//private Vector2 originPosition;

		//public void Awake()
		//{
		//	originPosition = image.rectTransform.anchoredPosition;
		//}

		public void SetView(bool selected, bool active, bool pressed)
		{
			image.sprite = selected ? selectedSprite : active ? pressed ? pressedSprite : normalSprite : disabledSprite;

			//if (active && downFeedback)
			//{
			//	if (pressed)
			//	{
			//		image.rectTransform.anchoredPosition = originPosition + new Vector2(0, -5f);
			//	}
			//	else
			//	{
			//		image.rectTransform.anchoredPosition = originPosition;
			//	}
			//}
		}

#if UNITY_EDITOR
		private void OnValidate()
		{
			if (normalSprite != null)
			{
				image.sprite = normalSprite;
			}
		}
#endif

		public void SetSprite(Sprite normal, Sprite pressed)
		{
			normalSprite = normal;
			pressedSprite = selectedSprite = pressed;
			image.sprite = normalSprite;
		}
	}
}