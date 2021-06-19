using UnityEngine;

namespace Utils
{
    public class WaitForSecondsPaused : CustomYieldInstruction
	{
		private float seconds;
		public override bool keepWaiting
		{
			get
			{
                if (DisplayTimerController.activeTimer.isPaused())
                {
                    return true;
                }
                else
                {
                    seconds -= Time.deltaTime;
                    return (seconds > 0.0f);
                }
			}
		}

		public WaitForSecondsPaused(float seconds)
		{
			this.seconds = seconds;
		}
	}
}