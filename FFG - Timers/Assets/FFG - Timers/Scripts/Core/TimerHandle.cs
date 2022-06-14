using System;
using UnityEngine;


namespace FFG
{
    /// <summary>
    /// Handle to a timer used to get overall progress, pause, resume, add and deduct time from the timer
    /// </summary>
    public class TimerHandle
    {
        #region Fields

        private Action _onTimerBeginCallback;
        private Action _onTimerCompleteCallback;
        private float _timerDuration;
        private int _repeatCount;

        private float _totalElapsedTime;
        private float _cachedTime;

        private bool _canTick;
        private int _lapCount;
        private bool _isFirstTick;

        #endregion
        #region Constructors

        public TimerHandle(Action onTimerBeginCallback, Action onTimerCompleteCallback, float timerDuration, int repeatCount)
        {
            _onTimerBeginCallback = onTimerBeginCallback;
            _onTimerCompleteCallback = onTimerCompleteCallback;
            _timerDuration = timerDuration != -1 ? Mathf.Abs(timerDuration) : -1;
            _repeatCount = repeatCount;

            // Adding error correction time
            _totalElapsedTime = Time.time == 0 ? -0.02f : 0;
            _cachedTime = Time.time;
            _canTick = true;
            _lapCount = 1;
            _isFirstTick = true;
        }

        #endregion
        #region Properties

        public Action OnTick => onTick;

        #endregion
        #region Private Methods

        private void onTick()
        {
            if (!_canTick)
                return;
            if (_isFirstTick)
            {
                _isFirstTick = false;
                _onTimerBeginCallback?.Invoke();
            }

            _totalElapsedTime += Time.deltaTime;

            if (_timerDuration == -1)
                return;
            else if (_totalElapsedTime >= _timerDuration)
            {
                if (_lapCount < _repeatCount)
                {
                    float err = (Time.time - _cachedTime) - _timerDuration;
                    _totalElapsedTime = err;
                    _cachedTime = Time.time - err;
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

        #endregion
        #region Public Methods

        /// <summary>
        /// Returns the actual elapsed time
        /// </summary>
        public float GetElapsedTime() => _totalElapsedTime;

        /// <summary>
        /// Returns the progress from 0 to 1 where 0 being incomplete and 1 being complete
        /// </summary>
        public float GetProgress() => _timerDuration == -1 ? 0 : Mathf.Clamp01(_totalElapsedTime / _timerDuration);

        /// <summary>
        /// Returns the current lap the timer is running on
        /// </summary>
        public int GetCurrentLap() => _timerDuration == -1 ? 0 : _lapCount;

        /// <summary>
        /// Call this method to pause timer
        /// </summary>
        public void PauseTimer() => _canTick = false;

        /// <summary>
        /// Call this method to resume timer
        /// </summary>
        public void ResumeTimer() => _canTick = true;

        #endregion
    }
}