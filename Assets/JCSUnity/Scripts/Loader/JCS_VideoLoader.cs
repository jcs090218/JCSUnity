#if UNITY_5_6_OR_NEWER
/**
 * $File: JCS_VideoLoader.cs $
 * $Date: 2018-07-30 18:55:46 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright © 2018 by Shen, Jen-Chieh $
 */
using UnityEngine.Video;

namespace JCSUnity
{
    /// <summary>
    /// Video loader, load an external video file.
    /// </summary>
    public static class JCS_VideoLoader
    {
        /// <summary>
        /// Video loader, load an external video file.
        /// </summary>
        /// <param name="vp"> Video player reference. </param>
        /// <param name="path"> directory to the target file. </param>
        /// <param name="filename"> file of the video file with extension. </param>
        /// <returns> Video player ref. </returns>
        public static VideoPlayer LoadVideo(VideoPlayer vp, string path, string filename)
        {
            string url = path + filename;
            return LoadVideo(vp, url);
        }

        /// <summary>
        /// Load an external video file.
        /// </summary>
        /// <param name="vp"> Video player reference. </param>
        /// <param name="url"> Absolute file path to the external video file. </param>
        /// <returns> Video player ref. </returns>
        public static VideoPlayer LoadVideo(VideoPlayer vp, string url)
        {
            VideoPlayer tempVP = vp;

            // Since Unity 5.6, they provide their own video encoder which
            // is nice we do not need to consider this too much.
            // We simply just apply the file path to the VideoPlayer's
            // url variable then it should work.
            vp.url = url;

            return tempVP;
        }
    }
}
#endif
