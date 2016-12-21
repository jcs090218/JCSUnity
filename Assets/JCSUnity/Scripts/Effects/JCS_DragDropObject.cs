/**
 * $File: JCS_DragDropObject.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;


namespace JCSUnity
{

    /// <summary>
    /// Object that can be drag and drop.
    /// </summary>
    public class JCS_DragDropObject 
        : MonoBehaviour
    {
        private enum DragDrop
        {
            Drag,
            Drop
        };


        //----------------------
        // Public Variables

        //----------------------
        // Private Variables

        [Tooltip("")]
        [SerializeField]
        private JCS_DragDropType mType = JCS_DragDropType.DialogueBox;

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------

        //========================================
        //      Unity's function
        //------------------------------

        public void JCS_OnDrag()
        {
            switch (mType)
            {
                case JCS_DragDropType.DialogueBox:
                    ProcessGUI(DragDrop.Drag);
                    break;

                case JCS_DragDropType.Inventory:
                    break;

                    // TODO(JenChieh): 
                case JCS_DragDropType.GameObject_2D:
                    break;

                case JCS_DragDropType.GameObject_25D:
                    break;

                case JCS_DragDropType.GameObjbect_3D:
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
