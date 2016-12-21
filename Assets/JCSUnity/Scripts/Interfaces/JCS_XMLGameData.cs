/**
 * $File: JCS_XMLGameData.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;
using System.Xml.Serialization;
using System.IO;
using System;


namespace JCSUnity
{

    /// <summary>
    /// Base if the game data format.
    /// </summary>
    public abstract class JCS_XMLGameData
    {

        public string Copyright = "";
        public string Version = "";


        private void InitJCSFile()
        {
            if (JCS_PackageDataSettings.instance == null)
            {
#if (UNITY_EDITOR)
                JCS_Debug.JcsErrors("JCS_XMLGameData", 
                    "Failed to load the copyright and version information text...");
#endif 
                return;
            }

            Copyright = JCS_PackageDataSettings.instance.CopyrightString;
            Version = JCS_PackageDataSettings.instance.VersionString;
        }

        /// <summary>
        /// Save the game data into xml file format.
        /// </summary>
        /// <typeparam name="T"> type of the data save. </typeparam>
        /// <param name="filePath"> where to save. </param>
        /// <param name="fileName"> name of the file u want to save. </param>
        public void Save<T>(string filePath, string fileName)
        {
            // if Directory does not exits, create it prevent error!
            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);

            InitJCSFile();

            using (var stream = new FileStream(filePath + fileName, FileMode.Create))
            {
                var XML = new XmlSerializer(typeof(T));
                XML.Serialize(stream, this);
            }
        }

        /// <summary>
        /// Load the game data from a directory path.
        /// </summary>
        /// <typeparam name="T"> type of the game data u want to load. </typeparam>
        /// <param name="filePath"> file directory, location, path. </param>
        /// <param name="fileName"> name of the file u want to load. </param>
        /// <returns></returns>
        public static T LoadFromFile<T>(string filePath, string fileName)
        {
            // if Directory does not exits, create it prevent error!
            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);

            using (var stream = new FileStream(filePath + fileName, FileMode.Open))
            {
                var xml = new XmlSerializer(typeof(T));
                return (T)xml.Deserialize(stream);
            }
        }

    }
}
