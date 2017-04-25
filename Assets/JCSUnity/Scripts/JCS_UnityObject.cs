/**
 * $File: JCS_UnityObject.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;
using UnityEngine.UI;


namespace JCSUnity
{

    /// <summary>
    /// 
    /// </summary>
    public class JCS_UnityObject
        : MonoBehaviour
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables

        [Header("** Initialize Variables (JCS_Unityobject) **")]

        [Tooltip("")]
        [SerializeField]
        protected JCS_UnityObjectType mObjectType = JCS_UnityObjectType.SPRITE;

        //-- Game Object
        protected Renderer mRenderer = null;
        //-- UI
        protected Image mImage = null;
        protected RectTransform mRectTransform = null;
        //-- Sprite
        protected SpriteRenderer mSpriteRenderer = null;
        //-- Text
        protected Text mText = null;

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

        /// <summary>
        /// Get unity specific data by type.
        /// </summary>
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
                case JCS_UnityObjectType.TEXT:
                    this.mText = this.GetComponent<Text>();
                    this.mRectTransform = this.GetComponent<RectTransform>();
                    break;
            }
        }

        /// <summary>
        /// Get/Set the local type.
        /// 
        /// GameObject -> Renderer
        /// Sprite -> Sprite Renderer
        /// UI -> Image
        /// Text -> Text
        /// </summary>
        public Component LocalType
        {
            get
            {
                switch (GetObjectType())
                {
                    case JCS_UnityObjectType.GAME_OBJECT:
                        return this.mRenderer;
                    case JCS_UnityObjectType.SPRITE:
                        return this.mSpriteRenderer;
                    case JCS_UnityObjectType.UI:
                        return this.mImage;
                    case JCS_UnityObjectType.TEXT:
                        return this.mText;
                }

                return null;
            }

            set
            {
                switch (GetObjectType())
                {
                    case JCS_UnityObjectType.GAME_OBJECT:
                        this.mRenderer = (Renderer)value;
                        break;
                    case JCS_UnityObjectType.SPRITE:
                        this.mSpriteRenderer = (SpriteRenderer)value;
                        break;
                    case JCS_UnityObjectType.UI:
                        this.mImage = (Image)value;
                        break;
                    case JCS_UnityObjectType.TEXT:
                        this.mText = (Text)value;
                        break;
                }
            }
        }

        /// <summary>
        /// Get Current type's transform
        /// </summary>
        public Transform LocalTransform
        {
            get
            {
                switch (mObjectType)
                {
                    case JCS_UnityObjectType.GAME_OBJECT:
                        return this.transform;
                    case JCS_UnityObjectType.TEXT:
                    case JCS_UnityObjectType.UI:
                        return this.mRectTransform;
                    case JCS_UnityObjectType.SPRITE:
                        return this.mSpriteRenderer.transform;
                }

                JCS_Debug.JcsErrors(
                    this,
                    "Return default local position...(This should not happens...)");

                return this.transform;
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
                    case JCS_UnityObjectType.TEXT:
                    case JCS_UnityObjectType.UI:
                        return this.mRectTransform.localPosition;
                    case JCS_UnityObjectType.SPRITE:
                        return this.mSpriteRenderer.transform.localPosition;
                }

                JCS_Debug.JcsErrors(
                "JCS_UnityObject",

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
                    case JCS_UnityObjectType.TEXT:
                    case JCS_UnityObjectType.UI:
                        this.mRectTransform.localPosition = value;
                        return;
                    case JCS_UnityObjectType.SPRITE:
                        this.mSpriteRenderer.transform.localPosition = value;
                        return;
                }

                JCS_Debug.JcsErrors(
                "JCS_UnityObject",

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
                    case JCS_UnityObjectType.TEXT:
                    case JCS_UnityObjectType.UI:
                        return this.mRectTransform.localEulerAngles;
                    case JCS_UnityObjectType.SPRITE:
                        return this.mSpriteRenderer.transform.localEulerAngles;
                }

                JCS_Debug.JcsErrors(
                "JCS_UnityObject",

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
                    case JCS_UnityObjectType.TEXT:
                    case JCS_UnityObjectType.UI:
                        this.mRectTransform.localEulerAngles = value;
                        return;
                    case JCS_UnityObjectType.SPRITE:
                        this.mSpriteRenderer.transform.localEulerAngles = value;
                        return;
                }

                JCS_Debug.JcsErrors(
                "JCS_UnityObject",

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
                    case JCS_UnityObjectType.TEXT:
                    case JCS_UnityObjectType.UI:
                        return this.mRectTransform.localScale;
                    case JCS_UnityObjectType.SPRITE:
                        return this.mSpriteRenderer.transform.localScale;
                }

                JCS_Debug.JcsErrors(
                "JCS_UnityObject",

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
                    case JCS_UnityObjectType.TEXT:
                    case JCS_UnityObjectType.UI:
                        this.mRectTransform.localScale = value;
                        return;
                    case JCS_UnityObjectType.SPRITE:
                        this.mSpriteRenderer.transform.localScale = value;
                        return;
                }

                JCS_Debug.JcsErrors(
                "JCS_UnityObject",

                "Set default local scale...(This should not happens...)");
            }
        }

        /// <summary>
        /// Get current type's Enable
        /// </summary>
        public bool LocalEnabled
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
                    case JCS_UnityObjectType.TEXT:
                        return this.mText.enabled;
                }

                JCS_Debug.JcsErrors(
               "JCS_UnityObject",

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
                    case JCS_UnityObjectType.TEXT:
                        this.mSpriteRenderer.enabled = value;
                        return;
                }
            }
        }

        /// <summary>
        /// Get current type's color
        /// </summary>
        public Color LocalColor
        {
            get
            {
                switch (mObjectType)
                {
                    case JCS_UnityObjectType.GAME_OBJECT:
                        return this.mRenderer.material.color;
                    case JCS_UnityObjectType.UI:
                        return this.mImage.color;
                    case JCS_UnityObjectType.SPRITE:
                        return this.mSpriteRenderer.color;
                    case JCS_UnityObjectType.TEXT:
                        return this.mText.color;
                }

                JCS_Debug.JcsErrors(
                    this,
                    "Return default Local Red...(This should not happens...)");

                return new Color(255, 128, 64, 32);
            }

            set
            {
                switch (mObjectType)
                {
                    case JCS_UnityObjectType.GAME_OBJECT:
                        {
                            this.mRenderer.material.color = value;
                        }
                        return;
                    case JCS_UnityObjectType.UI:
                        {
                            this.mImage.color = value;
                        }
                        return;
                    case JCS_UnityObjectType.SPRITE:
                        {
                            this.mSpriteRenderer.color = value;
                        }
                        return;
                    case JCS_UnityObjectType.TEXT:
                        {
                            this.mText.color = value;
                        }
                        return;
                }

                JCS_Debug.JcsErrors(
                    this,
                    "Set default Local Red...(This should not happens...)");
            }
        }

        /// <summary>
        /// Get current type's alpha
        /// </summary>
        public float LocalAlpha
        {
            get
            {
                switch (mObjectType)
                {
                    case JCS_UnityObjectType.GAME_OBJECT:
                        return this.mRenderer.material.color.a;
                    case JCS_UnityObjectType.UI:
                        return this.mImage.color.a;
                    case JCS_UnityObjectType.SPRITE:
                        return this.mSpriteRenderer.color.a;
                    case JCS_UnityObjectType.TEXT:
                        return this.mText.color.a;
                }

                JCS_Debug.JcsErrors(
                    this, "Return default Local Alpha...(This should not happens...)");

                return 0;
            }

            set
            {
                Color newColor;

                switch (mObjectType)
                {
                    case JCS_UnityObjectType.GAME_OBJECT:
                        {
                            newColor = this.mRenderer.material.color;
                            newColor.a = value;
                            this.mRenderer.material.color = newColor;
                        }
                        return;
                    case JCS_UnityObjectType.UI:
                        {
                            newColor = this.mImage.color;
                            newColor.a = value;
                            this.mImage.color = newColor;
                        }
                        return;
                    case JCS_UnityObjectType.SPRITE:
                        {
                            newColor = this.mSpriteRenderer.color;
                            newColor.a = value;
                            this.mSpriteRenderer.color = newColor;
                        }
                        return;
                    case JCS_UnityObjectType.TEXT:
                        {
                            newColor = this.mText.color;
                            newColor.a = value;
                            this.mText.color = newColor;
                        }
                        return;
                }

                JCS_Debug.JcsErrors(
                    this, "Set default Local Alpha...(This should not happens...)");
            }
        }

        /// <summary>
        /// Get current type's red
        /// </summary>
        public float LocalRed
        {
            get
            {
                switch (mObjectType)
                {
                    case JCS_UnityObjectType.GAME_OBJECT:
                        return this.mRenderer.material.color.r;
                    case JCS_UnityObjectType.UI:
                        return this.mImage.color.r;
                    case JCS_UnityObjectType.SPRITE:
                        return this.mSpriteRenderer.color.r;
                    case JCS_UnityObjectType.TEXT:
                        return this.mText.color.r;
                }

                JCS_Debug.JcsErrors(
               "JCS_UnityObject",

               "Return default Local Red...(This should not happens...)");

                return 0;
            }

            set
            {
                Color newColor;

                switch (mObjectType)
                {
                    case JCS_UnityObjectType.GAME_OBJECT:
                        {
                            newColor = this.mRenderer.material.color;
                            newColor.r = value;
                            this.mRenderer.material.color = newColor;
                        }
                        return;
                    case JCS_UnityObjectType.UI:
                        {
                            newColor = this.mImage.color;
                            newColor.r = value;
                            this.mImage.color = newColor;
                        }
                        return;
                    case JCS_UnityObjectType.SPRITE:
                        {
                            newColor = this.mSpriteRenderer.color;
                            newColor.r = value;
                            this.mSpriteRenderer.color = newColor;
                        }
                        return;
                    case JCS_UnityObjectType.TEXT:
                        {
                            newColor = this.mText.color;
                            newColor.r = value;
                            this.mText.color = newColor;
                        }
                        return;
                }

                JCS_Debug.JcsErrors(
              "JCS_UnityObject",

              "Set default Local Red...(This should not happens...)");
            }
        }

        /// <summary>
        /// Get current type's green
        /// </summary>
        public float LocalGreen
        {
            get
            {
                switch (mObjectType)
                {
                    case JCS_UnityObjectType.GAME_OBJECT:
                        return this.mRenderer.material.color.g;
                    case JCS_UnityObjectType.UI:
                        return this.mImage.color.g;
                    case JCS_UnityObjectType.SPRITE:
                        return this.mSpriteRenderer.color.g;
                    case JCS_UnityObjectType.TEXT:
                        return this.mText.color.g;
                }

                JCS_Debug.JcsErrors(
              "JCS_UnityObject",

              "Return default Local Green...(This should not happens...)");

                return 0;
            }

            set
            {
                Color newColor;

                switch (mObjectType)
                {
                    case JCS_UnityObjectType.GAME_OBJECT:
                        {
                            newColor = this.mRenderer.material.color;
                            newColor.g = value;
                            this.mRenderer.material.color = newColor;
                        }
                        return;
                    case JCS_UnityObjectType.UI:
                        {
                            newColor = this.mImage.color;
                            newColor.g = value;
                            this.mImage.color = newColor;
                        }
                        return;
                    case JCS_UnityObjectType.SPRITE:
                        {
                            newColor = this.mSpriteRenderer.color;
                            newColor.g = value;
                            this.mSpriteRenderer.color = newColor;
                        }
                        return;
                    case JCS_UnityObjectType.TEXT:
                        {
                            newColor = this.mText.color;
                            newColor.g = value;
                            this.mText.color = newColor;
                        }
                        return;
                }

                JCS_Debug.JcsErrors(
              "JCS_UnityObject",

              "Set default Local Blue...(This should not happens...)");
            }
        }

        /// <summary>
        /// Get current type's blue
        /// </summary>
        public float LocalBlue
        {
            get
            {
                switch (mObjectType)
                {
                    case JCS_UnityObjectType.GAME_OBJECT:
                        return this.mRenderer.material.color.b;
                    case JCS_UnityObjectType.UI:
                        return this.mImage.color.b;
                    case JCS_UnityObjectType.SPRITE:
                        return this.mSpriteRenderer.color.b;
                    case JCS_UnityObjectType.TEXT:
                        return this.mText.color.b;
                }

                JCS_Debug.JcsErrors(
                    "JCS_UnityObject",
                    "Return default Local Blue...(This should not happens...)");

                return 0;
            }

            set
            {
                Color newColor;

                switch (mObjectType)
                {
                    case JCS_UnityObjectType.GAME_OBJECT:
                        {
                            newColor = this.mRenderer.material.color;
                            newColor.b = value;
                            this.mRenderer.material.color = newColor;
                        }
                        return;
                    case JCS_UnityObjectType.UI:
                        {
                            newColor = this.mImage.color;
                            newColor.b = value;
                            this.mImage.color = newColor;
                        }
                        return;
                    case JCS_UnityObjectType.SPRITE:
                        {
                            newColor = this.mSpriteRenderer.color;
                            newColor.b = value;
                            this.mSpriteRenderer.color = newColor;
                        }
                        return;
                    case JCS_UnityObjectType.TEXT:
                        {
                            newColor = this.mText.color;
                            newColor.b = value;
                            this.mText.color = newColor;
                        }
                        return;
                }

                JCS_Debug.JcsErrors(
              "JCS_UnityObject",

              "Set default Local Blue...(This should not happens...)");
            }
        }

        /// <summary>
        /// Get current type's main texture
        /// </summary>
        public Texture LocalMainTexture
        {
            get
            {
                switch (mObjectType)
                {
                    case JCS_UnityObjectType.GAME_OBJECT:
                        return this.mRenderer.material.mainTexture;
                    case JCS_UnityObjectType.UI:
                        return this.mImage.material.mainTexture;
                    case JCS_UnityObjectType.SPRITE:
                        return this.mSpriteRenderer.material.mainTexture;
                    case JCS_UnityObjectType.TEXT:
                        return this.mText.material.mainTexture;
                }

                JCS_Debug.JcsErrors(
                    "JCS_UnityObject",
                    "Return default Local Blue...(This should not happens...)");

                return null;
            }

            set
            {
                switch (mObjectType)
                {
                    case JCS_UnityObjectType.GAME_OBJECT:
                        {
                            this.mRenderer.material.mainTexture = value;
                        }
                        return;
                    case JCS_UnityObjectType.UI:
                        {
                            this.mImage.material.mainTexture = value;
                        }
                        return;
                    case JCS_UnityObjectType.SPRITE:
                        {
                            this.mSpriteRenderer.material.mainTexture = value;
                        }
                        return;
                    case JCS_UnityObjectType.TEXT:
                        {
                            this.mText.material.mainTexture = value;
                        }
                        return;
                }

                JCS_Debug.JcsErrors(
              "JCS_UnityObject",

              "Set default Local Blue...(This should not happens...)");
            }
        }

        /// <summary>
        /// Get the sprite.
        /// </summary>
        public Sprite LocalSprite
        {
            get
            {
                switch (GetObjectType())
                {
                    case JCS_UnityObjectType.SPRITE:
                        return this.mSpriteRenderer.sprite;
                    case JCS_UnityObjectType.UI:
                        return this.mImage.sprite;
                }

                JCS_Debug.JcsErrors(
                    "JCS_UnityObject",
                    "Failed to get sprite composite cuz current unity object setting does not have it.");

                return null;
            }

            set
            {
                switch (GetObjectType())
                {
                    case JCS_UnityObjectType.SPRITE:
                        this.mSpriteRenderer.sprite = value;
                        return;
                    case JCS_UnityObjectType.UI:
                        this.mImage.sprite = value;
                        return;
                }

                JCS_Debug.JcsErrors(
                    "JCS_UnityObject",
                    "Failed to set the sprite cuz the current unity object setting does not have sprite coposite.");
            }
        }

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions

    }
}
