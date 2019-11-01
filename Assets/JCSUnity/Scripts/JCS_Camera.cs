/**
 * $File: JCS_Camera.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;
using System.IO;

namespace JCSUnity
{
    /// <summary>
    /// Camera class for JCSUnity framework.
    /// </summary>
    [RequireComponent(typeof(Camera))]
    public abstract class JCS_Camera 
        : MonoBehaviour
    {
        public static JCS_Camera main = null;

        // Unity's camera object to handle.
        protected Camera mCamera = null;


        [Header("** Check Variables (JCS_Camera) **")]

        [SerializeField]
        protected Vector3 mVelocity = Vector3.zero;

        
        [Header("** Initialize Variables (JCS_Camera) **")]

        [Tooltip("Distance as game origin depth.")]
        [SerializeField]
        protected float mGameDepth = 0;

        [SerializeField]
        protected bool mDisplayGameDepthCamera = false;

        [SerializeField]
        protected Color mGameCamColor = Color.white;


        [Header("** Runtime Variables (JCS_Camera) **")]

        [SerializeField]
        protected bool mFollowing = true;

        // x = width, y = height, z = 0 (no involve in depth.)
        protected Vector3 mCamRectSize = Vector3.one;

        // x = left, y = top, width = right, height = bottom.
        // TopLeft -> BottomLeft
        protected Rect mCamRect = new Rect();

        [Tooltip("Offset the camera position from its' original position.")]
        [SerializeField]
        protected Vector3 mPositionOffset = Vector3.zero;

        // Record any necessary data, for resize screen event.
        protected bool mSceneJustLoad = true;

        // Record down the camera data, orthographic size.
        protected float mRecordOrthographicSize = 0.0f;

        // Record down the camera data, filed of view.
        protected float mRecordFieldOfView = 0.0f;


        //========================================
        //      setter / getter
        //------------------------------
        public Vector3 PositionOffset { get { return this.mPositionOffset; } }
        public Vector3 CamRectSize { get { return this.mCamRectSize; } set { this.mCamRectSize = value; } }
        public Rect CamRect { get { return this.mCamRect; } }
        public Camera GetCamera() { return this.mCamera; }
        public float fieldOfView { get { return this.mCamera.fieldOfView; } set { this.mCamera.fieldOfView = value; } }
        public Vector3 Velocity { get { return this.mVelocity; } set { this.mVelocity = value; } }
        public bool Following { get { return this.mFollowing; } set { this.mFollowing = value; } }

        /* Get/Set the target this camera follows. */
        public abstract void SetFollowTarget(Transform trans);
        public abstract Transform GetFollowTarget();

        //========================================
        //      Unity's function
        //------------------------------

        protected virtual void Awake()
        {
            main = this;

            this.mCamera = this.GetComponent<Camera>();
        }

        protected virtual void Start()
        {
            // Record down the camera data.
            mRecordOrthographicSize = mCamera.orthographicSize;
            mRecordFieldOfView = mCamera.fieldOfView;

            // add to on screen resize callback.
            JCS_ScreenSettings.instance.onScreenResize += OnResizeGame;
        }

        protected virtual void LateUpdate()
        {
#if (UNITY_EDITOR)
            DisplayGameDepthCamera();
#endif
        }

        //========================================
        //      Self-Define
        //------------------------------

        /// <summary>
        /// Weather if we want to take a screen shot, call
        /// this function will do the trick!
        /// </summary>
        public virtual void TakeScreenShot()
        {
#if (UNITY_STANDALONE || UNITY_EDITOR)
            /**
             * Data path info:
             *      - http://docs.unity3d.com/ScriptReference/Application-dataPath.html
             */


            // get the last saved screen shot's index
            int last_saved_index = SearchDirectory(
                Application.dataPath +
                JCS_GameSettings.instance.SCREENSHOT_PATH,
                JCS_GameSettings.instance.SCREENSHOT_FILENAME) + 1;

            ScreenCapture.CaptureScreenshot(
                Application.dataPath +
                JCS_GameSettings.instance.SCREENSHOT_PATH +
                JCS_GameSettings.instance.SCREENSHOT_FILENAME +
                last_saved_index +
                JCS_GameSettings.instance.SAVED_IMG_EXTENSION);
#endif
        }

        /// <summary>
        /// Method to do search directory and check to see 
        /// image index that are already exist.
        /// </summary>
        /// <param name="path"> path to search index. </param>
        /// <param name="removeStr">  </param>
        /// <returns></returns>
        public virtual int SearchDirectory(string path, string removeStr)
        {
            // if Directory does not exits, create it prevent error!
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            string fileName = "";
            string ext = "";
            int last_saved_screenshot = 0;
            foreach (string file in Directory.GetFiles(path))
            {
                fileName = Path.GetFileNameWithoutExtension(file);
                ext = Path.GetExtension(file);

                // check if is the .png file 
                // (screen shot can only be image file)
                if (!ext.Equals(".png"))
                    continue;

                int index = fileName.IndexOf(removeStr);
                int len = removeStr.Length;
                string startOfString = fileName.Substring(0, index);
                string endOfString = fileName.Substring(index + len);
                string cleanPath = startOfString + endOfString;

                //print(cleanPath);

                last_saved_screenshot = System.Int32.Parse(cleanPath);
            }

            return last_saved_screenshot;
        }

