/**
 * $File: RC_EffectObject.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using JCSUnity;

public class RC_EffectObject : MonoBehaviour 
{
    /* Variables */

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

    [Header("Point Type")]
    [Tooltip("If add point absolute number of this value, if lose point then opposite of this method.")]
    [SerializeField] private float mPoint = 10;

    /* Setter & Getter */

    public bool autoEffect { get { return mAutoEffect; } set { mAutoEffect = value; } }

    /* Functions */

    private void OnTriggerEnter(Collider other)
    {
        if (!mAutoEffect)
            return;

        var p = other.GetComponent<RC_Player>();
        if (p == null)
            return;

        DoEffect(p);
    }

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
            case RC_EffectType.ADD_POINT:
                p.DeltaPoint(JCS_Mathf.ToPositive(mPoint));
                break;
            case RC_EffectType.LOSE_POINT:
                p.DeltaPoint(JCS_Mathf.ToNegative(mPoint));
                break;
        }
    }
}
