using UnityEngine;
using UnityEngine.Events;


namespace FFG
{
    public enum TimerMode
    {
        Stopwatch,
        Countdown,
        Repeating,
    }

    public sealed class TimerComponent : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        private bool _shouldStartOnAwake = true;
        [SerializeField]
        private TimerMode _timerMode = TimerMode.Stopwatch;
        [SerializeField, Min(0)]
        private float _duration = 0;
        [SerializeField, Min(0)]
        private int _repeatCount = 0;
        [SerializeField]
        private UnityEvent _onTimerStartCallback = null;
        [SerializeField]
        private UnityEvent _onTimerCompleteCallback = null;

        private bool _isRunnning = false;

        #endregion
        #region Properties

        /// <summary>
        /// Returns the timer handle for this component
        /// </summary>
        public TimerHandle TimerHandle { get; private set; }

        #endregion
        #region Private Methods

        private void createTimer()
        {
            switch (_timerMode)
            {
                case TimerMode.Repeating:
                    TimerHandle = Timer.CreateRepeatingFunctionTimer(onTimerBegin, onTimerComplete, _duration, _repeatCount);
                    break;
                case TimerMode.Stopwatch:
                    TimerHandle = Timer.CreateStopwatch(onTimerBegin);
                    break;
                case TimerMode.Countdown:
                    TimerHandle = Timer.CreateFunctionTimer(onTimerBegin, onTimerComplete, _duration);
                    break;
            }
        }

        private void onTimerBegin()
        {
            _isRunnning = true;
            _onTimerStartCallback?.Invoke();
        }

        private void onTimerComplete()
        {
            _onTimerCompleteCallback?.Invoke();
            _isRunnning = false;
        }

        #endregion
        #region Public methods

        public void StartTimer()
        {
            if (!_isRunnning)
                createTimer();
        }

        #endregion
        #region Lifecycle methods

        private void Awake()
        {
            if (_shouldStartOnAwake)
                createTimer();
        }

        #endregion
    }
}