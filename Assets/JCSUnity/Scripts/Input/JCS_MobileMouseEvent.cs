/**
 * $File: JCS_MobileMouseEvent.cs $
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
    /// Use this to receiving the single from mobile touch input/bufffer.
    /// </summary>
    public class JCS_MobileMouseEvent 
        : MonoBehaviour
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables

        [Header("** Check Variables (JCS_MobileMouseEvent) **")]

        [SerializeField]
        private Camera mCamera = null;
        [SerializeField]
        private bool mTouched = false;


        [Header("** Runtime Variables (JCS_MobileMouseEvent) **")]

        [Tooltip("Distance Raycast shoot.")]
        [SerializeField]
        private float mRaycastDistance = 100.0f;
        

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------
        public float RaycastDistance { get { return this.mRaycastDistance; } set { this.mRaycastDistance = value; } }

        //========================================
        //      Unity's function
        //------------------------------
        private void Start()
        {
            if (mCamera == null)
                this.mCamera = JCS_Camera.main.GetCamera();

            // set to manager in order to get manage by "JCS_InputManager"
            JCS_InputManager.instance.SetJCSMobileMouseEvent(this);
        }

        private void Update()
        {
            mTouched = JCS_Input.GetMouseButton(0);
            if (mTouched)
            {
                // A ray is an infinite line starting at an origin and going into a direction
                // For this we will use our mouse position
                Ray ray = mCamera.ScreenPointToRay(Input.mousePosition);

                RaycastHit[] hits = Physics.RaycastAll(ray, mRaycastDistance);

                foreach (RaycastHit hit in hits)
                {
                    hit.transform.gameObject.SendMessage("OnMouseDown", SendMessageOptions.DontRequireReceiver);

#if (UNITY_EDITOR)
                    if (JCS_GameSettings.instance.DEBUG_MODE)
                    {
                        // print the name of the detected transform.
                        JCS_Debug.PrintName(hit.transform);
                    }
#endif
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
