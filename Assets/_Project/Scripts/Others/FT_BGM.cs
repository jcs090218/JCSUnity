/**
 * $File: FT_BGM.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using JCSUnity;

public class FT_BGM : MonoBehaviour
{
    /* Variables */

    public AudioClip mBGM_01 = null;
    public AudioClip mBGM_02 = null;

    public AudioClip mOneShotBGM = null;
    public AudioClip mOnStackBGM = null;

    /* Setter & Getter */

    /* Functions */

    private void Update()
    {
        var sm = JCS_SoundManager.FirstInstance();

        if (JCS_Input.GetKeyDown(KeyCode.A))
            sm.SwitchBGM(mBGM_01, 0.5f, 0.5f);
        if (JCS_Input.GetKeyDown(KeyCode.S))
            sm.SwitchBGM(mBGM_02, 0.5f, 0.5f);

        if (JCS_Input.GetKeyDown(KeyCode.D))
            sm.PlayOneShotBGM(mOneShotBGM, mOnStackBGM);
    }
}
