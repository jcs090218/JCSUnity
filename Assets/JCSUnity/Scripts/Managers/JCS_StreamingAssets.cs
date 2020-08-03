/**
 * $File: JCS_StreamingAssets.cs $
 * $Date: 2020-08-03 22:47:47 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright © 2020 by Shen, Jen-Chieh $
 */
using Ookii.Dialogs;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UIElements;

namespace JCSUnity
{
    public delegate void RequestCallback(string path);

    /// <summary>
    /// Generic streaming assets interface.
    /// </summary>
    public class JCS_StreamingAssets
        : JCS_Managers<JCS_StreamingAssets>
    {
        /* Variables */

        private byte[] REQ_KEY = Encoding.ASCII.GetBytes("WAIT-090218");

        private byte[] mResultData = null;

        private RequestCallback mRequestCallback = null;

        [Header("** Check Variables (JCS_StreamingAssets) **")]

        [Tooltip("List of file that we are going to download as target.")]
        [SerializeField]
        private List<string> mDownloadList = null;

        [Tooltip("Flag to see if currently downloading the file.")]
        [SerializeField]
        private bool mRequesting = false;

        [Tooltip("Full request URL.")]
        [SerializeField]
        private string mRequestURL = "";

        [Tooltip("Request data path.")]
        [SerializeField]
        private string mRequestPath = "";

        [Header("** Initialize Variables (JCS_StreamingAssets) **")]

        [Tooltip("Preload streaming assets path.")]
        public List<string> preloadPath = null;

        /* Setter & Getter */

        public bool Requesting { get { return this.mRequesting; } }
        public string RequestURL { get { return this.mRequestURL; } }
        public string RequestPath { get { return this.mRequestPath; } }

        /* Functions */

        private void Awake()
        {
            instance = this;

            mDownloadList = preloadPath;
        }

        private void Update()
        {
            ProcessDownload();
        }

        /// <summary>
        /// Return streaming assets path.
        /// </summary>
        public static string StreamingAssetsPath()
        {
            return Application.streamingAssetsPath;
        }

        /// <summary>
        /// Return the streaming assets cache path.
        /// </summary>
        public static string CachePath()
        {
            var gs = JCS_GameSettings.instance;
            return Application.dataPath + gs.STREAMING_CACHE_PATH;
        }

        /// <summary>
        /// Return static URL path.
        /// </summary>
        public static string UrlPath()
        {
            var gs = JCS_GameSettings.instance;
            return gs.STREAMING_BASE_URL;
        }

        /// <summary>
        /// Check if current data are requesting data.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool NeedRequestData(byte[] data)
        {
            return REQ_KEY == data;
        }

        /// <summary>
        /// Get all the bytes by PATH.
        /// </summary>
        public byte[] ReadAllBytes(string path, RequestCallback callback = null)
        {
            byte[] data = TryCacheData(path);
            if (data == null) data = TryStreamingAssets(path);
            if (data == null)
            {
                AddDownloadTarget(path);
                data = REQ_KEY;
            }
            return data;
        }

        /// <summary>
        /// Add a download target by streaming assets PATH.
        /// </summary>
        public void AddDownloadTarget(string path)
        {
            mDownloadList.Add(path);
        }

        private bool ValidCacheData(string path)
        {
            string filePath = CachePath() + path;
            return JCS_IO.IsFile(filePath);
        }

        private bool ValidStreamingAssets(string path)
        {
            string filePath = StreamingAssetsPath() + path;
            return JCS_IO.IsFile(filePath);
        }

        private byte[] TryCacheData(string path)
        {
            if (!ValidCacheData(path))
                return null;
            string filePath = CachePath() + path;
            return File.ReadAllBytes(filePath);
        }

        private byte[] TryStreamingAssets(string path)
        {
            if (!ValidStreamingAssets(path))
                return null;
            string filePath = StreamingAssetsPath() + path;
            return File.ReadAllBytes(filePath);
        }

        private void TryUrlData(string path)
        {
            mResultData = null;
            mRequestPath = path;
            mRequestURL = UrlPath() + mRequestPath;

            mRequesting = true;

            StartCoroutine(GetData());
        }

        private IEnumerator GetData()
        {
            UnityWebRequest www = UnityWebRequest.Get(mRequestURL);
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                if (JCS_GameSettings.instance.DEBUG_MODE)
                    JCS_Debug.LogWarning(www.error);
            }
            else
            {
                mResultData = www.downloadHandler.data;
                WriteFileAsCache(mRequestPath, mResultData);
            }

            mRequesting = false;
        }

        /// <summary>
        /// Write a file and make it as cache data file.
        /// </summary>
        /// <param name="path"> Path to write to. </param>
        /// <param name="data"> Bytes data to write with. </param>
        private static void WriteFileAsCache(string path, byte[] data)
        {
            string filePath = CachePath() + path;
            string dirPath = Path.GetDirectoryName(filePath);
            JCS_IO.CreateDirectory(dirPath);
            File.WriteAllBytes(filePath, data);
        }

        /// <summary>
        /// Process the download list and try to download all target files.
        /// </summary>
        private void ProcessDownload()
        {
            if (mDownloadList.Count == 0 || mRequesting)
                return;

            TryUrlData(mDownloadList[0]);

            mDownloadList.RemoveAt(0);
        }
    }
}
