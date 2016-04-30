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
    [RequireComponent(typeof(SpriteRenderer))]
    public class JCS_OrderLayerObject : MonoBehaviour
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
