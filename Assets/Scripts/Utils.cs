using System.Collections.Generic;
using System.Linq;
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

    public struct InhaleExhalePair
    {
        public float InhaleDuration { get; set; }
        public float ExhaleDuration { get; set; }
    }

    public class BreathingParams
    {
        public float breathInTime;
        public float breathOutTime;
        public float breathInHoldTime;
        public float breathOutHoldTime;

        public BreathingParams(IEnumerable<InhaleExhalePair> inhaleExhaleParams)
        {
            // calculates last values of inhaling and exhaling above the average
            float breathInAverage = inhaleExhaleParams.Average(p => p.InhaleDuration),
                breathOutAverage = inhaleExhaleParams.Average(p => p.ExhaleDuration);

            breathInTime = inhaleExhaleParams.Last(p => p.InhaleDuration >= breathInAverage).InhaleDuration;
            breathOutTime = inhaleExhaleParams.Last(p => p.ExhaleDuration >= breathOutAverage).ExhaleDuration;

            breathInHoldTime = breathInTime / 2;
            breathOutHoldTime = breathOutTime / 2;
        }
    }
}