#if (UNITY_EDITOR)
        /// <summary>
        /// Square inside the game editor screen. Display 
        /// the screen width and height in current game depth.
        /// 2D Game probably need this. 3D Game is optional.
        /// </summary>
        private void DisplayGameDepthCamera()
        {
            if (!mDisplayGameDepthCamera)
                return;

            // Next step: find camera 4 bounds.
            Camera cam = JCS_Camera.main.GetCamera();

            JCS_Canvas jcsCanvas = JCS_Canvas.instance;

            Vector3 camPos = cam.transform.position;
            // only need to know the depth.
            {
                camPos.x = 0;
                camPos.y = 0;
            }
            Vector3 canvasPos = JCS_Canvas.instance.transform.position;
            // only need to know the depth.
            {
                canvasPos.x = 0;
                canvasPos.y = 0;
            }
            float camToCanvasDistance = Vector3.Distance(camPos, canvasPos);

            Vector3 gameDepth = new Vector3(0, mGameDepth, 0);
            float camToGameDepthDistance = Vector3.Distance(camPos, gameDepth);

            //print("To Game depth Distance: " + camToGameDepthDistance);
            //print("To Cavas Distnace: " + camToCanvasDistance);

            Vector2 canvasRect = jcsCanvas.GetAppRect().sizeDelta;
            // transfer rect from screen space to world space
            {
                canvasRect.x *= jcsCanvas.GetAppRect().localScale.x;
                canvasRect.y *= jcsCanvas.GetAppRect().localScale.y;
            }

            mCamRectSize = new Vector3(
                camToGameDepthDistance * canvasRect.x / camToCanvasDistance,
                camToGameDepthDistance * canvasRect.y / camToCanvasDistance,
                0);

            // camPos name are named up there.
            // cannot name the same.
            Vector3 cCamPos = cam.transform.position;

            float camTopBound = cCamPos.y + mCamRectSize.y / 2;
            float camBotBound = cCamPos.y - mCamRectSize.y / 2;
            float camRightBound = cCamPos.x + mCamRectSize.x / 2;
            float camLeftBound = cCamPos.x - mCamRectSize.x / 2;

            // top left -> bot right
            mCamRect.x = camLeftBound;
            mCamRect.y = camTopBound;
            mCamRect.width = camRightBound;
            mCamRect.height = camBotBound;

            Vector3 topLeft = cam.transform.position;
            topLeft.x -= mCamRectSize.x / 2;
            topLeft.y += mCamRectSize.y / 2;

            Vector3 topRight = cam.transform.position;
            topRight.x += mCamRectSize.x / 2;
            topRight.y += mCamRectSize.y / 2;

            Vector3 botLeft = cam.transform.position;
            botLeft.x -= mCamRectSize.x / 2;
            botLeft.y -= mCamRectSize.y / 2;

            Vector3 botRight = cam.transform.position;
            botRight.x += mCamRectSize.x / 2;
            botRight.y -= mCamRectSize.y / 2;

            // set depth to the same
            topLeft.z = mGameDepth;
            topRight.z = mGameDepth;
            botLeft.z = mGameDepth;
            botRight.z = mGameDepth;

            // Draw the box
            JCS_Debug.DrawRect(topLeft, topRight, botRight, botLeft, mGameCamColor);

        }
