/**
 * $File: JCS_2DAIStateSystem.cs $
 * $Date: 2016-09-05 21:23:59 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information
 *	                 Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Handle AI action and allow scripter to use to switch the state using 
    /// State Pattern.
    /// </summary>
    public class JCS_2DAIStateSystem : MonoBehaviour
    {
        /* Variables */

        /// <summary>
        /// List of AI movement state.
        /// </summary>
        public enum JCS_AIMoveActionType
        {
            NONE,

            WALK,
            JUMP,
            FLY,
            SWIM
        };

        [Separator("Check Variables (JCS_AIStateSystem)")]

        [Tooltip("State of the AI move action.")]
        [SerializeField]
        [ReadOnly]
        private JCS_AIMoveActionType mAIMoveActionType = JCS_AIMoveActionType.NONE;

        private JCS_Vec<JCS_AIAction> mAIActions = null;

        /* Setter & Getter */

        public JCS_AIMoveActionType GetAIMoveActionType() { return mAIMoveActionType; }

        /* Functions */

        private void Awake()
        {
            mAIActions = new JCS_Vec<JCS_AIAction>();

            // add all the ai action into the array.
            JCS_AIAction[] actions = GetComponents<JCS_AIAction>();
            for (int index = 0; index < actions.Length; ++index)
            {
                mAIActions.push(actions[index]);
            }
        }

#if UNITY_EDITOR
        private void Update()
        {
            if (JCS_Input.GetKeyDown(KeyCode.F))
                SwitchAIMoveState(JCS_AIMoveActionType.FLY);
            if (JCS_Input.GetKeyDown(KeyCode.W))
                SwitchAIMoveState(JCS_AIMoveActionType.WALK);
        }
#endif

        /// <summary>
        /// Switch AI move action state.
        /// </summary>
        /// <param name="type"> type of the ai move action. </param>
        public void SwitchAIMoveState(JCS_AIMoveActionType type)
        {
            switch (type)
            {
                case JCS_AIMoveActionType.NONE:
                    {
                        DisableActions();
                    }
                    break;
                case JCS_AIMoveActionType.WALK:
                    {
                        // try to get the component on this transform.
                        JCS_2DWalkAction wa = GetComponent<JCS_2DWalkAction>();
                        if (wa == null)  // if nothing here.
                        {
                            // add it on to it
                            wa = gameObject.AddComponent<JCS_2DWalkAction>();

                            // and push into array.
                            mAIActions.push(wa);
                        }

                        // disable all the action
                        DisableActions();

                        // enable this one.
                        wa.enabled = true;
                    }
                    break;
                case JCS_AIMoveActionType.JUMP:
                    {
                        JCS_2DJumpAction ja = GetComponent<JCS_2DJumpAction>();
                        if (ja == null)
                        {
                            ja = gameObject.AddComponent<JCS_2DJumpAction>();
                            mAIActions.push(ja);
                        }
                        DisableActions();
                        ja.enabled = true;
                    }
                    break;
                case JCS_AIMoveActionType.FLY:
                    {
                        JCS_2DFlyAction fa = GetComponent<JCS_2DFlyAction>();
                        if (fa == null)
                        {
                            fa = gameObject.AddComponent<JCS_2DFlyAction>();
                            mAIActions.push(fa);
                        }
                        DisableActions();
                        fa.enabled = true;
                    }
                    break;
                case JCS_AIMoveActionType.SWIM:
                    {
                        JCS_2DSwimAction sa = GetComponent<JCS_2DSwimAction>();
                        if (sa == null)
                        {
                            sa = gameObject.AddComponent<JCS_2DSwimAction>();
                            mAIActions.push(sa);
                        }
                        DisableActions();
                        sa.enabled = true;
                    }
                    break;
            }

            mAIMoveActionType = type;
        }

        /// <summary>
        /// Disable all the actions in the array.
        /// </summary>
        private void DisableActions()
        {
            for (int index = 0; index < mAIActions.length; ++index)
            {
                JCS_AIAction aa = mAIActions.at(index);
                aa.enabled = false;
            }
        }
    }
}
