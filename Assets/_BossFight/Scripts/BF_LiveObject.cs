/**
 * $File: BF_LiveObject.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                    Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using JCSUnity;

/// <summary>
/// Live object for example game `Boss Fight`.
/// </summary>
public class BF_LiveObject : JCS_2DLiveObject
{
    /* Variables */

    [Header("** Check Variables (BF_LiveObject) **")]

    [SerializeField]
    private float mFreezeTime = 0;

    [SerializeField]
    private float mFreezeTimer = 0;

    [SerializeField]
    private float mBurnTime = 0;

    [SerializeField]
    private float mBurnTimer = 0;

    [Header("** Game Feature Settings (BF_LiveObject) **")]

    [Tooltip("Can this live object be freeze?")]
    [SerializeField]
    private bool mCanFreeze = true;

    private bool mIsFreeze = false;

    [Tooltip("Can this live object be burn?")]
    [SerializeField]
    private bool mCanBurn = true;

    private bool mIsBurn = false;

    /* Setter & Getter */

    /* Functions */

    protected void Start()
    {
        /* This will make the object follow the mad target. */
        var ar = this.GetComponent<JCS_AttackerRecorder>();
        if (ar != null)
            ar.LastAttacker = BF_GameManager.instance.PROTECT_OBJECT.transform;
    }

    private void Update()
    {
#if UNITY_EDITOR
        Test();
#endif

        DoBurn();
        DoFreeze();
    }

#if UNITY_EDITOR
    private void Test()
    {
        if (JCS_Input.GetKeyDown(KeyCode.V))
            Freeze(2);
        if (JCS_Input.GetKeyDown(KeyCode.C))
            Burn(2);
    }
#endif

    /// <summary>
    /// You died!
    /// </summary>
    public override void Die()
    {
        var bggm = BF_GameManager.instance;

        // add experience to the current level.
        bggm.DeltaCurrentExp(EXP);

        // minus monster count.
        --bggm.MOB_CURRENT_IN_SCENE;

        var utilm = JCS_UtilitiesManager.instance;
        if (utilm.GetIGLogSystem() != null)
        {
            string expMsg = BF_MessageSettings.instance.EXP_BASE + EXP.ToString();

            utilm.GetIGLogSystem().SendLogMessage(expMsg);
        }

        base.Die();
    }

    /// <summary>
    /// Freeze action.
    /// </summary>
    public void Freeze(float freezeTime)
    {
        if (!CanDamage)
            return;

        // cannot be freeze.
        if (!mCanFreeze)
            return;

        // freeze x velocity.
        if (VelocityInfo != null)
            VelocityInfo.Freeze();

        LiveObjectAnimator.StopAnimationInFrame();

        spriteRenderer.color = BF_GameSettings.instance.FREEZE_COLOR;

        mFreezeTime = freezeTime;

        mIsFreeze = true;
    }

    /// <summary>
    /// Opposite of freeze effect.
    /// </summary>
    public void UnFreeze()
    {
        if (VelocityInfo != null)
            VelocityInfo.UnFreeze();

        spriteRenderer.color = Color.white;
        mIsFreeze = false;

        LiveObjectAnimator.PlayAnimationInFrame();
    }

    /// <summary>
    /// Burn action.
    /// </summary>
    /// <param name="burnTime"></param>
    public void Burn(float burnTime)
    {
        if (!CanDamage)
            return;

        // cannot be burned.
        if (!mCanBurn)
            return;

        spriteRenderer.color = BF_GameSettings.instance.BURN_COLOR;

        mBurnTime = burnTime;

        mIsBurn = true;
    }

    /// <summary>
    /// Opposite of burn effect.
    /// </summary>
    public void UnBurn()
    {
        spriteRenderer.color = Color.white;
        mIsBurn = false;
    }

    /// <summary>
    /// Freeze Effect algorithms.
    /// </summary>
    private void DoFreeze()
    {
        if (!mIsFreeze)
            return;

        // start freezing calculation.

        mFreezeTimer += Time.deltaTime;

        if (mFreezeTimer < mFreezeTime)
            return;

        // reset timer.
        mFreezeTimer = 0;

        UnFreeze();
    }

    /// <summary>
    /// Burn Effect algorithms.
    /// </summary>
    private void DoBurn()
    {
        if (!mIsBurn)
            return;

        // start burn calculation.

        mBurnTimer += Time.deltaTime;


        if (mBurnTimer < mBurnTime)
            return;

        // reset timer.
        mBurnTimer = 0;

        UnBurn();
    }
}
