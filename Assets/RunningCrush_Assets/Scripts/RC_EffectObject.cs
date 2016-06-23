/**
 * $File: RC_EffectObject.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information $
 *		                Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;


public class RC_EffectObject 
    : MonoBehaviour 
{

    //----------------------
    // Public Variables

    //----------------------
    // Private Variables
    [SerializeField] private RC_EffectType mEffectType = RC_EffectType.BLOCK;
    [SerializeField] private bool mAutoEffect = true;

    [Header("Speed Type")]
    [SerializeField] private float mSpeedUp = 10;
    [SerializeField] private float mSpeedDown = -10;

    [Header("Push Type")]
    [SerializeField] private float mPushForce = 10;

    [Header("Jump Force")]
    [SerializeField] private float mJumpForceUp = 10;
    [SerializeField] private float mJumpForceDown = -10;

    //----------------------
    // Protected Variables

    //========================================
    //      setter / getter
    //------------------------------
    public bool AutoEffect { get { return this.mAutoEffect; } set { this.mAutoEffect = value; } }

    //========================================
    //      Unity's function
    //------------------------------
    private void OnTriggerEnter(Collider other)
    {
        if (!mAutoEffect)
            return;

        RC_Player p = other.GetComponent<RC_Player>();
        if (p == null)
            return;

        DoEffect(p);
    }

    //========================================
    //      Self-Define
    //------------------------------
    //----------------------
    // Public Functions
    public void DoEffect(RC_Player p)
    {
        switch (mEffectType)
        {
            case RC_EffectType.BLOCK:
                p.Block();
                break;
            case RC_EffectType.SPEED_UP:
                p.DeltaSpeed(mSpeedUp);
                break;
            case RC_EffectType.SPEED_DOWN:
                p.DeltaSpeed(mSpeedDown);
                break;
            case RC_EffectType.PUSH_UP:
                p.PushY(mPushForce);
                break;
            case RC_EffectType.PUSH_DOWN:
                p.PushY(-mPushForce);
                break;
            case RC_EffectType.WEAK:
                p.DeltaJumpForce(mJumpForceDown, 0);        // make it cannot jump
                break;
            case RC_EffectType.ENERGETIC:
                p.DeltaJumpForce(mJumpForceUp, 0);
                break;

        }

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions

    }
    
}
