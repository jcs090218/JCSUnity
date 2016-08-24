/**
 * $File: JCS_PackageDataSettings.cs $
 * $Date: $
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
    /// 
    /// </summary>
    public class JCS_PackageDataSettings
        : MonoBehaviour
    {

        //----------------------
        // Public Variables
        public static JCS_PackageDataSettings instance = null;


        [Header("** Package Data Settings **")]

        [Tooltip("Package Data copyright information.")]
        public string CopyrightString = "Copyright 2016 JCSUnity, Taiwan";

        [Tooltip("Package Data version information.")]
        public string VersionString = "Package file v1.0";

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
            instance = this;
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
