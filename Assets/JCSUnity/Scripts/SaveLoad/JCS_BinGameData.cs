/**
 * $File: JCS_BinGameData.cs $
 * $Date: 2018-06-27 11:03:19 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright © 2018 by Shen, Jen-Chieh $
 */
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;


namespace JCSUnity
{
    /// <summary>
    /// Binary game data format.
    /// </summary>
    [System.Serializable]
    public class JCS_BinGameData
    {
        public string Copyright = "";
        public string Version = "";

        private void InitJCSFile()
        {
            if (JCS_PackageDataSettings.instance == null)
            {
#if (UNITY_EDITOR)
                JCS_Debug.LogError("JCS_XMLGameData",
                    "Failed to load the copyright and version information text...");
#endif 
                return;
            }

            Copyright = JCS_PackageDataSettings.instance.CopyrightString;
            Version = JCS_PackageDataSettings.instance.VersionString;
        }

        /// <summary>
        /// Save the game data into binary file format.
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
                var binFmt = new BinaryFormatter();
                JCS_Debug.Log(Copyright);
                binFmt.Serialize(stream, this);
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
                var binFmt = new BinaryFormatter();
                return (T)binFmt.Deserialize(stream);
            }
        }
    }
}
