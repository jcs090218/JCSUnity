/**
 * $File: JCS_2DLadder.cs $
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

    [RequireComponent(typeof(BoxCollider))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class JCS_2DLadder 
        : MonoBehaviour
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables
        [SerializeField] private BoxCollider mTopCollider = null;
        [SerializeField] private BoxCollider mBottomCollider = null;

        private JCS_2DLadderTop mLadderTop = null;
        private JCS_2DLadderBottom mLadderBottom = null;

        private SpriteRenderer mSpriteRenderer = null;
        private BoxCollider mBoxCollider = null;

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

            mSpriteRenderer = this.GetComponent<SpriteRenderer>();
            mBoxCollider = this.GetComponent<BoxCollider>();

            // add to according object.
            if (mTopCollider != null)
            {
                mLadderTop = mTopCollider.gameObject.AddComponent<JCS_2DLadderTop>();
            }
            if (mBottomCollider != null)
            {
                mLadderBottom = mBottomCollider.gameObject.AddComponent<JCS_2DLadderBottom>();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            JCS_2DSideScrollerPlayer p = other.GetComponent<JCS_2DSideScrollerPlayer>();
            if (p == null)
                return;

            if (p.GetClimbingTransform() != this.transform)
            {
                p.CanLadder = true;
                p.SetClimbingTransform(this.transform);
            }
        }
        private void OnTriggerStay(Collider other)
        {
            JCS_2DSideScrollerPlayer p = other.GetComponent<JCS_2DSideScrollerPlayer>();
            if (p == null)
                return;

            if (p.CharacterMode == JCS_2DCharacterMode.CLIMBING)
                mSpriteRenderer.sortingOrder = p.GetSpriteRenderer().sortingOrder - 1;

            if (JCS_Physics.BottomOfBox(p.GetCharacterController(), mLadderBottom.GetBoxCollider()) &&
                JCS_Input.GetKey(KeyCode.DownArrow))
            {
                p.CharacterMode = JCS_2DCharacterMode.NORMAL;
                p.CanLadder = false;
                p.GetAnimator().enabled = true;
                p.SetClimbingTransform(null);
            }
        }
        private void OnTriggerExit(Collider other)
        {
            JCS_2DSideScrollerPlayer p = other.GetComponent<JCS_2DSideScrollerPlayer>();
            if (p == null)
                return;

            // check if player leave the ladder.
            if (!JCS_Physics.JcsOnTriggerCheck(p.GetCharacterController(), mBoxCollider))
            {
                p.CharacterMode = JCS_2DCharacterMode.NORMAL;
                p.CanLadder = false;
                p.GetAnimator().enabled = true;
                p.SetClimbingTransform(null);

                // set sorting layer higher than this player, 
                // so better to set all the play in one sorting layer.
                if (JCS_Physics.TopOfBox(p.GetCharacterController(), mLadderTop.GetBoxCollider()))
                {
                    mSpriteRenderer.sortingOrder = p.GetSpriteRenderer().sortingOrder + 1;
                }
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
