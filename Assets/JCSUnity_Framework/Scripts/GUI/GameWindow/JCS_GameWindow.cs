/**
 * $File: JCS_GameWindow.cs $
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

    [RequireComponent(typeof(JCS_DialogueObject))]
    public class JCS_GameWindow 
        : MonoBehaviour
    {

        //----------------------
        // Public Variables


        //----------------------
        // Private Variables
        private enum DragDrop
        {
            Drag,
            Drop
        };

        private JCS_DialogueObject mDialogueObject = null;

        private bool mIsDragging = false;
        private Vector3 mPosition = Vector3.zero;

        [SerializeField] private JCS_DragDropType mType = JCS_DragDropType.DialogueBox;

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------

        //========================================
        //      Unity's function
        //------------------------------
        public void Awake()
        {
            mDialogueObject = this.GetComponent<JCS_DialogueObject>();
        }


        public void JCS_PointerDown()
        {
            mDialogueObject.MoveToTheLastChild();
            if (mDialogueObject.GetDialogueType() == JCS_DialogueType.PLAYER_DIALOGUE)
                JCS_UIManager.instance.SetJCSDialogue(JCS_DialogueType.PLAYER_DIALOGUE, mDialogueObject);
        }

        public void JCS_Click()
        {
            
        }

        public void JCS_OnDrag()
        {

            switch (mType)
            {
                case JCS_DragDropType.DialogueBox:
                    ProcessGUI(DragDrop.Drag);
                    break;

                case JCS_DragDropType.Inventory:
                    break;
            }
        }

        public void JCS_OnDrop()
        {

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
        private void ProcessGUI(DragDrop type)
        {
            switch (type)
            {
                // Process Drag
                case DragDrop.Drag:
                    Vector3 delta = JCS_Input.MouseDeltaPosition();
                    this.transform.localPosition += delta;
                    break;

                // Process Drop
                case DragDrop.Drop:
                    break;
            }
        }

    }
}
