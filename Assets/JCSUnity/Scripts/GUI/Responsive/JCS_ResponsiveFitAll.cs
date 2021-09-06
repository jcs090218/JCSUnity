/**
 * $File: JCS_ResponsiveFitAll.cs $
 * $Date: 2021-09-06 15:49:01 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information
 *	                 Copyright © 2021 by Shen, Jen-Chieh $
 */
using System.Collections.Generic;
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// Force the rect transform to match to full screen.
    /// </summary>
    public class JCS_ResponsiveFitAll : MonoBehaviour
    {
        /* Variables */

        private RectTransform mRectTransform = null;

        /* Setter & Getter */

        /* Functions */

        private void Start()
        {
            this.mRectTransform = this.GetComponent<RectTransform>();

            var panelChild = this.GetComponent<JCS_PanelChild>();

            List<RectTransform> childs = null;
            if (panelChild == null)
                childs = JCS_Utility.ForceDetachChildren(this.mRectTransform);

            JCS_Utility.ReattachSelf(this.mRectTransform, (parent) =>
            {
                mRectTransform.localScale = Vector3.one;
                mRectTransform.sizeDelta = new Vector2(Screen.width, Screen.height);
            });

            if (panelChild == null)
                JCS_Utility.AttachChildren(this.mRectTransform, childs);

            mRectTransform.localPosition = Vector3.zero;
        }
    }
}