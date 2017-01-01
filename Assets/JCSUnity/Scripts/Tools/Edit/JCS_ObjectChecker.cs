/**
 * $File: JCS_ObjectChecker.cs $
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
    /// Draw the gizmos on the mesh collider.
    /// </summary>
    [RequireComponent(typeof(MeshCollider))]
    public class JCS_ObjectChecker
        : MonoBehaviour
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables
        public Color wireColor = Color.blue;

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------

        //========================================
        //      Unity's function
        //------------------------------
        private void OnDrawGizmos()
        {
            MeshFilter filter = this.GetComponent<MeshFilter>();
            if (filter != null)
            {
                Gizmos.color = wireColor;
                Mesh mesh = filter.sharedMesh;
                Gizmos.DrawWireMesh(mesh, transform.position, transform.rotation, transform.localScale);
            }
        }

        //========================================
        //      Self-Define
        //------------------------------
        //----------------------
        // Public Functions

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions

    }
}
