/**
 * $File: RC_ShopPageHandler.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;
using JCSUnity;


/// <summary>
/// 
/// </summary>
public class RC_ShopPageHandler 
    : MonoBehaviour 
{

    //----------------------
    // Public Variables

    //----------------------
    // Private Variables

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
    

    //----------------------
    // Protected Variables

    //========================================
    //      setter / getter
    //------------------------------

    //========================================
    //      Unity's function
    //------------------------------
    private void Awake()
    {
        if (mLeftBtn != null)
            mLeftBtn.SetCallback(LeftClick);

        if (mRightBtn != null)
            mRightBtn.SetCallback(RightClick);

        if (mBotBtn != null)
            mBotBtn.SetCallback(BotClick);

        if (mTopBtn != null)
            mTopBtn.SetCallback(TopClick);
    }
    private void Start()
    {
        mVerticalPageCounter = 0;
        mHorizontalPageCounter = 0;

        InitButton();
    }

    
    //========================================
    //      Self-Define
    //------------------------------
    //----------------------
    // Public Functions
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
    
    //----------------------
    // Protected Functions
    
    //----------------------
    // Private Functions
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

        if (mRightBtn!= null)
        {
            if (mHorizontalPageCounter == mMaxHorizontalPage)
                mRightBtn.SetInteractable(false);
        }
    }
    
}
