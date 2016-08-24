using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Soomla;

[CustomEditor(typeof(SoomlaEditorScript))]
public class SoomlaSettingsEditor : Editor
{

	public void OnEnable()
	{
		SoomlaEditorScript.OnEnable();
	}

    public override void OnInspectorGUI()
    {

		SoomlaEditorScript.OnInspectorGUI();
    }

	public void OnDisable() {
		// NOTE: nothing to do here..
	}

}
