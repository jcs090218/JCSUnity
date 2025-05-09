/**
 * $File: BF_RainFaller.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                    Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using JCSUnity;
using MyBox;

/// <summary>
/// Character shoot rain fall
/// </summary>
[RequireComponent(typeof(JCS_ShootAction))]
public class BF_RainFaller : MonoBehaviour
{
    /* Variables */

    private JCS_ShootAction mShootAction = null;
    private BF_Player mBFPlayer = null;

    private float mShootAngle = 0;

    [Separator("Runtime Variables (BF_RainFaller)")]

    [SerializeField]
    private bool mCanShoot = true;

    [Header("- Time")]

    [Tooltip("Time to do one walk.")]
    [SerializeField]
    [Range(0.01f, 10.0f)]
    private float mTime = 2.0f;

    [Tooltip("Time that will randomly affect the time.")]
    [SerializeField]
    [Range(0.0f, 3.0f)]
    private float mAdjustTime = 1.5f;

    // time to record down the real time to do one walk 
    // action after we calculate the real time.
    private float mRealTime = 0;

    // timer to do walk.
    private float mTimer = 0;

    // check to see if we can reset our time.
    private bool mShooted = false;

    /* Setter & Getter */

    /* Functions */

    private void Awake()
    {
        mShootAction = this.GetComponent<JCS_ShootAction>();
        mBFPlayer = this.GetComponent<BF_Player>();
    }

    private void Start()
    {
        switch (mBFPlayer.Face)
        {
            case JCS_2DFaceType.FACE_LEFT:
                mShootAngle = 90;
                break;
            case JCS_2DFaceType.FACE_RIGHT:
                mShootAngle = -90;
                break;
        }
    }

    private void Update()
    {
        DoRainFall();
    }

    public void ShootRainFall()
    {
        if (!mCanShoot)
            return;

        for (int count = 0; count < mShootAction.ShootCount; ++count)
        {
            JCS_Bullet bullet = mShootAction.Shoot();
            if (bullet != null)
            {
                bullet.transform.eulerAngles = new Vector3(0, 0, mShootAngle);
                mShootAction.DeviationEffect(bullet.transform);
            }
        }

        mShooted = true;
    }

    /// <summary>
    /// 
    /// </summary>
    private void DoRainFall()
    {
        if (mShooted)
            ResetTimeZone();

        mTimer += Time.deltaTime;

        if (mTimer < mRealTime)
            return;

        ShootRainFall();
    }

    /// <summary>
    /// Algorithm to calculate the time to do 
    /// this action include direction.
    /// </summary>
    private void ResetTimeZone()
    {
        float adjustTime = JCS_Random.Range(-mAdjustTime, mAdjustTime);
        mRealTime = mTime + adjustTime;

        mShooted = false;
        mTimer = 0;
    }
}
