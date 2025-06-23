#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CSVLoader))]
public class CSVLoaderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        CSVLoader loader = (CSVLoader)target;
        if (GUILayout.Button("CSVからマップをエディタ上で生成"))
        {
            loader.GenerateInEditor();
        }
    }
}
#endif
