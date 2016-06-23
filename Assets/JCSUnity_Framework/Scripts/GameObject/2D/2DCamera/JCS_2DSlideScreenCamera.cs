/**
 * $File: JCS_2DSlideScreenCamera.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information $
 *		                Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;

namespace JCSUnity
{
    /// <summary>
    /// Camera for GUI!! not in the game scene.
    /// </summary>
    [RequireComponent(typeof(AudioListener))]
    public class JCS_2DSlideScreenCamera 
        : MonoBehaviour
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables
        private AudioListener mAudioListener = null;

        // NOTE(JenChieh): according to Unity's low level architecture
        //                Canvas size will change 
        private Vector2 mRecordGUIScreenSize = Vector2.one;


        [Header("** Runtime Variables **")]
        // Notice important that Designer should know what 
        // Unity GUI type they are going to use!
        [SerializeField]
        private JCS_UnityGUIType mUnityGUIType = JCS_UnityGUIType.nGUI_3D;

        [Header("** Please set a 2DCamera from the asset, or it will spawn one if is null! **")]
        [SerializeField] private JCS_2DCamera mJCS_2DCamera = null;

        [Header("** Please set a JCS_SlideScreenPanelHolder from the asset, or it will spawn one if is null! **")]
        [SerializeField] private JCS_SlideScreenPanelHolder mPanelHolder = null;
        private string mPanelHolderPath = "JCSUnity_Framework_Resources/JCS_LevelDesignUI/JCS_SlideScreenPanelHolder";

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------
        public AudioListener GetAudioListener() { return this.mAudioListener; }

        //========================================
        //      Unity's function
        //------------------------------
        private void Awake()
        {
            mAudioListener = this.GetComponent<AudioListener>();

            InitCamera();

            // 
            if (mPanelHolder == null)
            {
                // spawn a default one!
                this.mPanelHolder = JCS_UsefualFunctions.SpawnGameObject(
                    mPanelHolderPath,
                    transform.position,
                    transform.rotation).GetComponent<JCS_SlideScreenPanelHolder>();
            }
        }

        private void Start()
        {
            JCS_SoundManager.instance.SetAudioListener(GetAudioListener());

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
            if (JCS_Input.GetKeyDown(KeyCode.W))
                SwitchScene(JCS_2D4Direction.TOP);
            if (JCS_Input.GetKeyDown(KeyCode.D))
                SwitchScene(JCS_2D4Direction.RIGHT);
            if (JCS_Input.GetKeyDown(KeyCode.X))
                SwitchScene(JCS_2D4Direction.BOTTOM);
            if (JCS_Input.GetKeyDown(KeyCode.A))
                SwitchScene(JCS_2D4Direction.LEFT);

            if (JCS_Input.GetKeyDown(KeyCode.Q))
                SwitchScene(JCS_2D8Direction.TOP_LEFT);
            if (JCS_Input.GetKeyDown(KeyCode.E))
                SwitchScene(JCS_2D8Direction.TOP_RIGHT);
            if (JCS_Input.GetKeyDown(KeyCode.C))
                SwitchScene(JCS_2D8Direction.BOTTOM_RIGHT);
            if (JCS_Input.GetKeyDown(KeyCode.Z))
                SwitchScene(JCS_2D8Direction.BOTTOM_LEFT);
        }
#endif

        //========================================
        //      Self-Define
        //------------------------------
        //----------------------
        // Public Functions
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
        // 2D
        private void InitCamera()
        {
            if (mJCS_2DCamera == null)
            {
                JCS_GameErrors.JcsErrors("JCS_2DSlideScreenCamera", -1, "There is not JCS_2DCamera attach to, spawn a default one!");

                // Spawn a Default one!
                this.mJCS_2DCamera = JCS_UsefualFunctions.SpawnGameObject(
                    JCS_2DCamera.JCS_2DCAMERA_PATH,
                    transform.position,
                    transform.rotation).GetComponent<JCS_2DCamera>();
            }

            // if still null, setting error!!
            if (mJCS_2DCamera == null)
            {
                JCS_GameErrors.JcsErrors("JCS_2DMultiTrackCamera", -1, "The object spawn does not have the \"JCS_2DCamera\" components...");
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

            // get the gui screen size the first time
            mRecordGUIScreenSize = appScreenSize;
        }
        private void UGUISwitchScene(JCS_2D4Direction towardDirection)
        {
            // get the Screen Width and Screen Height
            Vector2 appScreenSize = mRecordGUIScreenSize;
            float screenWidth = appScreenSize.x;
            float screenHeight = appScreenSize.y;

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
        private void UGUISwitchScene(JCS_2D8Direction towardDirection)
        {
            // get the Screen Width and Screen Height
            Vector2 appScreenSize = mRecordGUIScreenSize;
            float screenWidth = appScreenSize.x;
            float screenHeight = appScreenSize.y;

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
        // 3D
        private void NGUISwitchScene(JCS_2D4Direction towardDirection)
        {
            RectTransform appRect = JCS_Canvas.instance.GetAppRect();

            // get the Screen Width and Screen Height
            Vector2 appScreenSize = appRect.sizeDelta;
            float screenWidth = appScreenSize.x * appRect.localScale.x;
            float screenHeight = appScreenSize.y * appRect.localScale.y;

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
        private void NGUISwitchScene(JCS_2D8Direction towardDirection)
        {
            RectTransform appRect = JCS_Canvas.instance.GetAppRect();

            // get the Screen Width and Screen Height
            Vector2 appScreenSize = appRect.sizeDelta;
            float screenWidth = appScreenSize.x * appRect.localScale.x;
            float screenHeight = appScreenSize.y * appRect.localScale.y;

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
