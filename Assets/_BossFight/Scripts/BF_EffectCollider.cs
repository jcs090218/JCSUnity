/**
 * $File: BF_EffectCollider.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                    Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;

/// <summary>
/// When object collider with the live object, 
/// effect that live object. (BF_LiveObject)
/// </summary>
public class BF_EffectCollider : MonoBehaviour 
{
    /* Variables */

    /// <summary>
    /// Effect type list and design here...
    /// </summary>
    public enum EffectType
    {
        NONE, 

        FREEZE,
        BURN
    };

    [SerializeField]
    private EffectType mEffectType = EffectType.NONE;

    [SerializeField] [Range(1, 10)]
    private float mEffectTime = 1;

    /* Setter & Getter */

    /* Functions */

    private void OnTriggerEnter(Collider other)
    {
        var liveObject = other.GetComponent<BF_LiveObject>();
        if (liveObject == null)
            return;

        DoEffect(liveObject, mEffectType);
    }

    /// <summary>
    /// Do the effect base on the enum type.
    /// </summary>
    /// <param name="liveObject"></param>
    /// <param name="type"></param>
    public void DoEffect(BF_LiveObject liveObject, EffectType type)
    {
        DoEffect(liveObject, type, mEffectTime);
    }
    /// <summary>
    /// Do the effect base on the enum type.
    /// </summary>
    /// <param name="liveObject"></param>
    /// <param name="type"></param>
    /// <param name="effectTime"></param>
    public void DoEffect(BF_LiveObject liveObject, EffectType type, float effectTime)
    {
        switch (type)
        {
            case EffectType.NONE: break;
            case EffectType.FREEZE:
                liveObject.Freeze(effectTime);
                break;
            case EffectType.BURN:
                liveObject.Burn(effectTime);
                break;
        }
    }
}
