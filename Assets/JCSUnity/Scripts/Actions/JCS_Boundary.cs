/**
 * $File: JCS_Boundary.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information
 *                   Copyright (c) 2025 by Shen, Jen-Chieh $
 */
using UnityEngine;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// The boundary definition.
    /// </summary>
    public class JCS_Boundary : MonoBehaviour
    {
        /* Variables */

#if UNITY_EDITOR
        [Separator("Helper Variables (JCS_Boundary)")]

        [Tooltip("The wire color for gizmos draw.")]
        [SerializeField]
        private Color mWireColor = new Color(0.8f, 0.3f, 0.3f, 0.8f);
#endif

        /* Setter & Getter */

        /* Functions */

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = mWireColor;
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
        }
#endif

        /// <summary>
        /// The bounds of this boundary.
        /// </summary>
        public Bounds GetBounds()
        {
            Matrix4x4 matrix = transform.localToWorldMatrix;

            // Transform each corner to world space
            Vector3 worldCorner = matrix.MultiplyPoint3x4(JCS_Constants.CORNERS_CUBE[0]);

            var bounds = new Bounds(worldCorner, Vector3.zero);

            for (int index = 1; index < JCS_Constants.CORNERS_CUBE.Length; ++index)
            {
                worldCorner = matrix.MultiplyPoint3x4(JCS_Constants.CORNERS_CUBE[index]);

                bounds.Encapsulate(worldCorner);
            }

            return bounds;
        }
    }
}
