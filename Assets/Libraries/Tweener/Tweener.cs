using UnityEngine;
using System.Collections;
using JCSUnity;


namespace PeterVuorela.Tweener
{

    /// <summary>
    /// Vector 3 Tweening, handle progression.
    /// </summary>
    public class Vector3Tweener
    {
        private CallBackDelegate _Callback = null;

        private TweenDelegate _EasingX = null;
        private TweenDelegate _EasingY = null;
        private TweenDelegate _EasingZ = null;

        private Vector3 _From = Vector3.zero;
        private Vector3 _To = Vector3.zero;


        private Vector3 _ProgressPct = Vector3.zero;

        public Vector3 progressPct { get { return _ProgressPct; } }

        private bool _AnimatingX = false;
        private bool _AnimatingY = false;
        private bool _AnimatingZ = false;

        private Vector3 _TimeElapsed = Vector3.zero;

        private Vector3 _Duration = Vector3.one;

        private Vector3 _Progression = Vector3.zero;

        /// <summary>
        /// Callback when reach destination.
        /// </summary>
        /// <param name="func"> function pointer </param>
        public void SetCallback(CallBackDelegate func)
        {
            if (func == null)
                return;

            this._Callback += func;
        }

        public Vector3Tweener() { }

        /// <summary>
        /// Eases from value to value.
        /// </summary>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        /// <param name="duration">Duration.</param>
        /// <param name="easing">Easing.</param>
        public void easeFromTo(
            Vector3 from, Vector3 to, 
            bool resetElapsedTime = true,
            float durationX = 1f,
            float durationY = 1f,
            float durationZ = 1f,
            TweenDelegate easingX = null,
            TweenDelegate easingY = null,
            TweenDelegate easingZ = null,
            CallBackDelegate callback = null)
        {
            if (easingX == null)
            {
                // give the default value
                easingX = Easing.Linear;
            }

            if (easingY == null)
            {
                // give the default value
                easingY = Easing.Linear;
            }

            if (easingZ == null)
            {
                // give the default value
                easingZ = Easing.Linear;
            }

            _EasingX = easingX;
            _EasingY = easingY;
            _EasingZ = easingZ;
            SetCallback(callback);

            _From = from;
            _To = to;

            _Duration.x = durationX;
            _ProgressPct.x = 0f;

            _Duration.y = durationY;
            _ProgressPct.y = 0f;

            _Duration.z = durationZ;
            _ProgressPct.z = 0f;

            _AnimatingX = true;
            _AnimatingY = true;
            _AnimatingZ = true;

            if (resetElapsedTime)
            {
                _TimeElapsed.x = 0f;
                _TimeElapsed.y = 0f;
                _TimeElapsed.z = 0f;
            }
        }

        public void updateX(bool callCallBack = true)
        {
            if (!_AnimatingX)
                return;

            if (_TimeElapsed.x < _Duration.x)
            {
                if (_EasingX != null)
                {
                    _Progression.x = _EasingX.Invoke(_TimeElapsed.x, _From.x, (_To.x - _From.x), _Duration.x);

                    _ProgressPct.x = _TimeElapsed.x / _Duration.x;

                    _TimeElapsed.x += Time.deltaTime;
                }
            }
            else
            {
                _Progression.x = _To.x;

                _AnimatingX = false;
                _TimeElapsed.x = 0f;
                _ProgressPct.x = 1f;

                CheckUpdate();
            }
        }
        public void updateY(bool callCallBack = true)
        {
            if (!_AnimatingY)
                return;

            if (_TimeElapsed.y < _Duration.y)
            {
                if (_EasingY != null)
                {
                    _Progression.y = _EasingY.Invoke(_TimeElapsed.y, _From.y, (_To.y - _From.y), _Duration.y);

                    _ProgressPct.y = _TimeElapsed.y / _Duration.y;

                    _TimeElapsed.y += Time.deltaTime;
                }
            }
            else
            {
                _Progression.y = _To.y;

                _AnimatingY = false;
                _TimeElapsed.y = 0f;
                _ProgressPct.y = 1f;

                CheckUpdate();
            }
        }
        public void updateZ(bool callCallBack = true)
        {
            if (!_AnimatingZ)
                return;

            if (_TimeElapsed.z < _Duration.z)
            {
                if (_EasingZ != null)
                {
                    _Progression.z = _EasingZ.Invoke(_TimeElapsed.z, _From.z, (_To.z - _From.z), _Duration.z);

                    _ProgressPct.z = _TimeElapsed.z / _Duration.z;

                    _TimeElapsed.z += Time.deltaTime;
                }
            }
            else
            {
                _Progression.z = _To.z;

                _AnimatingZ = false;
                _TimeElapsed.z = 0f;
                _ProgressPct.z = 1f;

                CheckUpdate();
            }
        }

        public void CheckUpdate(bool callCallBack = true)
        {
            if (_AnimatingX && _AnimatingY && _AnimatingZ)
                return;

            if (callCallBack && _Callback != null)
            {
                _Callback.Invoke();
            }
        }

        /// <summary>
        /// Reset tweener effect setting.
        /// </summary>
        public void ResetTweener()
        {
            _Progression.x = _To.x;
            _AnimatingX = false;
            _TimeElapsed.x = 0f;
            _ProgressPct.x = 1f;

            _Progression.y = _To.y;
            _AnimatingY = false;
            _TimeElapsed.y = 0f;
            _ProgressPct.y = 1f;

            _Progression.z = _To.z;
            _AnimatingZ = false;
            _TimeElapsed.z = 0f;
            _ProgressPct.z = 1f;
        }

        public void updateX(ref Vector2 whatToTween)
        {
            bool wasAnimating = _AnimatingX;
            updateX(false);
            whatToTween = _Progression;

            if (wasAnimating && !_AnimatingX && _Callback != null)
            {
                _Callback.Invoke();
            }

        }

        public void updateY(ref Vector2 whatToTween)
        {
            bool wasAnimating = _AnimatingY;
            updateY(false);
            whatToTween = _Progression;

            if (wasAnimating && !_AnimatingY && _Callback != null)
            {
                _Callback.Invoke();
            }

        }

        public void updateZ(ref Vector2 whatToTween)
        {
            bool wasAnimating = _AnimatingZ;
            updateZ(false);
            whatToTween = _Progression;

            if (wasAnimating && !_AnimatingZ && _Callback != null)
            {
                _Callback.Invoke();
            }
        }

        public void DoCallBack()
        {
            if (_Callback != null)
                _Callback.Invoke();
        }

        public bool animating { get { return (_AnimatingX || _AnimatingY || _AnimatingZ); } }
        public Vector3 progression { get { return _Progression; } }
        public Vector3 from { get { return _From; } set { _From = value; } }
        public Vector3 to { get { return _To; } set { _To = value; } }
    }
}
