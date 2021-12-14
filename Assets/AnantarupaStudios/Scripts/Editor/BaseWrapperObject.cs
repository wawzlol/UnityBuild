using Sirenix.OdinInspector;

namespace AnantarupStudios.Editor
{
	public abstract class BaseWrapperObject
	{
		protected bool IsValueChanged = false;

		public virtual void ValueChanged()
		{
			IsValueChanged = true;
		}

		public virtual void ValueSet()
		{
			IsValueChanged = false;
		}

		[HorizontalGroup("General/Split", 0.3f, MaxWidth = 180)]
		[HorizontalGroup("General/Split/Right")]
		[Button(ButtonSizes.Large), GUIColor(1, 0.2f, 1)]
		[LabelText("Revert")]
		[EnableIf("IsValueChanged", true)]
		public abstract void Load();

		[HorizontalGroup("General/Split/Right")]
		[Button(ButtonSizes.Large), GUIColor(0, 1f, 0)]
		[EnableIf("IsValueChanged", true)]
		public abstract void Save();

		protected abstract void Reload();
	}
}