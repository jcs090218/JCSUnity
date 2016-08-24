/**
 * $File: JCS_Managers.cs $
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
    /// Interface of manages.
    /// 
    /// Manager have to be in every scene 
    /// in Unity Engine's scene system.
    /// </summary>
    public class JCS_Managers<T>
        : MonoBehaviour
    {

        //----------------------
        // Public Variables
        public static T instance = default(T);

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
