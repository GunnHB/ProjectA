using UnityEditor;

[CustomEditor(typeof(UIRawImage))]
public class UIRawImageEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
    }
}
