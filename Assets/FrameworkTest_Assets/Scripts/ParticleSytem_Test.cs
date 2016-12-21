/**
 * $File: ParticleSytem_Test.cs $
 * $Date: 2016-11-13 00:14:51 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;
using JCSUnity;

/// <summary>
/// 
/// </summary>
public class ParticleSytem_Test 
    : MonoBehaviour 
{

    //----------------------
    // Public Variables

    //----------------------
    // Private Variables

    [SerializeField]
    private JCS_TowardTarget mParticle = null;

    [SerializeField]
    private JCS_2DParticleSystem mParticleSystem = null;

    private JCS_Vector<JCS_Particle> mParticles = null;

    //----------------------
    // Protected Variables

    //========================================
    //      setter / getter
    //------------------------------

    //========================================
    //      Unity's function
    //------------------------------
    private void Start()
    {
        this.mParticle.transform.SetParent(this.transform);

        mParticles = mParticleSystem.GetParticles();
    }

    private void Update()
    {
        if (JCS_Input.GetKeyDown(KeyCode.G))
        {
            JCS_Tweener masterTweener = mParticle.GetComponent<JCS_Tweener>();
            JCS_TowardTarget masterTt = mParticle.GetComponent<JCS_TowardTarget>();

            for (int index = 0;
                index < mParticles.length;
                ++index)
            {
                JCS_Tweener tweener = mParticles.at(index).GetComponent<JCS_Tweener>();

                tweener.EasingX = masterTweener.EasingX;
                tweener.EasingY = masterTweener.EasingY;
                tweener.EasingZ = masterTweener.EasingZ;

                tweener.DurationX = masterTweener.DurationX;
                tweener.DurationY = masterTweener.DurationY;
                tweener.DurationZ = masterTweener.DurationZ;


                JCS_TowardTarget tt = mParticles.at(index).GetComponent<JCS_TowardTarget>();

                tt.Range = masterTt.Range;
                tt.AdjustRange = masterTt.AdjustRange;
                tt.IncludeDepth = masterTt.IncludeDepth;
            }

            print("Particles updated.");
        }
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
