/**
 * $File: RC_PlayerPointer.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information $
 *		                Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;
using JCSUnity;


[RequireComponent(typeof(SpriteRenderer))]
public class RC_PlayerPointer 
    : MonoBehaviour 
{

    //----------------------
    // Public Variables

    //----------------------
    // Private Variables
    private RC_Player mRCPlayer = null;

    [SerializeField] private Sprite mPlayerImage = null;

    private SpriteRenderer mSpriteRenderer = null;
    [SerializeField] private SpriteRenderer mPhotoSpriteRenderer = null;

    [SerializeField] private Vector3 mOffset = Vector3.zero;

    [Tooltip("How big the image in unit per pixel. 越大圖片越小, 越小圖越大.")]
    [SerializeField] private float mUnitPerPixel = 400;
    [Tooltip(@"Order Layer for frame and the photo in frame. 
Photo Order Layer will minus one in case the photo does not be ontop of the frame.")]
    [SerializeField] private int mOrderLayer = 17;

    //----------------------
    // Protected Variables

    //========================================
    //      setter / getter
    //------------------------------
    public void SetRCPlayer(RC_Player p) { this.mRCPlayer = p; }

    //========================================
    //      Unity's function
    //------------------------------
    private void Awake()
    {
        mSpriteRenderer = this.GetComponent<SpriteRenderer>();

        // set the order layer, this should be higher than player's order layer.
        mSpriteRenderer.sortingOrder = mOrderLayer;

        // get the photo sprite renderer from the child.
        if (mPhotoSpriteRenderer == null)
            mPhotoSpriteRenderer = this.GetComponentInChildren<SpriteRenderer>();
    }
    private void Start()
    {

        if (RC_GameSettings.instance.WEBCAM_MODE &&
            RC_GameSettings.instance.GAME_MODE != RC_GameMode.SINGLE_PLAYERS)
        {
            mPlayerImage = JCS_ImageLoader.LoadImage(
                Application.dataPath + "/JCS_GameData/WebcamShot/" + mRCPlayer.ControlIndex + ".png", mUnitPerPixel);

            if (mPhotoSpriteRenderer != null)
            {
                mPhotoSpriteRenderer.sprite = mPlayerImage;

                // order layer should be lower than the frame. (框框)
                mPhotoSpriteRenderer.sortingOrder = mOrderLayer - 1;
            }
            else
            {
                JCS_GameErrors.JcsErrors(
                    "RC_PlayerPointer",
                    -1,
                    "No Photo sprite renderer assigned.");
            }
        }

        // find player position
        Vector3 newPos = mRCPlayer.transform.localPosition;

        // add pivot
        newPos += mOffset;

        // set pointer to player + pivot position
        this.transform.localPosition = newPos;
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
