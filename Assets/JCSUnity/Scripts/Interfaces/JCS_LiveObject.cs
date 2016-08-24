/**
 * $File: JCS_LiveObject.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                    Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;


namespace JCSUnity
{

    /// <summary>
    /// 
    /// </summary>
    public abstract class JCS_LiveObject
        : MonoBehaviour
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables

        //----------------------
        // Protected Variables

        [Header("** Runtime Variables (JCS_LiveObject) **")]
        [Tooltip(@"Health, Auto add, for better design plz 
add JCS_LiquidBarInfo manually.")]
        [SerializeField]
        protected JCS_LiquidBarInfo mHPInfo = null;

        [Tooltip(@"Mana, Auto add, for better design plz 
add JCS_LiquidBarInfo manually.")]
        [SerializeField]
        protected JCS_LiquidBarInfo mMPInfo = null;

        [Tooltip(@"Experience, Auto add, for better design plz 
add JCS_LiquidBarInfo manually.")]
        [SerializeField]
        protected JCS_LiquidBarInfo mEXPInfo = null;


        // HP before really apply the attack
        protected int mPreCalHP = 0;

        [Tooltip("How much damage could knock back?")]
        [SerializeField] [Range(0, 999999999)]
        protected int mKB = 0;

        // Should we set it to global?
        [Tooltip("How much force to knock back?")]
        [SerializeField] [Range(1, 200)]
        protected float mKnockBackForce = 1;

        [Tooltip("Do the damage text effect after damage?")]
        [SerializeField]
        protected bool mDamageTextEffect = true;

        //========================================
        //      setter / getter
        //------------------------------
        public JCS_LiquidBarInfo HPInfo { get { return this.mHPInfo; } }
        public JCS_LiquidBarInfo MPInfo { get { return this.mMPInfo; } }
        public JCS_LiquidBarInfo EXPInfo { get { return this.mEXPInfo; } }
        public int HP { get { return this.mHPInfo.CurrentValue; } set { this.mHPInfo.CurrentValue = value; } }
        public int MP { get { return this.mMPInfo.CurrentValue; } set { this.mMPInfo.CurrentValue = value; } }
        public int EXP { get { return this.mEXPInfo.CurrentValue; } set { this.mEXPInfo.CurrentValue = value; } }
        public void SetHP(int val)
        {
            if (val < 0)
                val = 0;
            this.HP = val;
        }
        public void SetMP(int val)
        {
            if (val < 0)
                val = 0;
            this.MP = val;
        }

        //========================================
        //      Unity's function
        //------------------------------
        protected virtual void Awake()
        {
            // Auto add, for better design plz 
            // add "JCS_LiquidBarInfo" manually.
            if (mHPInfo == null)
                mHPInfo = this.gameObject.AddComponent<JCS_LiquidBarInfo>();
            mHPInfo.TagName = "HP";


            if (mMPInfo == null)
                mMPInfo = this.gameObject.AddComponent<JCS_LiquidBarInfo>();
            mMPInfo.TagName = "MP";


            if (mEXPInfo == null)
            {
                mEXPInfo = this.gameObject.AddComponent<JCS_LiquidBarInfo>();
                mEXPInfo.CurrentValue = 0;
            }
            mEXPInfo.TagName = "EXP";
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
