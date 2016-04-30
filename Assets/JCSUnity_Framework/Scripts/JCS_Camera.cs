/**
 * $File: JCS_Camera.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information $
 *		                Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;
using System.IO;

namespace JCSUnity
{

    [RequireComponent(typeof(Camera))]
    public abstract class JCS_Camera : MonoBehaviour
    {
        protected Camera mCamera = null;

        public Camera GetCamera() { return this.mCamera; }



        protected virtual void Awake()
        {
            this.mCamera = this.GetComponent<Camera>();
        }

        /// <summary>
        /// Weather if we want to take a screen shot, call
        /// this function will do the trick!
        /// </summary>
        public virtual void TakeScreenShot()
        {
#if (UNITY_STANDALONE || UNITY_EDITOR)
            /**
             * Data path info:
             *      - http://docs.unity3d.com/ScriptReference/Application-dataPath.html
             */


            // get the last saved screen shot's index
            int last_saved_index = SearchDirectory(
                Application.dataPath + JCS_GameSettings.SCREENSHOT_PATH,
                JCS_GameSettings.SCREENSHOT_FILENAME) + 1;

            Application.CaptureScreenshot(
                Application.dataPath +
                JCS_GameSettings.SCREENSHOT_PATH +
                JCS_GameSettings.SCREENSHOT_FILENAME +
                last_saved_index +
                JCS_GameSettings.SAVED_IMG_EXTENSION);
#endif
        }

        public virtual int SearchDirectory(string path, string removeStr)
        {
            // if Directory does not exits, create it prevent error!
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            string fileName = "";
            string ext = "";
            int last_saved_screenshot = 0;
            foreach (string file in Directory.GetFiles(path))
            {
                fileName = Path.GetFileNameWithoutExtension(file);
                ext = Path.GetExtension(file);

                // check if is the .png file 
                // (screen shot can only be image file)
                if (!ext.Equals(".png"))
                    continue;

                int index = fileName.IndexOf(removeStr);
                int len = removeStr.Length;
                string startOfString = fileName.Substring(0, index);
                string endOfString = fileName.Substring(index + len);
                string cleanPath = startOfString + endOfString;

                print(cleanPath);

                last_saved_screenshot = System.Int32.Parse(cleanPath);
            }

            return last_saved_screenshot;
        }
    }
}
