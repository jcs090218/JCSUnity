/**
 * $File: BF_HealthTarget.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                    Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;

/// <summary>
/// 
/// </summary>
[RequireComponent(typeof(BF_LiveObject))]
public class BF_HealthTarget : MonoBehaviour
{
    /* Variables */

    private BF_LiveObject mLiveObject = null;

    [Tooltip("Plz plugin this!!")]
    [SerializeField]
    private BF_LiquidBarHandler mLiquidBarHandler = null;

    /* Setter & Getter */
    
    public BF_LiveObject liveObject { get { return this.mLiveObject; } }

    /* Functions */

    private void Awake()
    {
        mLiveObject = this.GetComponent<BF_LiveObject>();
    }
    
    private void Start()
    {
        mLiquidBarHandler = BF_GameManager.instance.HEALTH_LIQUIDBAR;

        // set the info.
        mLiquidBarHandler.AttachInfo(mLiveObject.hpInfo);

        mLiveObject.isPlayer = true;
    }
}
