/*     INFINITY CODE 2013-2016      */
/*   http://www.infinity-code.com   */

using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(OnlineMapsBuildings))]
public class OnlineMapsBuildingsEditor:Editor
{
    private OnlineMapsBuildings buildings;
    private bool showMaterials;

    public void OnEnable()
    {
        buildings = target as OnlineMapsBuildings;
    }

    public override void OnInspectorGUI()
    {
        bool dirty = false;

        if (buildings.zoomRange == null) buildings.zoomRange = new OnlineMapsRange(18, 20);
        float min = buildings.zoomRange.min;
        float max = buildings.zoomRange.max;

        EditorGUI.BeginChangeCheck();
        EditorGUILayout.MinMaxSlider(new GUIContent(string.Format("Zoom ({0}-{1}): ", min, max)), ref min, ref max, 3, 20);
        buildings.zoomRange.min = Mathf.Clamp(Mathf.RoundToInt(min), 3, 20);
        buildings.zoomRange.max = Mathf.Clamp(Mathf.RoundToInt(max), 3, 20);

        if (buildings.zoomRange.min > buildings.zoomRange.max) buildings.zoomRange.min = buildings.zoomRange.max;
        if (buildings.zoomRange.min < 17) EditorGUILayout.HelpBox("Can create a very large number of buildings. This may work slowly.", MessageType.Warning);

        float levelsMin = buildings.levelsRange.min;
        float levelsMax = buildings.levelsRange.max;
        EditorGUILayout.MinMaxSlider(new GUIContent(string.Format("Levels ({0}-{1}): ", levelsMin, levelsMax)), ref levelsMin, ref levelsMax, 1, 100);
        buildings.levelsRange.min = Mathf.Clamp(Mathf.RoundToInt(levelsMin), 1, 100);
        buildings.levelsRange.max = Mathf.Clamp(Mathf.RoundToInt(levelsMax), 1, 100);
        if (buildings.levelsRange.min > buildings.levelsRange.max) buildings.levelsRange.min = buildings.levelsRange.max;

        buildings.levelHeight = EditorGUILayout.FloatField("Level Height:", buildings.levelHeight);
        buildings.minHeight = EditorGUILayout.FloatField("Min Building Height:", buildings.minHeight);
        buildings.heightScale = EditorGUILayout.FloatField("Height Scale:", buildings.heightScale);
        buildings.maxBuilding = EditorGUILayout.IntField("Max Number of Buildings (0-unlimited):", buildings.maxBuilding);
        buildings.maxActiveBuildings = EditorGUILayout.IntField("Max Number of Active Buildings (0-unlimited):", buildings.maxActiveBuildings);

        if (EditorGUI.EndChangeCheck()) dirty = true;

        OnMaterialsGUI(ref dirty);

        if (dirty) EditorUtility.SetDirty(buildings);
    }

    private void OnMaterialsGUI(ref bool dirty)
    {
        bool showMaterialGroup = showMaterials;
        if (showMaterialGroup) EditorGUILayout.BeginVertical(GUI.skin.box);

        if (buildings.materials == null) buildings.materials = new OnlineMapsBuildingMaterial[0];

        showMaterials = OnlineMapsEditor.Foldout(showMaterials, "Materials");
        if (showMaterials)
        {
            int deleteIndex = -1;
            for (int i = 0; i < buildings.materials.Length; i++)
            {
                OnlineMapsBuildingMaterial material = buildings.materials[i];
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("X", GUILayout.ExpandWidth(false))) deleteIndex = i;
                GUILayout.Label((i + 1) + ": ");
                EditorGUILayout.EndHorizontal();

                EditorGUI.BeginChangeCheck();
                material.wall =
                    EditorGUILayout.ObjectField("Wall material", material.wall, typeof (Material), false) as Material;
                material.roof =
                    EditorGUILayout.ObjectField("Roof material", material.roof, typeof (Material), false) as Material;
                material.scale = EditorGUILayout.Vector2Field("Scale", material.scale);

                if (EditorGUI.EndChangeCheck()) dirty = true;
            }

            if (deleteIndex != -1)
            {
                ArrayUtility.RemoveAt(ref buildings.materials, deleteIndex);
                dirty = true;
            }

            if (GUILayout.Button("Add Material"))
            {
                ArrayUtility.Add(ref buildings.materials, new OnlineMapsBuildingMaterial());
                dirty = true;
            }
        }

        if (showMaterialGroup) EditorGUILayout.EndVertical();
    }
}