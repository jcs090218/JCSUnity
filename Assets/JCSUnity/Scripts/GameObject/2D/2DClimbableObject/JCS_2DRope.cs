/**
 * $File: JCS_2DRope.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace JCSUnity
{

    /// <summary>
    /// 2D rope object, that player could climb on.
    /// </summary>
    public class JCS_2DRope
        : JCS_2DClimbableObject
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables

        //----------------------
        // Protected Variables

        [Header("** Check Variable (JCS_2DRope) **")]

        [SerializeField]
        protected List<JCS_2DSideScrollerPlayer> mSSPlayers = null;


        [Header("** Initilaize Variable (JCS_2DRope) **")]

        [Tooltip("Ground/Platform the ladder lean on.")]
        [SerializeField]
        protected JCS_2DPositionPlatform mPositionPlatform = null;

        //========================================
        //      setter / getter
        //------------------------------

        //========================================
        //      Unity's function
        //------------------------------
        protected override void Awake()
        {
            base.Awake();

            if (mPositionPlatform == null)
            {
                JCS_Debug.LogError(
                    this,
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

            AddSafe(player);

            bool isTopOfBox = JCS_Physics.TopOfBox(
                player.GetCharacterController(),
                mPositionPlatform.GetPlatformCollider());

            if (isTopOfBox)
            {
                // show character behind the ladder
                int backOrderLayer = OrderLayerObject.sortingOrder - 1;
                SetPlayerSortingOrder(player, backOrderLayer);
            }
            else
            {
                // show character infront
                int frontOrderLayer = OrderLayerObject.sortingOrder + 1;
                SetPlayerSortingOrder(player, frontOrderLayer);
            }

            player.CanRope = true;
            player.CanLadder = false;
            player.SetClimbingTransform(this.transform);
        }

        protected virtual void OnTriggerExit(Collider other)
        {
            JCS_2DSideScrollerPlayer player = other.GetComponent<JCS_2DSideScrollerPlayer>();
            if (player == null)
                return;

            player.SetClimbingTransform(null);
            player.GetCharacterAnimator().PlayAnimationInFrame();
            player.CanRope = false;

            RemoveNullPlayerAndSelf(player);
        }


        //========================================
        //      Self-Define
        //------------------------------
        //----------------------
        // Public Functions

        /// <summary>
        /// Something that needed to check in update can be design here.
        /// This function should get call by when the player is done climbing.
        /// </summary>
        public override void ClimbableUpdate()
        {
            foreach (JCS_2DSideScrollerPlayer player in mSSPlayers)
            {
                if (player.isGrounded())
                {
                    player.CanRope = true;
                    player.CanLadder = false;
                    player.SetClimbingTransform(this.transform);

                    continue;
                }

                if (player.CharacterState == JCS_2DCharacterState.CLIMBING &&
                    player.ClimbMoveType == JCS_ClimbMoveType.MOVE_UP)
                {
                    // show character infront
                    int frontOrderLayer = OrderLayerObject.sortingOrder + 1;
                    SetPlayerSortingOrder(player, frontOrderLayer);


                    bool isTopOfBox = JCS_Physics.TopOfBox(
                        player.GetCharacterController(),
                        mPositionPlatform.GetPlatformCollider());

                    /* Check top of the platform */
                    if (isTopOfBox)
                    {
                        // set on the platform
                        JCS_Physics.SetOnTopOfBox(
                            player.GetCharacterController(),
                            mPositionPlatform.GetPlatformCollider(),
                            0.03f);

                        player.SetClimbingTransform(null);
                        player.GetCharacterAnimator().PlayAnimationInFrame();
                        player.CanRope = false;
                        player.VelY = 0;
                        player.JustClimbOnTopOfBox = true;

                        // show character behind the ladder
                        int backOrderLayer = OrderLayerObject.sortingOrder - 1;
                        SetPlayerSortingOrder(player, backOrderLayer);
                    }
                }
            }
        }

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions

        /// <summary>
        /// Add object with no duplicate.
        /// </summary>
        /// <param name="player"></param>
        private void AddSafe(JCS_2DSideScrollerPlayer player)
        {
            for (int index = 0;
                index < mSSPlayers.Count;
                ++index)
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
            for (int index = 0;
                index < mSSPlayers.Count;
                ++index)
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
#if (UNITY_EDITOR)
            player.OrderLayerObject.SetObjectParentToOrderLayerByOrderLayerIndex(
                layer);
#else
            player.GetSpriteRenderer().sortingOrder = layer;
#endif
        }

    }
}
