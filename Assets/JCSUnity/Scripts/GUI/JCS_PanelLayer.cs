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
    /// This will always be ontop of any other GUI!!
    /// so use this carefully.
    /// </summary>
    [RequireComponent(typeof(JCS_PanelRoot))] 
    public class JCS_PanelLayer
        : JCS_SortingObject
    {

        //----------------------
        // Public Variables
        public static bool DO_SORTING = false;

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
            if (DO_SORTING)
                return;

            // get all the jcs_panellayer in the scene
            JCS_PanelLayer[] jcspls = (JCS_PanelLayer[])Resources.FindObjectsOfTypeAll(typeof(JCS_PanelLayer));

            JCS_Sort jcsS = new JCS_Sort();

            jcsS.AddAll(jcspls);

            jcspls = (JCS_PanelLayer[])jcsS.InsertionSort();

            OriganizeChildOrder(jcspls);

            DO_SORTING = true;
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
        private void OriganizeChildOrder(JCS_PanelLayer[] arr)
        {
            for (int index = 0;
                index < arr.Length;
                ++index)
            {
                JCS_Utility.MoveToTheLastChild(arr[index].transform);
            }
        }

    }
}
