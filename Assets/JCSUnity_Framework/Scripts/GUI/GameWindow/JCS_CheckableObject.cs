/**
 * $File: JCS_CheckableObject.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information $
 *		                Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace JCSUnity
{

    /// <summary>
    /// Object will pop out a dialogue so there will be 
    /// a description on it
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public class JCS_CheckableObject
        : MonoBehaviour
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables
        private RectTransform mRectTransform = null;

        // Item Contain the following data
        private Image mItemImage = null;

        private enum EventType
        {
            ON_MOUSE_OVER,
            ON_MOUSE_DOWN,
            ON_MOUSE_DOUBLE_CLICK
        };

        [Header("** Runtime Variables **")]
        private bool mShowing = false;

        [Header("NOTE: nGUI_2D please use Event Trigger from Unity.")]
        [SerializeField]
        private JCS_CheckableDialogue mDescDialogue = null;
        // Position that dialogue will be at
        [SerializeField]
        private JCS_2D8Direction mSpawnPosition = JCS_2D8Direction.TOP_LEFT;
        [SerializeField] private RectTransform mRootPanel = null;

        [Header("NOTE: For 3D Object only.")]
        // Event to show the dialogue
        [SerializeField]
        private EventType mEventType = EventType.ON_MOUSE_OVER;

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------

        //========================================
        //      Unity's function
        //------------------------------
        private void Start()
        {
            this.mRectTransform = this.GetComponent<RectTransform>();
            this.mItemImage = this.GetComponent<Image>();

            JCS_HideDescDialogue();

            mDescDialogue.SetItemSprite(this.mItemImage.sprite);

            if (mRootPanel == null)
            {
                // Try to find the root panel
                JCS_GameWindow gw = this.GetComponentInParent<JCS_GameWindow>();
                if (gw != null)
                {
                    // gw must have rect transform, no worry about this!
                    // and if is a window must have "JCS_GameWindow"!
                    mRootPanel = gw.transform.GetComponent<RectTransform>();
                }
            }
        }
#if (UNITY_EDITOR || UNITY_STANDALONE)
        private void Update()
        {
            if (!mShowing)
                return;

            FollowMouse();

            JCS_HideDescDialogue();
        }
#endif
        private void OnMouseDown()
        {
            if (mEventType != EventType.ON_MOUSE_DOWN)
                return;

            JCS_ShowDescDialogue();
        }
        private void OnMouseOver()
        {
            //Check event
            if (mEventType == EventType.ON_MOUSE_OVER)
            {
                JCS_ShowDescDialogue();
            }
            //Check event
            else if (mEventType == EventType.ON_MOUSE_DOUBLE_CLICK && 
                // Check if double click
                JCS_Input.OnMouseDoubleClick(0))
            {
                JCS_ShowDescDialogue();
            }
        }

        //========================================
        //      Self-Define
        //------------------------------
        //----------------------
        // Public Functions
        /// <summary>
        /// Pop out the dialogue to show
        /// </summary>
        public void JCS_ShowDescDialogue()
        {
            if (mDescDialogue == null)
            {
                JCS_GameErrors.JcsErrors("JCS_CheckableObject", -1, "No dialogue object attached...");
                return;
            }

            mDescDialogue.ShowDialogueWithoutSound();
            mShowing = true;
        }
        public void JCS_HideDescDialogue()
        {
            if (mDescDialogue == null)
            {
                JCS_GameErrors.JcsErrors("JCS_CheckableObject", -1, "No dialogue object attached...");
                return;
            }

#if (UNITY_STANDALONE || UNITY_EDITOR)
            // is is still on top of the image, return it
            if (CheckMouseOver())
                return;
#endif

            mDescDialogue.HideDialogueWithoutSound();
            mShowing = false;
        }

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions
        private void FollowMouse()
        {
            mDescDialogue.FollowMouse(mSpawnPosition);
        }
        /// <summary>
        /// Solve the flash problem!
        /// 
        /// Check if the mouse still on top of the image!
        /// </summary>
        /// <returns></returns>
        private bool CheckMouseOver()
        {
            Vector2 mousePos = JCS_Input.MousePositionOnGUILayer();
            Vector2 checkPos = this.mRectTransform.localPosition;

            if (mRootPanel != null)
                checkPos += new Vector2(mRootPanel.localPosition.x, mRootPanel.localPosition.y);

            // this item image size
            Vector2 slotRect = this.mRectTransform.sizeDelta;

            float halfSlotWidth = slotRect.x / 2 * mRectTransform.localScale.x;
            float halfSlotHeight = slotRect.y / 2 * mRectTransform.localScale.y;

            float leftBorder = checkPos.x - halfSlotWidth;
            float rightBorder = checkPos.x + halfSlotWidth;
            float topBorder = checkPos.y + halfSlotHeight;
            float bottomBorder = checkPos.y - halfSlotHeight;

            // Basic AABB collide math
            if (mousePos.x <= rightBorder && 
                mousePos.x >= leftBorder &&
                mousePos.y <= topBorder && 
                mousePos.y >= bottomBorder)
            {
                return true;
            }

            return false;
        }

    }
}
