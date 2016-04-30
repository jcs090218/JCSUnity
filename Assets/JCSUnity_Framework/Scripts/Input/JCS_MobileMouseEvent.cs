/**
 * $File: $
 * $Date: $
 * $Reveision: $
 * $Creator: Jen-Chieh Shen $
 */
using UnityEngine;
using System.Collections;

namespace JCSUnity
{
    public class JCS_MobileMouseEvent : MonoBehaviour
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
