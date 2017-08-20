
using System;
/**
* $File: JCS_NetworkSettings.cs $
* $Date: 2017-08-20 07:08:46 $
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
    /// Store all the network settings.
    /// </summary>
    public class JCS_NetworkSettings
        : JCS_Settings<JCS_NetworkSettings>
    {

        /*******************************************/
        /*            Public Variables             */
        /*******************************************/

        [Header("** Online Game Configuration **")]
        public bool ONLINE_MODE = false;

        public string HOST_NAME = "127.0.0.1";
        public int PORT = 5454;

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
            instance = CheckSingleton(instance, this);
        }

        /*******************************************/
        /*              Self-Define                */
        /*******************************************/
        //----------------------
        // Public Functions

        //----------------------
        // Protected Functions

        /// <summary>
        /// Instead of Unity Engine's scripting layer's DontDestroyOnLoad.
        /// I would like to use own define to transfer the old instance
        /// to the newer instance.
        /// 
        /// Every time when unity load the scene. The script have been
        /// reset, in order not to lose the original setting.
        /// transfer the data from old instance to new instance.
        /// </summary>
        /// <param name="_old"> old instance </param>
        /// <param name="_new"> new instance </param>
        protected override void TransferData(JCS_NetworkSettings _old, JCS_NetworkSettings _new)
        {
            _new.ONLINE_MODE = _old.ONLINE_MODE;
            _new.HOST_NAME = _old.HOST_NAME;
            _new.PORT = _old.PORT;
        }

        //----------------------
        // Private Functions

    }
}
