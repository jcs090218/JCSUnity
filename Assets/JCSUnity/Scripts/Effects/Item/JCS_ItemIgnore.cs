/**
 * $File: JCS_ItemIgnore.cs $
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
    /// When every item touch this will ignore the collision
    /// </summary>
    public class JCS_ItemIgnore
        : MonoBehaviour
    {
        [SerializeField] private bool mEffectToAllChild = true;

        private void Start()
        {
            AddEffectToAllChild();
        }

        private void AddEffectToAllChild()
        {
            if (!mEffectToAllChild)
                return;

            // add to all the child as the same effect
            for (int index = 0;
                index < this.transform.childCount;
                ++index)
            {
                transform.GetChild(index).gameObject.AddComponent<JCS_ItemIgnore>();
            }
        }
    }
}
