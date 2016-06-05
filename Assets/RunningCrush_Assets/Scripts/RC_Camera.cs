/**
 * $File: RC_Camera.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information $
 *		                Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;
using JCSUnity;
using System.Collections.Generic;

[RequireComponent(typeof(JCS_2DCamera))]
public class RC_Camera 
    : MonoBehaviour
{

    //----------------------
    // Public Variables
    public enum TrackingTarget
    {
        LEFT,
        RIGHT,
        TOP,
        DOWN
    };

    //----------------------
    // Private Variables
    private JCS_2DCamera mJCSCamera = null;

    [Header("** RC_Camera Settings **")]
    [Tooltip("Depend on which direction should the camera track.")]
    [SerializeField]
    private JCS_2D4Direction mTrackingTarget 
        = JCS_2D4Direction.RIGHT;

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
        mJCSCamera = this.GetComponent<JCS_2DCamera>();
    }
	
	private void Update() 
    {
        DecideTrack();

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

    /// <summary>
    /// Decide which player we want to track.
    /// </summary>
    private void DecideTrack()
    {
        JCS_PlayerManager pm = JCS_PlayerManager.instance;

        JCS_Player target = FindActivePlayer();

        // set the player to follow.
        if (target != null)
        {
            mJCSCamera.SetFollowTarget(target.transform);
            mJCSCamera.SetFollowing(true);
        }
        else
            mJCSCamera.SetFollowing(false);
    }

    private RC_Player FindActivePlayer()
    {
        JCS_PlayerManager pm = JCS_PlayerManager.instance;

        List<JCS_Player> players = pm.GetJCSPlayerList();
        List<JCS_Player> activePlayers = new List<JCS_Player>();

        for (int index= 0;
            index < players.Count;
            ++index)
        {
            RC_Player rcPlayer = ((RC_Player)players[index]);

            // only player that are not dead.
            if (!rcPlayer.IsDead)
                activePlayers.Add(rcPlayer);
        }

        JCS_Player currentPlayerAndNotDead =
            pm.FindPlayerByDirectionUsingList(mTrackingTarget, activePlayers);

        return (RC_Player)currentPlayerAndNotDead;
    }
    
}
