/**
 * $File: RC_StartGameButton.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using UnityEngine.UI;
using JCSUnity;

[RequireComponent(typeof(RectTransform))]
public class RC_StartGameButton : MonoBehaviour 
{
    /* Variables */

    private RectTransform mRectTransform = null;
    private Image mImage = null;

    /* Setter & Getter */

    public RectTransform rectTransform { get { return this.mRectTransform; } }

    /* Functions */

    private void Awake()
    {
        mRectTransform = this.GetComponent<RectTransform>();
        mImage = this.GetComponent<Image>();

        mImage.enabled = false;
        JCS_Util.SetActiveToAllChildren(this.transform, false);
    }

    private void Update()
    {
        if (RC_GameSettings.FirstInstance().READY_TO_START_GAME)
        {
            mImage.enabled = true;
            JCS_Util.SetActiveToAllChildren(this.transform, true);
        }
    }
}
