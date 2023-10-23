using System;
using PS.TimerFeature.Types;
using UnityEngine;

namespace PS.TimerFeature.Timer
{
    public class Timer
    {
        public event Action<float> TimerValueChanged;
        public event Action TimerFinished;
        public event Action TimerPaused;
        public event Action TimerUnpaused;
        
        public TimerType Type { get; private set; }
        public float RemainingTime { get; private set; }
        public bool IsPaused { get; private set; }
        
        public Timer(TimerType type, float duration)
        {
            Type = type;
            SetRemainingTime(duration);
        }
        
        public void SetRemainingTime(float remainingTime)
        {
            RemainingTime = remainingTime;
            TimerValueChanged?.Invoke(RemainingTime);
        }

        public void Start()
        {
            if (RemainingTime <= 0f)
            {
                Debug.Log($"Timer ({Type}) already finished");
                return;
            }
            
            IsPaused = false;
            
            SubscribeEvents();
            
            TimerValueChanged?.Invoke(RemainingTime);
        }

        public void Start(float duration)
        {
            SetRemainingTime(duration);
            Start();
        }

        public void Pause()
        {
            IsPaused = true;
            
            UnsubscribeEvents();
            
            TimerPaused?.Invoke();
        }

        public void Unpause()
        {
            IsPaused = false;
            
            Start();
            
            TimerUnpaused?.Invoke();
        }

        public void Stop()
        {
            IsPaused = false;
            
            SetRemainingTime(0f);
            
            TimerFinished?.Invoke();
        }

        #region Internal events work

        private void SubscribeEvents()
        {
            switch (Type)
            {
                case TimerType.SecondTick:
                    TimeInvoker.TimeInvoker.Instance.SecondTicked += OnSecondTimerTicked;
                    break;
                
                case TimerType.UpdateTick:
                    TimeInvoker.TimeInvoker.Instance.UpdateTicked += OnUpdateTimerTicked;
                    break;
                
                case TimerType.SecondTickUnscaled:
                    TimeInvoker.TimeInvoker.Instance.SecondTickedUnscaled += OnSecondTimerTicked;
                    break;
                
                case TimerType.UpdateTickUnscaled:
                    TimeInvoker.TimeInvoker.Instance.UpdateTickedUnscaled += OnUpdateTimerTicked;
                    break;
            }
        }

        private void UnsubscribeEvents()
        {
            switch (Type)
            {
                case TimerType.SecondTick:
                    TimeInvoker.TimeInvoker.Instance.SecondTicked -= OnSecondTimerTicked;
                    break;
                
                case TimerType.UpdateTick:
                    TimeInvoker.TimeInvoker.Instance.UpdateTicked -= OnUpdateTimerTicked;
                    break;
                
                case TimerType.SecondTickUnscaled:
                    TimeInvoker.TimeInvoker.Instance.SecondTickedUnscaled -= OnSecondTimerTicked;
                    break;
                
                case TimerType.UpdateTickUnscaled:
                    TimeInvoker.TimeInvoker.Instance.UpdateTickedUnscaled -= OnUpdateTimerTicked;
                    break;
            }
        }

        #endregion

        #region Internal timer work

        private void CheckFinish()
        {
            if (RemainingTime <= 0f)
                Stop();
        }
        
        private void OnUpdateTimerTicked(float deltaTime)
        {
            if (IsPaused)
                return;

            SetRemainingTime(RemainingTime -= deltaTime);
            CheckFinish();
        }

        private void OnSecondTimerTicked()
        {
            OnUpdateTimerTicked(1f);
        }

        #endregion
    }
}