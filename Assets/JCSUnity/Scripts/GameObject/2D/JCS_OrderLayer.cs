﻿/**
 * $File: JCS_OrderLayer.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// Scene layer.
    /// </summary>
    public class JCS_OrderLayer : MonoBehaviour
    {
        /* Variables */

        [Header("** Runtime Variables (JCS_OrderLayer) **")]

        [Tooltip("Rendering order.")]
        [SerializeField]
        private int mOrderLayer = 0;

        [Tooltip("How fast this layer moves.")]
        [SerializeField]
        private float mLayerFriction = 0;

        [Tooltip("Type of the delta time.")]
        [SerializeField]
        private JCS_DeltaTimeType mDeltaTimeType = JCS_DeltaTimeType.DELTA_TIME;

        /* Setter & Getter */

        public int OrderLayer { get { return this.mOrderLayer; } }
        public JCS_DeltaTimeType DeltaTimeType { get { return this.mDeltaTimeType; } set { this.mDeltaTimeType = value; } }

        /* Functions */

        private void FixedUpdate()       /* Should use FixedUpdate if no jitter. */
        {
            if (mLayerFriction == 0)
                return;

            var cam = JCS_Camera.main;
            if (cam == null)
                return;

            float dt = JCS_Time.DeltaTime(mDeltaTimeType);

            Vector3 newPos = this.transform.position;
            newPos.x += cam.Velocity.x / mLayerFriction * dt;
            newPos.y += cam.Velocity.y / mLayerFriction * dt;
            this.transform.position = newPos;
        }
    }
}
