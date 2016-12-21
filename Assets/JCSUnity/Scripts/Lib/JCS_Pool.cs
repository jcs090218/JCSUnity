/**
 * $File: JCS_Pool.cs $
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
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class JCS_Pool<T>
    {

        private JCS_Vector<T> objects = null;
        private int mLastUseIndex = 0;


        public JCS_Vector<T> GetObjects() { return this.objects; }
        public int LastUseIndex { get { return this.mLastUseIndex; } }

        public JCS_Pool(int num)
        {
            objects = new JCS_Vector<T>(num);
        }

        public void ReturnToPool()
        {

        }

    }
}
