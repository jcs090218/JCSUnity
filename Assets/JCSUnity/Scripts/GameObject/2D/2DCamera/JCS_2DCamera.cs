/**
 * $File: JCS_2DCamera.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;

namespace JCSUnity
{

    [RequireComponent(typeof(JCS_BGMPlayer))]     // for background music
    public class JCS_2DCamera 
        : JCS_Camera
    {
        public static string JCS_2DCAMERA_PATH = "JCSUnity_Framework_Resources/JCS_Camera/JCS_2DCamera";

        private const float MAX_ZOOM_DISTANCE = 30.0f;
        private const float MIN_ZOOM_DISTANCE = 4.0f;
        private const float MAX_MOVE_SPEED = 20.0f;
        private const float MIN_MOVE_SPEED = 4.0f;

        [Header("** Check Varialbes (JCS_2DCamera) **")]
        [Tooltip("Depth of camera.")]
        [SerializeField] [Range(MIN_ZOOM_DISTANCE, MAX_ZOOM_DISTANCE)]
        private float mCameraDepth = 10.0f;

        [Header("** Runtime Variables (JCS_2DCamera) **")]
        //-- Target information
        [SerializeField] private Transform mTargetTransform = null;
        private Vector3 mTargetPosition = Vector3.zero;
        [SerializeField] private Vector3 mPositionOffset = Vector3.zero;

        // effect: camera will do the smooth movement
        [SerializeField] private bool mSmoothMoveX = true;
        [SerializeField] private bool mSmoothMoveY = true;

        [Header("** Freeze Settings (Run Time) **")]
        [SerializeField] private bool mFreezeInRuntime = false;
        [SerializeField] private bool mFreezeX = false;
        [SerializeField] private bool mFreezeY = false;
        [SerializeField] private bool mFreezeZ = false;
        private Vector3 mFreezeRecord = Vector3.zero;

        //-- Scroll
        [Header("** Scroll Setting **")]

        [Tooltip("Distance once u scroll.")]
        [SerializeField] [Range(0.0f, 100.0f)]
        private float mScrollRange = 2.0f;

        [Tooltip("How fast it scroll. (Zoom In/Out)")]
        [SerializeField] [Range(0.1f, 10.0f)]
        private float mScrollFriction = 0.4f;

        private float mWheelDegree = 0;

        //-- Move
        [SerializeField] private float mDampTime = 0.7f;
        [Header("** Scene Setting **")]
        [SerializeField] private float mMax_X_PositionInScene = float.PositiveInfinity;
        [SerializeField] private float mMin_X_PositionInScene = float.NegativeInfinity;
        [SerializeField] private float mMax_Y_PositionInScene = float.PositiveInfinity;
        [SerializeField] private float mMin_Y_PositionInScene = float.NegativeInfinity;
        [Header("** Camera Scope Setting **")]
        [SerializeField] private float mOutOfScopeDampeTime = 0.4f;
        [SerializeField] private float mOutOfScopeDistanceX = 10;
        [SerializeField] private float mOutOfScopeDistanceY = 10;
        private float mRecordDampTime = 0;

        private Vector3 mRecordPosition = Vector3.zero;

        //-- Audio References
        private JCS_BGMPlayer mJCSBGMPlayer = null;


        //========================================
        //      setter / getter
        //------------------------------
        public bool FreezeX { get { return this.mFreezeX; } set { this.mFreezeX = value; } }
        public bool FreezeY { get { return this.mFreezeY; } set { this.mFreezeY = value; } }
        public bool FreezeZ { get { return this.mFreezeZ; } set { this.mFreezeZ = value; } }
        public void SetFollowTarget(Transform trans) { this.mTargetTransform = trans; }
        public bool GetFollowing() { return this.mFollowing; }
        public void SetFollowing(bool val) { this.mFollowing = val; }
        public Transform GetTargetTransform() { return this.mTargetTransform; }
        public JCS_BGMPlayer GetJCSBGMPlayer() { return this.mJCSBGMPlayer; }

        //========================================
        //      Unity's function
        //------------------------------
        protected override void Awake()
        {
            base.Awake();

            mJCSBGMPlayer = this.GetComponent<JCS_BGMPlayer>();

            // record down the LD settings, in order
            // to change the damp time with out losing the 
            // LD setting!!!
            this.mRecordDampTime = mDampTime;

            this.mFreezeRecord = this.transform.position;
        }

        private void Start()
        {
            // Use player from "JCS_GameManager" as default
            if (this.mTargetTransform == null)
            {
                JCS_Player player = JCS_GameManager.instance.GetJCSPlayer();
                if (player != null)
                    SetFollowTarget(player.transform);
            }

            if (mTargetTransform != null)
            {
                Vector3 newPos = mTargetTransform.position;
                newPos.z = this.transform.position.z;
                this.transform.position = newPos;
            }

        }

        private void Update()
        {
            if (this.mTargetTransform == null)
                return;

            // Do the camera boundaries check!!
            CameraBoundaries();

            if (!mFollowing)
                return;

            // if freezing effect in runtime, we have to record down this
            if (mFreezeInRuntime)
                this.mFreezeRecord = this.transform.position;

            // get the wheel value from the Unity API
            // (physical layer[mouse wheel] -> 
            // os layer[windows7] -> 
            // application layer[Unity itself]) -> 
            // to here...
            mWheelDegree = Input.GetAxis("Mouse ScrollWheel");
            ZoomCamera(mWheelDegree);

            // Check if target out of the distance
            CheckTargetOutOfScope();

            Vector3 point = GetCamera().WorldToViewportPoint(mTargetTransform.position);
            Vector3 delta = (mTargetTransform.position + mPositionOffset) - GetCamera().ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z)); //(new Vector3(0.5, 0.5, point.z));
            Vector3 destination = transform.position + delta;

            if (mSmoothMoveX && mSmoothMoveY)
            {
                // record down the current position, 
                // in order to get the last  un effect position
                mRecordPosition = this.transform.position;
            }
            else if (mSmoothMoveX)
            {
                // help record the other direction
                mRecordPosition.x = this.transform.position.x;

                // apply last position
                destination.y = mRecordPosition.y;

                mVelocity.y = 0;
            }
            else if (mSmoothMoveY)
            {
                // help record the other direction
                mRecordPosition.y = this.transform.position.y;

                // apply last position
                destination.x = mRecordPosition.x;

                mVelocity.x = 0;
            }


            // Only when the effect is on will call this movement function
            if (mSmoothMoveX || mSmoothMoveY)
                transform.position = Vector3.SmoothDamp(transform.position, destination, ref mVelocity, mDampTime);

            // Update self position
            this.transform.position += this.mVelocity * Time.deltaTime;

            // do freezing the last
            DoFreezing();
        }

        //========================================
        //      Self-Define
        //------------------------------

        /// <summary>
        /// Set to the target's position in frame.
        /// </summary>
        public void SetToTargetImmediate()
        {
            if (GetTargetTransform() == null)
                return;

            Vector3 targetPos = GetTargetTransform().position;

            Vector3 newPos = this.transform.position;

            newPos.x = targetPos.x;
            newPos.y = targetPos.y;

            this.transform.position = newPos;
        }

        /// <summary>
        /// Do the zooming in Z axis!!
        /// </summary>
        /// <param name="depthDistance"></param>
        private void ZoomCamera(float depthDistance)
        {
            if (depthDistance == 0)
                return;

            this.mCameraDepth -= mWheelDegree * mScrollRange;

            if (mCameraDepth <= MIN_ZOOM_DISTANCE)
                mCameraDepth = MIN_ZOOM_DISTANCE;
            else if (mCameraDepth > MAX_ZOOM_DISTANCE)
                mCameraDepth = MAX_ZOOM_DISTANCE;


            float targetDepth = mTargetPosition.z - mCameraDepth;
            mVelocity.z = (targetDepth - this.transform.position.z) / mScrollFriction;
        }
        /// <summary>
        /// if the camera and target have certain amount of distance
        /// make the damp time lower in order to catch up the target
        /// that camera (this) is following!!
        /// </summary>
        private void CheckTargetOutOfScope()
        {
            Vector3 targetVec = GetTargetTransform().position;
            Vector3 thisVec = this.transform.position;
            float distanceX = targetVec.x - thisVec.x;
            float distanceY = targetVec.y - thisVec.y;

            // make it absolute value in order to get the 
            // distance correctly
            distanceX = JCS_Mathf.AbsoluteValue(distanceX);
            distanceY = JCS_Mathf.AbsoluteValue(distanceY);

            if (distanceX > mOutOfScopeDistanceX ||
                distanceY > mOutOfScopeDistanceY)
            {
                // change to the fast damp time so we dont lose 
                // the target from the camera!!
                mDampTime = mOutOfScopeDampeTime;
            }
            else
            {
                // change the damp time back to normal
                mDampTime = mRecordDampTime;
            }
        }
        /// <summary>
        /// 4 boundaries (top, bottom, right, left) that camera 
        /// should not go through.
        /// check the boundries and do the trick!
        /// </summary>
        private void CameraBoundaries()
        {
            Vector3 camPos = this.transform.position;
            Vector3 targetPos = GetTargetTransform().position;
            if (camPos.x > mMax_X_PositionInScene)
            {
                mSmoothMoveX = false;
                camPos.x = mMax_X_PositionInScene;

                if (targetPos.x < camPos.x)
                    mSmoothMoveX = true;
            }
            else if (camPos.x < mMin_X_PositionInScene)
            {
                mSmoothMoveX = false;
                camPos.x = mMin_X_PositionInScene;

                if (targetPos.x > camPos.x)
                    mSmoothMoveX = true;
            }

            if (camPos.y > mMax_Y_PositionInScene)
            {
                mSmoothMoveY = false;
                camPos.y = mMax_Y_PositionInScene;

                if (targetPos.y < camPos.y)
                    mSmoothMoveY = true;
            }
            else if (camPos.y < mMin_Y_PositionInScene)
            {
                mSmoothMoveY = false;
                camPos.y = mMin_Y_PositionInScene;

                if (targetPos.y > camPos.y)
                    mSmoothMoveY = true;
            }
        }
        /// <summary>
        /// Do freezing.
        /// </summary>
        private void DoFreezing()
        {
            Vector3 newPos = this.transform.position;
            if (mFreezeX)
            {
                newPos.x = mFreezeRecord.x;
                mVelocity.x = 0;
            }
            if (mFreezeY)
            {
                newPos.y = mFreezeRecord.y;
                mVelocity.y = 0;
            }
            if (mFreezeZ)
            {
                newPos.z = mFreezeRecord.z;
                mVelocity.z = 0;
            }

            this.transform.position = newPos;
        }

    }
}
