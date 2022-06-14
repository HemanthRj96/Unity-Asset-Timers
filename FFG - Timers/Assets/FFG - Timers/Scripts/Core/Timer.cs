using System;
using UnityEngine;


namespace FFG
{
    /// <summary>
    /// Use this class to create function timers
    /// </summary>
    public static class Timer
    {
        #region Fields

        private static TimerMonobehaviourHook s_hook = null;

        #endregion
        #region Private Methods

        private static void initHook()
        {
            var gameobject = new GameObject("-Timer-MonobehaviourHook-", typeof(TimerMonobehaviourHook));
            s_hook = gameobject.GetComponent<TimerMonobehaviourHook>();
            s_hook.MakePersistent();
        }

        #endregion
        #region Public Methods

        /// <summary>
        /// Creates a countdown timer which invoked the callback after duration (seconds)
        /// </summary>
        /// <param name="onTimerCompleteCallback">Method to be called after timer completes</param>
        /// <param name="duration">Total duration of timer</param>
        /// <returns>Returns the timer handle which can be used to pause, resume, and know the elapsed time</returns>
        public static TimerHandle CreateFunctionTimer(Action onTimerBeginCallback, Action onTimerCompleteCallback, float duration)
        {
            if (s_hook == null)
                initHook();

            var handle = new TimerHandle(onTimerBeginCallback, onTimerCompleteCallback, duration, 0);
            s_hook.OnUpdate += handle.OnTick;
            return handle;
        }

        /// <summary>
        /// Creates a timer that'll run forever. Used generally to run a background timer without any duration or callbacks
        /// </summary>
        /// <returns>Returns the timer handle which can be used to pause, resume, and know the elapsed time</returns>
        public static TimerHandle CreateStopwatch(Action onTimerBeginCallback)
        {
            if (s_hook == null)
                initHook();

            var handle = new TimerHandle(onTimerBeginCallback, null, -1, 0);
            s_hook.OnUpdate += handle.OnTick;
            return handle;
        }

        /// <summary>
        /// Creates a repeating timer which invokes callback every duration (seconds)
        /// </summary>
        /// <param name="onTimeCompleteCallback">Method to be called after timer completes</param>
        /// <param name="singleLapDuration">Duration between laps</param>
        /// <returns>Returns the timer handle which can be used to pause, resume, and know the elapsed time</returns>
        public static TimerHandle CreateRepeatingFunctionTimer(Action onTimerBeginCallback, Action onTimeCompleteCallback, float singleLapDuration, int totalLaps)
        {
            if (s_hook == null)
                initHook();

            var handle = new TimerHandle(onTimerBeginCallback, onTimeCompleteCallback, singleLapDuration, Mathf.Abs(totalLaps));
            s_hook.OnUpdate += handle.OnTick;
            return handle;
        }

        #endregion
        #region Nested type

        private class TimerMonobehaviourHook : MonoBehaviour
        {
            public Action OnUpdate = delegate { };

            private void Update() => OnUpdate();

            public void MakePersistent() => DontDestroyOnLoad(this);
        }

        #endregion
    }
}