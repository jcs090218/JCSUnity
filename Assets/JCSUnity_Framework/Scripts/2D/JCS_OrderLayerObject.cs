/**
 * $File: JCS_OrderLayerObject.cs $
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
    [RequireComponent(typeof(SpriteRenderer))]
    public class JCS_OrderLayerObject 
        : MonoBehaviour
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables

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
            SpriteRenderer spriteRenderer = this.GetComponent<SpriteRenderer>();
            JCS_OrderLayer jcsOrderLayer = this.GetComponentInParent<JCS_OrderLayer>();
            if (jcsOrderLayer != null)
            {
                spriteRenderer.sortingOrder = jcsOrderLayer.GetOrderLayer();
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
