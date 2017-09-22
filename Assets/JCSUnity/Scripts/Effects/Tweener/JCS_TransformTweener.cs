/**
 * $File: JCS_TransformTweener.cs $
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
    public class JCS_TransformTweener
        : JCS_UnityObject
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables

        private CallBackDelegate mDestinationCallback = null;


        [Header("** Check Variables (JCS_TransformTweener) **")]

        [SerializeField]
        private bool mContinueTween = false;

        [Tooltip("Whats the target we tween to?")]
        [SerializeField]
        private Transform mTargetTransform = null;

        // use to check if the target transform move or not.
        private Vector3 mRecordTargetTransformValue = Vector3.zero;

        private Transform mRecordTransform = null;

        [Tooltip("Is done tweening/animating?")]
        [SerializeField]
        private bool mIsDoneTweening = false;


        [Header("** Runtime Variables (JCS_TransformTweener) **")]

        [Tooltip("Do the tween effect?")]
        [SerializeField]
        private bool mTween = true;

        [Tooltip("Value offset.")]
        [SerializeField]
        private Vector3 mValueOffset = Vector3.zero;

        [Tooltip("How fase it move on x axis.")]
        [SerializeField]
        [Range(0.01f, 1000.0f)]
        private float mDurationX = 1.0f;

        [Tooltip("How fase it move on y axis.")]
        [SerializeField]
        [Range(0.01f, 1000.0f)]
        private float mDurationY = 1.0f;

        [Tooltip("How fase it move on z axis.")]
        [SerializeField]
        [Range(0.01f, 1000.0f)]
        private float mDurationZ = 1.0f;


        [Header("- Destory")]

        [Tooltip("Destory this object when done tweening?")]
        [SerializeField]
        private bool mDestroyWhenDoneTweening = false;

        [Tooltip("How many times of done tweening destroy will active?")]
        [SerializeField]
        [Range(1, 10)]
        private int mDestroyDoneTweeningCount = 1;


        [Header("- Randomize Duration")]

        [Tooltip("Randomize the duration with all axis at start. (x, y, z)")]
        [SerializeField]
        [Range(0.0f, 1000.0f)]
        private float mRandomizeDuration = 0.0f;


        [Header("- Tweener Effect Transform")]

        [Tooltip("Which transform's properties to tween.")]
        [SerializeField]
        private JCS_TransformType mTweenType = JCS_TransformType.POSITION;

        [Tooltip("Change the self position as local position.")]
        [SerializeField]
        private bool mTrackAsLocalSelf = false;

        [Tooltip("Track the target as local position.")]
        [SerializeField]
        private bool mTrackAsLocalTarget = false;


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

        private Vector3Tweener tweener = new Vector3Tweener();


        [Header("- Continuous Tween (JCS_TransformTweener) ")]

        [Tooltip("While Continue tween when did the tweener algorithm stop?")]
        [SerializeField]
        [Range(0.0f, 1000.0f)]
        private float mStopTweenDistance = 1;

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------
        public bool IsDoneTweening { get { return this.mIsDoneTweening; } }
        public bool Tween { get { return this.mTween; } set { this.mTween = value; } }
        public bool TrackAsLocalSelf { get { return this.mTrackAsLocalSelf; } set { this.mTrackAsLocalSelf = value; } }
        public bool TrackAsLocalTarget { get { return this.mTrackAsLocalTarget; } set { this.mTrackAsLocalTarget = value; } }
        public float StopTweenDistance { get { return this.mStopTweenDistance; } set { this.mStopTweenDistance = value; } }
        public float DurationX { get { return this.mDurationX; } set { this.mDurationX = value; } }
        public float DurationY { get { return this.mDurationY; } set { this.mDurationY = value; } }
        public float DurationZ { get { return this.mDurationZ; } set { this.mDurationZ = value; } }
        public JCS_TweenType EasingX { get { return this.mEasingX; } set { this.mEasingX = value; } }
        public JCS_TweenType EasingY { get { return this.mEasingY; } set { this.mEasingY = value; } }
        public JCS_TweenType EasingZ { get { return this.mEasingZ; } set { this.mEasingZ = value; } }
        public void SetTargetTransform(Transform trans) { this.mTargetTransform = trans; }
        public Transform RecordTransform { get { return this.mRecordTransform; } }
        public bool DestroyWhenDoneTweening { get { return this.mDestroyWhenDoneTweening; } set { this.mDestroyWhenDoneTweening = value; } }
        public JCS_TransformType TweenType { get { return this.mTweenType; } set { this.mTweenType = value; } }
        public Vector3 ValueOffset { get { return this.mValueOffset; } set { this.mValueOffset = value; } }
        public float ValueOffsetX { get { return this.mValueOffset.x; } set { this.mValueOffset.x = value; } }
        public float ValueOffsetY { get { return this.mValueOffset.y; } set { this.mValueOffset.y = value; } }
        public float ValueOffsetZ { get { return this.mValueOffset.z; } set { this.mValueOffset.z = value; } }

        //========================================
        //      Unity's function
        //------------------------------
        protected override void Awake()
        {
            base.Awake();

            RandomizeDuration();

            tweener.SetCallback(DoneTweening);
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

                // Set value to one of the transform properties.
                SetSelfTransformTypeVector3(tweener.progression);

                mIsDoneTweening = false;
            }
            else
            {
                mIsDoneTweening = true;
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
        public void SetCallback(CallBackDelegate func)
        {
            mDestinationCallback = func;
        }


        /// <summary>
        /// Tween to this vector either position, scale, rotation.
        /// </summary>
        /// <param name="to"> target vector 3 </param>
        /// <param name="callback"> callback function pointer. </param>
        public void DoTween(Vector3 to, CallBackDelegate callback = null)
        {
            DoTween(to, true, mEasingX, mEasingY, mEasingZ, callback);
        }

        /// <summary>
        /// Tween to this vector either position, scale, rotation.
        /// </summary>
        /// <param name="to"> target vector 3 </param>
        /// <param name="resetElapsedTime"> reset elapsed time? (default : true) </param>
        /// <param name="callback"> callback function pointer. </param>
        public void DoTween(Vector3 to, bool resetElapsedTime, CallBackDelegate callback = null)
        {
            DoTween(to, resetElapsedTime, mEasingX, mEasingY, mEasingZ, callback);
        }

        /// <summary>
        /// Tween to this vector either position, scale, rotation.
        /// </summary>
        /// <param name="to"> target vector 3 </param>
        /// <param name="resetElapsedTime"> reset elapsed time? (default : true) </param>
        /// <param name="typeX"> easing type for x axis. </param>
        /// <param name="typeY"> easing type for y axis. </param>
        /// <param name="typeZ"> easing type for z axis. </param>
        /// <param name="callback"> callback function pointer. </param>
        public void DoTween(
            Vector3 to,
            bool resetElapsedTime,
            JCS_TweenType typeX,
            JCS_TweenType typeY,
            JCS_TweenType typeZ,
            CallBackDelegate callback = null)
        {
            DoTween(to, resetElapsedTime, mDurationX, mDurationY, mDurationZ, typeX, typeY, typeZ, callback);
        }

        /// <summary>
        /// Tween to this vector either position, scale, rotation.
        /// </summary>
        /// <param name="to"> target vector 3 </param>
        /// <param name="resetElapsedTime"> reset elapsed time? (default : true) </param>
        /// <param name="durationX"> how fast it tween on x axis. </param>
        /// <param name="durationY"> how fast it tween on y axis. </param>
        /// <param name="durationZ"> how fast it tween on z axis. </param>
        /// <param name="typeX"> easing type for x axis. </param>
        /// <param name="typeY"> easing type for y axis. </param>
        /// <param name="typeZ"> easing type for z axis. </param>
        /// <param name="callback"> callback function pointer. </param>
        public void DoTween(
            Vector3 to,
            bool resetElapsedTime,
            float durationX,
            float durationY,
            float durationZ,
            JCS_TweenType typeX,
            JCS_TweenType typeY,
            JCS_TweenType typeZ,
            CallBackDelegate callback = null)
        {
            Vector3 from = GetSelfTransformTypeVector3();

            DoTween(from, to, resetElapsedTime, durationX, durationY, durationZ, typeX, typeY, typeZ, callback);
        }

        /// <summary>
        /// Tween to this vector either position, scale, rotation.
        /// </summary>
        /// <param name="from"> starting vector 3 </param>
        /// <param name="to"> target vector 3 </param>
        /// <param name="resetElapsedTime"> reset elapsed time? (default : true) </param>
        /// <param name="durationX"> how fast it tween on x axis. </param>
        /// <param name="durationY"> how fast it tween on y axis. </param>
        /// <param name="durationZ"> how fast it tween on z axis. </param>
        /// <param name="typeX"> easing type for x axis. </param>
        /// <param name="typeY"> easing type for y axis. </param>
        /// <param name="typeZ"> easing type for z axis. </param>
        /// <param name="callback"> callback function pointer. </param>
        public void DoTween(
            Vector3 from,
            Vector3 to,
            bool resetElapsedTime,
            float durationX,
            float durationY,
            float durationZ,
            JCS_TweenType typeX,
            JCS_TweenType typeY,
            JCS_TweenType typeZ,
            CallBackDelegate callback = null)
        {
            TweenDelegate easingX = JCS_Utility.GetEasing(typeX);
            TweenDelegate easingY = JCS_Utility.GetEasing(typeY);
            TweenDelegate easingZ = JCS_Utility.GetEasing(typeZ);

            StartTween(from, to, resetElapsedTime, durationX, durationY, durationZ, easingX, easingY, easingZ, callback);
        }

        /// <summary>
        /// Continue Tween to this target's position.
        /// </summary>
        /// <param name="target"> target's transform </param>
        public void DoTweenContinue(Transform target)
        {
            SetTargetTransform(target);

            mRecordTransform = target;

            // reset record
            mRecordTargetTransformValue = Vector3.zero;

            mContinueTween = true;
        }

        /// <summary>
        /// Get itself (JCS_UnityObject) transform type's vector3 value.
        /// </summary>
        /// <returns> Vector3 value base on transform type selected. </returns>
        public Vector3 GetSelfTransformTypeVector3()
        {
            Vector3 val = Vector3.zero;

            switch (mTweenType)
            {
                case JCS_TransformType.POSITION:
                    {
                        if (mTrackAsLocalSelf)
                            val = LocalPosition;
                        else
                            val = Position;
                    }
                    break;
                case JCS_TransformType.ROTATION:
                    {
                        if (mTrackAsLocalSelf)
                            val = LocalEulerAngles;
                        else
                            val = EulerAngles;
                    }
                    break;
                case JCS_TransformType.SCALE:
                    val = LocalScale;
                    break;
            }
            return val;
        }

        /// <summary>
        /// Set self transform value.
        /// </summary>
        /// <param name="newVal"></param>
        public void SetSelfTransformTypeVector3(Vector3 newVal)
        {
            switch (mTweenType)
            {
                case JCS_TransformType.POSITION:
                    {
                        if (mTrackAsLocalSelf)
                            LocalPosition = newVal;
                        else
                            Position = newVal;
                    }
                    break;
                case JCS_TransformType.ROTATION:
                    {
                        if (mTrackAsLocalSelf)
                            LocalEulerAngles = newVal;
                        else
                            EulerAngles = newVal;
                    }
                    break;
                case JCS_TransformType.SCALE:
                    LocalScale = newVal;
                    break;
            }
        }

        /// <summary>
        /// Get target transform type's vector3 value.
        /// </summary>
        /// <returns> Target transform's vector3 value base on transform 
        /// type selected. </returns>
        public Vector3 GetTargetTransformTypeVector3()
        {
            Vector3 val = Vector3.zero;

            switch (mTweenType)
            {
                case JCS_TransformType.POSITION:
                    {
                        if (mTrackAsLocalTarget)
                            val = mTargetTransform.localPosition;
                        else
                            val = mTargetTransform.position;
                    }
                    break;
                case JCS_TransformType.ROTATION:
                    {
                        if (mTrackAsLocalTarget)
                            val = mTargetTransform.localEulerAngles;
                        else
                            val = mTargetTransform.eulerAngles;
                    }
                    break;
                case JCS_TransformType.SCALE:
                    val = mTargetTransform.localScale;
                    break;
            }
            return val;
        }

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions

        /// <summary>
        /// Default callback when done tweening.
        /// </summary>
        protected void DoneTweening()
        {
            --mDestroyDoneTweeningCount;

            if (mDestroyWhenDoneTweening)
            {
                if (mDestroyDoneTweeningCount <= 0)
                    Destroy(this.gameObject);
            }
        }

        /// <summary>
        /// Prepare for tweening.
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
            bool resetElapsedTime = true,
            float durationX = 1f,
            float durationY = 1f,
            float durationZ = 1f,
            TweenDelegate easingX = null,
            TweenDelegate easingY = null,
            TweenDelegate easingZ = null,
            CallBackDelegate callback = null)
        {
            if (tweener != null)
            {
                // Sets The Position From -> To
                tweener.easeFromTo(
                    from,
                    to + mValueOffset,  // add offset to final value.
                    resetElapsedTime,
                    durationX,
                    durationY,
                    durationZ,
                    easingX,
                    easingY,
                    easingZ,
                    mDestinationCallback);

                this.mIsDoneTweening = false;

                this.mContinueTween = false;
            }
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
                    JCS_Debug.LogError(
                        "Start the tween but the target transform are null...");
                }
#endif

                mContinueTween = false;
                return;
            }

            float distance = 0;
            Vector3 selfVal = GetSelfTransformTypeVector3();
            Vector3 targetVal = GetTargetTransformTypeVector3();
            distance = Vector3.Distance(selfVal, targetVal);

            print("self val: " + selfVal);
            print("target val: " + targetVal);

            // no need to tween again if the position has not change.
            if (mRecordTargetTransformValue == targetVal)
                return;

            DoTween(targetVal, false);

            // Everytime we do tween will make 'mContinueTween' to false,
            // so make sure we enable this back here after do tween.
            mContinueTween = true;

            // record down the target position
            mRecordTargetTransformValue = targetVal;


            // Check if close enough to the distance we target.
            if (distance <= mStopTweenDistance)
            {
                mContinueTween = false;

                // call the call back.
                tweener.ResetTweener();
                tweener.DoCallBack();
            }
        }

        /// <summary>
        /// Randomize the duration with all axis. (x, y, z)
        /// </summary>
        private void RandomizeDuration()
        {
            if (mRandomizeDuration != 0)
            {
                float randomizeDuration = JCS_Random.Range(-mRandomizeDuration, mRandomizeDuration);

                this.mDurationX += randomizeDuration;
                this.mDurationY += randomizeDuration;
                this.mDurationZ += randomizeDuration;
            }
        }

    }
}
