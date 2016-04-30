/**
 * $File: JCS_TalkObject.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information $
 *		                Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;
using System;


namespace JCSUnity
{
    public class JCS_TalkObject : MonoBehaviour
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables
        [SerializeField] private TextAsset mFile = null;
        private JCS_XML_Loader mLoader = null;

        [SerializeField] private string[] mLine = null;
        [SerializeField] private JCS_PageLook[] mType = null;

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------

        //========================================
        //      Unity's function
        //------------------------------
        private void Awake()
        {
            if (mLoader != null)
            {
                mLoader = new JCS_XML_Loader(mFile);

                //Array.Clear(mLine, 0, mLine.Length);
                //Array.Clear(mType, 0, mType.Length);
            }
        }

        private void Update()
        {

        }

        private void OnMouseOver()
        {
            if (JCS_Input.OnMouseDoubleClick(JCS_InputType.MOUSE_LEFT))
            {
                JCS_ButtonFunctions.PopTalkDialogue();
            }
        }

        //========================================
        //      Self-Define
        //------------------------------
        //----------------------
        // Public Functions

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions


    }
}
