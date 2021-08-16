using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class InventoryTest : MonoBehaviour {
    public int x = 4;
    public int y = 2;
    public float cellSize = 10f;
    public Vector2 offset = new Vector2(0f, 0f);
    public Transform tests;

    private InventoryGrid inventory_grid;

    void Start() {
        //inventory_grid = new InventoryGrid(10, 10, 2f, new Vector2(0f, 0f));
        //new InventoryGrid(2, 5, 5f, new Vector2(30f, 0f));
    }

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            //inventory_grid.SetValue(Utils.Utils.GetWorldObjcetPosition(), 56);
        }

        if (Input.GetMouseButtonDown(1)) {
            //Debug.Log(inventory_grid.GetValue(Utils.Utils.GetWorldObjcetPosition()));
        }
    }
}
