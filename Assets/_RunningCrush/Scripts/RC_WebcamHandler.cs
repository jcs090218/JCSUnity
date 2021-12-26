/**
 * $File: RC_WebcamHandler.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using JCSUnity;

[RequireComponent(typeof(RectTransform))]
public class RC_WebcamHandler : MonoBehaviour
{
    /* Variables */

    [SerializeField]
    private RectTransform mRectTransform = null;

    [SerializeField] private RectTransform[] mWebcamPanel = null;
    [SerializeField] private JCS_Webcam mWebcam = null;

    [SerializeField] private RectTransform mStartGameButton = null;
    [SerializeField] private float mTimeBeforeDisable = 0;
    [SerializeField] private float mTimeAfterDisable = 0;
    private float mTimer = 0;

    private JCS_IncDec mType = JCS_IncDec.INCREASE;

    private bool mStartTimer = false;
    private bool mCloseTimer = false;

    private int mPanelIndex = 0;

    /* Setter & Getter */

    /* Functions */

    private void Start()
    {
        mRectTransform = this.GetComponent<RectTransform>();

        // try to get the object
        if (mWebcam == null)
            mWebcam = (JCS_Webcam)FindObjectOfType(typeof(JCS_Webcam));
    }

    private void Update()
    {

        if (mCloseTimer)
        {
            mTimer += Time.deltaTime;

            if (mTimeAfterDisable < mTimer)
            {
                PluseAppRect();

                mWebcam.GetImage().enabled = true;

                mCloseTimer = false;
            }
        }

        if (mStartTimer)
        {
            mTimer += Time.deltaTime;

            if (mTimeBeforeDisable < mTimer)
            {
                mWebcam.GetImage().enabled = false;
                mCloseTimer = true;

                mTimer = 0;
                mStartTimer = false;
            }
        }
    }

    public void RC_SetActiveInTime(int way)
    {
        if (mStartTimer)
            return;

        mType = (JCS_IncDec)way;

        mStartTimer = true;
    }

    public void PluseAppRect()
    {
        if (mType == JCS_IncDec.INCREASE)
            ++mPanelIndex;
        else
            --mPanelIndex;

        // check length
        if (mWebcamPanel.Length < mPanelIndex || mPanelIndex < 0)
        {
            JCS_Debug.LogError("Out of range index");

            return;
        }
        // check object
        if (mWebcamPanel[mPanelIndex] == null)
        {
            JCS_Debug.LogError("Call the function but does not assign panel at [" + mPanelIndex + "]...");
            return;
        }

        RectTransform appRectTransform = JCS_Canvas.GuessCanvas().AppRect;
        Vector2 appRect = appRectTransform.sizeDelta;

        Vector3 newPosButton = mRectTransform.localPosition;
        Vector3 newPosWebcam = mWebcam.GetRectTransform().localPosition;
        Vector3 newStartGameButtonPos = mStartGameButton.localPosition;

        newPosButton.x += appRect.x;
        newPosWebcam.x += appRect.x;
        newStartGameButtonPos.x += appRect.x;


        if ((mPanelIndex) == RC_GameSettings.instance.PLAYER_IN_GAME)
        {
            RC_GameSettings.instance.READY_TO_START_GAME = true;
        }
        else
        {
            mRectTransform.localPosition = newPosButton;
            mWebcam.GetRectTransform().localPosition = newPosWebcam;

            mWebcam.transform.SetParent(mWebcamPanel[mPanelIndex].transform);
            this.transform.SetParent(mWebcamPanel[mPanelIndex].transform);
        }

        mStartGameButton.localPosition = newStartGameButtonPos;
        mStartGameButton.SetParent(mWebcamPanel[mPanelIndex].transform);
    }
}
