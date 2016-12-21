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

    /// <summary>
    /// 
    /// </summary>
    [RequireComponent(typeof(JCS_BGMPlayer))]     // for background music
    public class JCS_2DCamera 
        : JCS_Camera
    {
        public static string JCS_2DCAMERA_PATH = "JCSUnity_Framework_Resources/JCS_Camera/JCS_2DCamera";


        [Header("** Check Varialbes (JCS_2DCamera) **")]

        [SerializeField]
        private Vector3 mTargetPosition = Vector3.zero;


        [Header("** Initialize Variables (JCS_2DCamera) **")]

        [Tooltip("")]
        [SerializeField]
        private bool mSetToPlayerPositionAtStart = true;


        [Header("** Runtime Variables (JCS_2DCamera) **")]

        //-- Target information
        [Tooltip("Target transform information.")]
        [SerializeField]
        private Transform mTargetTransform = null;

        // effect: camera will do the smooth movement
        //[SerializeField] private bool mSmoothMoveX = true;
        //[SerializeField] private bool mSmoothMoveY = true;

        [Header("** Freeze Settings (Run Time) **")]
        [SerializeField] private bool mFreezeInRuntime = false;
        [SerializeField] private bool mFreezeX = false;
        [SerializeField] private bool mFreezeY = false;
        [SerializeField] private bool mFreezeZ = false;
        private Vector3 mFreezeRecord = Vector3.zero;

        //-- Scroll
        [Header("** Scroll Setting **")]

        [Tooltip("Do the zoom effect?")]
        [SerializeField]
        private bool mZoomEffect = true;

        [Tooltip("Distance once u scroll.")]
        [SerializeField] [Range(0.0f, 100.0f)]
        private float mScrollRange = 2.0f;

        [Tooltip("How fast it scroll. (Zoom In/Out)")]
        [SerializeField] [Range(0.1f, 10.0f)]
        private float mScrollFriction = 0.4f;

        private float mWheelDegree = 0;


        [Header("** Speed / Friction **")]

        [Tooltip("")]
        [SerializeField]
        private float mFrictionX = 0.6f;

        [Tooltip("")]
        [SerializeField]
        private float mFrictionY = 0.6f;


        //-- Move
        [Header("** Scene Setting **")]

        [Tooltip("")]
        [SerializeField]
        private float mMax_X_PositionInScene = float.PositiveInfinity;
        [Tooltip("")]
        [SerializeField]
        private float mMin_X_PositionInScene = float.NegativeInfinity;
        [Tooltip("")]
        [SerializeField]
        private float mMax_Y_PositionInScene = float.PositiveInfinity;
        [Tooltip("")]
        [SerializeField]
        private float mMin_Y_PositionInScene = float.NegativeInfinity;

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
                // first assign the target transform's position 
                // to target position.
                mTargetPosition = this.mTargetTransform.position;
            }

            if (mSetToPlayerPositionAtStart)
            {
                JCS_Player player = JCS_PlayerManager.instance.GetActivePlayer();

                if (player != null)
                {
                    // set the camera position
                    JCS_Camera.main.SetPosition(
                        player.transform.position.x,
                        player.transform.position.y);
                }
            }
        }

        private void Update()
        {
            if (this.mTargetTransform == null)
                return;

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

            // Try to approach to the target position.
            mVelocity.x = ((this.mTargetTransform.position.x + mPositionOffset.x) - 
                this.transform.position.x) / mFrictionX;
            mVelocity.y = ((this.mTargetTransform.position.y + mPositionOffset.y) - 
                this.transform.position.y) / mFrictionY;
            mVelocity.z = ((this.mTargetPosition.z + mPositionOffset.z) -
                this.transform.position.z) / mScrollFriction;

            // Update self position
            this.transform.position += this.mVelocity * Time.deltaTime;

            // do freezing the last
            DoFreezing();

            // Do the camera boundaries check!!
            CameraBoundaries();
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
            // check the trigger of the 
            // scrolling effect.
            if (!mZoomEffect)
                return;

            if (depthDistance == 0)
                return;

            float cameraDepth = mWheelDegree * mScrollRange;

            this.mTargetPosition.z += cameraDepth;
        }

        /// <summary>
        /// 4 boundaries (top, bottom, right, left) that camera 
        /// should not go through.
        /// check the boundries and do the trick!
        /// </summary>
        private void CameraBoundaries()
        {
            Vector3 camPos = this.transform.position;

            if (camPos.x > mMax_X_PositionInScene)
            {
                camPos.x = mMax_X_PositionInScene;
            }
            else if (camPos.x < mMin_X_PositionInScene)
            {
                camPos.x = mMin_X_PositionInScene;
            }

            if (camPos.y > mMax_Y_PositionInScene)
            {
                camPos.y = mMax_Y_PositionInScene;
            }
            else if (camPos.y < mMin_Y_PositionInScene)
            {
                camPos.y = mMin_Y_PositionInScene;
            }

            this.transform.position = camPos;
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
