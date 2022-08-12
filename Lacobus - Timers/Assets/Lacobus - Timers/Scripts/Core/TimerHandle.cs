using System;
using UnityEngine;


namespace Lacobus.Timers
{
    /// <summary>
    /// Handle to a timer used to get overall progress, pause, resume, add and deduct time from the timer
    /// </summary>
    public class TimerHandle
    {
        // Fields

        private Action _onTimerBeginCallback;
        private Action _onTimerCompleteCallback;
        private float _timerDuration;
        private int _repeatCount;

        private float _totalElapsedTime;
        private float _lastCycleTime;

        private int _lapCount;
        private bool _canTick;
        private bool _isFirstTick;
        private bool _shouldUseUnscaledTime = true;


        // Constructors

        public TimerHandle(Action onTimerBeginCallback, Action onTimerCompleteCallback, float timerDuration, int repeatCount, bool shouldUseUnscaledTime)
        {
            _onTimerBeginCallback = onTimerBeginCallback;
            _onTimerCompleteCallback = onTimerCompleteCallback;
            _timerDuration = timerDuration != -1 ? Mathf.Abs(timerDuration) : -1;
            _repeatCount = Mathf.Min(0, repeatCount);
            _shouldUseUnscaledTime = shouldUseUnscaledTime;

            // Adding error correction time
            _totalElapsedTime = getCurrentTime() == 0 ? -0.02f : 0;
            _lastCycleTime = getCurrentTime();

            _canTick = true;
            _lapCount = 1;
            _isFirstTick = true;
        }


        // Properties

        public Action OnTick => onTick;


        // Public methods

        /// <summary>
        /// Returns the actual elapsed time
        /// </summary>
        public float GetElapsedTime() => _totalElapsedTime == -1 ? _timerDuration : _totalElapsedTime;

        /// <summary>
        /// Returns the time remaining until timer completes
        /// </summary>
        public float GetRemainingTime() => _totalElapsedTime - _timerDuration;

        /// <summary>
        /// Returns the progress from 0 to 1 where 0 being incomplete and 1 being complete
        /// </summary>
        public float GetFraction() => _timerDuration == -1 ? 0 : Mathf.Clamp01(_totalElapsedTime / _timerDuration);

        /// <summary>
        /// Returns the current lap the timer is running on
        /// </summary>
        public int GetCurrentLap() => _timerDuration == -1 ? _repeatCount : _lapCount;

        /// <summary>
        /// Call this method to pause timer
        /// </summary>
        public void PauseTimer() => _canTick = false;

        /// <summary>
        /// Call this method to resume timer
        /// </summary>
        public void ResumeTimer() => _canTick = true;


        // Private methods

        private void onTick()
        {
            if (!_canTick)
                return;

            if (_isFirstTick)
            {
                _isFirstTick = false;
                _onTimerBeginCallback?.Invoke();
            }

            _totalElapsedTime += getCurrentTime();

            if (_timerDuration == -1)
                return;

            else if (_totalElapsedTime >= _timerDuration)
            {
                if (_lapCount < _repeatCount)
                {
                    float cycleError = (getCurrentTime() - _lastCycleTime) - _timerDuration;
                    _totalElapsedTime = cycleError;
                    _lastCycleTime = getCurrentTime() - cycleError;
                    ++_lapCount;
                    _onTimerCompleteCallback?.Invoke();
                }
                else
                {
                    _canTick = false;
                    _onTimerCompleteCallback?.Invoke();
                }
            }
        }

        private float getCurrentTime() => _shouldUseUnscaledTime ? Time.unscaledTime : Time.time;
    }
}