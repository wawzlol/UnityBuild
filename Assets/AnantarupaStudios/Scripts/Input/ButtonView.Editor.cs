using AnantarupaStudios.Utility;
using UnityEngine;
using UnityEngine.UI;

namespace AnantarupaStudios.Input
{
	[RequireComponent(typeof(Button))]
	public partial class ButtonView : MonoBehaviour, IButtonView
	{
#if UNITY_EDITOR
		internal enum ButtonType
		{
			green,
			passive,
			purple,
			yellow
		}

		[ContextMenu("Setup Button Green")]
		public void SetupButtonGreen()
		{
			SetupButton(ButtonType.green);
		}

		[ContextMenu("Setup Button Passive")]
		public void SetupButtonPassive()
		{
			SetupButton(ButtonType.passive);
		}

		[ContextMenu("Setup Button Purple")]
		public void SetupButtonPurple()
		{
			SetupButton(ButtonType.purple);
		}

		[ContextMenu("Setup Button Yellow")]
		public void SetupButtonYellow()
		{
			SetupButton(ButtonType.yellow);
		}

		private void SetupButton(ButtonType buttonType)
		{
			if (image == null)
				image = GetComponent<Image>();

			TMPro.TextMeshProUGUI label = GetComponentInChildren<TMPro.TextMeshProUGUI>();
			if (label == null)
			{
				label = Instantiate(new GameObject(), transform).AddComponent<TMPro.TextMeshProUGUI>();
			}

			image.sprite = ResourceManager.LoadSprite("asset/ui/common", "common." + buttonType.ToString() + "_button");
			image.type = Image.Type.Simple;
			image.SetNativeSize();
			label.alignment = TMPro.TextAlignmentOptions.Center;
			label.name = "Label";
			label.fontSize = 30f;
			label.richText = false;
			label.raycastTarget = false;
			label.parseCtrlCharacters = false;
			label.rectTransform.sizeDelta = image.rectTransform.sizeDelta;
			label.rectTransform.anchorMax = label.rectTransform.anchorMin = label.rectTransform.pivot = new Vector2(0.5f, 0.5f);
			label.rectTransform.anchoredPosition = new Vector2(-2f, 2f);

			disabledSprite = ResourceManager.LoadSprite("asset/ui/common", "common." + ButtonType.passive.ToString() + "_button");
			normalSprite = ResourceManager.LoadSprite("asset/ui/common", "common." + buttonType.ToString() + "_button");
			pressedSprite = ResourceManager.LoadSprite("asset/ui/common", "common." + buttonType.ToString() + "_button");
			image.sprite = normalSprite;
		}

#endif
	}

}