using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace AnantarupaStudios.Input
{
	public class TabName : MonoBehaviour
	{
		[Required, SerializeField] private TextMeshProUGUI nameText;

		public void SetName(string name)
		{
			nameText.text = name;
		}
	}
}
