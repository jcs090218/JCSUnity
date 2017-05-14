/**
 * $File: JCS_Debug.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;


namespace JCSUnity
{

    /// <summary>
    /// Debugging class.
    /// </summary>
    public static class JCS_Debug
    {
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="script"></param>
        /// <param name="desc"></param>
        public static void LogError(string script, string desc)
        {
            //string currentFile = new System.Diagnostics.StackTrace(true).GetFrame(0).GetFileName();
            int currentLine = -1;
#if (UNITY_EDITOR)
            currentLine = new System.Diagnostics.StackTrace(true).GetFrame(0).GetFileLineNumber();
#endif

            JcsLog("Errors", script, currentLine, desc);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="script"></param>
        /// <param name="desc"></param>
        public static void LogError(Object script, string desc)
        {
            LogError(script.GetType().Name, desc);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="script"></param>
        /// <param name="desc"></param>
        public static void LogReminders(string script, string desc)
        {
            int currentLine = -1;
#if (UNITY_EDITOR)
            currentLine = new System.Diagnostics.StackTrace(true).GetFrame(0).GetFileLineNumber();
#endif

            JcsLog("Reminders", script, currentLine, desc);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="script"></param>
        /// <param name="desc"></param>
        public static void LogReminders(Object script, string desc)
        {
            LogReminders(script.GetType().Name, desc);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="script"></param>
        /// <param name="desc"></param>
        public static void Log(string script, string desc)
        {
            int currentLine = -1;
#if (UNITY_EDITOR)
            currentLine = new System.Diagnostics.StackTrace(true).GetFrame(0).GetFileLineNumber();
#endif

            JcsLog("Log", script, currentLine, desc);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="script"></param>
        /// <param name="desc"></param>
        public static void Log(Object script, string desc)
        {
            JcsLog(script.GetType().Name, desc);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="script"></param>
        /// <param name="desc"></param>
        public static void LogWarning(string script, string desc)
        {
            int currentLine = -1;
#if (UNITY_EDITOR)
            currentLine = new System.Diagnostics.StackTrace(true).GetFrame(0).GetFileLineNumber();
#endif

            JcsLog("Warnings", script, currentLine, desc);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="script"></param>
        /// <param name="desc"></param>
        public static void LogWarning(Object script, string desc)
        {
            LogWarning(script.GetType().Name, desc);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="script"></param>
        /// <param name="line"></param>
        /// <param name="desc"></param>
        private static void JcsLog(string type, string script, int line = -1, string desc = "")
        {
#if (UNITY_EDITOR)
            Debug.Log("\n=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=");
            Debug.Log("=-= JCSUnity." + type + " - [" + script + "](" + line + ")" + desc + " =-=");
            Debug.Log("=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=\n");
#endif
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="script"></param>
        /// <param name="line"></param>
        /// <param name="desc"></param>
        private static void LogError(string type, string script, int line = -1, string desc = "")
        {
#if (UNITY_EDITOR)
            Debug.LogError("\n=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=");
            Debug.LogError("=-= JCSUnity." + type + " - [" + script + "](" + line + ")" + desc + " =-=");
            Debug.LogError("=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=\n");
#endif
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="script"></param>
        /// <param name="line"></param>
        /// <param name="desc"></param>
        private static void LogWarning(string type, string script, int line = -1, string desc = "")
        {
#if (UNITY_EDITOR)
            Debug.LogWarning("\n=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=");
            Debug.LogWarning("=-= JCSUnity." + type + " - [" + script + "](" + line + ")" + desc + " =-=");
            Debug.LogWarning("=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=\n");
#endif
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        public static void DrawLine(Vector2 from, Vector2 to)
        {
            Vector3 tempFrom = new Vector3(from.x, from.y, 0);
            Vector3 tempTo = new Vector3(to.x, to.y, 0);

            Debug.DrawLine(tempFrom, tempTo);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="col"></param>
        public static void DrawLine(Vector2 from, Vector2 to, Color col)
        {
            Vector3 tempFrom = new Vector3(from.x, from.y, 0);
            Vector3 tempTo = new Vector3(to.x, to.y, 0);

            Debug.DrawLine(tempFrom, tempTo, col);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        public static void DrawLine(Vector3 from, Vector3 to)
        {
            Debug.DrawLine(from, to);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="col"></param>
        public static void DrawLine(Vector3 from, Vector3 to, Color col)
        {
            Debug.DrawLine(from, to, col);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="topLeft"></param>
        /// <param name="topRight"></param>
        /// <param name="botRight"></param>
        /// <param name="botLeft"></param>
        /// <param name="col"></param>
        public static void DrawRect(Vector3 topLeft, Vector3 topRight, Vector3 botRight, Vector3 botLeft, Color col)
        {
            Debug.DrawLine(topLeft, topRight, col);
            Debug.DrawLine(topRight, botRight, col);
            Debug.DrawLine(botRight, botLeft, col);
            Debug.DrawLine(botLeft, topLeft, col);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="topLeft"></param>
        /// <param name="topRight"></param>
        /// <param name="botRight"></param>
        /// <param name="botLeft"></param>
        public static void DrawRect(Vector3 topLeft, Vector3 topRight, Vector3 botRight, Vector3 botLeft)
        {
            // set default color as white
            DrawRect(topLeft, topRight, botRight, botLeft, Color.white);
        }

        /// <summary>
        /// Print out the transform name.
        /// </summary>
        /// <param name="trans"> name of the transform. </param>
        public static void PrintName(Transform trans)
        {
            Debug.Log("Name: " + trans.name);
        }
        

        /// <summary>
        /// Draw the collider
        /// </summary>
        /// <param name="collider"> Collider u want to draw. </param>
        /// <param name="col"> Color type. </param>
        public static void DrawCollider(BoxCollider2D collider, Color col)
        {
#if (UNITY_EDITOR)
            // get width and height information.
            Vector2 boxInfo = JCS_Physics.GetColliderInfo(collider);

            Vector3 pos = JCS_Physics.GetColliderPosition(collider);

            Vector3 topLeft = new Vector3(
                pos.x - boxInfo.x / 2,
                pos.y + boxInfo.y / 2);
            Vector3 topRight = new Vector3(
                pos.x + boxInfo.x / 2,
                pos.y + boxInfo.y / 2);
            Vector3 botRight = new Vector3(
                pos.x + boxInfo.x / 2,
                pos.y - boxInfo.y / 2);
            Vector3 botLeft = new Vector3(
                pos.x - boxInfo.x / 2,
                pos.y - boxInfo.y / 2);

            DrawRect(topLeft, topRight, botRight, botLeft, col);
#endif
        }

        /// <summary>
        /// Draw the collider, use for check last frame.
        /// </summary>
        /// <param name="collider"> Collider u want to draw. </param>
        /// <param name="col"> Color type. </param>
        /// <param name="origin">Provide the origin position. </param>
        public static void DrawCollider(BoxCollider2D collider, Color col, Vector3 origin)
        {
#if (UNITY_EDITOR)
            // get width and height information.
            Vector2 boxInfo = JCS_Physics.GetColliderInfo(collider);

            Vector3 pos = origin;

            Vector3 topLeft = new Vector3(
                pos.x - boxInfo.x / 2,
                pos.y + boxInfo.y / 2);
            Vector3 topRight = new Vector3(
                pos.x + boxInfo.x / 2,
                pos.y + boxInfo.y / 2);
            Vector3 botRight = new Vector3(
                pos.x + boxInfo.x / 2,
                pos.y - boxInfo.y / 2);
            Vector3 botLeft = new Vector3(
                pos.x - boxInfo.x / 2,
                pos.y - boxInfo.y / 2);

            DrawRect(topLeft, topRight, botRight, botLeft, col);
#endif
        }

    }
}
