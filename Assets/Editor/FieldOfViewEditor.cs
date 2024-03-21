using UnityEngine;

using UnityEditor;

using Sirenix.OdinInspector.Editor;

[CustomEditor(typeof(FieldOfView))]
public class FieldOfViewEditor : OdinEditor
{
    private void OnSceneGUI()
    {
        FieldOfView fov = target as FieldOfView;

        Handles.color = Color.white;
        Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.forward, 360, fov.ViewRadius);

        Handles.color = Color.magenta;
        Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.forward, 360, fov.RangeAttackRange);

        Handles.color = Color.yellow;
        Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.forward, 360, fov.MeleeAttackRange);

        Handles.color = Color.cyan;
        Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.forward, 360, fov.AbleToMeleeAttackRange);

        if (ColorUtility.TryParseHtmlString("#ff33ff", out Color color))
        {
            Handles.color = color;
            Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.forward, 360, fov.ReadyToCombatRange);
        }

        Vector3 viewAngleA = fov.DirectionFromAngle(-fov.ViewAngle / 2, false);
        Vector3 viewAngleB = fov.DirectionFromAngle(fov.ViewAngle / 2, false);

        Handles.color = Color.white;
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngleA * fov.ViewRadius);
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngleB * fov.ViewRadius);

        Handles.color = Color.red;

        foreach (Transform visibleTarget in fov.VisibleTargetList)
            Handles.DrawLine(fov.transform.position, visibleTarget.position);
    }
}
