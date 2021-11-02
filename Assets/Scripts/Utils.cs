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
                if (DisplayTimerController.activeTimer == null || DisplayTimerController.activeTimer.isPaused())
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

    public class BreathParams
    {
        public float breathInTime;
        public float breathOutTime;
        public float breathInHoldTime;
        public float breathOutHoldTime;

        public BreathParams()
        {
            breathInTime = 4.0f;
            breathOutTime = 4.0f;
            breathInHoldTime = 4.0f;
            breathOutHoldTime = 4.0f;
        }

        public BreathParams(float inTime, float outTime, float inHoldTime, float outHoldTime)
        {
            breathInTime = inTime;
            breathOutTime = outTime;
            breathInHoldTime = inHoldTime;
            breathOutHoldTime = outHoldTime;
        }
    }
}