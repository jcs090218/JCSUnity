/**
 * $File: JCS_CashObject.cs $
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

    /// <summary>
    /// In game item stand for cash.
    /// 
    /// Not sure we need this class...
    /// </summary>
    public class JCS_CashObject
        : JCS_Item
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables

        //----------------------
        // Protected Variables
        [Header("** Initialize Variables (JCS_CashObject) **")]
        [Tooltip("Value in the game.")]
        [SerializeField] protected int mCashValue = 1;

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
        public override void SubclassCallBack(Collider other)
        {
            // current empty base function...
            base.SubclassCallBack(other);

            // apply value to gold system.
        }

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions

    }
}
