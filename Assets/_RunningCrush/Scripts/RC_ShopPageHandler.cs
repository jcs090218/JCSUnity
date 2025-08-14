﻿/**
 * $File: RC_ShopPageHandler.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using JCSUnity;

public class RC_ShopPageHandler : MonoBehaviour
{
    /* Variables */

    [SerializeField] private int mMaxHorizontalPage = 1;
    [SerializeField] private int mMinHorizontalPage = 0;

    [SerializeField] private int mMaxVerticalPage = 1;
    [SerializeField] private int mMinVerticalPage = 0;

    [SerializeField]
    private int mVerticalPageCounter = 0;
    [SerializeField]
    private int mHorizontalPageCounter = 0;

    [SerializeField] private JCS_Button mTopBtn = null;
    [SerializeField] private JCS_Button mBotBtn = null;
    [SerializeField] private JCS_Button mLeftBtn = null;
    [SerializeField] private JCS_Button mRightBtn = null;

    /* Setter & Getter */

    private int maxVeticalPage { get { return mMaxVerticalPage; } }
    private int minVerticalPage { get { return mMinVerticalPage; } }

    /* Functions */

    private void Awake()
    {
        if (mLeftBtn != null)
            mLeftBtn.onClick += LeftClick;

        if (mRightBtn != null)
            mRightBtn.onClick += RightClick;

        if (mBotBtn != null)
            mBotBtn.onClick += BotClick;

        if (mTopBtn != null)
            mTopBtn.onClick += TopClick;
    }
    private void Start()
    {
        mVerticalPageCounter = 0;
        mHorizontalPageCounter = 0;

        InitButton();
    }

    public void LeftClick()
    {
        --mHorizontalPageCounter;


        if (mHorizontalPageCounter <= mMinHorizontalPage)
        {
            mLeftBtn.SetInteractable(false);
            mRightBtn.SetInteractable(true);

            mHorizontalPageCounter = mMinHorizontalPage;
        }
        else
        {
            mLeftBtn.SetInteractable(true);
            mRightBtn.SetInteractable(true);
        }


    }
    public void RightClick()
    {
        ++mHorizontalPageCounter;

        if (mHorizontalPageCounter >= mMaxHorizontalPage)
        {
            mLeftBtn.SetInteractable(true);
            mRightBtn.SetInteractable(false);

            mHorizontalPageCounter = mMaxHorizontalPage;
        }
        else
        {
            mLeftBtn.SetInteractable(true);
            mRightBtn.SetInteractable(true);
        }

    }
    public void TopClick()
    {
        ++mVerticalPageCounter;
    }
    public void BotClick()
    {
        --mVerticalPageCounter;
    }

    /// <summary>
    /// Initialize the button the first time entered.
    /// </summary>
    private void InitButton()
    {
        if (mLeftBtn != null)
        {
            if (mHorizontalPageCounter == mMinHorizontalPage)
                mLeftBtn.SetInteractable(false);
        }

        if (mRightBtn != null)
        {
            if (mHorizontalPageCounter == mMaxHorizontalPage)
                mRightBtn.SetInteractable(false);
        }
    }
}
