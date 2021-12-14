using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace AnantarupaStudios.Input
{
	[RequireComponent(typeof(Button))]
	public partial class ButtonView : MonoBehaviour, IButtonView
	{
		[Required, SerializeField, SceneObjectsOnly] private Image image;
		[Required, SerializeField, AssetsOnly] private Sprite normalSprite;
		[Required, SerializeField, AssetsOnly] private Sprite pressedSprite;
		[SerializeField, AssetsOnly] private Sprite disabledSprite;

		public void SetView(bool active, bool pressed)
		{
			image.sprite = active ? pressed ? pressedSprite : normalSprite : disabledSprite == null ? normalSprite : disabledSprite;
		}
	}
}