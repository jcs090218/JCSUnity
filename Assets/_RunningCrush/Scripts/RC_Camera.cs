/**
 * $File: RC_Camera.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using System.Collections.Generic;
using UnityEngine;
using JCSUnity;
using MyBox;

[RequireComponent(typeof(JCS_2DCamera))]
public class RC_Camera : MonoBehaviour
{
    /* Variables */

    public static RC_Camera instance = null;

    public enum TrackingTarget
    {
        LEFT,
        RIGHT,
        TOP,
        DOWN
    };

    private JCS_2DCamera mJCSCamera = null;

    [Separator("Runtime Variables (RC_Camera)")]

    [Tooltip("Depend on which direction should the camera track.")]
    [SerializeField]
    private JCS_2D4Direction mTrackingTarget = JCS_2D4Direction.RIGHT;

    private RC_Player mTrackingPlayer = null;

    /* Setter & Getter */

    public RC_Player GetTrackingPlayer() { return this.mTrackingPlayer; }

    /* Functions */
    private void Awake() 
    {
        instance = this;

        mJCSCamera = this.GetComponent<JCS_2DCamera>();
    }
    
    private void Update() 
    {
        DecideTrack();

    }

    /// <summary>
    /// Decide which player we want to track.
    /// </summary>
    private void DecideTrack()
    {
        if (RC_GameSettings.instance.GAME_OVER)
        {
            // stop tracking if the game is over.
            mJCSCamera.SetFollowing(false);
            return;
        }

        JCS_Player target = FindActivePlayer();

        // set the player to follow.
        if (target != null)
        {
            mJCSCamera.SetFollowTarget(target.transform);
            mJCSCamera.SetFollowing(true);
        }
        else
            mJCSCamera.SetFollowing(false);

        // down cast to rc player.
        mTrackingPlayer = (RC_Player)target;
    }

    private RC_Player FindActivePlayer()
    {
        var pm = JCS_PlayerManager.instance;

        List<JCS_Player> players = pm.GetPlayerList();
        var activePlayers = new List<JCS_Player>();

        for (int index= 0; index < players.Count; ++index)
        {
            var rcPlayer = ((RC_Player)players[index]);

            // only player that are not dead.
            if (!rcPlayer.IsDead)
                activePlayers.Add(rcPlayer);
        }

        JCS_Player currentPlayerAndNotDead =
            pm.FindPlayerByDirectionUsingList(mTrackingTarget, activePlayers);

        return (RC_Player)currentPlayerAndNotDead;
    }
}
