/**
 * $File: JCS_UnityObject.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information $
 *		                Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;
using UnityEngine.UI;


namespace JCSUnity
{

    public class JCS_UnityObject
        : MonoBehaviour
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables
        [SerializeField] protected JCS_UnityObjectType mObjectType = JCS_UnityObjectType.SPRITE;

        //-- Game Object
        protected Renderer mRenderer = null;
        //-- UI
        protected Image mImage = null;
        protected RectTransform mRectTransform = null;
        //-- Sprite
        protected SpriteRenderer mSpriteRenderer = null;

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------
        public JCS_UnityObjectType GetObjectType() { return this.mObjectType; }
        public Image GetImage() { return this.mImage; }
        public Renderer GetRenderer() { return this.mRenderer; }
        public SpriteRenderer GetSpriteRenderer() { return this.mSpriteRenderer; }

        //========================================
        //      Unity's function
        //------------------------------

        //========================================
        //      Self-Define
        //------------------------------
        //----------------------
        // Public Functions
        public virtual void UpdateUnityData()
        {
            switch (GetObjectType())
            {
                case JCS_UnityObjectType.GAME_OBJECT:
                    this.mRenderer = this.GetComponent<Renderer>();
                    break;
                case JCS_UnityObjectType.UI:
                    this.mImage = this.GetComponent<Image>();
                    this.mRectTransform = this.GetComponent<RectTransform>();
                    break;
                case JCS_UnityObjectType.SPRITE:
                    this.mSpriteRenderer = this.GetComponent<SpriteRenderer>();
                    break;
            }
        }

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions

    }
}
