/**
 * $File: JCS_2DLadder.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using System.Collections.Generic;
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// 2D ladder object, that player could climb on.
    /// </summary>
    public class JCS_2DLadder : JCS_2DClimbableObject
    {
        /* Variables */

        [Header("** Check Variable (JCS_2DLadder) **")]

        [SerializeField]
        protected List<JCS_2DSideScrollerPlayer> mSSPlayers = null;


        /* Setter & Getter */

        /* Functions */

        protected override void Awake()
        {
            base.Awake();

            if (mPositionPlatform == null)
            {
                JCS_Debug.LogError(
                    "U have a ladder without a platform/ground to lean on.");
            }
        }

        protected virtual void Update()
        {
            // TODO(jenchieh): see if we can save some performance here.
            ClimbableUpdate();
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            JCS_2DSideScrollerPlayer player = other.GetComponent<JCS_2DSideScrollerPlayer>();
            if (player == null)
                return;

            JCS_ClimbableManager cm = JCS_ClimbableManager.instance;

            AddSafe(player);

            bool isTopOfBox = JCS_Physics.TopOfBox(
                            player.GetCharacterController(),
                            mPositionPlatform.GetPlatformCollider());

            if (isTopOfBox)
            {
                // show character behind the ladder
                int backOrderLayer = OrderLayerObject.sortingOrder - cm.SORTING_ORDER_BEHIND_OFFSET;
                SetPlayerSortingOrder(player, backOrderLayer);
            }
            else
            {
                // show character infront
                int frontOrderLayer = OrderLayerObject.sortingOrder + cm.SORTING_ORDER_INFRONT_OFFSET;
                SetPlayerSortingOrder(player, frontOrderLayer);
            }

            player.CanLadder = true;
            player.CanRope = false;
            player.ClimbableObject = this;
        }

        protected virtual void OnTriggerExit(Collider other)
        {
            JCS_2DSideScrollerPlayer player = other.GetComponent<JCS_2DSideScrollerPlayer>();
            if (player == null)
                return;

            player.ClimbableObject = null;
            player.GetCharacterAnimator().PlayAnimationInFrame();
            player.CanLadder = false;

            RemoveNullPlayerAndSelf(player);
        }

        /// <summary>
        /// Something that needed to check in update can be design here.
        /// This function should get call by when the player is done climbing.
        /// </summary>
        public override void ClimbableUpdate()
        {
            JCS_ClimbableManager cm = JCS_ClimbableManager.instance;

            foreach (JCS_2DSideScrollerPlayer player in mSSPlayers)
            {
                if (player.isGrounded())
                {
                    player.CanLadder = true;
                    player.CanRope = false;
                    player.ClimbableObject = this;

                    continue;
                }

                if (player.CharacterState == JCS_2DCharacterState.CLIMBING)
                {
                    // show character infront
                    int frontOrderLayer = OrderLayerObject.sortingOrder + cm.SORTING_ORDER_INFRONT_OFFSET;
                    SetPlayerSortingOrder(player, frontOrderLayer);


                    bool isTopOfBox = JCS_Physics.TopOfBox(
                        player.GetCharacterController(),
                        mPositionPlatform.GetPlatformCollider());

                    /* Check top of the platform */
                    if (isTopOfBox)
                    {
                        player.ClimbableObject = null;
                        player.GetCharacterAnimator().PlayAnimationInFrame();
                        player.CanLadder = false;
                        player.VelY = 0;
                        player.JustClimbOnTopOfBox = true;

                        // show character behind the ladder
                        int backOrderLayer = OrderLayerObject.sortingOrder - cm.SORTING_ORDER_BEHIND_OFFSET;
                        SetPlayerSortingOrder(player, backOrderLayer);
                    }
                }
            }
        }

        /// <summary>
        /// Add object with no duplicate.
        /// </summary>
        /// <param name="player"></param>
        private void AddSafe(JCS_2DSideScrollerPlayer player)
        {
            for (int index = 0; index < mSSPlayers.Count; ++index)
            {
                // already in here.
                if (mSSPlayers[index] == player)
                    return;
            }

            mSSPlayers.Add(player);
        }

        /// <summary>
        /// If the player is no longer exists, remove the empty slot.
        /// </summary>
        /// <param name="player"></param>
        private void RemoveNullPlayerAndSelf(JCS_2DSideScrollerPlayer player = null)
        {
            for (int index = 0; index < mSSPlayers.Count; ++index)
            {
                // remove itself.
                if (mSSPlayers[index] == null)
                    mSSPlayers.RemoveAt(index);

                if (mSSPlayers[index] == player)
                    mSSPlayers.RemoveAt(index);
            }
        }

        /// <summary>
        /// Set the player in specific sorting order 
        /// layer. (JCS_OrderLayer)
        /// </summary>
        /// <param name="player"> player to sort </param>
        /// <param name="layer"> layer number/id. </param>
        private void SetPlayerSortingOrder(JCS_2DSideScrollerPlayer player, int layer)
        {
            player.OrderLayerObject.SetObjectParentToOrderLayerByOrderLayerIndex(
                layer);
        }
    }
}
