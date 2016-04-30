/*     INFINITY CODE 2013-2016      */
/*   http://www.infinity-code.com   */

using UnityEditor;

[CustomEditor(typeof(OnlineMapsBuildingBase), true)]
public class OnlineMapsBuildingBaseEditor:Editor
{
    private OnlineMapsBuildingBase building;

    private void OnEnable()
    {
        building = target as OnlineMapsBuildingBase;
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.LabelField("Meta count: " + building.metaInfo.Length);
        EditorGUI.BeginDisabledGroup(true);
        foreach (OnlineMapsBuildingMetaInfo item in building.metaInfo) EditorGUILayout.TextField(item.title, item.info);
        EditorGUI.EndDisabledGroup();
    }
}