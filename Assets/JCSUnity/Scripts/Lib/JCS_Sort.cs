/**
 * $File: JCS_Sort.cs $
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
    /// Sorting object.
    /// </summary>
    public class JCS_Sort<T>
        where T : JCS_SortingObject
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables

        // NOTE(JenChieh): not sure template support this kind of feature.
        private T[] mArray = null;  // any array with this sort

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------
        public T[] Array { get { return this.mArray; } }

        //========================================
        //      Self-Define
        //------------------------------
        //----------------------
        // Public Functions
        public JCS_Sort() { }

        public void AddAll(T[] arr)
        {
            // copy array
            mArray = arr;
        }

        public T[] InsertionSort()
        {
            for (int i = 0; i < mArray.Length; ++i)
            {
                for (int j = i; j > 0; --j)
                {
                    if (mArray[j].OrderIndex < mArray[j - 1].OrderIndex)
                    {
                        T temp = mArray[j];
                        mArray[j] = mArray[j - 1];
                        mArray[j - 1] = temp;
                    }
                }
            }

            return mArray;
        }

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions

    }
}
