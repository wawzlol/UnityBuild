using System;
using UnityEngine;

namespace AnantarupaStudios.Input
{
	public class RepeaterButton : Button
	{
		public event Action ButtonHeld;

		private const float Interval = 0.5f;
		private const float MaxHold = 0.5f; // max hold time = maxhold + interval

		private float holdTimer;
		private float intervalTimer;

		private void Update()
		{
			if (Pressed)
			{
				if (MaxHold > holdTimer)
				{
					holdTimer += Time.deltaTime;
					return;
				}

				if (Interval > intervalTimer)
				{
					intervalTimer += Time.deltaTime;
				}
				else
				{
					ButtonHeld?.Invoke();
					intervalTimer = 0;
				}
			}
			else if (!Pressed && holdTimer > 0)
			{
				holdTimer = 0;
				intervalTimer = 0;
			}
		}
	}
}