#endif

        /// <summary>
        /// Check weather the "type" in the screen space. (Character Controller)
        /// </summary>
        /// <param name="cap"> character controller to check. (Collider) </param>
        /// <returns>
        /// true: in screen space, 
        /// false: not in screen space
        /// </returns>
        public bool CheckInScreenSpace(CharacterController cap)
        {
            // First Step: Find CharacterController's 4 bounds.
            Vector3 capScale = cap.transform.localScale;

            capScale = JCS_Mathf.AbsoluteValue(capScale);

            Vector3 cCenter = new Vector3(
                cap.center.x * capScale.x,
                cap.center.y * capScale.y,
                cap.center.z * capScale.z);

            float cR = cap.radius * capScale.x;
            float cH = (cap.height - (cap.radius * 2.0f)) * capScale.y;

            if (cH < 0)
                cH = 0;

            float cTopBound = cap.transform.position.y + cCenter.y + (cH / 2.0f) + cR;
            float cBotBound = cap.transform.position.y + cCenter.y - (cH / 2.0f) - cR;
            float cRightBound = cap.transform.position.x + cCenter.x + cR;
            float cLeftBound = cap.transform.position.x + cCenter.x - cR;

#if (UNITY_EDITOR)
            Vector3 cOrigin = cap.transform.position + cCenter;
            Debug.DrawLine(cOrigin, 
                new Vector3(cOrigin.x, cTopBound, cOrigin.z));
            Debug.DrawLine(cOrigin,
                new Vector3(cOrigin.x, cBotBound, cOrigin.z));
            Debug.DrawLine(cOrigin,
                new Vector3(cRightBound, cOrigin.y, cOrigin.z));
            Debug.DrawLine(cOrigin,
                new Vector3(cLeftBound, cOrigin.y, cOrigin.z));
#endif

            // Next step: find camera 4 bounds.
            Camera cam = JCS_Camera.main.GetCamera();

            JCS_Canvas jcsCanvas = JCS_Canvas.instance;

            Vector3 camPos = cam.transform.position;
            // only need to know the depth.
            {
                camPos.x = 0;
                camPos.y = 0;
            }
            Vector3 canvasPos = jcsCanvas.transform.position;
            // only need to know the depth.
            {
                canvasPos.x = 0;
                canvasPos.y = 0;
            }
            float camToCanvasDistance = Vector3.Distance(camPos, canvasPos);

            Vector3 gameDepth = new Vector3(0, cap.transform.position.z, 0);
            float camToGameDepthDistance = Vector3.Distance(camPos, gameDepth);

            //print("To Game depth Distance: " + camToGameDepthDistance);
            //print("To Cavas Distnace: " + camToCanvasDistance);

            Vector2 canvasRect = jcsCanvas.GetAppRect().sizeDelta;
            // transfer rect from screen space to world space
            {
                canvasRect.x *= jcsCanvas.GetAppRect().localScale.x;
                canvasRect.y *= jcsCanvas.GetAppRect().localScale.y;
            }

            Vector3 gameRect = new Vector3(
                camToGameDepthDistance * canvasRect.x / camToCanvasDistance,
                camToGameDepthDistance * canvasRect.y / camToCanvasDistance, 
                0);

            // camPos name are named up there.
            // cannot name the same.
            Vector3 cCamPos = cam.transform.position;

            float camTopBound = cCamPos.y + gameRect.y / 2;
            float camBotBound = cCamPos.y - gameRect.y / 2;
            float camRightBound = cCamPos.x + gameRect.x / 2;
            float camLeftBound = cCamPos.x - gameRect.x / 2;

#if (UNITY_EDITOR)
            Vector3 topLeft = cam.transform.position;
            topLeft.x -= gameRect.x / 2;
            topLeft.y += gameRect.y / 2;

            Vector3 topRight = cam.transform.position;
            topRight.x += gameRect.x / 2;
            topRight.y += gameRect.y / 2;

            Vector3 botLeft = cam.transform.position;
            botLeft.x -= gameRect.x / 2;
            botLeft.y -= gameRect.y / 2;

            Vector3 botRight = cam.transform.position;
            botRight.x += gameRect.x / 2;
            botRight.y -= gameRect.y / 2;

            // set depth to the same
            topLeft.z = mGameDepth;
            topRight.z = mGameDepth;
            botLeft.z = mGameDepth;
            botRight.z = mGameDepth;

            // Draw the box
            JCS_Debug.DrawRect(topLeft, topRight, botRight, botLeft);
#endif

            if (cRightBound < camLeftBound ||
                camRightBound < cLeftBound || 
                cTopBound < camBotBound ||
                camTopBound < cBotBound)
            {
                // no in the screen
                return false;
            }

            // in screen
            return true;
        }

        /// <summary>
        /// Check weather the "type" in the screen space. (Sprite Renderer)
        /// </summary>
        /// <param name="checkTrans"> sprite renderer to check. (Sprite width & height) </param>
        /// <returns>
        /// true: in screen space, 
        /// false: not in screen space
        /// </returns>
        public bool CheckInScreenSpace(SpriteRenderer checkTrans)
        {
            Vector2 objectRect = JCS_Utility.GetSpriteRendererRect(checkTrans);

            Camera cam = JCS_Camera.main.GetCamera();
            Vector2 objPos = cam.WorldToViewportPoint(checkTrans.transform.position);
            Vector2 camPos = cam.WorldToViewportPoint(cam.transform.position);

            float objLeft = objPos.x - (objectRect.x / 2);
            float objRight = objPos.x + (objectRect.x / 2);
            float objTop = objPos.y + (objectRect.y / 2);
            float objBot = objPos.y - (objectRect.y / 2);

            RectTransform appRect = JCS_Canvas.instance.GetAppRect();

            float camWidth = appRect.sizeDelta.x;
            float camHeight = appRect.sizeDelta.y;

            float camLeft = camPos.x - (camWidth / 2);
            float camRight = camPos.x + (camWidth / 2);
            float camTop = camPos.y + (camHeight / 2);
            float camBot = camPos.y - (camHeight / 2);

#if (UNITY_EDITOR)
            Vector3 topLeft = new Vector3(objLeft, objTop, 0);
            Vector3 topRight = new Vector3(objRight, objTop, 0);
            Vector3 botRight = new Vector3(objRight, objBot, 0);
            Vector3 botLeft = new Vector3(objLeft, objBot, 0);

            Debug.DrawLine(topLeft, topRight);
            Debug.DrawLine(topLeft, botLeft);
            Debug.DrawLine(botRight, botLeft);
            Debug.DrawLine(topRight, botRight);
#endif

            // TODO(JenChieh): Not done.

            if ((objRight < camLeft || objLeft > camRight) && 
                (objTop < camBot || objBot > camTop))
            {
                // out of screen.
                return false;
            }

            return true;
        }

        /// <summary>
        /// Check weather the "type" in the screen space. (Transform)
        /// </summary>
        /// <param name="checkTrans"></param>
        /// <returns></returns>
        public bool CheckInScreenSpace(Transform checkTrans)
        {
            // TODO(JenChieh): continue finish the function?

            return true;
        }

        /// <summary>
        /// Check weather the "type" in the screen space. (RectTransform)
        /// </summary>
        /// <param name="checkTrans"> rect transform's size, etc. </param>
        /// <returns>
        /// true: in screen space, 
        /// false: not in screen space
        /// </returns>
        public bool CheckInScreenSpace(RectTransform checkTrans)
        {
            Vector2 rectSize = checkTrans.sizeDelta;
            Vector3 panelPos = checkTrans.localPosition;

            float halfSlotWidth = rectSize.x / 2 * checkTrans.localScale.x;
            float halfSlotHeight = rectSize.y / 2 * checkTrans.localScale.y;

            float panelLeftBorder = panelPos.x - halfSlotWidth;
            float panelRightBorder = panelPos.x + halfSlotWidth;
            float panelTopBorder = panelPos.y + halfSlotHeight;
            float panelBottomBorder = panelPos.y - halfSlotHeight;

            Camera cam = JCS_Camera.main.GetCamera();
            Vector3 camPos = cam.transform.position;
            // Transfer 3D space to 2D space
            Vector2 camPosToScreen = cam.WorldToScreenPoint(camPos);

            // Get application rect
            RectTransform appRect = JCS_Canvas.instance.GetAppRect();
            Vector2 screenRect = appRect.sizeDelta;

            float camLeftBorder = camPosToScreen.x - screenRect.x / 2;
            float camRightBorder = camPosToScreen.x + screenRect.x / 2;
            float camTopBorder = camPosToScreen.y + screenRect.y / 2;
            float camBottomBorder = camPosToScreen.y - screenRect.y / 2; ;

            if (panelRightBorder - rectSize.x > camRightBorder ||
                panelLeftBorder + rectSize.x < camLeftBorder ||
                panelTopBorder - rectSize.y > camTopBorder ||
                panelBottomBorder + rectSize.y < camBottomBorder)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Transfer world space to canvas space.
        /// Source -> http://answers.unity3d.com/questions/799616/unity-46-beta-19-how-to-convert-from-world-space-t.html
        /// </summary>
        /// <param name="targetWorldPos"> world position to transfer. </param>
        /// <returns> Canvas space position </returns>
        public Vector2 WorldToCanvasSpace(Vector3 targetWorldPos)
        {
            Camera cam = GetCamera();

            //first you need the RectTransform component of your canvas
            RectTransform canvasRect = JCS_Canvas.instance.GetAppRect();

            //then you calculate the position of the UI element

            // 0,0 for the canvas is at the center of the screen, 
            // whereas WorldToViewPortPoint treats 
            // the lower left corner as 0,0. Because of 
            // this, you need to subtract the height / width of 
            // the canvas * 0.5 to get the correct position.

            Vector3 viewportPos = cam.WorldToViewportPoint(targetWorldPos);

            Vector2 worldObject_ScreenPosition = new Vector2(
                ((viewportPos.x * canvasRect.sizeDelta.x) - (canvasRect.sizeDelta.x * 0.5f)),
                ((viewportPos.y * canvasRect.sizeDelta.y) - (canvasRect.sizeDelta.y * 0.5f)));

            //now you can set the position to the ui element
            return worldObject_ScreenPosition;
        }

        /// <summary>
        /// Transfer canvas space to world space.
        /// 
        /// NOTE(jenchieh): this dont often use, cuz world space element
        /// dont often follow the ui element.
        /// </summary>
        /// <param name="targetCanvasPos"> canvas position to transfer. </param>
        /// <returns> world space position </returns>
        public Vector3 CanvasToWorldSpace(Vector2 targetCanvasPos)
        {
            Camera cam = GetCamera();

            //first you need the RectTransform component of your canvas
            RectTransform canvasRect = JCS_Canvas.instance.GetAppRect();

            Vector2 canvasObject_WorldPosition = new Vector2(
                ((targetCanvasPos.x + (canvasRect.sizeDelta.x * 0.5f)) / canvasRect.sizeDelta.x),
                ((targetCanvasPos.y + (canvasRect.sizeDelta.y * 0.5f)) / canvasRect.sizeDelta.y));

            Vector3 worldPos = cam.ViewportToWorldPoint(canvasObject_WorldPosition);

            //now you can set the position to the world element
            return worldPos;
        }

        /// <summary>
        /// Set the position (float x, y)
        /// Position offset added already.
        /// </summary>
        /// <param name="xPos"> x position </param>
        /// <param name="yPos"> y position </param>
        public void SetPosition(float xPos, float yPos)
        {
            Vector3 newPos = this.transform.position;
            newPos.x = xPos + mPositionOffset.x;
            newPos.y = yPos + mPositionOffset.y;
            this.transform.position = newPos;
        }

        /// <summary>
        /// Set the position (Vector2)
        /// Position offset added already.
        /// </summary>
        /// <param name="vec"> position x axis </param>
        public void SetPosition(Vector2 vec)
        {
            SetPosition(vec.x, vec.y);
        }

        /// <summary>
        /// Set the position (float x, y, z)
        /// Position offset added already.
        /// </summary>
        /// <param name="xPos"> x position </param>
        /// <param name="yPos"> y position </param>
        /// <param name="zPos"> z position </param>
        public void SetPosition(float xPos, float yPos, float zPos)
        {
            Vector3 newPos = this.transform.position;
            newPos.x = xPos + mPositionOffset.x;
            newPos.y = yPos + mPositionOffset.y;
            newPos.z = zPos + mPositionOffset.z;
            this.transform.position = newPos;
        }

        /// <summary>
        /// Set the position (Vector3)
        /// Position offset added already.
        /// </summary>
        /// <param name="vec"> position 3 axis </param>
        public void SetPosition(Vector3 vec)
        {
            SetPosition(vec.x, vec.y, vec.z);
        }


        /// <summary>
        /// Resize the game if screen size changes.
        /// </summary>
        protected virtual void OnResizeGame()
        {
            JCS_ScreenSettings ss = JCS_ScreenSettings.instance;

            float currentScreenRatio = ss.CURRENT_SCREEN_WIDTH / ss.CURRENT_SCREEN_HEIGHT;
            float startingScreenRatio = (float)ss.STARTING_SCREEN_WIDTH / (float)ss.STARTING_SCREEN_HEIGHT;

            if (currentScreenRatio > startingScreenRatio)
            {
                // Set the limit if reach the starting screen ratio.
                ss.CURRENT_SCREEN_WIDTH = (float)ss.STARTING_SCREEN_WIDTH;
                ss.CURRENT_SCREEN_HEIGHT = (float)ss.STARTING_SCREEN_HEIGHT;
            }

            float prevRatio = ss.PREV_SCREEN_WIDTH / ss.PREV_SCREEN_HEIGHT;
            float newRatio = ss.CURRENT_SCREEN_WIDTH / ss.CURRENT_SCREEN_HEIGHT;

            float divRatio = prevRatio / newRatio;

            mCamera.orthographicSize *= divRatio;
            mCamera.fieldOfView *= divRatio;

            if (mSceneJustLoad)
            {
                float bw = ss.BlackspaceWidth();
                float bh = ss.BlackspaceHeight();

                // Width does not need to be calculate, but 
                // need to set back to the original value.
                // Hence, the 'mRecordOrthographicSize' variable.
                if (bw > bh)
                {
                    mCamera.orthographicSize = mRecordOrthographicSize;
                    mCamera.fieldOfView = mRecordFieldOfView;
                }
                // Calculating the proper hight.
                else
                {
                    // Calculate what the height suppose to be!
                    float supposeHeight = ((float)Screen.width * (float)ss.STARTING_SCREEN_HEIGHT) / (float)ss.STARTING_SCREEN_WIDTH;

                    // Use the 'suppose height' to find the proper 
                    // height ratio.
                    float heightRatio = ((float)Screen.height / supposeHeight);

                    mCamera.orthographicSize = heightRatio * mRecordOrthographicSize;
                    mCamera.fieldOfView = heightRatio * mRecordFieldOfView;
                }

                mSceneJustLoad = false;
            }

            /* Store it to screen settings. */
            {
                ss.ORTHOGRAPHIC_SIZE = mCamera.orthographicSize;
                ss.FIELD_OF_VIEW = mCamera.fieldOfView;
            }
        }
    }
}
