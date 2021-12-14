using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace AnantarupaStudios.Input
{
	[RequireComponent(typeof(Button))]
	public partial class ButtonAnimatedView : MonoBehaviour, IButtonView
	{
		[Required, SerializeField, SceneObjectsOnly] private Animator animator;
		
		public void SetView(bool active, bool pressed)
		{
			if (active && pressed)
			{
				animator.SetBool("Input", true);
			}
		}
	}
}