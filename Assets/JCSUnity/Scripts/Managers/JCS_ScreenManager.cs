/**
 * $File: JCS_ScreenManager.cs $
 * $Date: 2018-09-12 02:50:44 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information
 *	                 Copyright © 2018 by Shen, Jen-Chieh $
 */
using UnityEngine;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Manages screen action.
    /// </summary>
    public class JCS_ScreenManager : JCS_Manager<JCS_ScreenManager>
    {
        /* Variables */

        private const string mResizableScreenPanelPath = "UI/JCS_ResizableScreenPanel";

        [Separator("Check Variables (JCS_ScreenManager)")]

        [Tooltip("Top resizable screen panel.")]
        [SerializeField]
        [ReadOnly]
        private JCS_ResizableScreenPanel mTopASP = null;

        [Tooltip("Bottom resizable screen panel.")]
        [SerializeField]
        [ReadOnly]
        private JCS_ResizableScreenPanel mBottomASP = null;

        [Tooltip("Left resizable screen panel.")]
        [SerializeField]
        [ReadOnly]
        private JCS_ResizableScreenPanel mLeftASP = null;

        [Tooltip("Right resizable screen panel.")]
        [SerializeField]
        [ReadOnly]
        private JCS_ResizableScreenPanel mRightASP = null;

        [Separator("Initialize Variables (JCS_ScreenManager)")]

        [Tooltip("Resize screen once on this scene?")]
        public bool RESIZE_SCREEN_THIS_SCENE = false;

        [Tooltip("Aspect ratio screen width for this scene you want to resize.")]
        public int ASPECT_RATION_SCREEN_WIDTH_THIS_SCENE = 16;

        [Tooltip("Aspect ratio screen height for this scene you want to resize.")]
        public int ASPECT_RATION_SCREEN_HEIGHT_THIS_SCENE = 9;

        /* Setter & Getter */

        public JCS_ResizableScreenPanel TopASP { get { return this.mTopASP; } }
        public JCS_ResizableScreenPanel BottomASP { get { return this.mBottomASP; } }
        public JCS_ResizableScreenPanel LeftASP { get { return this.mLeftASP; } }
        public JCS_ResizableScreenPanel RightASP { get { return this.mRightASP; } }

        /* Functions */

        private void Awake()
        {
            instance = this;

            if (ShouldSpawnResizablePanels())
            {
                // Spawn the four aspect screen panels.
                this.mTopASP = JCS_Util.Instantiate(mResizableScreenPanelPath).GetComponent<JCS_ResizableScreenPanel>();
                this.mBottomASP = JCS_Util.Instantiate(mResizableScreenPanelPath).GetComponent<JCS_ResizableScreenPanel>();
                this.mLeftASP = JCS_Util.Instantiate(mResizableScreenPanelPath).GetComponent<JCS_ResizableScreenPanel>();
                this.mRightASP = JCS_Util.Instantiate(mResizableScreenPanelPath).GetComponent<JCS_ResizableScreenPanel>();

                // Set the ASP direction.
                this.mTopASP.PlaceDirection = JCS_2D4Direction.TOP;
                this.mBottomASP.PlaceDirection = JCS_2D4Direction.BOTTOM;
                this.mLeftASP.PlaceDirection = JCS_2D4Direction.LEFT;
                this.mRightASP.PlaceDirection = JCS_2D4Direction.RIGHT;
            }
        }

        private void Start()
        {
            var ss = JCS_ScreenSettings.instance;

            if (RESIZE_SCREEN_THIS_SCENE)
            {
                // Apply new screen aspect ratio.
                ss.ASPECT_RATIO_SCREEN_SIZE.width = ASPECT_RATION_SCREEN_WIDTH_THIS_SCENE;
                ss.ASPECT_RATIO_SCREEN_SIZE.height = ASPECT_RATION_SCREEN_HEIGHT_THIS_SCENE;

                // Resize the screen base on the new screen aspect ratio.
                ss.ForceAspectScreenOnce();
            }

            // Set the panels' color
            SetResizablePanelsColor(ss.RESIZABLE_PANELS_COLOR);
        }

#if UNITY_EDITOR
        private void Update()
        {
            ControlResizablePanels();
        }

        private void ControlResizablePanels()
        {
            var ss = JCS_ScreenSettings.instance;

            /* Handle color. */
            {
                // Make color editable in runtime.
                SetResizablePanelsColor(ss.RESIZABLE_PANELS_COLOR);
            }

            /* Show hide the panels. */
            {
                if (ss.SHOW_RESIZABLE_PANELS)
                    ShowResizablePanels();
                else
                    HideResizablePanels();
            }
        }
#endif

        /// <summary>
        /// Show the resizable panels.
        /// </summary>
        public void ShowResizablePanels()
        {
            if (!ShouldSpawnResizablePanels())
                return;

            this.mTopASP.ShowPanel();
            this.mBottomASP.ShowPanel();
            this.mLeftASP.ShowPanel();
            this.mRightASP.ShowPanel();
        }

        /// <summary>
        /// Hide the resizable panels.
        /// </summary>
        public void HideResizablePanels()
        {
            if (!ShouldSpawnResizablePanels())
                return;

            this.mTopASP.HidePanel();
            this.mBottomASP.HidePanel();
            this.mLeftASP.HidePanel();
            this.mRightASP.HidePanel();
        }

        /// <summary>
        /// Set the color to all resizable panels.
        /// </summary>
        /// <param name="newColor"></param>
        public void SetResizablePanelsColor(Color newColor)
        {
            if (!ShouldSpawnResizablePanels())
                return;

            this.mTopASP.image.color = newColor;
            this.mBottomASP.image.color = newColor;
            this.mLeftASP.image.color = newColor;
            this.mRightASP.image.color = newColor;
        }

        /// <summary>
        /// Return true, if we should use resizalbe panels.
        /// </summary>
        private bool ShouldSpawnResizablePanels()
        {
            var screenS = JCS_ScreenSettings.instance;
            return screenS.ShouldSpawnResizablePanels();
        }
    }
}
