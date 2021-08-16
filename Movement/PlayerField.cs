using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerField : MonoBehaviour {
    public float field_radius;
    [Range(0, 360)]
    public float field_angle;

    public LayerMask target_mask;
    public LayerMask wall_mask;

    public List<GameObject> find_target = new List<GameObject>();

    void Start() {
        StartCoroutine(Find_Target_Delay(0.2f));
    }

    IEnumerator Find_Target_Delay(float delay) {
        while (true) {
            yield return new WaitForSeconds(delay);
            Find_Object();
        }
    }

    void Find_Object() {
        find_target.Clear();
        Collider[] target_field = Physics.OverlapSphere(transform.position, field_radius, target_mask);

        for (int i = 0; i < target_field.Length; i++) {
            Transform target = target_field[i].transform;
            Vector3 dir_target = (target.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dir_target) < field_angle / 2) {
                float distance_target = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, dir_target, distance_target, wall_mask)) {
                    find_target.Add(target.gameObject);
                }
            }
        }
    }


    public Vector3 Get_Angle(float angle, bool is_angle) {
        if (!is_angle) {
            angle += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0, Mathf.Cos(angle * Mathf.Deg2Rad));
    }
}
