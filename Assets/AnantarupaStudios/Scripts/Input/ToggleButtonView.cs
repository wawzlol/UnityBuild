using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace AnantarupaStudios.Input
{
	[RequireComponent(typeof(ToggleButton))]
    public class ToggleButtonView : MonoBehaviour, IToggleButtonView
	{
        [Required, SerializeField, SceneObjectsOnly] private Image image;
		[Required, SerializeField, AssetsOnly] private Sprite trueSprite;
		[Required, SerializeField, AssetsOnly] private Sprite falseSprite;

		public void SetView(bool value)
		{
			image.sprite = value ? trueSprite : falseSprite;
		}

#if UNITY_EDITOR
		private void OnValidate()
		{
			image.sprite = falseSprite;
		}
#endif
	}
}