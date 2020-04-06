/**
 * $File: JCS_3DHintBubble.cs $
 * $Date: 2020-04-06 14:25:49 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright © 2020 by Shen, Jen-Chieh $
 */

/* NOTE: If you are using `TextMesh Pro` uncomment this line.
 */
#define TMP_PRO

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if TMP_PRO
using TMPro;
#endif

namespace JCSUnity
{
    /// <summary>
    /// Hint bubble in 3D using Text Mesh Pro.
    /// </summary>
    [RequireComponent(typeof(TextMeshPro))]
    [RequireComponent(typeof(JCS_TweenerHandler))]
    [RequireComponent(typeof(JCS_FadeObject))]
    public class JCS_3DHintBubble
        : MonoBehaviour
    {
        /* Variables */

        private TextMeshPro mTextMesh = null;

#if UNITY_EDITOR
        [Header("** Helper Variables (JCS_3DHintBubble) **")]

        [Tooltip("Key to active hint bubble.")]
        [SerializeField]
        private KeyCode mActiveKey = KeyCode.A;

        [Tooltip("Key to active hint bubble.")]
        [SerializeField]
        private KeyCode mDeactiveKey = KeyCode.S;

#endif

        [Header("** Check Variables (JCS_3DHintBubble) **")]

        [Tooltip("Tweeener handler.")]
        [SerializeField]
        private JCS_TweenerHandler mTweenerHandler = null;

        [Tooltip("Fade the bubble hint.")]
        [SerializeField]
        private JCS_FadeObject mFadeObject = null;

        /* Setter & Getter */

        public TextMeshPro TextMesh { get { return this.mTextMesh; } }
        public JCS_TweenerHandler TweenerHandler { get { return this.mTweenerHandler; } }
        public JCS_FadeObject FadeObject { get { return this.mFadeObject; } }

        /* Functions */

        private void Awake()
        {
            this.mTextMesh = this.GetComponent<TextMeshPro>();
            this.mTweenerHandler = this.GetComponent<JCS_TweenerHandler>();
            this.mFadeObject = this.GetComponent<JCS_FadeObject>();
        }

#if UNITY_EDITOR
        private void Update()
        {
            if (JCS_Input.GetKeyDown(mActiveKey))
                Active();
            else if (JCS_Input.GetKeyDown(mDeactiveKey))
                Deactive();
        }
#endif

        public void SendText(string msg)
        {
            mTextMesh.text = msg;
        }

        public void Active()
        {
            TweenerHandler.DoAllTweenToTargetValue();
        }

        public void Deactive()
        {
            TweenerHandler.DoAllTweenToStartValue();
        }
    }
}
