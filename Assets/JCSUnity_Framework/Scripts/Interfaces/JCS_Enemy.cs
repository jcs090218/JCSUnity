/**
 * $File: JCS_Enemy.cs $
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

    public class JCS_Enemy 
        : MonoBehaviour
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables
        [Header("** Check Variables **")]
        [SerializeField] private bool mCanDamage = true;

        [Header("** Runtime Variables **")]
        [SerializeField] private int mHP = 0;
        [SerializeField] private int mMP = 0;

        [SerializeField] private bool mDamageTextEffect = true;

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------
        public bool CanDamage { get { return this.mCanDamage; } set { this.mCanDamage = value; } }
        public bool DamageTextEffect { get { return this.mDamageTextEffect; } set { this.mDamageTextEffect = value; } }
        public void SetHP(int val)
        {
            if (val < 0)
                val = 0;
            this.mHP = val;
        }
        public void SetMP(int val)
        {
            if (val < 0)
                val = 0;
            this.mMP = val;
        }

        //========================================
        //      Unity's function
        //------------------------------

        //========================================
        //      Self-Define
        //------------------------------
        //----------------------
        // Public Functions

        public void DeltaMP(int val)
        {
            mMP += val;
        }
        public void Dead()
        {
            mHP = 0;
            mMP = 0;
            mCanDamage = false;

            Destroy(this.gameObject);
        }
        public void ApplyDamageText(int minDamage, int maxDamage, Vector2 pos, int hit = 1, int criticalChance = 0)
        {
            JCS_MixDamageTextPool mixTP = JCS_GameManager.instance.GetMixDamageTextPool();
            if (mixTP == null)
            {
                JCS_GameErrors.JcsErrors("JCS_Enemy", -1, "There is no Mix Damage Text Pool in the scene. Consider to grab one?");
                return;
            }

            int[] damages = mixTP.DamageTextSpawnerSimple(minDamage, maxDamage, pos, hit, criticalChance);

            ReceiveDamage(damages);
        }

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions
        private void ApplyDamage(int damage)
        {
            mHP -= damage;
            if (mHP <= 0)
            {
                // Enemy is Dead
                Dead();
            }
        }
        private void ReceiveDamage(int[] damages)
        {
            foreach (int damage in damages)
            {
                if (!mCanDamage)
                    break;

                ApplyDamage(damage);
            }
        }

    }
}
