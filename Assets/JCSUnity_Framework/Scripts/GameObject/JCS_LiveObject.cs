/**
 * $File: JCS_LiveObject.cs $
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
    /// Living thing related object.
    /// </summary>
    public class JCS_LiveObject 
        : MonoBehaviour
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables
        [Header("** Check Variables **")]
        [SerializeField] private bool mIsPlayer = false;
        [SerializeField] private bool mCanDamage = true;

        [Header("** Runtime Variables **")]
        [SerializeField] private int mHP = 0;
        [SerializeField] private int mMP = 0;

        [Tooltip("Do the damage text effect after damage?")]
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
        public bool IsPlayer { get { return this.mIsPlayer; } }

        //========================================
        //      Unity's function
        //------------------------------
        public void Start()
        {
            // auto detect to see if this gameobject player or not.
            JCS_Player p = this.GetComponent<JCS_Player>();

            // if found it return true!
            if (p != null)
                mIsPlayer = true;
            else
                mIsPlayer = false;
        }

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

            // TODO(JenChieh): Do the dead animation

            Destroy(this.gameObject);
        }
        public void ApplyDamageText(int minDamage, int maxDamage, Vector2 pos, int hit = 1, int criticalChance = 0)
        {
            int[] damages = null;

            // Damage text version
            if (mDamageTextEffect)
            {

                JCS_MixDamageTextPool mixTP = JCS_GameManager.instance.GetMixDamageTextPool();
                if (mixTP == null)
                {
                    JCS_GameErrors.JcsErrors("JCS_LiveObject", -1, "There is no Mix Damage Text Pool in the scene. Consider to grab one?");
                    return;
                }

                damages = mixTP.DamageTextSpawnerSimple(minDamage, maxDamage, pos, hit, criticalChance);
            }
            // Do damage without damage text.
            else {

            }

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
