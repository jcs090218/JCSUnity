/**
 * $File: JCS_UsefualFunctions.cs $
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
    // Function pointer
    public delegate int JCS_Range(int min, int max);


    public class JCS_UsefualFunctions 
        : MonoBehaviour
    {
        public static GameObject SpawnGameObject(string objectPath, Vector3 position = new Vector3(), Quaternion rotation = new Quaternion())
        {
            return Instantiate(Resources.Load<GameObject>(objectPath), position, rotation) as GameObject;
        }

        public static Object SpawnGameObject(Object trans, Vector3 position = new Vector3(), Quaternion rotation = new Quaternion())
        {
            if (trans == null)
                return null;

            return Instantiate(trans, position, rotation);
        }

        public static string HeaderDecorator(string desc)
        {
            return "** " + desc + " **";
        }

        public static bool WithInAcceptRange(float range, float acceptRange, float currentVal)
        {
            return WithInRange(range - acceptRange, range + acceptRange, currentVal);
        }

        public static bool WithInRange(float minRange, float maxRange, float currentVal)
        {
            if (currentVal >= minRange &&
                currentVal <= maxRange)
            {
                return true;
            }

            return false;
        }

        public static Vector2 GetImageRect(Image img)
        {
            RectTransform rt = img.transform.GetComponent<RectTransform>();
            if (rt == null)
            {
                JCS_GameErrors.JcsErrors("JCS_UsefulFunctions", -1, "No RectTransform on ur image!");
                return Vector2.one;
            }

            float width = rt.sizeDelta.x * rt.localPosition.x;
            float height = rt.sizeDelta.y * rt.localPosition.y;

            return new Vector2(width, height);
        }

        public static Vector2 GetSpriteRendererRect(SpriteRenderer sr)
        {
            float width = sr.bounds.extents[0] * 2;
            float height = sr.bounds.extents[1] * 2;

            return new Vector2(width, height);
        }

        /// <summary>
        /// Return normal random range (Integer)
        /// </summary>
        /// <param name="min"> mininum value </param>
        /// <param name="max"> maxinum value </param>
        /// <returns> random number </returns>
        public static int JCS_IntRange(int min, int max)
        {
            return Random.Range(min, max);
        }
        public static uint JCS_IntRange(uint min, uint max)
        {
            return (uint)Random.Range(min, max);
        }
        /// <summary>
        /// Return normal random range (Float)
        /// </summary>
        /// <param name="min"> mininum value </param>
        /// <param name="max"> maxinum value </param>
        /// <returns> random number </returns>
        public static float JCS_FloatRange(float min, float max)
        {
            return Random.Range(min, max);
        }

        /// <summary>
        /// Solve the flash problem! (JCS_CheckableObject)
        /// 
        /// Check if the mouse still on top of the image!
        /// </summary>
        /// <returns></returns>
        public static bool MouseOverGUI(RectTransform imageRect, RectTransform rootPanel = null)
        {
            Vector2 mousePos = JCS_Input.MousePositionOnGUILayer();
            Vector2 checkPos = imageRect.localPosition;

            if (rootPanel != null)
                checkPos += new Vector2(rootPanel.localPosition.x, rootPanel.localPosition.y);

            // this item image size
            Vector2 slotRect = imageRect.sizeDelta;

            float halfSlotWidth = slotRect.x / 2 * imageRect.localScale.x;
            float halfSlotHeight = slotRect.y / 2 * imageRect.localScale.y;

            float leftBorder = checkPos.x - halfSlotWidth;
            float rightBorder = checkPos.x + halfSlotWidth;
            float topBorder = checkPos.y + halfSlotHeight;
            float bottomBorder = checkPos.y - halfSlotHeight;

            // Basic AABB collide math
            if (mousePos.x <= rightBorder &&
                mousePos.x >= leftBorder &&
                mousePos.y <= topBorder &&
                mousePos.y >= bottomBorder)
            {
                return true;
            }

            return false;
        }

    }
}
