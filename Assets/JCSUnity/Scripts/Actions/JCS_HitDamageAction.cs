/**
 * $File: JCS_HitDamageAction.cs $
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
    /// Simulate the action when object damage the player
    /// by hitting with his body. (physically)
    /// </summary>
    [RequireComponent(typeof(BoxCollider))]
    // Object with this action must be live object!!
    [RequireComponent(typeof(JCS_2DLiveObject))]
    public class JCS_HitDamageAction
        : MonoBehaviour
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables

        // collider use to detect player to give damage
        private BoxCollider mBoxCollider = null;

        private JCS_AbilityFormat mAbilityFormat = null;

        private JCS_2DLiveObject mLiveObject = null;

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
            mBoxCollider = this.GetComponent<BoxCollider>();

            // try to get the ability format from this object.
            mAbilityFormat = this.GetComponent<JCS_AbilityFormat>();

            mLiveObject = this.GetComponent<JCS_2DLiveObject>();
        }

        private void OnTriggerEnter(Collider other)
        {
            JCS_2DLiveObject liveObject = other.GetComponent<JCS_2DLiveObject>();
            if (liveObject == null)
                return;

            DamageLiveObject(liveObject);
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

        private void DamageLiveObject(JCS_2DLiveObject liveObject)
        {
            // if cannot damage this live object than do nothing.
            if (!liveObject.CanDamage)
                return;

            if (!JCS_GameSettings.instance.TRIBE_DAMAGE_EACH_OTHER)
            {
                // if both player does not need to add in to list.
                // or if both enemy does not need to add in to list.
                if (liveObject.IsPlayer == mLiveObject.IsPlayer)
                    return;
            }

            if (mAbilityFormat == null)
            {
                JCS_GameErrors.JcsReminders(
                    "JCS_HitDamageAction",
                     
                    "You sure to not using any \"JCS_AbilityFormat\"?");

                return;
            }

            liveObject.ApplyDamageText(
                mAbilityFormat.GetMinDamage(),
                mAbilityFormat.GetMaxDamage(),
                this.transform.position,
                1,      // hit
                0);     // critical chance

            // -------------- PLAYER ---------------

            // see if the living object player.
            JCS_Player p = liveObject.GetComponent<JCS_Player>();
            if (p == null)
                return;

            // if the living object we are attacking is
            // player do the hit effect.
            p.Hit();
        }

    }
}
