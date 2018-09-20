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
        protected bool mEffect = false;


        [Header("** Runtime Variables (JCS_2DEffect) **")]

        [Tooltip("")]
        [SerializeField]
        protected bool mStopInputWhileThisEffect = false;
        

        public bool Effect { get { return this.mEffect; } set { this.mEffect = value; } }

    }
}
