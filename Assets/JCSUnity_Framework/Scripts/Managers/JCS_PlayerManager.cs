/**
 * $File: JCS_PlayerManager.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information $
 *		                Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace JCSUnity
{

    public class JCS_PlayerManager 
        : MonoBehaviour
    {

        //----------------------
        // Public Variables
        public static JCS_PlayerManager instance = null;

        //----------------------
        // Private Variables
        [SerializeField] private List<JCS_Player> mPlayers = null;

        // current player that are active
        private JCS_Player mActivePlayer = null;

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------
        public JCS_Player GetActivePlayer() { return this.mActivePlayer; }
        public List<JCS_Player> GetJCSPlayerList() { return this.mPlayers; }
        public JCS_Player GetJCSPlayerAt(int index) { return this.mPlayers[index]; }

        //========================================
        //      Unity's function
        //------------------------------
        private void Awake()
        {
            instance = this;
            mPlayers = new List<JCS_Player>();
        }

        private void Start()
        {
            if (JCS_GameSettings.instance.PLAYER_IGNORE_EACH_OTHER)
            {
                // Make all the player ignore each other
                for (int index = 0;
                    index < mPlayers.Count;
                    ++index)
                {
                    for (int pairIndex = index + 1;
                        pairIndex < mPlayers.Count;
                        ++pairIndex)
                    {

                        Physics.IgnoreCollision(
                                mPlayers[index].GetCharacterController(),
                                mPlayers[pairIndex].GetCharacterController(), true);
                    }
                }
            }

            ActiveOnePlayer(JCS_GameManager.instance.GetJCSPlayer());
        }

#if (UNITY_EDITOR)
        private void Update()
        {
            if (JCS_GameSettings.instance.GAME_TYPE == JCS_GameType.GAME_2D)
                PlayerManageTest();
        }

        private void PlayerManageTest()
        {
            if (JCS_Input.GetKeyDown(KeyCode.L))
            {
                JCS_2DCamera cam = (JCS_2DCamera)JCS_GameManager.instance.GetJCSCamera();
                cam.SetFollowTarget(mPlayers[0].transform);
                JCS_GameManager.instance.SetJCSPlayer(mPlayers[0]);
                ActiveOnePlayer(0);
            }
            if (JCS_Input.GetKeyDown(KeyCode.K))
            {
                JCS_2DCamera cam = (JCS_2DCamera)JCS_GameManager.instance.GetJCSCamera();
                cam.SetFollowTarget(mPlayers[1].transform);
                JCS_GameManager.instance.SetJCSPlayer(mPlayers[1]);
                ActiveOnePlayer(1);
            }
        }

#endif

        //========================================
        //      Self-Define
        //------------------------------
        //----------------------
        // Public Functions
        public void AddPlayerToManage(JCS_Player player)
        {
            if (player == null)
            {
                JCS_GameErrors.JcsErrors("JCS_PlayerManager");
                return;
            }

            mPlayers.Add(player);
        }

        public void ActiveOnePlayer(int index)
        {
            ActiveOnePlayer(mPlayers[index]);
        }
        public void ActiveOnePlayer(JCS_Player player)
        {
            foreach (JCS_Player p in mPlayers)
            {
                if (p == player)
                {
                    p.ControlEnable(true);
                    mActivePlayer = p;
                }
                else
                    p.ControlEnable(false);
            }
        }
        public void IgnorePhysicsToAllPlayer(Collider cc)
        {
            // Make all the player ignore each other
            for (int index = 0;
                index < mPlayers.Count;
                ++index)
            {
                Physics.IgnoreCollision(
                            mPlayers[index].GetCharacterController(), 
                            cc, true);
            }
        }
        public void EnablePhysicsToAllPlayer(Collider cc)
        {
            // Make all the player ignore each other
            for (int index = 0;
                index < mPlayers.Count;
                ++index)
            {
                Physics.IgnoreCollision(
                            mPlayers[index].GetCharacterController(),
                            cc, false);
            }
        }

        /// <summary>
        /// Comparing to the transform see if the same
        /// transform as player did. (typeid method)
        /// </summary>
        /// <param name="trans"></param>
        /// <returns></returns>
        public bool IsPlayerTransform(Transform trans)
        {
            if (trans == null)
                return false;

            foreach (JCS_Player player in mPlayers)
            {
                if (trans == player.transform)
                    return true;
            }

            return false;
        }
        public bool IsActivePlayerTransform(Transform tran)
        {
            if (tran == mActivePlayer.transform)
                return true;

            return false;
        }
        public void IgnorePhysicsToAllExceptActivePlayer(Collider cc)
        {
            // ignore all
            IgnorePhysicsToAllPlayer(cc);

            if (mActivePlayer == null)
                return;

            // active
            Physics.IgnoreCollision(
                            mActivePlayer.GetCharacterController(),
                            cc, true);
        }

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions

    }
}
