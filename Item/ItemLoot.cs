using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ItemLoot : MonoBehaviour {
    public Collider[] area_object;
    public List<GameObject> area_item = new List<GameObject>();

    public float loot_angle;
    public float loot_range;

    void Start() {
        loot_angle = 30f;
        loot_range = 3f;
    }

    
    void Update() {
    }

    void OnDrawGizmos() {
        Color green = new Color(0.0f, 1.0f, 0.0f, 0.2f);
        Color red = new Color(1.0f, 0.0f, 0.0f, 0.6f);

        Handles.color = red;
        Handles.DrawWireDisc(transform.position, Vector3.up, loot_range);
        //Handles.DrawSolidArc(transform.position, Vector3.up, transform.forward, loot_angle, loot_range);
        //Handles.DrawSolidArc(transform.position, Vector3.up, transform.forward, -loot_angle, loot_range);
    }

    public void Check_Area() {
        List<float> distance = new List<float>();
        area_item = new List<GameObject>();
        area_object = Physics.OverlapSphere(transform.position, loot_range);

        if(area_object.Length != 0) {
            for (int i = 0; i < area_object.Length; i++) {
                if(area_object[i].CompareTag("Item")) {
                    distance.Add(Vector3.Distance(transform.position, area_object[i].transform.position));
                    area_item.Add(area_object[i].gameObject);
                }
            }
            if(distance.Count == 1) {
                Debug.Log("가장 가까운 아이템 = " + area_item[0]);
            } else if(distance.Count >= 1) {
                float min_distance = distance[0];
                int min_index = 0;
                for (int i = 0; i < distance.Count; i++) {
                    if(distance[i] < min_distance) {
                        min_distance = distance[i];
                        min_index = i;
                    }
                }
                Debug.Log("가장 가까운 아이템 = " + area_item[min_index]);
            }
        }
    }
}
