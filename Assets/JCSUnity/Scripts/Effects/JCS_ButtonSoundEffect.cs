/**
 * $File: JCS_ButtonSoundEffect.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
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
        private EventTrigger mEventTrigger = null;

        [Header("Auto add to Unity's \"Event Trigger(Script)\" or not?")]
        [Tooltip("is true u dont have to add manully!")]
        [SerializeField] private bool mAutoAddEvent = true;


        [Header("*USAGE: Please use this class with Unity's \"Event Trigger(Script)\"!!!")]
        [SerializeField] private AudioClip mOnMouseOver = null;
        [SerializeField] private AudioClip mOnMouseExit = null;
        [SerializeField] private AudioClip mOnMouseDown = null;
        [SerializeField] private AudioClip mOnMouseUp = null;
        [SerializeField] private AudioClip mOnMouseClick = null;
        [SerializeField] private AudioClip mOnMouseDoubleClick = null;
        private bool mIsOver = false;


        [Header("** Optional Settings **")]

        [Tooltip("use to detect to see if the button is interactable or not.")]
        [SerializeField]
        private JCS_Button mJCSButton = null;

        [Tooltip(@"When button is not interactable will active these when 
on mouse down.")]
        [SerializeField] private AudioClip mOnMouseOverRefuse = null;
        [SerializeField] private AudioClip mOnMouseExitRefuse = null;
        [SerializeField] private AudioClip mOnMouseDownRefuse = null;
        [SerializeField] private AudioClip mOnMouseUpRefuse = null;
        [SerializeField] private AudioClip mOnMouseClickRefuse = null;
        [SerializeField] private AudioClip mOnMouseDoubleClickRefuse = null;


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
            mEventTrigger = this.GetComponent<EventTrigger>();

            if (mJCSButton == null)
                mJCSButton = this.GetComponent<JCS_Button>();
        }

        private void Start()
        {
            if (mAutoAddEvent)
            {
                JCS_Utility.AddEventTriggerEvent(mEventTrigger, EventTriggerType.PointerEnter, JCS_OnMouseOver);
                JCS_Utility.AddEventTriggerEvent(mEventTrigger, EventTriggerType.PointerEnter, JCS_OnMouseDoubleClick);
                JCS_Utility.AddEventTriggerEvent(mEventTrigger, EventTriggerType.PointerExit, JCS_OnMouseExit);
                JCS_Utility.AddEventTriggerEvent(mEventTrigger, EventTriggerType.PointerDown, JCS_OnMouseDown);
                JCS_Utility.AddEventTriggerEvent(mEventTrigger, EventTriggerType.PointerUp, JCS_OnMouseUp);
                JCS_Utility.AddEventTriggerEvent(mEventTrigger, EventTriggerType.PointerClick, JCS_OnMouseClick);
            }
        }

        private void Update()
        {
            // IMPORTANT(JenChieh): only double click need update
            if (mIsOver)
            {
                if (JCS_Input.OnMouseDoubleClick(0))
                {
                    // either time is out or double click, 
                    // both are all over the "double click event".
                    mIsOver = false;

                    if (mJCSButton != null)
                    {
                        if (!mJCSButton.Interactable)
                        {
                            // play not ineractable sound
                            mSoundPlayer.PlayOneShot(mOnMouseDoubleClickRefuse);
                            return;
                        }
                    }

                    // play normal double click sound
                    mSoundPlayer.PlayOneShot(mOnMouseDoubleClick);
                }

                // check if the mouse still over or not
                if (!JCS_Utility.MouseOverGUI(this.mRectTransform))
                    mIsOver = false;
            }
        }

        //========================================
        //      Self-Define
        //------------------------------
        //----------------------
        // Public Functions
        public void JCS_OnMouseOver(PointerEventData data)
        {
            JCS_OnMouseOver();
        }
        public void JCS_OnMouseOver()
        {
            if (mJCSButton != null)
            {
                if (!mJCSButton.Interactable)
                {
                    // play not ineractable sound
                    mSoundPlayer.PlayOneShot(mOnMouseOverRefuse);
                    return;
                }
            }

            mSoundPlayer.PlayOneShot(mOnMouseOver);
        }

        public void JCS_OnMouseExit(PointerEventData data)
        {
            JCS_OnMouseExit();
        }
        public void JCS_OnMouseExit()
        {
            if (mJCSButton != null)
            {
                if (!mJCSButton.Interactable)
                {
                    // play not ineractable sound
                    mSoundPlayer.PlayOneShot(mOnMouseExitRefuse);
                    return;
                }
            }

            mSoundPlayer.PlayOneShot(mOnMouseExit);
        }

        public void JCS_OnMouseDown(PointerEventData data)
        {
            JCS_OnMouseDown();
        }
        public void JCS_OnMouseDown()
        {
            if (mJCSButton != null)
            {
                if (!mJCSButton.Interactable)
                {
                    // play not ineractable sound
                    mSoundPlayer.PlayOneShot(mOnMouseDownRefuse);
                    return;
                }
            }

            // play normal sound
            mSoundPlayer.PlayOneShot(mOnMouseDown);
        }

        public void JCS_OnMouseUp(PointerEventData data)
        {
            JCS_OnMouseUp();
        }
        public void JCS_OnMouseUp()
        {
            if (mJCSButton != null)
            {
                if (!mJCSButton.Interactable)
                {
                    // play not ineractable sound
                    mSoundPlayer.PlayOneShot(mOnMouseUpRefuse);
                    return;
                }
            }

            mSoundPlayer.PlayOneShot(mOnMouseUp);
        }

        public void JCS_OnMouseClick(PointerEventData data)
        {
            JCS_OnMouseClick();
        }
        public void JCS_OnMouseClick()
        {
            if (mJCSButton != null)
            {
                if (!mJCSButton.Interactable)
                {
                    // play not ineractable sound
                    mSoundPlayer.PlayOneShot(mOnMouseClickRefuse);
                    return;
                }
            }

            mSoundPlayer.PlayOneShot(mOnMouseClick);
        }

        // plz put this in Pointer Enter event
        public void JCS_OnMouseDoubleClick(PointerEventData data)
        {
            JCS_OnMouseDoubleClick();
        }
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
