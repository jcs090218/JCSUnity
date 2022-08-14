/**
 * $File: BF_AutoAttacker.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                    Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using JCSUnity;

/// <summary>
/// Player that will keep attacking the closest enmey, 
/// in the scene.
/// </summary>
[RequireComponent(typeof(JCS_DetectAreaAction))]
[RequireComponent(typeof(JCS_ShootAction))]
[RequireComponent(typeof(BF_Player))]
public class BF_AutoAttacker : MonoBehaviour
{
    /* Variables */

    [Header("** Check Variables (BF_AutoAttacker) **")]

    [SerializeField] 
    private JCS_DetectAreaAction mDetectAreaAction = null;

    [SerializeField] 
    private JCS_ShootAction mShootAction = null;

    [SerializeField] 
    private BF_Player mBFPlayer = null;

    [Header("** Runtime Variables (BF_AutoAttacker) **")]

    [Tooltip("How much time per shoot?")]
    [SerializeField]
    [Range(0.01f, 10.0f)]
    private float mTimeZone = 0;

    [SerializeField]
    [Range(0.0f, 5.0f)]
    private float mAdjustTimeZone = 1.5f;

    private float mRealTimeZone = 0;

    private float mTimer = 0;

    private bool mShooted = false;

    /* Setter & Getter */

    /* Functions */

    private void Awake()
    {
        mDetectAreaAction = this.GetComponentInParent<JCS_DetectAreaAction>();
        mShootAction = this.GetComponentInParent<JCS_ShootAction>();
        mBFPlayer = this.GetComponentInParent<BF_Player>();
    }

    private void Update()
    {
#if (UNITY_EDITOR)
        Test();
#endif

        DoLockShoot();
    }

#if (UNITY_EDITOR)
    private void Test()
    {
        if (JCS_Input.GetKeyDown(KeyCode.H))
            LockShoot();
    }
#endif

    /// <summary>
    /// Calculate the time and see when to shoot the bullet.
    /// </summary>
    private void DoLockShoot()
    {
        if (mShooted)
            ResetTimeZone();

        mTimer += Time.deltaTime;

        if (mRealTimeZone < mTimer)
        {

            for (int count = 0; count < mShootAction.ShootCount; ++count)
            {
                LockShoot();
            }

            mShooted = true;
        }
    }

    /// <summary>
    /// Shoot one bullet.
    /// </summary>
    private void LockShoot()
    {
        // get the closest object's transform.
        JCS_DetectAreaObject dao = mDetectAreaAction.FindTheClosest();
        if (dao == null)
            return;

        JCS_Bullet bullet = mShootAction.Shoot();

        if (bullet == null)
            return;

        bullet.transform.LookAt(dao.transform);

        if (dao.transform.position.x < this.transform.position.x)
        {
            bullet.transform.Rotate(0, 90, 0);

            switch (mBFPlayer.Face)
            {
                case JCS_2DFaceType.FACE_LEFT:
                    bullet.MoveSpeed = -bullet.MoveSpeed;
                    break;
                case JCS_2DFaceType.FACE_RIGHT:

                    break;
            }
        }
        else
        {
            bullet.transform.Rotate(0, -90, 0);

            switch (mBFPlayer.Face)
            {
                case JCS_2DFaceType.FACE_LEFT:

                    break;
                case JCS_2DFaceType.FACE_RIGHT:
                    bullet.MoveSpeed = -bullet.MoveSpeed;
                    break;
            }
        }

        // redo the deviation effect, cuz we reset the angle
        // after we did the deviation effect.
        mShootAction.DeviationEffect(bullet.transform);
    }

    /// <summary>
    /// Algorithm to calculate the time to do 
    /// lock shoot action include direction.
    /// </summary>
    private void ResetTimeZone()
    {
        float adjustTime = JCS_Random.Range(-mAdjustTimeZone, mAdjustTimeZone);
        mRealTimeZone = mTimeZone + adjustTime;

        mShooted = false;
        mTimer = 0;
    }
}
