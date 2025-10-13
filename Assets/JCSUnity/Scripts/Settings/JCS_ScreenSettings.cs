/**
 * $File: JCS_ScreenSettings.cs $
 * $Date: 2018-09-08 15:15:28 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright © 2018 by Shen, Jen-Chieh $
 */
using System;
using UnityEngine;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Screen related settings.
    /// </summary>
    public class JCS_ScreenSettings : JCS_Settings<JCS_ScreenSettings>
    {
        /* Variables */

        // In general
        public Action onChanged = null;
        public Action onChangedResolution = null;
        public Action onChangedSize = null;
        public Action onChangedMode = null;

        // Resizable
        public Action onResizableResize = null;
        public Action onResizableIdle = null;

        // Record
        private Resolution mPrevResolution = default;

        private float mSizeWidth = 0;
        private float mSizeHeight = 0;

        private FullScreenMode mPrevScreenMode = FullScreenMode.FullScreenWindow;

#if UNITY_EDITOR
        [Separator("Helper Variables (JCS_ScreenManager)")]

        [Tooltip("Show the resizable screen panel in game?")]
        public bool showResizablePanels = true;
#endif

        [Separator("Check Variables (JCS_ScreenSettings)")]

        [Tooltip("Screen size when the application starts.")]
        [ReadOnly]
        public JCS_ScreenSizef startingSize = JCS_ScreenSizef.zero;

        [Tooltip("Store the camera orthographic size value over scene.")]
        [ReadOnly]
        public float orthographicSize = 0.0f;

        [Tooltip("Store the camera filed of view value over scene.")]
        [ReadOnly]
        public float fieldOfView = 0.0f;

        [Tooltip("Previous screen size.")]
        [ReadOnly]
        public JCS_ScreenSizef prevSize = JCS_ScreenSizef.zero;

        [Tooltip("Current screen size.")]
        [ReadOnly]
        public JCS_ScreenSizef currentSize = JCS_ScreenSizef.zero;

        [Tooltip("Target aspect ratio screen size.")]
        [ReadOnly]
        public JCS_ScreenSize aspectRatioSize = JCS_ScreenSize.zero;

        [Separator("Initialize Variables (JCS_ScreenSettings)")]

        [Tooltip("Resize the screen/window to certain aspect when " +
            "the application starts. Aspect ratio can be set at 'JCS_ScreenManager'.")]
        public bool resizeToAspectWhenAppStarts = true;

        [Tooltip("Resize the screen/window to standard resoltuion when application starts.")]
        public bool resizeToStandardWhenAppStarts = false;

        [Tooltip("Resize the screen/window everytime a scene are loaded.")]
        public bool resizeToAspectEverytimeSceneLoaded = false;

        [Tooltip("When resize, resize to the smaller edge, if not true will resize to larger edge.")]
        public bool resizeToSmallerEdge = true;

        [Tooltip("Defualt color to aspect panels.")]
        [SerializeField]
        private Color mResizablePanelsColor = Color.black;

        [Separator("Runtime Variables (JCS_ScreenSettings)")]

        [Tooltip("Standard screen size to calculate the worldspace obejct's camera view.")]
        public JCS_ScreenSize standardSize = new JCS_ScreenSize(1920, 1080);

        [Tooltip("Type of the screen handle.")]
        public JCS_ScreenType screenType = JCS_ScreenType.RESIZABLE;

        /* Setter & Getter */

        public Color RESIZABLE_PANELS_COLOR
        {
            get { return mResizablePanelsColor; }
            set
            {
                mResizablePanelsColor = value;

                JCS_ScreenManager.FirstInstance().SetResizablePanelsColor(mResizablePanelsColor);
            }
        }

        /* Functions */

        private void Awake()
        {
            CheckInstance(this);

            // This will only run once at the time when 
            // the application is starts.
            if (!JCS_AppSettings.FirstInstance().appStarts)
            {
                // Calculate standard screen width and screen height.
                {
                    float gcd = JCS_Mathf.GCD(standardSize.width, standardSize.height);

                    aspectRatioSize.width = (int)((float)standardSize.width / gcd);
                    aspectRatioSize.height = (int)((float)standardSize.height / gcd);
                }

                if (resizeToAspectWhenAppStarts)
                {
                    // Force resize screen/window to certain aspect
                    // ratio once.
                    ForceAspectScreenOnce(true);
                }

                if (resizeToStandardWhenAppStarts)
                {
                    // Force resize screen/window to standard 
                    // resolution once.
                    ForceStandardScreenOnce(true);
                }

                /*
                 * NOTE(jenchieh): This is really weird, that even we 
                 * use 'Screen.SetResolution' function, the 'Screen.width'
                 * and 'Screen.height' will not change immediately.
                 * We just have to get it ourselves in all resize event
                 * function like above these functions.
                 * 
                 *   -> ForceAspectScreenOnce
                 *   -> ForceStandardScreenOnce
                 *   
                 */
                // Record down the starting screen width and screen height.
                //STARTING_SCREEN_SIZE.width = JCS_Screen.width;
                //STARTING_SCREEN_SIZE.height = JCS_Screen.height;
            }
            else
            {
                // Othereise, check if new scene loaded resize the 
                // screen once?
                if (resizeToAspectEverytimeSceneLoaded)
                    ForceAspectScreenOnce();
            }

            // Initialize.
            {
                Resolution currentResolution = Screen.currentResolution;

                mPrevResolution = new Resolution();
                mPrevResolution.width = currentResolution.width;
                mPrevResolution.height = currentResolution.height;

                mSizeWidth = JCS_Screen.width;
                mSizeHeight = JCS_Screen.height;

                mPrevScreenMode = Screen.fullScreenMode;
            }
        }

        private void Start()
        {
            // When first run in the application...
            if (!JCS_AppSettings.FirstInstance().appStarts)
            {
                Camera cam = JCS_Camera.main.GetCamera();

                // Update the data just to see better.
                orthographicSize = cam.orthographicSize;
                fieldOfView = cam.fieldOfView;
            }
        }

        private void LateUpdate()
        {
            OnChanged();

            DoScreenType();
        }

        private void OnChanged()
        {
            Resolution currentResolution = Screen.currentResolution;
            FullScreenMode fullScreenMode = Screen.fullScreenMode;

            bool resolutionChanged =
                mPrevResolution.width != currentResolution.width ||
                mPrevResolution.height != currentResolution.height;

            if (resolutionChanged)
            {
                onChangedResolution?.Invoke();

                mPrevResolution.width = currentResolution.width;
                mPrevResolution.height = currentResolution.height;
            }

            bool sizeChanged = mSizeWidth != JCS_Screen.width ||
                mSizeHeight != JCS_Screen.height;

            if (sizeChanged)
            {
                onChangedSize?.Invoke();

                mSizeWidth = JCS_Screen.width;
                mSizeHeight = JCS_Screen.height;
            }

            bool modeChanged = mPrevScreenMode != fullScreenMode;

            if (modeChanged)
            {
                onChangedMode?.Invoke();

                mPrevScreenMode = fullScreenMode;
            }

            if (modeChanged || sizeChanged || resolutionChanged)
            {
                onChanged?.Invoke();
            }
        }

        /// <summary>
        /// Return true, if we should use resizalbe panels.
        /// </summary>
        public bool ShouldSpawnResizablePanels()
        {
            switch (screenType)
            {
                case JCS_ScreenType.NONE:
                case JCS_ScreenType.FIT_ALL:
                case JCS_ScreenType.MIXED:
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Return true, if current screen type is responsive.
        /// </summary>
        public bool IsResponsive()
        {
            switch (screenType)
            {
                case JCS_ScreenType.MIXED:
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Return true if current screen type is none.
        /// </summary>
        public bool IsNone()
        {
            return screenType == JCS_ScreenType.NONE;
        }

        /// <summary>
        /// Return the starting screen size by the current screen type.
        /// </summary>
        public JCS_ScreenSizef StartingSize()
        {
            float newWidth;
            float newHeight;

            if (ShouldSpawnResizablePanels())
            {
                newWidth = startingSize.width;
                newHeight = startingSize.height;
            }
            else
            {
                newWidth = JCS_Screen.width;
                newHeight = JCS_Screen.height;
            }

            return new JCS_ScreenSizef(newWidth, newHeight);
        }

        /// <summary>
        /// Return the ratio from expected screen size to actual screen size.
        /// </summary>
        public JCS_ScreenSizef ScreenRatio()
        {
            return new JCS_ScreenSizef(
                standardSize.width / startingSize.width,
                standardSize.height / startingSize.height);
        }

        /// <summary>
        /// Return screen size of the blackspace on the screen, if any 
        /// after resizing the screen.
        /// </summary>
        /// <returns>
        /// Screen size information contain `width` and `height` value.
        /// </returns>
        public JCS_ScreenSizef BlackspaceSize()
        {
            return new JCS_ScreenSizef(
                JCS_Screen.width - startingSize.width,
                JCS_Screen.height - startingSize.height);
        }

        /// <summary>
        /// Get the screen size of the visible area.
        /// </summary>
        /// <returns>
        /// Screen size information contain `width` and `height` value.
        /// </returns>
        public JCS_ScreenSizef VisibleScreenSize()
        {
            JCS_ScreenSizef blackScreenSize = BlackspaceSize();
            return new JCS_ScreenSizef(
                JCS_Screen.width - blackScreenSize.width,
                JCS_Screen.height - blackScreenSize.height);
        }

        /// <summary>
        /// Make the screen in certain aspect ratio.
        /// </summary>
        /// <param name="starting"> Change the starting screen as well? </param>
        public void ForceAspectScreenOnce(bool starting = false)
        {
            float width = JCS_Screen.width;
            float height = JCS_Screen.height;

            bool smaller = aspectRatioSize.width > aspectRatioSize.height;

            // Reverse it if resize to larger edge.
            if (!resizeToSmallerEdge)
                smaller = !smaller;

            if (smaller)
            {
                // update the height
                float heightAccordingToWidth = width / aspectRatioSize.width * aspectRatioSize.height;
                JCS_Screen.SetResolution(width, (int)Mathf.Round(heightAccordingToWidth), false);

                if (starting)
                {
                    startingSize.width = width;
                    startingSize.height = (int)heightAccordingToWidth;
                }
            }
            else
            {
                // update the width
                float widthAccordingToHeight = height / aspectRatioSize.height * aspectRatioSize.width;
                JCS_Screen.SetResolution((int)Mathf.Round(widthAccordingToHeight), height, false);

                if (starting)
                {
                    startingSize.width = (int)widthAccordingToHeight;
                    startingSize.height = height;
                }
            }
        }

        /// <summary>
        /// Resize the screen resolution to standard resolution once.
        /// </summary>
        /// <param name="starting"> Change the starting screen as well? </param>
        public void ForceStandardScreenOnce(bool starting = false)
        {
            JCS_Screen.SetResolution(standardSize.width, standardSize.height, false);

            if (starting)
            {
                startingSize.width = standardSize.width;
                startingSize.height = standardSize.height;
            }
        }

        /// <summary>
        /// Instead of Unity Engine's scripting layer's DontDestroyOnLoad.
        /// I would like to use own define to transfer the old instance
        /// to the newer instance.
        /// 
        /// Every time when unity load the scene. The script have been
        /// reset, in order not to lose the original setting.
        /// transfer the data from old instance to new instance.
        /// </summary>
        /// <param name="_old"> old instance </param>
        /// <param name="_new"> new instance </param>
        protected override void TransferData(JCS_ScreenSettings _old, JCS_ScreenSettings _new)
        {
            _new.resizeToAspectWhenAppStarts = _old.resizeToAspectWhenAppStarts;
            _new.resizeToStandardWhenAppStarts = _old.resizeToStandardWhenAppStarts;
            _new.resizeToAspectEverytimeSceneLoaded = _old.resizeToAspectEverytimeSceneLoaded;

            _new.orthographicSize = _old.orthographicSize;
            _new.fieldOfView = _old.fieldOfView;

            _new.startingSize = _old.startingSize;
            _new.aspectRatioSize = _old.aspectRatioSize;
            _new.standardSize = _old.standardSize;

            _new.RESIZABLE_PANELS_COLOR = _old.RESIZABLE_PANELS_COLOR;

            _new.screenType = _old.screenType;
        }

        /// <summary>
        /// Do the task base on the screen type handle.
        /// </summary>
        private void DoScreenType()
        {
            if (!ShouldSpawnResizablePanels())
            {
                // These types do not expect resize!
                return;
            }

            switch (screenType)
            {
                case JCS_ScreenType.ALWAYS_STANDARD:
                    DoAlwaysStandard();
                    break;
                case JCS_ScreenType.FORCE_ASPECT:
                    DoFoceAspect();
                    break;
                case JCS_ScreenType.RESIZABLE:
                    DoResizable();
                    break;
            }
        }

        /// <summary>
        /// Always make the screen the same as standards screen width and 
        /// standard screen height.
        /// </summary>
        private void DoAlwaysStandard()
        {
            float width = JCS_Screen.width;
            float height = JCS_Screen.height;

            if (width != standardSize.width || height != standardSize.height)
            {
                JCS_Screen.SetResolution(standardSize.width, standardSize.height, false);
            }

            prevSize.width = width;
            prevSize.height = height;
        }

        /// <summary>
        /// Force aspect window.
        /// 
        /// SOURCE: https://gamedev.stackexchange.com/questions/86707/how-to-lock-aspect-ratio-when-resizing-game-window-in-unity
        /// AUTHOR: Entity in JavaScript.
        /// Modefied: Jen-Chieh Shen to C#.
        /// </summary>
        private void DoFoceAspect()
        {
            if (Screen.fullScreen)
                return;

            float width = JCS_Screen.width;
            float height = JCS_Screen.height;

            // if the user is changing the width
            if (prevSize.width != width)
            {
                // update the height
                float heightAccordingToWidth = width / aspectRatioSize.width * aspectRatioSize.height;
                JCS_Screen.SetResolution(width, (int)Mathf.Round(heightAccordingToWidth), false);
            }

            // if the user is changing the height
            if (prevSize.height != height)
            {
                // update the width
                float widthAccordingToHeight = height / aspectRatioSize.height * aspectRatioSize.width;
                JCS_Screen.SetResolution((int)Mathf.Round(widthAccordingToHeight), height, false);
            }

            prevSize.width = width;
            prevSize.height = height;
        }

        /// <summary>
        /// Do the resizable window.
        /// </summary>
        private void DoResizable()
        {
            float width = JCS_Screen.width;
            float height = JCS_Screen.height;

            if (currentSize.width == width && currentSize.height == height)
            {
                onResizableIdle?.Invoke();
                return;
            }

            if (prevSize.width == 0.0f || prevSize.height == 0.0f)
            {
                // If zero, set to the same value.
                prevSize.width = width;
                prevSize.height = height;
            }
            else
            {
                // Record previous screen info.
                prevSize.width = currentSize.width;
                prevSize.height = currentSize.height;
            }

            // Update current screen info.
            currentSize.width = width;
            currentSize.height = height;

            // Do callback.
            onResizableResize?.Invoke();
        }
    }
}
