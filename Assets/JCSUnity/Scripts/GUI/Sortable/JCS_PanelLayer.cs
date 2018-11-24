/**
 * $File: JCS_PanelLayer.cs $
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
    /// Sort the panels so they will have correct render order.
    /// 
    /// NOTE(jenchieh): This will always be ontop of any other 
    /// GUI, so use this carefully!
    /// </summary>
    [RequireComponent(typeof(JCS_PanelRoot))] 
    public class JCS_PanelLayer
        : JCS_SortingObject
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
        private void Start()
        {
            // if already sorted, return.
            if (this.Sorted)
                return;

            // get all the jcs_panellayer in the scene
            JCS_PanelLayer[] jcspls = (JCS_PanelLayer[])Resources.FindObjectsOfTypeAll(typeof(JCS_PanelLayer));

            JCS_Sort<JCS_PanelLayer> jcsS = new JCS_Sort<JCS_PanelLayer>();

            jcsS.AddAll(jcspls);

            jcspls = jcsS.InsertionSort();

            OriganizeChildOrder(jcspls);
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

        /// <summary>
        /// Re-order all the object.
        /// </summary>
        /// <param name="arr">sorted object. </param>
        private void OriganizeChildOrder(JCS_PanelLayer[] arr)
        {
            for (int index = 0;
                index < arr.Length;
                ++index)
            {
                // this will make gui ontop of each other.
                JCS_Utility.MoveToTheLastChild(arr[index].transform);

                // make sure is sorted already.
                arr[index].Sorted = true;
            }
        }

    }
}
