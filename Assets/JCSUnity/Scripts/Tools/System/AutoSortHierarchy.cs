/**
 * $File: A_to_Z.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using UnityEditor;

public class A_to_Z : BaseHierarchySort
{

    public override int Compare(GameObject lhs, GameObject rhs)
    {
        if (lhs == rhs) return 0;
        if (lhs == null) return -1;
        if (rhs == null) return 1;

        return EditorUtility.NaturalCompare(lhs.name, rhs.name);
    }
}

public class Z_to_A : BaseHierarchySort
{

    public override int Compare(GameObject lhs, GameObject rhs)
    {
        if (lhs == rhs) return 0;
        if (lhs == null) return -1;
        if (rhs == null) return 1;

        return EditorUtility.NaturalCompare(rhs.name, lhs.name);
    }
}
