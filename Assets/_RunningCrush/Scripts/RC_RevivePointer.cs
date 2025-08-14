/**
 * $File: RC_RevivePointer.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using JCSUnity;

[RequireComponent(typeof(SpriteRenderer))]
public class RC_RevivePointer : MonoBehaviour
{
    /* Variables */
    
    private SpriteRenderer mSpriteRenderer = null;

    private RC_Player mRCPlayer = null;

    [Tooltip("This should be lower than player? (Up to u)")]
    [SerializeField] 
    private int mOrderLayer = 14;

    [Tooltip("How low below the top boundary.")]
    [SerializeField] 
    private float mOffsetY = -1;

    /* Setter & Getter */
    
    public void SetRCPlayer(RC_Player p) { this.mRCPlayer = p; }

    /* Functions */
    private void Awake()
    {
        mSpriteRenderer = this.GetComponent<SpriteRenderer>();
        mSpriteRenderer.sortingOrder = mOrderLayer;     // set sorting order

        // disable this.
        mSpriteRenderer.enabled = false;
    }

    private void Update()
    {
        // if player is not dead return and do nothing
        if (mRCPlayer == null)
        {
            // disable this.
            mSpriteRenderer.enabled = false;

            return;
        }

        // if player still reviving
        if (mRCPlayer.reviving || !mRCPlayer.isDead)
        {
            // disable this.
            mSpriteRenderer.enabled = false;

            return;
        }

        mSpriteRenderer.enabled = true;

        FollowPlayer();
    }
    private void FollowPlayer()
    {
        Vector3 newPos = this.transform.position;

        // Since this is the child of player, 
        // so we do not need to set position the same.
        //newPos.x = mRCPlayer.transform.localPosition.x;

        // get camera top bound.
        float camTop = JCS_Camera.main.camRect.y;

        // set offset 
        newPos.y = camTop + mOffsetY;

        this.transform.position = newPos;
    }
}
