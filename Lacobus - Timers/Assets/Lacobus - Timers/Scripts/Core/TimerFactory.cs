using System;
using UnityEngine;


namespace Lacobus.Timers
{
    /// <summary>
    /// Use this class to create function timers
    /// </summary>
    public static class TimerFactory
    {
        // Fields

        private static TimerMonobehaviourHook s_hook = null;


        // Public methods

        /// <summary>
        /// Method to create a timer which invokes callbacks
        /// </summary>
        /// <param name="timerType">
        /// Type of the timer, 
        /// [RepeatingTimer : will invoke onTimerEnd event a set number of times in the set intervals]  \\n
        /// [DefaultTimer : will invoke onTimerEnd event in a set duration and the repeat count is irrelevant] \\n
        /// [EndlessTimer : will invoke onTimerBegin event in the start and will run continously unless stopped] \n
        /// </param>
        /// <param name="onTimerBeginCallback">Event will be invoked at the beginning of timer</param>
        /// <param name="onTimerEndCallback">Event will be invoked when the timer completes its cycle</param>
        /// <param name="duration">Duration of a single timer cycle</param>
        /// <param name="repeatCount">Total number of cycles the timer should perform</param>
        /// <param name="shouldUseUnscaledTime">Set this as true if you want to use unscaled time</param>
        /// <returns>Returns a timer handle that can be used to pause, resume, stop and retrieve other informations</returns>
        public static TimerHandle CreateTimer
            (
                TimerType timerType,
                Action onTimerBeginCallback,
                Action onTimerEndCallback,
                float duration,
                int repeatCount = 1,
                bool shouldUseUnscaledTime = false
            )
        {
            if (s_hook == null)
                initHook();

            switch (timerType)
            {
                case TimerType.RepeatingTimer:
                    {
                        var handle = new TimerHandle(onTimerBeginCallback, onTimerEndCallback, duration, repeatCount, shouldUseUnscaledTime);
                        s_hook.OnUpdate += handle.OnTick;
                        return handle;
                    }
                case TimerType.DefaultTimer:
                    {
                        var handle = new TimerHandle(onTimerBeginCallback, onTimerEndCallback, duration, 0, shouldUseUnscaledTime);
                        s_hook.OnUpdate += handle.OnTick;
                        return handle;
                    }
                case TimerType.EndlessTimer:
                    {
                        var handle = new TimerHandle(onTimerBeginCallback, null, -1, 0, shouldUseUnscaledTime);
                        s_hook.OnUpdate += handle.OnTick;
                        return handle;
                    }
                default:
                    return null;
            }
        }


        // Private methods

        private static void initHook()
        {
            var gameobject = new GameObject("-Timer-MonobehaviourHook-", typeof(TimerMonobehaviourHook));
            s_hook = gameobject.GetComponent<TimerMonobehaviourHook>();
            s_hook.MakePersistent();
        }


        // Nested types

        private class TimerMonobehaviourHook : MonoBehaviour
        {
            public Action OnUpdate = delegate { };

            private void Update() => OnUpdate();

            public void MakePersistent() => DontDestroyOnLoad(this);
        }

        public enum TimerType
        {
            RepeatingTimer,
            DefaultTimer,
            EndlessTimer
        }
    }
}