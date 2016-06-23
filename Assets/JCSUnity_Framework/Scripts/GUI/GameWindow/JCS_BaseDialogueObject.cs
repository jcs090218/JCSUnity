/**
 * $File: JCS_BaseDialogueObject.cs $
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

    /// <summary>
    /// Common Dialogue Object!
    /// 
    /// Include function, 
    /// Show Dialogue, 
    /// Hide Dialoguem, 
    /// Move To the front of the screen, 
    /// Set the Parent by mode, 
    /// etc.
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public class JCS_BaseDialogueObject
        : MonoBehaviour
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables

        //----------------------
        // Protected Variables
        protected RectTransform mRectTransform = null;
        protected bool mIsVisible = false;

        [Header("** Runtime Variables (JCS_BaseDialogueObject) **")]
        [SerializeField] protected bool mAsOriginalSize = false;
        [SerializeField] protected bool mAsOriginalPosition = false;
        [SerializeField] protected bool mAsOriginalRotation = false;

        protected Vector3 mOriginalScale = Vector3.zero;
        protected Vector3 mOriginalPosition = Vector3.zero;
        protected Quaternion mOriginalRotation = new Quaternion(0, 0, 0, 0);

        //========================================
        //      setter / getter
        //------------------------------

        //========================================
        //      Unity's function
        //------------------------------
        protected virtual void Awake()
        {
            if (mRectTransform == null)
                this.mRectTransform = this.GetComponent<RectTransform>();

            mOriginalScale = mRectTransform.localScale;
            mOriginalPosition = mRectTransform.localPosition;
            mOriginalRotation = mRectTransform.localRotation;

            // Find the correct parent depend on the mode
            // developer choose and do the command.
            //     - Either "ResizeUI" or "JCS_Canvas"
            SetParentObjectByMode();

            ResetDialogue();
        }
        protected virtual void Start()
        {
            
        }

        //========================================
        //      Self-Define
        //------------------------------
        //----------------------
        // Public Functions
        public void ResetDialogue()
        {
            // IMPORTANT(JenChieh): override the resize UI part
            if (mAsOriginalSize)
                mRectTransform.localScale = mOriginalScale;
            else
                mRectTransform.localScale = Vector3.one;


            if (mAsOriginalPosition)
                mRectTransform.localPosition = mOriginalPosition;
            else
                mRectTransform.localPosition = Vector3.zero;


            if (mAsOriginalRotation)
                mRectTransform.localRotation = mOriginalRotation;
            else
                mRectTransform.localRotation = new Quaternion(0, 0, 0, 0);

        }
        public void DestroyDialogue()
        {
            // start the app
            JCS_ApplicationManager.APP_PAUSE = false;

            // destroy this dialogue
            Destroy(this.gameObject);
        }
        public void ShowDialogueWithoutSound()
        {
            mIsVisible = true;

            //this.gameObject.SetActive(true);

            // active all the child object
            for (int index = 0;
                index < this.transform.childCount;
                ++index)
            {
                this.transform.GetChild(index).gameObject.SetActive(true);
            }

            MoveToTheLastChild();
        }
        public virtual void HideDialogueWithoutSound()
        {
            mIsVisible = false;

            // Instead of disable the object it self,
            // we deactive all the child object
            //this.gameObject.SetActive(false);


            // deactive all the child object
            for (int index = 0;
                index < this.transform.childCount;
                ++index)
            {
                this.transform.GetChild(index).gameObject.SetActive(false);
            }

        }
        public virtual void MoveToTheLastChild()
        {
            Transform parent = this.transform.parent;

            Vector3 recordPos = this.transform.localPosition;
            Vector3 recordScale = this.transform.localScale;
            Quaternion recordRot = this.transform.localRotation;

            // this part will mess up the transform
            // so we record all we need and set it back
            {
                this.transform.SetParent(null);
                this.transform.SetParent(parent);
            }

            // here we set it back!
            this.transform.localPosition = recordPos;
            this.transform.localScale = recordScale;
            this.transform.localRotation = recordRot;
        }

        //----------------------
        // Protected Functions
        protected void SetParentObjectByMode()
        {
            JCS_Canvas jcsCanvas = JCS_Canvas.instance;

            Transform parentObject = null;

            // if is Resize UI is enable than add Dialogue under
            // resize ui transform
            if (JCS_GameSettings.instance.RESIZE_UI)
                parentObject = jcsCanvas.GetResizeUI().transform;
            // Else we add it directly under the Canvas
            else
                parentObject = jcsCanvas.GetCanvas().transform;

            // set it to parent
            this.gameObject.transform.SetParent(parentObject);
        }

        //----------------------
        // Private Functions

    }
}
