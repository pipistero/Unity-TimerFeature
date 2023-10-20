using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace PS.TimerFeature.TimeInvoker
{
    public class TimeInvoker : MonoBehaviour
    {
        public event Action<float> UpdateTicked;
        public event Action<float> UpdateTickedUnscaled;
        public event Action SecondTicked;
        public event Action SecondTickedUnscaled;

        private static TimeInvoker _instance;
        public static TimeInvoker Instance
        {
            get
            {
                if (_instance == null)
                {
                    var timeInvoker = new GameObject("TimeInvoker");
                    _instance = timeInvoker.AddComponent<TimeInvoker>();
                    DontDestroyOnLoad(timeInvoker);
                }

                return _instance;
            }
        }

        private float _secondTime;
        private float _secondTimeUnscaled;
        
        private void Update()
        {
            #region UpdateTick

            float deltaTime = Time.deltaTime;
            UpdateTicked?.Invoke(deltaTime);

            #endregion

            #region SecondTick

            _secondTime += deltaTime;
            if (_secondTime >= 1f)
            {
                _secondTime -= 1f;
                SecondTicked?.Invoke();
            }

            #endregion

            #region UpdateTickUnscaled

            float deltaTimeUnscaled = Time.unscaledDeltaTime;
            UpdateTickedUnscaled?.Invoke(deltaTimeUnscaled);

            #endregion

            #region SecondTickUnscaled

            _secondTimeUnscaled += deltaTimeUnscaled;
            if (_secondTimeUnscaled >= 1f)
            {
                _secondTimeUnscaled -= 1f;
                SecondTickedUnscaled?.Invoke();
            }

            #endregion

        }
    }
}