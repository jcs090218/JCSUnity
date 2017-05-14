/**
 * $File: JCS_2DActiveSkillAction.cs $
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
    /// Spawn a skill and destroy itself after 
    /// done displaying the skill effect.
    /// </summary>
    public class JCS_2DActiveSkillAction
        : MonoBehaviour
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables
        [Header("** Runtime Variables **")]
        [Tooltip("Key to active this skill.")]
        [SerializeField] private KeyCode mKeyCode = KeyCode.None;
        [SerializeField] private int mOrderLayer = 15;
        [SerializeField] private RuntimeAnimatorController mSkillAnim = null;

        [Header("** Spawn Settings **")]
        [SerializeField] private bool mSamePosition = true;
        [SerializeField] private bool mSameRotation = true;

        [Header("** Runtime Settings **")]
        [SerializeField]
        private bool mStayWithActiveTarget = true;

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
            if (JCS_Input.GetKeyDown(mKeyCode))
                ActiveSkill();
        }

        //========================================
        //      Self-Define
        //------------------------------
        //----------------------
        // Public Functions

        /// <summary>
        /// Either use keycode or call it with script!
        /// </summary>
        public void ActiveSkill()
        {
            if (mSkillAnim == null)
            {
                JCS_Debug.LogReminders(
                    this, 
                    "Assigning active skill action without animation is not allowed.");

                return;
            }

            GameObject obj = JCS_Utility.SpawnAnimateObject(mSkillAnim, mOrderLayer);

            if (mSamePosition)
                obj.transform.position = this.transform.position;
            if (mSameRotation)
                obj.transform.rotation = this.transform.rotation;

            // if stay with player, simple set the position to
            // same position and set to child so it will follows
            // the active target!
            if (mStayWithActiveTarget)
                obj.transform.SetParent(this.transform);


            // add anim death event,
            // so when animation ends destroy itself.
            obj.AddComponent<JCS_DestroyAnimEndEvent>();
        }

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions


    }
}
