using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace AnantarupaStudios.Menu.Transition
{
	public class TransitionState : MonoBehaviour
	{
		public bool ShouldActive { get; private set; }
		private bool currentActive = false;

		public async Task<bool> In()
		{
			if (ShouldActive) return false;
			ShouldActive = true;
			await new WaitForEndOfFrame();
			if (!currentActive && ShouldActive)
			{
				List<Task> transitions = new List<Task>();
				foreach (ITransition transition in GetComponentsInChildren<ITransition>(true))
				{
					transitions.Add(transition.In());
				}
				await Task.WhenAll(transitions);
				currentActive = true;
				if (!ShouldActive)
				{
					ShouldActive = true;
					await Out();
					return false;
				}
				return true;
			}
			return false;
		}

		public async Task<bool> Out()
		{
			if (!ShouldActive) return false;
			ShouldActive = false;
			await new WaitForEndOfFrame();
			if (currentActive && !ShouldActive)
			{
				List<Task> transitions = new List<Task>();
				foreach (ITransition transition in GetComponentsInChildren<ITransition>(true))
				{
					transitions.Add(transition.Out());
				}
				await Task.WhenAll(transitions);
				currentActive = false;
				if (ShouldActive)
				{
					ShouldActive = false;
					await In();
					return false;
				}
				return true;
			}
			return false;
		}
	}
}
