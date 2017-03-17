/**
 * $File: JCS_Tweener.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
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

        [Header("** Check Variables (JCS_Tweener) **")]

        [SerializeField]
        private bool mContinueTween = false;

        [Tooltip("Whats the target we tween to?")]
        [SerializeField]
        private Transform mTargetTransform = null;

        private Transform mRecordTransform = null;


        [Header("** Runtime Variables (JCS_Tweener) **")]

        [Tooltip("Do the tween effect?")]
        [SerializeField]
        private bool mTween = true;

        [Tooltip("How fase it move on x axis.")]
        [SerializeField] [Range(0.01f, 1000.0f)]
        private float mDurationX = 1.0f;

        [Tooltip("How fase it move on y axis.")]
        [SerializeField] [Range(0.01f, 1000.0f)]
        private float mDurationY = 1.0f;

        [Tooltip("How fase it move on z axis.")]
        [SerializeField] [Range(0.01f, 1000.0f)]
        private float mDurationZ = 1.0f;


        [Header("- Randomize Duration")]

        [Tooltip("Randomize the duration with all axis at start. (x, y, z)")]
        [SerializeField] [Range(0.0f, 1000.0f)]
        private float mRandomizeDuration = 0.0f;


        [Header("- Tweener Effect Transform")]

        [Tooltip("Do tween effect with position?")]
        [SerializeField]
        private bool mTweenPosition = true;

        [Tooltip("Do tween effect with rotation?")]
        [SerializeField]
        private bool mTweenRotation = false;

        [Tooltip("Do tween effect with scale?")]
        [SerializeField]
        private bool mTweenScale = false;

        [Tooltip("Do the track base on location position.")]
        [SerializeField]
        private bool mTrackAsLocalPosition = false;


        [Header("- Tweener Formula Type")]

        [Tooltip("Tweener formula on x axis.")]
        [SerializeField]
        private JCS_TweenType mEasingX = JCS_TweenType.LINEAR;

        [Tooltip("Tweener formula on y axis.")]
        [SerializeField]
        private JCS_TweenType mEasingY = JCS_TweenType.LINEAR;

        [Tooltip("Tweener formula on z axis.")]
        [SerializeField]
        private JCS_TweenType mEasingZ = JCS_TweenType.LINEAR;

        private Tweener tweener = new Tweener();


        [Header("- Continuous Tween (JCS_Tweener) ")]

        [Tooltip("While Continue tween when did the tweener algorithm stop?")]
        [SerializeField]
        private float mStopTweenDistance = 1;

        private Tweener.CallBackDelegate mDestinationCallback = null;


        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------
        public bool Tween { get { return this.mTween; } set { this.mTween = value; } }
        public float StopTweenDistance { get { return this.mStopTweenDistance; } set { this.mStopTweenDistance = value; } }
        public float DurationX { get { return this.mDurationX; } set { this.mDurationX = value; } }
        public float DurationY { get { return this.mDurationY; } set { this.mDurationY = value; } }
        public float DurationZ { get { return this.mDurationZ; } set { this.mDurationZ = value; } }
        public JCS_TweenType EasingX { get { return this.mEasingX; } set { this.mEasingX = value; } }
        public JCS_TweenType EasingY { get { return this.mEasingY; } set { this.mEasingY = value; } }
        public JCS_TweenType EasingZ { get { return this.mEasingZ; } set { this.mEasingZ = value; } }
        public void SetTargetTransform(Transform trans) { this.mTargetTransform = trans; }
        public Transform RecordTransform { get { return this.mRecordTransform; } }

        //========================================
        //      Unity's function
        //------------------------------
        private void Awake()
        {
            // this alway need to be call once.
            UpdateUnityData();
            
            RandomizeDuration();
        }

        private void LateUpdate()
        {
#if (UNITY_EDITOR)
            Test();
#endif

            // check the effect enable?
            if (!Tween)
                return;

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

        /// <summary>
        /// Reset tweener effect setting.
        /// </summary>
        public void ResetTweener()
        {
            tweener.ResetTweener();
        }

        /// <summary>
        /// Callback when reach destination.
        /// </summary>
        /// <param name="func"> function pointer </param>
        public void SetCallback(Tweener.CallBackDelegate func)
        {
            mDestinationCallback = func;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="to"></param>
        /// <param name="callback"></param>
        public void DoTween(Vector3 to, Tweener.CallBackDelegate callback = null)
        {
            DoTween(to, mEasingX, mEasingY, mEasingZ, callback);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="to"></param>
        /// <param name="typeX"></param>
        /// <param name="typeY"></param>
        /// <param name="typeZ"></param>
        /// <param name="callback"></param>
        public void DoTween(Vector3 to,
            JCS_TweenType typeX,
            JCS_TweenType typeY,
            JCS_TweenType typeZ,
            Tweener.CallBackDelegate callback = null)
        {
            DoTween(to, mDurationX, mDurationY, mDurationZ, typeX, typeY, typeZ, callback);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="to"></param>
        /// <param name="durationX"></param>
        /// <param name="durationY"></param>
        /// <param name="durationZ"></param>
        /// <param name="typeX"></param>
        /// <param name="typeY"></param>
        /// <param name="typeZ"></param>
        /// <param name="callback"></param>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="durationX"></param>
        /// <param name="durationY"></param>
        /// <param name="durationZ"></param>
        /// <param name="typeX"></param>
        /// <param name="typeY"></param>
        /// <param name="typeZ"></param>
        /// <param name="callback"></param>
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

        /// <summary>
        /// Continue Tween to this target's position.
        /// </summary>
        /// <param name="target"> target's transform </param>
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
        /// 
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="durationX"></param>
        /// <param name="durationY"></param>
        /// <param name="durationZ"></param>
        /// <param name="easingX"></param>
        /// <param name="easingY"></param>
        /// <param name="easingZ"></param>
        /// <param name="callback"></param>
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
                    mDestinationCallback);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Continue tween to the target's position in update.
        /// </summary>
        private void ContinueTween()
        {
            if (!mContinueTween)
                return;

            if (mTargetTransform == null)
            {
#if (UNITY_EDITOR)
                // log string to console cost alost of performance.
                // so do it only when is debug mode.
                if (JCS_GameSettings.instance.DEBUG_MODE)
                {
                    JCS_Debug.JcsErrors(
                        this, "Start the tween but the target transform are null...");
                }
#endif

                mContinueTween = false;
                return;
            }

            float distance = 0;
            if (mTrackAsLocalPosition)
            {
                distance = Vector3.Distance(LocalPosition, mTargetTransform.localPosition);
                DoTween(mTargetTransform.localPosition);
            }
            else
            {
                distance = Vector3.Distance(LocalPosition, mTargetTransform.position);
                DoTween(mTargetTransform.position);
            }

            if (distance <= mStopTweenDistance)
            {
                mContinueTween = false;

                // call the call back.
                tweener.ResetTweener();
                tweener.CheckUpdate(true);
            }
        }

        /// <summary>
        /// Randomize the duration with all axis. (x, y, z)
        /// </summary>
        private void RandomizeDuration()
        {
            if (mRandomizeDuration != 0)
            {
                float randomizeDuration = JCS_Utility.JCS_FloatRange(-mRandomizeDuration, mRandomizeDuration);

                this.mDurationX += randomizeDuration;
                this.mDurationY += randomizeDuration;
                this.mDurationZ += randomizeDuration;
            }
        }

    }
}
