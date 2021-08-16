using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemLoot : MonoBehaviour {
    public Collider[] area_object;
    public List<GameObject> area_item = new List<GameObject>();

    public float loot_angle;
    public float loot_range;

    PlayerInventory playerinventory;
    PlayerMovement playermovement;

    void Awake() {
        playerinventory = GameObject.Find("System").GetComponent<PlayerInventory>();
        playermovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
    }

    void Start() {
        loot_angle = 30f;
        loot_range = 1.5f;
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
                ItemData itemdata = area_item[0].gameObject.GetComponent<ItemData>();
                Vector3 player_forward = Quaternion.Euler(0f, 0f, 0f) * transform.forward;
                //Destroy(itemdata.gameObject);
                StartCoroutine(playermovement.Drop_Item(0.5f));
                StartCoroutine(playermovement.UI_Chat(5.0f));
                playerinventory.Find_Empty_ItemSlot(itemdata.input_index, itemdata.input_count);
            } else if(distance.Count >= 1) {
                float min_distance = distance[0];
                int min_index = 0;
                for (int i = 0; i < distance.Count; i++) {
                    if(distance[i] < min_distance) {
                        min_distance = distance[i];
                        min_index = i;
                    }
                }
                ItemData itemdata = area_item[min_index].gameObject.GetComponent<ItemData>();
                //Destroy(itemdata.gameObject);
                StartCoroutine(playermovement.Drop_Item(0.5f));
                StartCoroutine(playermovement.UI_Chat(5.0f));
                playerinventory.Find_Empty_ItemSlot(itemdata.input_index, itemdata.input_count);
            } else {
                //Debug.Log("아이템이 없습니다.");
            }
        }
    }
}
