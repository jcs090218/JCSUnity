/**
 * $File: JCS_GUIComponentLayer.cs $
 * $Date: 2016-11-20 21:58:21 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;


namespace JCSUnity
{
    /// <summary>
    /// This will only do under the jcs panel layer.
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public class JCS_GUIComponentLayer
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

            // get the parent.
            Transform parentObj = this.transform.parent;

            if (parentObj != null)
            {
                // get all the jcs_panellayer in the scene
                JCS_GUIComponentLayer[] jcspls = parentObj.GetComponentsInChildren<JCS_GUIComponentLayer>();

                JCS_Sort<JCS_GUIComponentLayer> jcsS = new JCS_Sort<JCS_GUIComponentLayer>();

                jcsS.AddAll(jcspls);

                jcspls = jcsS.InsertionSort();

                OriganizeChildOrder(jcspls);
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

        /// <summary>
        /// Re-order all the object.
        /// </summary>
        /// <param name="arr"> sorted object. </param>
        private void OriganizeChildOrder(JCS_GUIComponentLayer[] arr)
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
