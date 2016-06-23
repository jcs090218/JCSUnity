/**
 * $File: JCS_Tweener.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information $
 *		                Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;
using PeterVuorela.Tweener;


namespace JCSUnity
{

    /// <summary>
    /// Tweener Effect
    /// </summary>
    public class JCS_Tweener
        : JCS_UnityObject
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables
        [Header("** Runtime Variables (JCS_Tweener) **")]
        [SerializeField] private float mDurationX = 1.0f;
        [SerializeField] private float mDurationY = 1.0f;
        [SerializeField] private float mDurationZ = 1.0f;

        [SerializeField] private bool mTweenPosition = true;
        [SerializeField] private bool mTweenRotation = false;
        [SerializeField] private bool mTweenScale = false;

        [SerializeField]
        private JCS_TweenType mEasingX= JCS_TweenType.LINEAR;
        [SerializeField]
        private JCS_TweenType mEasingY = JCS_TweenType.LINEAR;
        [SerializeField]
        private JCS_TweenType mEasingZ = JCS_TweenType.LINEAR;

        private Tweener tweener = new Tweener();

        [SerializeField] private Transform mTargetTransform = null;
        private Transform mRecordTransform = null;
        private bool mContinueTween = false;
        private float mStopTweenDistance = 1;


        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------
        public float StopTweenDistance { get { return this.mStopTweenDistance; } set { this.mStopTweenDistance = value; } }
        public float DurationX { get { return this.mDurationX; } set { this.mDurationX = value; } }
        public float DurationY { get { return this.mDurationY; } set { this.mDurationY = value; } }
        public float DurationZ { get { return this.mDurationZ; } set { this.mDurationZ = value; } }
        public JCS_TweenType EasingX { get { return this.mEasingX; } set { this.mEasingX = value; } }
        public JCS_TweenType EasingY { get { return this.mEasingY; } set { this.mEasingY = value; } }
        public JCS_TweenType EasingZ { get { return this.mEasingZ; } set { this.mEasingZ = value; } }
        public void SetTargetTransform(Transform trans) { this.mTargetTransform = trans; }

        //========================================
        //      Unity's function
        //------------------------------
        private void Awake()
        {
            // this alway need to be call once.
            UpdateUnityData();
        }

        private void FixedUpdate()
        {
#if (UNITY_EDITOR)
            Test();
#endif

            // check if do continue tweening
            ContinueTween();

            if (tweener.animating)
            {
                // Updates the Tweener
                tweener.updateX();
                tweener.updateY();
                tweener.updateZ();

                // Set the Position
                if (mTweenPosition)
                    transform.localPosition = tweener.progression;
                if (mTweenRotation)
                    transform.localEulerAngles = tweener.progression;
                if (mTweenScale)
                    transform.localScale = tweener.progression;
            }

        }

#if (UNITY_EDITOR)
        private void Test()
        {
            if (Input.GetKey(KeyCode.M))
                DoTween(new Vector3(0, 0, 0));
            if (Input.GetKey(KeyCode.N))
                DoTween(new Vector3(10, 10, 10));
            if (Input.GetKey(KeyCode.P))
                DoTween(new Vector3(-10, -10, -10));

            if (Input.GetKey(KeyCode.Z))
                DoTweenContinue(mTargetTransform);
        }
#endif

        //========================================
        //      Self-Define
        //------------------------------
        //----------------------
        // Public Functions
        public void DoTween(Vector3 to, Tweener.CallBackDelegate callback = null)
        {
            DoTween(to, mEasingX, mEasingY, mEasingZ, callback);
        }
        public void DoTween(Vector3 to,
            JCS_TweenType typeX,
            JCS_TweenType typeY,
            JCS_TweenType typeZ,
            Tweener.CallBackDelegate callback = null)
        {
            DoTween(to, mDurationX, mDurationY, mDurationZ, typeX, typeY, typeZ, callback);
        }
        public void DoTween(
            Vector3 to,
            float durationX,
            float durationY,
            float durationZ,
            JCS_TweenType typeX,
            JCS_TweenType typeY,
            JCS_TweenType typeZ,
            Tweener.CallBackDelegate callback = null)
        {

            DoTween(LocalPosition, to, durationX, durationY, durationZ, typeX, typeY, typeZ, callback);
        }
        public void DoTween(
            Vector3 from, 
            Vector3 to,
            float durationX,
            float durationY,
            float durationZ,
            JCS_TweenType typeX,
            JCS_TweenType typeY,
            JCS_TweenType typeZ,
            Tweener.CallBackDelegate callback = null)
        {
            Tweener.TweenDelegate easingX = GetEasing(typeX);
            Tweener.TweenDelegate easingY = GetEasing(typeY);
            Tweener.TweenDelegate easingZ = GetEasing(typeZ);

            StartTween(from, to, durationX, durationY, durationZ, easingX, easingY, easingZ, callback);
        }

        public void DoTweenContinue(Transform target)
        {
            SetTargetTransform(target);

            mRecordTransform = target;

            mContinueTween = true;
        }

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions

        /// <summary>
        /// Default callback function.
        /// </summary>
        private void JcsCallBack()
        {
            //Debug.Log("JcsCallBack (Default)");
        }
        private void StartTween(
            Vector3 from, 
            Vector3 to, 
            float durationX = 1f,
            float durationY = 1f,
            float durationZ = 1f,
            Tweener.TweenDelegate easingX = null,
            Tweener.TweenDelegate easingY = null,
            Tweener.TweenDelegate easingZ = null,
            Tweener.CallBackDelegate callback = null)
        {
            if (tweener != null)
            {
                // Sets The Position From -> To
                tweener.easeFromTo(
                    from, 
                    to,
                    durationX,
                    durationY,
                    durationZ,
                    easingX,
                    easingY,
                    easingZ,
                    JcsCallBack);
            }
        }


        private Tweener.TweenDelegate GetEasing(JCS_TweenType type)
        {
            Tweener.TweenDelegate easing = null;

            switch (type)
            {
                // default to linear
                case JCS_TweenType.LINEAR:
                    easing = Easing.Linear;
                    break;

                case JCS_TweenType.EASE_IN_SINE:
                    easing = Easing.SineEaseIn;
                    break;
                case JCS_TweenType.EASE_IN_CUBIC:
                    easing = Easing.CubicEaseIn;
                    break;
                case JCS_TweenType.EASE_IN_QUINT:
                    easing = Easing.QuintEaseIn;
                    break;
                case JCS_TweenType.EASE_IN_CIRC:
                    easing = Easing.CircEaseIn;
                    break;
                case JCS_TweenType.EASE_IN_BACK:
                    easing = Easing.BackEaseIn;
                    break;
                case JCS_TweenType.EASE_OUT_SINE:
                    easing = Easing.SineEaseInOut;
                    break;
                case JCS_TweenType.EASE_OUT_CUBIC:
                    easing = Easing.CubicEaseInOut;
                    break;
                case JCS_TweenType.EASE_OUT_QUINT:
                    easing = Easing.QuintEaseInOut;
                    break;
                case JCS_TweenType.EASE_OUT_CIRC:
                    easing = Easing.CircEaseOut;
                    break;
                case JCS_TweenType.EASE_OUT_BACK:
                    easing = Easing.BackEaseOut;
                    break;
                case JCS_TweenType.EASE_IN_OUT_SINE:
                    easing = Easing.SineEaseInOut;
                    break;
                case JCS_TweenType.EASE_IN_OUT_CUBIC:
                    easing = Easing.CubicEaseInOut;
                    break;
                case JCS_TweenType.EASE_IN_OUT_QUINT:
                    easing = Easing.QuintEaseInOut;
                    break;
                case JCS_TweenType.EASE_IN_OUT_CIRC:
                    easing = Easing.CircEaseInOut;
                    break;
                case JCS_TweenType.EASE_IN_OUT_BACK:
                    easing = Easing.BackEaseInOut;
                    break;
                case JCS_TweenType.EASE_IN_QUAD:
                    easing = Easing.QuadEaseIn;
                    break;
                case JCS_TweenType.EASE_IN_QUART:
                    easing = Easing.QuartEaseIn;
                    break;
                case JCS_TweenType.EASE_IN_EXPO:
                    easing = Easing.ExpoEaseIn;
                    break;
                case JCS_TweenType.EASE_IN_ELASTIC:
                    easing = Easing.ElasticEaseIn;
                    break;
                case JCS_TweenType.EASE_IN_BOUNCE:
                    easing = Easing.BounceEaseIn;
                    break;
                case JCS_TweenType.EASE_OUT_QUAD:
                    easing = Easing.QuadEaseInOut;
                    break;
                case JCS_TweenType.EASE_OUT_QUART:
                    easing = Easing.QuartEaseOut;
                    break;
                case JCS_TweenType.EASE_OUT_EXPO:
                    easing = Easing.ExpoEaseInOut;
                    break;
                case JCS_TweenType.EASE_OUT_ELASTIC:
                    easing = Easing.ElasticEaseOut;
                    break;
                case JCS_TweenType.EASE_OUT_BOUNCE:
                    easing = Easing.BounceEaseOut;
                    break;
                case JCS_TweenType.EASE_IN_OUT_QUAD:
                    easing = Easing.QuadEaseInOut;
                    break;
                case JCS_TweenType.EASE_IN_OUT_QUART:
                    easing = Easing.QuartEaseInOut;
                    break;
                case JCS_TweenType.EASE_IN_OUT_EXPO:
                    easing = Easing.ExpoEaseInOut;
                    break;
                case JCS_TweenType.EASE_IN_OUT_ELASTIC:
                    easing = Easing.ElasticEaseInOut;
                    break;
                case JCS_TweenType.EASE_IN_OUT_BOUNCE:
                    easing = Easing.BounceEaseInOut;
                    break;
            }

            return easing;
        }

        private void ContinueTween()
        {
            if (!mContinueTween)
                return;

            if (mTargetTransform == null)
            {
                JCS_GameErrors.JcsErrors(
                    "JCS_Tweener", 
                    -1,
                    "Start the tween but the target transform are null...");

                mContinueTween = false;
                return;
            }

            DoTween(mTargetTransform.position);

            float distance = Vector3.Distance(LocalPosition, mTargetTransform.position);
            if (distance <= mStopTweenDistance)
                mContinueTween = false;
        }


    }
}
