/**
 * $File: JCS_ClimbableManager.cs $
 * $Date: 2017-05-16 19:49:31 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2017 by Shen, Jen-Chieh $
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace JCSUnity
{
    /// <summary>
    /// Manage all Climbable object in the scene. (Under Indie manager)
    /// </summary>
    public class JCS_ClimbableManager
        : MonoBehaviour
    {

        /*******************************************/
        /*            Public Variables             */
        /*******************************************/
        public static JCS_ClimbableManager instance = null;

        /*******************************************/
        /*           Private Variables             */
        /*******************************************/

        [Header("** Runtime Variables (JCS_ClimbableManager) **")]

        [Tooltip("")]
        [Range(0, 20)]
        public int SORTING_ORDER_INFRONT_OFFSET = 1;

        [Tooltip("")]
        [Range(0, 20)]
        public int SORTING_ORDER_BEHIND_OFFSET = 1;

        /*******************************************/
        /*           Protected Variables           */
        /*******************************************/

        /*******************************************/
        /*             setter / getter             */
        /*******************************************/

        /*******************************************/
        /*            Unity's function             */
        /*******************************************/
        private void Awake()
        {
            instance = this;
        }

        /*******************************************/
        /*              Self-Define                */
        /*******************************************/
        //----------------------
        // Public Functions

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions

    }
}
