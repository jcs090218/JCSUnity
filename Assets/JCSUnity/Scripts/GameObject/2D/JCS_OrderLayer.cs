/**
 * $File: JCS_OrderLayer.cs $
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
    /// Scene layer.
    /// </summary>
    public class JCS_OrderLayer 
        : MonoBehaviour
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables
        [SerializeField] private int mOrderLayer = 0;
        [SerializeField] private float mLayerFriction = 0;


        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------
        public int GetOrderLayer() { return this.mOrderLayer; }

        //========================================
        //      Unity's function
        //------------------------------
        private void LateUpdate()
        {
            if (mLayerFriction == 0)
                return;

            JCS_Camera cam = JCS_Camera.main;
            if (cam == null)
                return;

            Vector3 newPos = this.transform.position;
            newPos.x += cam.Velocity.x / mLayerFriction * Time.deltaTime;
            newPos.y += cam.Velocity.y / mLayerFriction * Time.deltaTime;
            this.transform.position = newPos;
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
