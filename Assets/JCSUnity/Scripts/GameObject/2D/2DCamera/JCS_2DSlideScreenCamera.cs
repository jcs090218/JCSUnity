/**
 * $File: JCS_2DSlideScreenCamera.cs $
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
    /// Camera for GUI!! not in the game scene.
    ///
    /// use for switching scene panel.
    /// </summary>
    public class JCS_2DSlideScreenCamera
        : MonoBehaviour
    {
        /* Variables */

#if (UNITY_EDITOR)
        [Header("** Helper Variables (JCS_2DSlideScreenCamera) **")]

        public bool testWithKey = false;

        public KeyCode upKey = KeyCode.W;
        public KeyCode downKey = KeyCode.X;
        public KeyCode leftKey = KeyCode.A;
        public KeyCode rightKey = KeyCode.D;

        public KeyCode upLeftKey = KeyCode.Q;
        public KeyCode downLeftKey = KeyCode.Z;
        public KeyCode upRightKey = KeyCode.E;
        public KeyCode downRightKey = KeyCode.C;
#endif

        // Path that points to the panel.
        private string mPanelHolderPath = "JCSUnity_Framework_Resources/LevelDesignUI/JCS_SlideScreenPanelHolder";

        [Header("** Runtime Variables (JCS_2DSlideScreenCamera) **")]

        // Notice important that Designer should know what
        // Unity GUI type they are going to use!
        [Tooltip("GUI type.")]
        [SerializeField]
        private JCS_UnityGUIType mUnityGUIType = JCS_UnityGUIType.nGUI_3D;

        [Tooltip("Camera itself.")]
        [SerializeField]
        private JCS_2DCamera mJCS_2DCamera = null;

        [Tooltip("Slide screen panel holder for this camera.")]
        [SerializeField]
        private JCS_SlideScreenPanelHolder mPanelHolder = null;

        [Header("## Mobile")]

        [Tooltip("How sticky to the original of the panel's position.")]
        [Range(0.001f, 300.0f)]
        [SerializeField]
        private float mSlideStickiness = 60.0f;

        [Tooltip("Distance to slide over next scene on x axis.")]
        [Range(0.0f, 5000.0f)]
        [SerializeField]
        private float mSlideDistanceX = 5.0f;

        [Tooltip("Distance to slide over next scene on y axis.")]
        [Range(0.0f, 5000)]
        [SerializeField]
        private float mSlideDistanceY = 5.0f;

        [Tooltip("Freeze the x axis sliding action.")]
        [SerializeField]
        private bool mFreezeX = false;

        [Tooltip("Freeze the y axis sliding action.")]
        [SerializeField]
        private bool mFreezeY = false;

        [Header("## Sound")]

        [Tooltip("Sound when trigger switch scene.")]
        [SerializeField]
        private AudioClip mSwitchSceneSound = null;


        /* Setter & Getter */

        public JCS_SlideScreenPanelHolder PanelHolder { get { return this.mPanelHolder; } set { this.mPanelHolder = value; } }
        public void SetJCS2DCamera(JCS_2DCamera cam) { this.mJCS_2DCamera = cam; }
        public JCS_UnityGUIType UnityGUIType { get { return this.mUnityGUIType; } set { this.mUnityGUIType = value; } }
        public bool FreezeX { get { return this.mFreezeX; } set { this.mFreezeX = value; } }
        public bool FreezeY { get { return this.mFreezeY; } set { this.mFreezeY = value; } }
        public AudioClip SwitchSceneSound { get { return this.mSwitchSceneSound; } set { this.mSwitchSceneSound = value; } }

        /* Functions */

        private void Awake()
        {
            InitCamera();

            //
            if (mPanelHolder == null)
            {
                // spawn a default one!
                this.mPanelHolder = JCS_Utility.SpawnGameObject(
                    mPanelHolderPath,
                    transform.position,
                    transform.rotation).GetComponent<JCS_SlideScreenPanelHolder>();
            }
        }

        private void Update()
        {
#if (UNITY_EDITOR)
            Test();
#endif
            DoMobileSlide();
        }

#if (UNITY_EDITOR)
        private void Test()
        {
            if (!testWithKey)
                return;

            if (JCS_Input.GetKeyDown(upKey))
                SwitchScene(JCS_2D4Direction.TOP);
            if (JCS_Input.GetKeyDown(rightKey))
                SwitchScene(JCS_2D4Direction.RIGHT);
            if (JCS_Input.GetKeyDown(downKey))
                SwitchScene(JCS_2D4Direction.BOTTOM);
            if (JCS_Input.GetKeyDown(leftKey))
                SwitchScene(JCS_2D4Direction.LEFT);

            if (JCS_Input.GetKeyDown(upLeftKey))
                SwitchScene(JCS_2D8Direction.TOP_LEFT);
            if (JCS_Input.GetKeyDown(upRightKey))
                SwitchScene(JCS_2D8Direction.TOP_RIGHT);
            if (JCS_Input.GetKeyDown(downRightKey))
                SwitchScene(JCS_2D8Direction.BOTTOM_RIGHT);
            if (JCS_Input.GetKeyDown(downLeftKey))
                SwitchScene(JCS_2D8Direction.BOTTOM_LEFT);
        }
