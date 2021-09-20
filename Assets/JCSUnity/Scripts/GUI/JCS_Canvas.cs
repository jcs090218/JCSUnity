/**
 * $File: JCS_Canvas.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using System.Collections.Generic;
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// Default canvas class for JCSUnity.
    /// </summary>
    [RequireComponent(typeof(Canvas))]
    [RequireComponent(typeof(RectTransform))]
    public class JCS_Canvas : MonoBehaviour
    {
        /* Variables */

        public static JCS_Canvas instance = null;

        private const string RESIZE_UI_PATH = "LevelDesignUI/ResizeUI";

        [Header("** Check Variables (JCS_Canvas) **")]

        [Tooltip("Canvas object.")]
        [SerializeField]
        private Canvas mCanvas = null;

        // Application Rect (Window)
        private RectTransform mAppRect = null;

        [Header("** Initialize Variables (JCS_Canvas) **")]

        [Tooltip("Enable this if the canvas is master canvas.")]
        [SerializeField]
        private bool mMaster = true;

        /* Setter & Getter */

        public bool Master { get { return this.mMaster; } }
        public RectTransform GetAppRect() { return this.mAppRect; }
        public Canvas GetCanvas() { return this.mCanvas; }

        /* Functions */

        private void Awake()
        {
            if (mMaster)
            {
                if (instance == null)
                    instance = this;
                else
                    Debug.LogError("You can't have more than one master Canvas in the scene");
            }

            this.mAppRect = this.GetComponent<RectTransform>();
            this.mCanvas = this.GetComponent<Canvas>();

            if (JCS_UISettings.instance.RESIZE_UI)
            {
                // resizable UI in order to resize the UI correctly
                JCS_ResizeUI rui = JCS_Utility.SpawnGameObject(RESIZE_UI_PATH).GetComponent<JCS_ResizeUI>();
                rui.transform.SetParent(this.transform);
            }
        }

        private void Start()
        {
            var resizeUI = JCS_ResizeUI.instance;

            if (JCS_UISettings.instance.RESIZE_UI)
            {
                if (resizeUI == null)
                    return;

                // get the screen width and height
                Vector2 actualRect = this.GetAppRect().sizeDelta;

                // set it to the right resolution
                resizeUI.GetResizeRect().sizeDelta = actualRect;
            }
        }

        /// <summary>
        /// Add component to resize canvas.
        /// </summary>
        /// <param name="com"> Component add to canvas. </param>
        public void AddComponentToResizeCanvas(Component com)
        {
            var resizeUI = JCS_ResizeUI.instance;

            Transform newParent = (resizeUI != null) ? resizeUI.transform : this.mCanvas.transform;
            if (newParent == null)
                JCS_Debug.LogError("Attach resize canvas exception: " + com);
            else
                com.transform.SetParent(newParent);

            // We will expect COM to be one of the UI component from built-in 
            // Unity. If this is true, we resize it's component to match
            // the current screen space.
            var rect = com.GetComponent<RectTransform>();
            FitScreenSize(rect);
        }

        /// <summary>
        /// Fit RECT into our screen space.
        /// </summary>
        /// <param name="rect"> The `RectTransform` object to resize fro screen. </param>
        private void FitScreenSize(RectTransform rect)
        {
            if (rect == null)
                return;

            var ss = JCS_ScreenSettings.instance;
            JCS_ScreenSizef screenRaio = ss.ScreenRatio();

            Vector3 newScale = rect.localScale;
            newScale.x /= screenRaio.width;
            newScale.y /= screenRaio.height;
            rect.localScale = newScale;
        }
    }
}
