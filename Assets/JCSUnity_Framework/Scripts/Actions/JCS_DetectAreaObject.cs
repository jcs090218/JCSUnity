/**
 * $File: JCS_DetectAreaObject.cs $
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
    /// Tag for "JCS_DetectAreaAction"
    /// </summary>
    public class JCS_DetectAreaObject
        : MonoBehaviour
    {
        //----------------------
        // Public Variables

        //----------------------
        // Private Variables
        [Header("** Check Variables **")]
        [SerializeField] private bool mIsEnemy = false;
        private JCS_Enemy mEnemy = null;

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------
        public bool IsEnemy { get { return this.mIsEnemy; } set { this.mIsEnemy = value; } }
        public JCS_Enemy GetEnemy() { return this.mEnemy; }

        //========================================
        //      Unity's function
        //------------------------------
        private void Awake()
        {
            mEnemy = this.GetComponent<JCS_Enemy>();
            if (mEnemy == null)
                return;

            mIsEnemy = true;
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
