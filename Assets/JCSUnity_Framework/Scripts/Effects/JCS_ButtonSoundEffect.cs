/**
 * $File: JCS_ButtonSoundEffect.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information $
 *		                Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

namespace JCSUnity
{

    /// <summary>
    /// Please use this class with Unity's "Event Trigger (Script)"!!!
    /// </summary>
    [RequireComponent(typeof(JCS_SoundPlayer))]
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(EventTrigger))]
    public class JCS_ButtonSoundEffect
        : MonoBehaviour
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables
        private JCS_SoundPlayer mSoundPlayer = null;
        private RectTransform mRectTransform = null;

        [Header("*USAGE: Please use this class with Unity's \"Event Trigger(Script)\"!!!")]
        [SerializeField] private AudioClip mOnMouseOver = null;
        [SerializeField] private AudioClip mOnMouseExit = null;
        [SerializeField] private AudioClip mOnMouseDown = null;
        [SerializeField] private AudioClip mOnMouseUp = null;
        [SerializeField] private AudioClip mOnMouseClick = null;
        [SerializeField] private AudioClip mOnMouseDoubleClick = null;
        private bool mIsOver = false;


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
            mSoundPlayer = this.GetComponent<JCS_SoundPlayer>();
            mRectTransform = this.GetComponent<RectTransform>();
        }

        private void Update()
        {
            // IMPORTANT(JenChieh): only double click need update
            if (mIsOver)
            {
                if (JCS_Input.OnMouseDoubleClick(0))
                    mSoundPlayer.PlayOneShot(mOnMouseDoubleClick);

                // check if the mouse still over or not
                if (!JCS_UsefualFunctions.MouseOverGUI(this.mRectTransform))
                    mIsOver = false;
            }
        }

        //========================================
        //      Self-Define
        //------------------------------
        //----------------------
        // Public Functions
        public void JCS_OnMouseOver()
        {
            mSoundPlayer.PlayOneShot(mOnMouseOver);
        }
        public void JCS_OnMouseExit()
        {
            mSoundPlayer.PlayOneShot(mOnMouseExit);
        }
        public void JCS_OnMouseDown()
        {
            mSoundPlayer.PlayOneShot(mOnMouseDown);
        }
        public void JCS_OnMouseUp()
        {
            mSoundPlayer.PlayOneShot(mOnMouseUp);
        }
        public void JCS_OnMouseClick()
        {
            mSoundPlayer.PlayOneShot(mOnMouseClick);
        }
        // plz put this in Pointer Enter event
        public void JCS_OnMouseDoubleClick()
        {
            mIsOver = true;
        }

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions

    }
}
