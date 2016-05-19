/**
 * $File: JCS_2DRainy.cs $
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

    public class JCS_2DRainy
        : MonoBehaviour
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables
        [Header("** Initialize Variables **")]
        [SerializeField] private uint mNumOfParticle = 50;

        [Header("** Runtime Variables **")]
        [SerializeField]
        private RuntimeAnimatorController mRainAnim = null;

        [SerializeField]
        [Range(0, 10)] private float mRainDesity = 5;
        [SerializeField]
        [Range(-100, 100)] private float mWindSpeed = 0;

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

        }

        private void Update()
        {
            DoRainy();
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
        private void DoRainy()
        {
            if (mRainAnim == null)
                return;


        }

    }
}
