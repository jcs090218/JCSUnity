/**
 * $File: JCS_CameraRenderer.cs $
 * $Date: 2017-07-23 08:28:23 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2017 by Shen, Jen-Chieh $
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// Show the shader effect in the editor mode.
    /// </summary>
    [ExecuteInEditMode]
    public class JCS_CameraRenderer
        : MonoBehaviour
    {

        /*******************************************/
        /*            Public Variables             */
        /*******************************************/

        /*******************************************/
        /*           Private Variables             */
        /*******************************************/
        [Tooltip("Material going to use render the image.")]
        [SerializeField]
        private Material mRenderMaterial = null;

        /*******************************************/
        /*           Protected Variables           */
        /*******************************************/

        /*******************************************/
        /*             setter / getter             */
        /*******************************************/
        public Material RenderMaterial { get { return this.mRenderMaterial; } set { this.mRenderMaterial = value; } }

        /*******************************************/
        /*            Unity's function             */
        /*******************************************/
        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            if (mRenderMaterial == null)
                return;

            Graphics.Blit(source, destination, mRenderMaterial);
        }

        /*******************************************/
        /*              Self-Define                */
        /*******************************************/
        //----------------------
        // Public Functions

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions

    }
}
