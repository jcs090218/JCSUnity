/**
 * $File: FT_ParticleSytem.cs $
 * $Date: 2016-11-13 00:14:51 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using JCSUnity;

public class FT_ParticleSytem : MonoBehaviour 
{
    /* Variables */

    [SerializeField]
    private JCS_TowardTarget mParticle = null;

    [SerializeField]
    private JCS_ParticleSystem mParticleSystem = null;

    private JCS_Vec<JCS_Particle> mParticles = null;

    /* Setter & Getter */

    /* Functions */

    private void Start()
    {
        mParticle.transform.SetParent(transform);

        mParticles = mParticleSystem.GetParticles();
    }

    private void Update()
    {
        if (JCS_Input.GetKeyDown(KeyCode.G))
        {
            var masterTweener = mParticle.GetComponent<JCS_TransformTweener>();
            var masterTt = mParticle.GetComponent<JCS_TowardTarget>();

            for (int index = 0; index < mParticles.length; ++index)
            {
                var tweener = mParticles.at(index).GetComponent<JCS_TransformTweener>();

                tweener.easingX = masterTweener.easingX;
                tweener.easingY = masterTweener.easingY;
                tweener.easingZ = masterTweener.easingZ;

                tweener.durationX = masterTweener.durationX;
                tweener.durationY = masterTweener.durationY;
                tweener.durationZ = masterTweener.durationZ;


                JCS_TowardTarget tt = mParticles.at(index).GetComponent<JCS_TowardTarget>();

                tt.range = masterTt.range;
                tt.adjustRange = masterTt.adjustRange;
                tt.includeDepth = masterTt.includeDepth;
            }

            print("Particles updated.");
        }
    }
}
