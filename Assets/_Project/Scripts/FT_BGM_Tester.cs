/**
 * $File: FT_BGM_Tester.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using JCSUnity;

public class FT_BGM_Tester : MonoBehaviour
{
    /* Variables */

    public AudioClip mBGM_01 = null;
    public AudioClip mBGM_02 = null;

    public AudioClip mOneShotBGM = null;
    public AudioClip mOnStackBGM = null;

    /* Setter/Getter */

    /* Functions */

    private void Update()
    {
        if (JCS_Input.GetKeyDown(KeyCode.A))
            JCS_SoundManager.instance.SwitchBackgroundMusic(mBGM_01, 0.5f, 0.5f);
        if (JCS_Input.GetKeyDown(KeyCode.S))
            JCS_SoundManager.instance.SwitchBackgroundMusic(mBGM_02, 0.5f, 0.5f);

        if (JCS_Input.GetKeyDown(KeyCode.D))
            JCS_SoundManager.instance.PlayOneShotBackgroundMusic(mOneShotBGM, mOnStackBGM);
    }
}
