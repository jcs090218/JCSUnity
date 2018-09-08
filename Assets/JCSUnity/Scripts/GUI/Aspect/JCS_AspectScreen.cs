/**
 * $File: JCS_AspectScreen.cs $
 * $Date: 2018-09-03 22:27:30 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright © 2018 by Shen, Jen-Chieh $
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace JCSUnity
{
    /// <summary>
    /// Make the proportional screen/window. 
    /// 
    /// SOURCE: https://gamedev.stackexchange.com/questions/86707/how-to-lock-aspect-ratio-when-resizing-game-window-in-unity
    /// AUTHOR: Entity in JavaScript.
    /// Modefied: Jen-Chieh Shen to C#.
    /// </summary>
    public class JCS_AspectScreen
        : MonoBehaviour
    {

        /*******************************************/
        /*            Public Variables             */
        /*******************************************/

        public static JCS_AspectScreen instance = null;

        /*******************************************/
        /*           Private Variables             */
        /*******************************************/

#if (UNITY_EDITOR)
        [Header("** Helper Variables (JCS_AspectScreen) **")]

        [Tooltip("Show the aspect screen panel in game?")]
        [SerializeField]
        private bool mShowItInEditorMode = false;
#endif


        [Header("** Check Variables (JCS_AspectScreen) **")]

        [Tooltip("Invisible object area.")]
        [SerializeField]
        private string mAspectScreenPanelPath = "JCSUnity_Resources/GUI/JCS_AspectScreenPanel";

        [Tooltip("Top aspect screen panel.")]
        [SerializeField]
        private JCS_AspectScreenPanel mTopASP = null;

        [Tooltip("Bottom aspect screen panel.")]
        [SerializeField]
        private JCS_AspectScreenPanel mBottomASP = null;

        [Tooltip("Left aspect screen panel.")]
        [SerializeField]
        private JCS_AspectScreenPanel mLeftASP = null;

        [Tooltip("Right aspect screen panel.")]
        [SerializeField]
        private JCS_AspectScreenPanel mRightASP = null;

        /*******************************************/
        /*           Protected Variables           */
        /*******************************************/

        /*******************************************/
        /*             setter / getter             */
        /*******************************************/
        public JCS_AspectScreenPanel TopASP { get { return this.mTopASP; } }
        public JCS_AspectScreenPanel BottomASP { get { return this.mBottomASP; } }
        public JCS_AspectScreenPanel LeftASP { get { return this.mLeftASP; } }
        public JCS_AspectScreenPanel RightASP { get { return this.mRightASP; } }

        /*******************************************/
        /*            Unity's function             */
        /*******************************************/
        private void Awake()
        {
            instance = this;

            // Spawn the four aspect screen panels.
            this.mTopASP = JCS_Utility.SpawnGameObject(mAspectScreenPanelPath).GetComponent<JCS_AspectScreenPanel>();
            this.mBottomASP = JCS_Utility.SpawnGameObject(mAspectScreenPanelPath).GetComponent<JCS_AspectScreenPanel>();
            this.mLeftASP = JCS_Utility.SpawnGameObject(mAspectScreenPanelPath).GetComponent<JCS_AspectScreenPanel>();
            this.mRightASP = JCS_Utility.SpawnGameObject(mAspectScreenPanelPath).GetComponent<JCS_AspectScreenPanel>();

            // Set the ASP direction.
            this.mTopASP.ASPDirection = JCS_2D4Direction.TOP;
            this.mBottomASP.ASPDirection = JCS_2D4Direction.BOTTOM;
            this.mLeftASP.ASPDirection = JCS_2D4Direction.LEFT;
            this.mRightASP.ASPDirection = JCS_2D4Direction.RIGHT;
        }

#if (UNITY_EDITOR)
        private void Update()
        {
            // ATTENTION(jenchieh): For tesing we have to enable it.
            if (mShowItInEditorMode)
            {
                this.mTopASP.ShowASP();
                this.mBottomASP.ShowASP();
                this.mLeftASP.ShowASP();
                this.mRightASP.ShowASP();
            }
            else
            {
                this.mTopASP.HideASP();
                this.mBottomASP.HideASP();
                this.mLeftASP.HideASP();
                this.mRightASP.HideASP();
            }
        }
#endif

        /*******************************************/
        /*              Self-Define                */
        /*******************************************/
        //----------------------
        // Public Functions

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions

    }
}
