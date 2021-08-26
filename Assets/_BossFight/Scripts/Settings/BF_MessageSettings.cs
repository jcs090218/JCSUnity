/**
 * $File: BF_MessageSettings.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using JCSUnity;

/// <summary>
/// 
/// </summary>
public class BF_MessageSettings :  JCS_Settings<BF_MessageSettings>
{
    /* Variables */

    public string EXP_BASE = "EXP: ";

    /* Setter & Getter */

    /* Functions */

    private void Awake()
    {
        instance = CheckSingleton(instance, this);

        // do something?
    }

    protected override void TransferData(BF_MessageSettings _old, BF_MessageSettings _new)
    {
        _new.EXP_BASE = _old.EXP_BASE;
    }
}
