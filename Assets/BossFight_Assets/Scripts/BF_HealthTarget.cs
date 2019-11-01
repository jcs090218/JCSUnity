/**
 * $File: BF_HealthTarget.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                    Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;

/// <summary>
/// 
/// </summary>
[RequireComponent(typeof(BF_LiveObject))]
public class BF_HealthTarget 
    : MonoBehaviour 
{

    //----------------------
    // Public Variables

    //----------------------
    // Private Variables
    private BF_LiveObject mLiveObject = null;

    [Tooltip("Plz plugin this!!")]
    [SerializeField]
    private BF_LiquidBarHandler mLiquidBarHandler = null;

    //----------------------
    // Protected Variables

    //========================================
    //      setter / getter
    //------------------------------
    public BF_LiveObject LiveObject { get { return this.mLiveObject; } }

    //========================================
    //      Unity's function
    //------------------------------
    private void Awake()
    {
        mLiveObject = this.GetComponent<BF_LiveObject>();
    }
    
    private void Start()
    {
        mLiquidBarHandler = BF_GameManager.instance.HEALTH_LIQUIDBAR;

        // set the info.
        mLiquidBarHandler.AttachInfo(mLiveObject.HPInfo);

        mLiveObject.IsPlayer = true;
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
    
}
