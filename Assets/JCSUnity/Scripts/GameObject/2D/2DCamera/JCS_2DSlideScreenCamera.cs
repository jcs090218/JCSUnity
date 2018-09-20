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

        //----------------------
        // Public Variables
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

        //----------------------
        // Private Variables

        [Header("** Runtime Variables (JCS_2DSlideScreenCamera) **")]

        // Notice important that Designer should know what 
        // Unity GUI type they are going to use!
        [Tooltip("")]
        [SerializeField]
        private JCS_UnityGUIType mUnityGUIType = JCS_UnityGUIType.nGUI_3D;


        [Header("** Please set a 2DCamera from the asset, or it will spawn one if is null! **")]
        [Tooltip("")]

        [SerializeField]
        private JCS_2DCamera mJCS_2DCamera = null;


        [Header("** Please set a JCS_SlideScreenPanelHolder from the asset, or it will spawn one if is null! **")]

        [Tooltip("")]
        [SerializeField]
        private JCS_SlideScreenPanelHolder mPanelHolder = null;
        private string mPanelHolderPath = "JCSUnity_Framework_Resources/JCS_LevelDesignUI/JCS_SlideScreenPanelHolder";

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------
        public JCS_SlideScreenPanelHolder PanelHolder { get { return this.mPanelHolder; } set { this.mPanelHolder = value; } }
        public void SetJCS2DCamera(JCS_2DCamera cam) { this.mJCS_2DCamera = cam; }
        public JCS_UnityGUIType UnityGUIType { get { return this.mUnityGUIType; } set { this.mUnityGUIType = value; } }

        //========================================
        //      Unity's function
        //------------------------------
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

        private void Start()
        {
            GetInitGUIScreenSize();
        }

        private void Update()
        {
#if (UNITY_EDITOR)
            Test();
#endif

            if (mPanelHolder != null)
            {
                JCS_SlideInput si = JCS_InputManager.instance.GetJCSSlideInput();
                if (si != null)
                {
                    mPanelHolder.AddForce(-si.DeltaPos.x, JCS_Axis.AXIS_X);
                    mPanelHolder.AddForce(-si.DeltaPos.y, JCS_Axis.AXIS_Y);
                }
            }
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

        //========================================
        //      Self-Define
        //------------------------------
        //----------------------
        // Public Functions

        /// <summary>
        /// Switch the panel.
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
        }

        /// <summary>
        /// Swicth the scene by sliding it with direction.
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
        }

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions

        //////////// 2D //////////////////////////

        /// <summary>
        /// Iniialize the camera.
        /// </summary>
        private void InitCamera()
        {
            if (mJCS_2DCamera == null)
            {
                JCS_Debug.LogError("JCS_2DSlideScreenCamera",   "There is not JCS_2DCamera attach to, spawn a default one!");

                // Spawn a Default one!
                this.mJCS_2DCamera = JCS_Utility.SpawnGameObject(
                    JCS_2DCamera.JCS_2DCAMERA_PATH,
                    transform.position,
                    transform.rotation).GetComponent<JCS_2DCamera>();
            }

            // if still null, setting error!!
            if (mJCS_2DCamera == null)
            {
                JCS_Debug.LogError("JCS_2DMultiTrackCamera",   "The object spawn does not have the \"JCS_2DCamera\" components...");
                return;
            }

            // set target to follow!
            mJCS_2DCamera.SetFollowTarget(this.transform);
        }

        /// <summary>
        /// According to Unity's low level code architecture,
        /// Unity Engine itself start handle the screen size resolution
        /// only during runtime, so we just have to get the screen size once
        /// for GUI(UGUI) switch scene functions.
        /// </summary>
        private void GetInitGUIScreenSize()
        {
            RectTransform appRect = JCS_Canvas.instance.GetAppRect();
            Vector2 appScreenSize = appRect.sizeDelta;
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