#endif

        /// <summary>
        /// Swicth the scene by sliding its with direction.
        /// </summary>
        /// <param name="towardDirection"></param>
        public void SwitchScene(JCS_2D4Direction towardDirection)
        {
            switch (mUnityGUIType)
            {
                case JCS_UnityGUIType.uGUI_2D:
                    UGUISwitchScene(towardDirection);
                    break;
                case JCS_UnityGUIType.nGUI_3D:
                    NGUISwitchScene(towardDirection);
                    break;
            }

            PlaySwitchSceneSound();
        }

        /// <summary>
        /// Swicth the scene by sliding its with direction.
        /// </summary>
        /// <param name="towardDirection"></param>
        public void SwitchScene(JCS_2D8Direction towardDirection)
        {
            switch (mUnityGUIType)
            {
                case JCS_UnityGUIType.uGUI_2D:
                    UGUISwitchScene(towardDirection);
                    break;
                case JCS_UnityGUIType.nGUI_3D:
                    NGUISwitchScene(towardDirection);
                    break;
            }

            PlaySwitchSceneSound();
        }

        /// <summary>
        /// Handle the mobile slide input.
        /// </summary>
        private void DoMobileSlide()
        {
            JCS_SlideInput si = JCS_InputManager.instance.GetJCSSlideInput();
            if (si == null)
                return;

            if (si.Touched)
            {
                mPanelHolder.EnableSlidePanels(false);

                Vector3 deltaPos = si.DeltaPos;

                if (mFreezeX) deltaPos.x = 0.0f;
                if (mFreezeY) deltaPos.y = 0.0f;

                mPanelHolder.DeltaMove(-deltaPos / mSlideStickiness);
            }
            else
            {
                mPanelHolder.EnableSlidePanels(true);
            }

            if (JCS_Input.GetMouseButtonUp(JCS_MouseButton.LEFT))
            {
                Vector3 posDiff = mPanelHolder.PositionDiff();

                if (!mFreezeX && posDiff.x > this.mSlideDistanceX)
                {
                    if (JCS_Mathf.isPositive(si.DragDisplacement.x))
                        SwitchScene(JCS_2D4Direction.LEFT);
                    else
                        SwitchScene(JCS_2D4Direction.RIGHT);
                }

                if (!mFreezeY && posDiff.y > this.mSlideDistanceY)
                {
                    if (JCS_Mathf.isPositive(si.DragDisplacement.y))
                        SwitchScene(JCS_2D4Direction.BOTTOM);
                    else
                        SwitchScene(JCS_2D4Direction.TOP);
                }
            }
        }
        
        /// <summary>
        /// Play switch scene sound.
        /// </summary>
        private void PlaySwitchSceneSound()
        {
            JCS_SoundPlayer sp = JCS_SoundManager.instance.GetGlobalSoundPlayer();
            sp.PlayOneShot(this.mSwitchSceneSound);
        }

        //////////// 2D //////////////////////////

        /// <summary>
        /// Iniialize the camera.
        /// </summary>
        private void InitCamera()
        {
            if (mJCS_2DCamera == null)
            {
                JCS_Debug.LogError("There is not JCS_2DCamera attach to, spawn a default one!");

                // Spawn a Default one!
                this.mJCS_2DCamera = JCS_Utility.SpawnGameObject(
                    JCS_2DCamera.JCS_2DCAMERA_PATH,
                    transform.position,
                    transform.rotation).GetComponent<JCS_2DCamera>();
            }

            // if still null, setting error!!
            if (mJCS_2DCamera == null)
            {
                JCS_Debug.LogError("The object spawn does not have the \"JCS_2DCamera\" components...");
                return;
            }

            // set target to follow!
            mJCS_2DCamera.SetFollowTarget(this.transform);
        }

        /// <summary>
        /// UGUI method switch the panel.
        /// </summary>
        /// <param name="towardDirection"> direction to switch scene. </param>
        private void UGUISwitchScene(JCS_2D4Direction towardDirection)
        {
            // get the Screen Width and Screen Height
            JCS_ScreenSettings ss = JCS_ScreenSettings.instance;

            float screenWidth = ss.STARTING_SCREEN_WIDTH;
            float screenHeight = ss.STARTING_SCREEN_HEIGHT;

            // make a copy of old position
            Vector3 newScenePosition = Vector3.zero;

            switch (towardDirection)
            {
                case JCS_2D4Direction.TOP:
                    newScenePosition.y += screenHeight;
                    break;
                case JCS_2D4Direction.BOTTOM:
                    newScenePosition.y -= screenHeight;
                    break;
                case JCS_2D4Direction.RIGHT:
                    newScenePosition.x += screenWidth;
                    break;
                case JCS_2D4Direction.LEFT:
                    newScenePosition.x -= screenWidth;
                    break;
            }

            mPanelHolder.AddForce(newScenePosition);
        }

        /// <summary>
        /// UGUI method switch the panel.
        /// </summary>
        /// <param name="towardDirection"> direction to switch scene. </param>
        private void UGUISwitchScene(JCS_2D8Direction towardDirection)
        {
            // get the Screen Width and Screen Height
            JCS_ScreenSettings ss = JCS_ScreenSettings.instance;

            float screenWidth = ss.STARTING_SCREEN_WIDTH;
            float screenHeight = ss.STARTING_SCREEN_HEIGHT;

            // make a copy of old position
            Vector3 newScenePosition = Vector3.zero;

            switch (towardDirection)
            {
                case JCS_2D8Direction.TOP:
                    newScenePosition.y += screenHeight;
                    break;
                case JCS_2D8Direction.BOTTOM:
                    newScenePosition.y -= screenHeight;
                    break;
                case JCS_2D8Direction.RIGHT:
                    newScenePosition.x += screenWidth;
                    break;
                case JCS_2D8Direction.LEFT:
                    newScenePosition.x -= screenWidth;
                    break;

                // 4 corners
                case JCS_2D8Direction.TOP_LEFT:
                    newScenePosition.y += screenHeight;
                    newScenePosition.x -= screenWidth;
                    break;
                case JCS_2D8Direction.TOP_RIGHT:
                    newScenePosition.y += screenHeight;
                    newScenePosition.x += screenWidth;
                    break;
                case JCS_2D8Direction.BOTTOM_RIGHT:
                    newScenePosition.y -= screenHeight;
                    newScenePosition.x += screenWidth;
                    break;
                case JCS_2D8Direction.BOTTOM_LEFT:
                    newScenePosition.y -= screenHeight;
                    newScenePosition.x -= screenWidth;
                    break;
            }

            mPanelHolder.AddForce(newScenePosition);
        }

        //////////// 3D //////////////////////////

        /// <summary>
        /// NGUI method to switch panel/scene.
        /// </summary>
        /// <param name="towardDirection"> direction to switch scene. </param>
        private void NGUISwitchScene(JCS_2D4Direction towardDirection)
        {
            // get the Screen Width and Screen Height
            float screenWidth = JCS_Camera.main.CamRectSize.x;
            float screenHeight = JCS_Camera.main.CamRectSize.y;

            // make a copy of old position
            Vector3 newScenePosition = this.transform.position;

            // apply new position and set to its
            // position according to the direction
            // programmer pass in.
            switch (towardDirection)
            {
                case JCS_2D4Direction.TOP:
                    newScenePosition.y += screenHeight;
                    break;
                case JCS_2D4Direction.BOTTOM:
                    newScenePosition.y -= screenHeight;
                    break;
                case JCS_2D4Direction.RIGHT:
                    newScenePosition.x += screenWidth;
                    break;
                case JCS_2D4Direction.LEFT:
                    newScenePosition.x -= screenWidth;
                    break;
            }

            // set this position to new position
            // so the camera will follow this object!
            this.transform.position = newScenePosition;
        }

        /// <summary>
        /// NGUI method to switch panel/scene.
        /// </summary>
        /// <param name="towardDirection"> direction to switch scene. </param>
        private void NGUISwitchScene(JCS_2D8Direction towardDirection)
        {
            // get the Screen Width and Screen Height
            float screenWidth = JCS_Camera.main.CamRectSize.x;
            float screenHeight = JCS_Camera.main.CamRectSize.y;

            // make a copy of old position
            Vector3 newScenePosition = this.transform.position;

            // apply new position and set to its
            // position according to the direction
            // programmer pass in.
            switch (towardDirection)
            {
                case JCS_2D8Direction.CENTER:
                    // do nothing
                    return;

                // 4 sides
                case JCS_2D8Direction.TOP:
                    newScenePosition.y += screenHeight;
                    break;
                case JCS_2D8Direction.BOTTOM:
                    newScenePosition.y -= screenHeight;
                    break;
                case JCS_2D8Direction.RIGHT:
                    newScenePosition.x += screenWidth;
                    break;
                case JCS_2D8Direction.LEFT:
                    newScenePosition.x -= screenWidth;
                    break;

                // 4 corners
                case JCS_2D8Direction.TOP_LEFT:
                    newScenePosition.y += screenHeight;
                    newScenePosition.x -= screenWidth;
                    break;
                case JCS_2D8Direction.TOP_RIGHT:
                    newScenePosition.y += screenHeight;
                    newScenePosition.x += screenWidth;
                    break;
                case JCS_2D8Direction.BOTTOM_RIGHT:
                    newScenePosition.y -= screenHeight;
                    newScenePosition.x += screenWidth;
                    break;
                case JCS_2D8Direction.BOTTOM_LEFT:
                    newScenePosition.y -= screenHeight;
                    newScenePosition.x -= screenWidth;
                    break;
            }

            // set this position to new position
            // so the camera will follow this object!
            this.transform.position = newScenePosition;
        }
    }
}
