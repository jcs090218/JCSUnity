using UnityEngine;
using System.Collections;

namespace JCSUnity
{

    [RequireComponent(typeof(JCS_BGMPlayer))]     // for background music
    public class JCS_2DCamera : JCS_Camera
    {
        private const float MAX_ZOOM_DISTANCE = 30.0f;
        private const float MIN_ZOOM_DISTANCE = 4.0f;
        private const float MAX_MOVE_SPEED = 20.0f;
        private const float MIN_MOVE_SPEED = 4.0f;

        //-- Target information
        [SerializeField]
        private Transform mTargetTransform = null;
        private Vector3 mTargetPosition = Vector3.zero;

        [SerializeField] private bool mFollowing = true;

        // effect: camera will do the smooth movement
        [SerializeField] private bool mSmoothMoveX = true;
        [SerializeField] private bool mSmoothMoveY = true;

        //-- Scroll
        // [IMPOTATN]: 這個變量剛好是Camera到最邊邊的值! (Width)
        [SerializeField]
        [Range(MIN_ZOOM_DISTANCE, MAX_ZOOM_DISTANCE)]
        private float mCameraDepth = 10.0f;
        [SerializeField] private float mScrollRange = 2.0f;
        private float mWheelDegree = 0;

        //-- Move
        [SerializeField] private float mDampTime = 0.7f;
        [SerializeField] private float mMax_X_PositionInScene = 10.0f;
        [SerializeField] private float mMin_X_PositionInScene = -10.0f;
        [SerializeField] private float mMax_Y_PositionInScene = 10.0f;
        [SerializeField] private float mMin_Y_PositionInScene = -10.0f;
        [SerializeField] private float mOutOfScopeDampeTime = 0.4f;
        [SerializeField] private float mOutOfScopeDistanceX = 10;
        [SerializeField] private float mOutOfScopeDistanceY = 10;
        private float mRecordDampTime = 0;

        private Vector3 mRecordPosition = Vector3.zero;

        private Vector3 mPosition = Vector3.zero;
        [SerializeField] private Vector3 mVelocity = Vector3.zero;

        //-- Audio References
        private JCS_BGMPlayer mJCSBGMPlayer = null;


        //========================================
        //      setter / getter
        //------------------------------
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

            // set Execute order lower than "JCS_GameManager"
            JCS_GameManager.instance.SetJCSCamera(this);

            // record down the LD settings, in order
            // to change the damp time with out losing the 
            // LD setting!!!
            this.mRecordDampTime = mDampTime;
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

            this.mPosition = this.transform.position;
        }

        private void Update()
        {
            if (this.mTargetTransform == null)
                return;

            // Do the camera boundaries check!!
            CameraBoundaries();

            if (!mFollowing)
                return;

            // get the wheel value from the Unity API
            // (physical layer[mouse wheel] -> 
            // os layer[windows7] -> 
            // application layer[Unity itself]) -> 
            // to here...
            mWheelDegree = Input.GetAxis("Mouse ScrollWheel");
            ZoomCamera(mWheelDegree);

            // Check if target out of the distance
            CheckTargetOutOfScope();

            JCS_GameManager gm = JCS_GameManager.instance;
            Vector3 point = gm.GetJCSCamera().GetCamera().WorldToViewportPoint(mTargetTransform.position);
            Vector3 delta = mTargetTransform.position - gm.GetJCSCamera().GetCamera().ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z)); //(new Vector3(0.5, 0.5, point.z));
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
            this.transform.position = this.mPosition;
            this.mPosition += this.mVelocity * Time.deltaTime;
        }

        //========================================
        //      Self-Define
        //------------------------------

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

            mPosition.z = mTargetPosition.z - mCameraDepth;
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

    }
}
