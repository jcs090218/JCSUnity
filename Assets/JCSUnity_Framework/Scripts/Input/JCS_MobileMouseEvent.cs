/**
 * $File: JCS_MobileMouseEvent.cs $
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
    public class JCS_MobileMouseEvent 
        : MonoBehaviour
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables
        [SerializeField] private bool mTouched = false;
        private float mRaycastDistance = 100.0f;
        [SerializeField] private Camera mCamera = null;

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
            if (mCamera == null)
                this.mCamera = JCS_GameManager.instance.GetJCSCamera().GetCamera();

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

                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, mRaycastDistance))
                    hit.transform.gameObject.SendMessage("OnMouseDown", SendMessageOptions.DontRequireReceiver);
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
