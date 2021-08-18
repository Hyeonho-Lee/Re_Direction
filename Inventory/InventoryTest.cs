using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Utils;

public class InventoryTest : MonoBehaviour {
    public int x;
    public int y;
    public float cellSize;

    private Vector2 offset;

    public Transform tests;
    public Font font;

    private InventoryGrid inventory_grid;

    void Start() {
        offset = new Vector2(-(x / 2 * cellSize), -(y / 2 * cellSize));
        inventory_grid = new InventoryGrid(x, y, cellSize, offset, font, tests);
        //new InventoryGrid(2, 5, 5f, new Vector2(30f, 0f));
    }

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            if (EventSystem.current.IsPointerOverGameObject() == true) {
                Vector3 click_position = EventSystem.current.currentSelectedGameObject.transform.localPosition;
                inventory_grid.SetValue(click_position, 5);
            }
        }
    }
}
