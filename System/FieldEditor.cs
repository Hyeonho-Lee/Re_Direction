using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof (PlayerField))]
public class FieldEditor : Editor {
    void OnSceneGUI() {
        PlayerField fow = (PlayerField)target;
        Handles.color = Color.red;
        Handles.DrawWireArc(fow.transform.position, Vector3.up, Vector3.forward, 360, fow.field_radius);

        Vector3 angle_a = fow.Get_Angle(-fow.field_angle / 2, false);
        Vector3 angle_b = fow.Get_Angle(fow.field_angle / 2, false);
        Handles.DrawLine(fow.transform.position, fow.transform.position + angle_a * fow.field_radius);
        Handles.DrawLine(fow.transform.position, fow.transform.position + angle_b * fow.field_radius);
    }
}
