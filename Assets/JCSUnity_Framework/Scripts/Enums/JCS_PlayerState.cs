/**
 * $File: $
 * $Date: $
 * $Reveision: $
 * $Creator: Jen-Chieh Shen $
 */
using UnityEngine;
using System.Collections;

namespace JCSUnity
{

    public enum JCS_PlayerState
    {
        STAND = 0,
        WALK = 1,
        ATTACK = 2,
        JUMP = 3,
        PRONE = 4,
        ALERT = 5,
        FLY = 6,
        LADDER = 7,
        ROPE = 8,
        SIT = 9,
        HIT = 10,        // when player get hit
        DANCE = 11,
        SWIM = 12,
        DEAD = 13,       // when player is dead. (墓碑!)
        GHOST = 14
    }
}
