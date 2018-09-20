/**
 * $File: JCS_IndieManager.cs $
 * $Date: 2017-05-16 20:02:48 $
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
    /// Game independent manager tag.
    /// </summary>
    public class JCS_IndieManager
        : MonoBehaviour
    {

        /*******************************************/
        /*            Public Variables             */
        /*******************************************/
        public static JCS_IndieManager instance = null;

        /*******************************************/
        /*           Private Variables             */
        /*******************************************/

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
