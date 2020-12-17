/**
 * $File: JCS_SafeArea.cs $
 * $Date: 2020-12-17 14:36:09 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright © 2020 by Shen, Jen-Chieh $
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// 
    /// </summary>
    public class JCS_SafeArea
        : MonoBehaviour
    {
        /* Variables */

        private RectTransform Panel = null;
        private Rect LastSafeArea = new Rect(0, 0, 0, 0);

        /* Setter & Getter */

        /* Functions */

        private void Awake()
        {
            Panel = GetComponent<RectTransform>();
            Refresh();
        }

        private void Update()
        {
            Refresh();
        }

        private void Refresh()
        {
            Rect safeArea = GetSafeArea();

            if (safeArea != LastSafeArea)
                ApplySafeArea(safeArea);
        }

        private Rect GetSafeArea()
        {
            return Screen.safeArea;
        }

        private void ApplySafeArea(Rect r)
        {
            LastSafeArea = r;

            // Convert safe area rectangle from absolute pixels to normalised anchor coordinates
            Vector2 anchorMin = r.position;
            Vector2 anchorMax = r.position + r.size;
            anchorMin.x /= Screen.width;
            anchorMin.y /= Screen.height;
            anchorMax.x /= Screen.width;
            anchorMax.y /= Screen.height;
            Panel.anchorMin = anchorMin;
            Panel.anchorMax = anchorMax;
        }
    }
}
