/**
 * $File: JCS_Marquee.cs $
 * $Date: 2019-07-22 11:59:29 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information
 *                   Copyright © 2019 by Shen, Jen-Chieh $
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JCSUnity
{
    /// <summary>
    /// Marquee, for announcing, display highlighted text, etc.
    /// </summary>
    [RequireComponent(typeof(Image))]
    [RequireComponent(typeof(Mask))]
    public class JCS_Marquee
        : MonoBehaviour
    {
        /* Variables */

        [Header("** Check Variables (JCS_Marquee) **")]

        [SerializeField]
        private JCS_3DDistanceTileAction mDistanceTileAction = null;


        [Header("** Initialize Variables (JCS_Marquee) **")]

        [Tooltip("Text object that is hiddent under mask.")]
        [SerializeField]
        private Text mTextContainer = null;
        

        /* Setter/Getter */


        /* Functions */

        private void Awake()
        {
            if (mTextContainer == null)
                mTextContainer = this.GetComponentInChildren<Text>();

            mDistanceTileAction = JCS_Utility.ForceGetComponent<JCS_3DDistanceTileAction>(mTextContainer);
        }

#if UNITY_EDITOR
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.H))
                SetText("爽拉~", true);
            if (Input.GetKeyDown(KeyCode.J))
                SetText("又是一個廣播~", false);
        }
#endif

        /// <summary>
        /// Set the marquee text and replay from start.
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="loop"></param>
        public void SetText(string msg, bool loop = true)
        {
            if (mTextContainer == null)
            {
                JCS_Debug.Log("Can't display the marquee text without text container");
                return;
            }

            this.mTextContainer.text = msg;

            if (!loop)
            {
                mDistanceTileAction.afterResetCallback = () =>
                {
                    mDistanceTileAction.Active = false;
                };
            }
            else
            {
                mDistanceTileAction.afterResetCallback = null;
                mDistanceTileAction.Active = true;
            }

            mDistanceTileAction.ResetPosition();
        }
    }
}
