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
        [Header("** Initialize Variables (JCS_Unityobject) **")]
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
        public void SetObjectType(JCS_UnityObjectType ob) { this.mObjectType = ob; }
        public JCS_UnityObjectType GetObjectType() { return this.mObjectType; }
        public Image GetImage() { return this.mImage; }
        public Renderer GetRenderer() { return this.mRenderer; }
        public SpriteRenderer GetSpriteRenderer() { return this.mSpriteRenderer; }
        public RectTransform GetRectTransform() { return this.mRectTransform; }

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
        
        /// <summary>
        /// Get Current type's position
        /// </summary>
        public Vector3 LocalPosition
        {
            get
            {
                switch (mObjectType)
                {
                    case JCS_UnityObjectType.GAME_OBJECT:
                        return this.transform.localPosition;
                    case JCS_UnityObjectType.UI:
                        return this.mRectTransform.localPosition;
                    case JCS_UnityObjectType.SPRITE:
                        return this.mSpriteRenderer.transform.localPosition;
                }

                JCS_GameErrors.JcsErrors(
                "JCS_UnityObject",
                -1,
                "Return default local position...(This should not happens...)");

                return this.transform.localPosition;
            }

            set
            {
                switch (mObjectType)
                {
                    case JCS_UnityObjectType.GAME_OBJECT:
                        this.transform.localPosition = value;
                        return;
                    case JCS_UnityObjectType.UI:
                        this.mRectTransform.localPosition = value;
                        return;
                    case JCS_UnityObjectType.SPRITE:
                        this.mSpriteRenderer.transform.localPosition = value;
                        return;
                }

                JCS_GameErrors.JcsErrors(
                "JCS_UnityObject",
                -1,
                "Set default local position...(This should not happens...)");
            }
        }
        /// <summary>
        /// Get Current type's rotation
        /// </summary>
        public Vector3 LocalRotation
        {
            get
            {
                switch (mObjectType)
                {
                    case JCS_UnityObjectType.GAME_OBJECT:
                        return this.transform.localEulerAngles;
                    case JCS_UnityObjectType.UI:
                        return this.mRectTransform.localEulerAngles;
                    case JCS_UnityObjectType.SPRITE:
                        return this.mSpriteRenderer.transform.localEulerAngles;
                }

                JCS_GameErrors.JcsErrors(
                "JCS_UnityObject",
                -1,
                "Return default local rotation...(This should not happens...)");

                return this.transform.localEulerAngles;
            }

            set
            {
                switch (mObjectType)
                {
                    case JCS_UnityObjectType.GAME_OBJECT:
                        this.transform.localEulerAngles = value;
                        return;
                    case JCS_UnityObjectType.UI:
                        this.mRectTransform.localEulerAngles = value;
                        return;
                    case JCS_UnityObjectType.SPRITE:
                        this.mSpriteRenderer.transform.localEulerAngles = value;
                        return;
                }

                JCS_GameErrors.JcsErrors(
                "JCS_UnityObject",
                -1,
                "Set default local rotation...(This should not happens...)");
            }
        }
        /// <summary>
        /// Get Current type's scale
        /// </summary>
        public Vector3 LocalScale
        {
            get
            {
                switch (mObjectType)
                {
                    case JCS_UnityObjectType.GAME_OBJECT:
                        return this.transform.localScale;
                    case JCS_UnityObjectType.UI:
                        return this.mRectTransform.localScale;
                    case JCS_UnityObjectType.SPRITE:
                        return this.mSpriteRenderer.transform.localScale;
                }

                JCS_GameErrors.JcsErrors(
                "JCS_UnityObject",
                -1,
                "Return default local scale...(This should not happens...)");

                return this.transform.localScale;
            }

            set
            {
                switch (mObjectType)
                {
                    case JCS_UnityObjectType.GAME_OBJECT:
                        this.transform.localScale = value;
                        return;
                    case JCS_UnityObjectType.UI:
                        this.mRectTransform.localScale = value;
                        return;
                    case JCS_UnityObjectType.SPRITE:
                        this.mSpriteRenderer.transform.localScale = value;
                        return;
                }

                JCS_GameErrors.JcsErrors(
                "JCS_UnityObject",
                -1,
                "Set default local scale...(This should not happens...)");
            }
        }


        public bool Visible
        {
            get
            {
                switch (mObjectType)
                {
                    case JCS_UnityObjectType.GAME_OBJECT:
                        return this.mRenderer.enabled;
                    case JCS_UnityObjectType.UI:
                        return this.mImage.enabled;
                    case JCS_UnityObjectType.SPRITE:
                        return this.mSpriteRenderer.enabled;
                }

                JCS_GameErrors.JcsErrors(
               "JCS_UnityObject",
               -1,
               "Return default visible...(This should not happens...)");


                // return default
                return false;
            }

            set
            {
                switch (mObjectType)
                {
                    case JCS_UnityObjectType.GAME_OBJECT:
                        this.mRenderer.enabled = value;
                        return;
                    case JCS_UnityObjectType.UI:
                        this.mImage.enabled = value;
                        return;
                    case JCS_UnityObjectType.SPRITE:
                        this.mSpriteRenderer.enabled = value;
                        return;
                }
            }
        }

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions

    }
}
