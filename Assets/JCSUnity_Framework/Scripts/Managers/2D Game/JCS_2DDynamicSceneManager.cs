/**
 * $File: JCS_2DDynamicSceneManager.cs $
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

    public class JCS_2DDynamicSceneManager
        : MonoBehaviour
    {

        //----------------------
        // Public Variables
        public static JCS_2DDynamicSceneManager instance = null;

        //----------------------
        // Private Variables
        [SerializeField] private JCS_OrderLayer[] mJCSOrderLayer = null;

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
            instance = this;
            mJCSOrderLayer = (JCS_OrderLayer[])Resources.FindObjectsOfTypeAll(typeof(JCS_OrderLayer));
        }


        //========================================
        //      Self-Define
        //------------------------------
        //----------------------
        // Public Functions
        public JCS_OrderLayer GetOrderLayerByOrderLayerIndex(int orderLayerIndex)
        {
            foreach (JCS_OrderLayer jcsol in mJCSOrderLayer)
            {
                // find the order layer with the index passed in!
                if (jcsol.GetOrderLayer() == orderLayerIndex)
                    return jcsol;
            }

            JCS_GameErrors.JcsErrors(
                "JCS_2DDynamicSceneManager",
                -1,
                "Does not found the order layer u want.");

            return null;
        }

        public void SetObjectParentToOrderLayerByOrderLayerIndex(ref JCS_OrderLayerObject jcsOlo, int orderLayerIndex)
        {
            // get the order layer by order layer index!
            JCS_OrderLayer jcsol = GetOrderLayerByOrderLayerIndex(orderLayerIndex);

            // set parent
            jcsOlo.transform.SetParent(jcsol.transform);

            // set order layer to the pass in object.
            jcsOlo.SetOrderLayer(orderLayerIndex);
        }

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions

    }
}
