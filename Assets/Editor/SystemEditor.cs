using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;

using UnityEditor;

public class SystemEditor : OdinEditorWindow
{
    [MenuItem("CustomEditor/SystemEditor")]
    public static void OpenWindow()
    {
        GetWindow<SystemEditor>();
    }

    [BoxGroup("Play"), Button(ButtonSizes.Gigantic), DisableIf(nameof(IsPlaying)), GUIColor("magenta")]
    public void Run()
    {
        EditorApplication.isPlaying = true;
    }

    [BoxGroup("Play"), HorizontalGroup("Play/Horizontal"), Button(ButtonSizes.Large)]
    [DisableIf(nameof(IsPlaying)), GUIColor("green")]
    public void BuildBundleAndRun()
    {
        CreateAssetBundle.BuildAssetBundles();

        EditorApplication.isPlaying = true;
    }

    [BoxGroup("Play"), HorizontalGroup("Play/Horizontal"), Button(ButtonSizes.Large)]
    [EnableIf(nameof(IsPlaying)), GUIColor("cyan")]
    public void Stop()
    {
        EditorApplication.isPlaying = false;
    }

    private bool IsPlaying()
    {
        return Application.isPlaying;
    }
}
