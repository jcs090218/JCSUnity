/**
 * $File: JCS_Debug.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information $
 *		                Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;


namespace JCSUnity
{

    public class JCS_Debug
        : MonoBehaviour
    {

        public static void DrawLine(Vector2 from, Vector2 to)
        {
            Vector3 tempFrom = new Vector3(from.x, from.y, 0);
            Vector3 tempTo = new Vector3(to.x, to.y, 0);

            Debug.DrawLine(tempFrom, tempTo);
        }

        public static void DrawLine(Vector3 from, Vector3 to)
        {
            Debug.DrawLine(from, to);
        }

        public static void DrawRect(Vector3 topLeft, Vector3 topRight, Vector3 botRight, Vector3 botLeft, Color col)
        {
            Debug.DrawLine(topLeft, topRight, col);
            Debug.DrawLine(topRight, botRight, col);
            Debug.DrawLine(botRight, botLeft, col);
            Debug.DrawLine(botLeft, topLeft, col);
        }
        public static void DrawRect(Vector3 topLeft, Vector3 topRight, Vector3 botRight, Vector3 botLeft)
        {
            // set default color as white
            DrawRect(topLeft, topRight, botRight, botLeft, Color.white);
        }


    }
}
