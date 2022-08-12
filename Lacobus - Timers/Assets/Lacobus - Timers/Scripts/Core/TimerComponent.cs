using UnityEngine;
using UnityEngine.Events;


namespace Lacobus.Timers
{
    public sealed class TimerComponent : MonoBehaviour
    {
        // Fields

        [SerializeField] private bool _shouldStartOnAwake = true;
        [SerializeField] private TimerFactory.TimerType _timerType = TimerFactory.TimerType.DefaultTimer;

        [SerializeField, Min(0)] private float _duration = 0;
        [SerializeField, Min(0)] private int _repeatCount = 0;

        [SerializeField] private UnityEvent _onTimerStartCallback = null;
        [SerializeField] private UnityEvent _onTimerCompleteCallback = null;

        [SerializeField] private bool _shouldUseUnscaledTime = true;


        // Properties

        private TimerHandle _timerHandle { get; set; }


        // Public methods

        /// <summary>
        /// Call this method to start timer
        /// </summary>
        public void StartTimer()
        {
            if (_timerHandle == null)
                createTimer();
        }

        /// <summary>
        /// Call this method to pause timer
        /// </summary>
        public void PauseTimer()
        {
            if (_timerHandle == null)
            {
                Debug.LogWarning("Timer has been stopped, use StartTime method");
                return;
            }
            _timerHandle.PauseTimer();
        }

        /// <summary>
        /// Call this method to resume timer
        /// </summary>
        public void ResumeTimer()
        {
            if (_timerHandle == null)
            {
                Debug.LogWarning("Timer has been stopped, use StartTime method");
                return;
            }
            _timerHandle.ResumeTimer();
        }

        /// <summary>
        /// Call this method to abort current timer
        /// </summary>
        public void AbortTimer()
        {
            _timerHandle = null;
        }

        /// <summary>
        /// Returns the actual elapsed time
        /// </summary>
        public float GetElapsedTime()
        {
            if (_timerHandle == null)
            {
                Debug.LogWarning("Timer has been stopped, use StartTime method");
                return 0;
            }
            return _timerHandle.GetElapsedTime();
        }

        /// <summary>
        /// Returns the time remaining until timer completes
        /// </summary>
        public float GetRemainingTime()
        {
            if (_timerHandle == null)
            {
                Debug.LogWarning("Timer has been stopped, use StartTime method");
                return 0;
            }
            return _timerHandle.GetRemainingTime();
        }

        /// <summary>
        /// Returns the progress from 0 to 1 where 0 being incomplete and 1 being complete
        /// </summary>
        public float GetFraction()
        {
            if (_timerHandle == null)
            {
                Debug.LogWarning("Timer has been stopped, use StartTime method");
                return 0;
            }
            return _timerHandle.GetFraction();
        }

        /// <summary>
        /// Returns the current lap the timer is running on
        /// </summary>
        public int GetCurrentLap()
        {
            if (_timerHandle == null)
            {
                Debug.LogWarning("Timer has been stopped, use StartTime method");
                return 0;
            }
            return _timerHandle.GetCurrentLap();
        }


        // Private methods

        private void createTimer()
        {
            _timerHandle = TimerFactory.CreateTimer(_timerType, onTimerBegin, onTimerComplete, _duration, _repeatCount);
        }

        private void onTimerBegin()
        {
            _onTimerStartCallback?.Invoke();
        }

        private void onTimerComplete()
        {
            _onTimerCompleteCallback?.Invoke();
        }


        // Lifecycle methods

        private void Awake()
        {
            if (_shouldStartOnAwake)
                createTimer();
        }
    }
}