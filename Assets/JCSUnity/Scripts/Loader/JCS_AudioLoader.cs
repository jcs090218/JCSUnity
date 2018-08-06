/**
 * $File: JCS_AudioLoader.cs $
 * $Date: 2018-07-30 18:56:01 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright © 2018 by Shen, Jen-Chieh $
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace JCSUnity
{
    public delegate void AudioLoaded(AudioClip ac);

    /// <summary>
    /// Audiol loader, load an external audio file.
    /// </summary>
    public class JCS_AudioLoader
    {
        /// <summary>
        /// Load the music from path in runetime.
        /// </summary>
        /// <param name="ac"> A container for audio data. </param>
        /// <param name="path"> file path, not include filename. </param>
        /// <param name="filename"> file name of the ogg file. </param>
        /// <param name="callback"> Callback after the audio is loaded. </param>
        /// <returns> Coroutine status. </returns>
        public static IEnumerator LoadAudio(
            AudioClip ac,
            string path,
            string filename,
            AudioLoaded callback = null)
        {
            string fullFilePath = path + filename;

            return LoadAudio(ac, fullFilePath, callback);
        }

        /// <summary>
        /// Load the music from path in runtime.
        /// </summary>
        /// <param name="ac"> A container for audio data. </param>
        /// <param name="fullFilePath"> Filpath to the target audio file. </param>
        /// <param name="callback"> Callback after the audio is loaded. </param>
        /// <returns> Coroutine status. </returns>
        public static IEnumerator LoadAudio(
            AudioClip ac,
            string fullFilePath,
            AudioLoaded callback = null)
        {
            string formatFullFilePath = string.Format("file://{0}", fullFilePath);

            WWW request = new WWW(formatFullFilePath);
            yield return request;

            ac = request.GetAudioClip(false, false);

            if (callback != null)
                callback.Invoke(ac);
        }
    }
}
