/**
 * $File: JCS_2DEffect.cs $
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
    /// 2D effect base class.
    /// </summary>
    public abstract class JCS_2DEffect
        : MonoBehaviour    
    {
        /* Variables */

        protected bool mEffect = false;

        [Header("** Runtime Variables (JCS_2DEffect) **")]

        [Tooltip("Stop receive input while this effect is active.")]
        [SerializeField]
        protected bool mStopInputWhileThisEffect = false;

        /* Setter & Getter */

        public bool Effect { get { return this.mEffect; } set { this.mEffect = value; } }

        /* Functions */

        public bool StopInputWhileThisEffect { get { return this.mStopInputWhileThisEffect; } set { this.mStopInputWhileThisEffect = value; } }
    }
}
