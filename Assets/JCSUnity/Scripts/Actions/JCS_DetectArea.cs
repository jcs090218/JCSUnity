/**
 * $File: JCS_DetectArea.cs $
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
    /// The Collider to Detect "JCS_DetectAreaObject"!!
    /// </summary>
    public class JCS_DetectArea
        : MonoBehaviour
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables
        private JCS_DetectAreaAction mDetectAreaAction = null;

        [Header("** Check Variables **")]
        [SerializeField] private Collider mCollider = null;

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------
        public void SetAction(JCS_DetectAreaAction da) { this.mDetectAreaAction = da; }

        //========================================
        //      Unity's function
        //------------------------------

        private void Awake()
        {
            mCollider = this.GetComponent<Collider>();

            if (mCollider == null)
            {
                JCS_Debug.LogError(
                     "No collider attached to do the dectect action...");
            }
            else
                mCollider.enabled = false;
        }

        private void Start()
        {
            if (mCollider != null)
                mCollider.enabled = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            // check if anything we target are in there
            JCS_DetectAreaObject jcsDo = other.GetComponent<JCS_DetectAreaObject>();
            if (jcsDo == null)
                return;

            // if cannot damage
            if (!jcsDo.GetLiveObject().CanDamage)
                return;

            if (!JCS_GameSettings.instance.TRIBE_DAMAGE_EACH_OTHER)
            {
                // if both player does not need to add in to list.
                // or if both enemy does not need to add in to list.
                if (jcsDo.GetLiveObject().IsPlayer == mDetectAreaAction.GetLiveObject().IsPlayer)
                    return;
            }

            // add it to the list
            mDetectAreaAction.AddDetectedObject(jcsDo);
        }

        private void OnTriggerExit(Collider other)
        {
            // if the target thing left
            JCS_DetectAreaObject jcsDo = other.GetComponent<JCS_DetectAreaObject>();
            if (jcsDo == null)
                return;

            // remove from the list.
            mDetectAreaAction.RemoveDetectedObject(jcsDo);
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
