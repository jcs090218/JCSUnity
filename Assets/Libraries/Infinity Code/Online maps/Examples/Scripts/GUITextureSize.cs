/*     INFINITY CODE 2013-2016      */
/*   http://www.infinity-code.com   */

using UnityEngine;

namespace InfinityCode.OnlineMapsDemos
{
    [AddComponentMenu("")]
    // ReSharper disable once UnusedMember.Global
    public class GUITextureSize : MonoBehaviour
    {
        // ReSharper disable once UnusedMember.Local
        private void Start()
        {
#if UNITY_4_3 || UNITY_4_5 || UNITY_4_6
            GUITexture gt = guiTexture;
            Rect pi = guiTexture.pixelInset;
#else
            GUITexture gt = GetComponent<GUITexture>();
            Rect pi = gt.pixelInset;
    #endif
            float sw = Screen.width / (float) gt.texture.width;
            float sh = Screen.height / (float) gt.texture.height;

            if (sw > sh)
            {
                pi.width = Screen.width;
                pi.height = sw * gt.texture.height;
            }
            else
            {
                pi.height = Screen.height;
                pi.width = sh * gt.texture.width;
            }

            pi.x = pi.width / -2;
            pi.y = pi.height / -2;

            gt.pixelInset = pi;
        }
    }
}