using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utils;

public class InventoryGrid {
    private int width;
    private int height;
    private float cellSize;
    private Vector3 originPosition;
    private int[,] grid_array;
    //private TextMesh[,] debugTextArray;
    private GameObject[,] debugTextArray;

    public InventoryGrid(int width, int height, float cellSize, Vector3 originPosition, Font font, Transform parent) {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;

        grid_array = new int[width, height];
        //debugTextArray = new TextMesh[width, height];
        debugTextArray = new GameObject[width, height];

        for (int x = 0; x < grid_array.GetLength(0); x++) {
            for (int y = 0; y < grid_array.GetLength(1); y++) {
                //debugTextArray[x, y] = Utils.Utils.CreateTextMesh(grid_array[x, y].ToString(), null, GetWorldPosition(x, y) + (new Vector3(cellSize, cellSize) * 0.5f), cellSize, 20, Color.black, TextAnchor.MiddleCenter);
                debugTextArray[x, y] = Utils.Utils.CreateUIGrid(grid_array[x, y].ToString(), font, parent, GetWorldPosition(x, y) + (new Vector3(cellSize, cellSize) * 0.5f), cellSize, 40, Color.black, TextAnchor.MiddleCenter);
            }
        }
    }

    private Vector3 GetWorldPosition(int x, int y) {
        Vector3 result = new Vector3(x, y) * cellSize + originPosition;
        Debug.Log(result);
        return result;
    }

    private void GetXY(Vector3 worldPosition, out int x, out int y) {
        x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
        y = Mathf.FloorToInt((worldPosition - originPosition).y / cellSize);
    }

    // 수동 값 변경
    public void SetValue(int x, int y, int value) {
        if (x >= 0 && y >= 0 && x < width && y < height) {
            grid_array[x, y] = value;
            Text text = debugTextArray[x, y].GetComponent<Text>();
            text.text = grid_array[x, y].ToString();
        }
    }

    // 마우스 값 변경(월드 좌표)
    public void SetValue(Vector3 worldPosition, int value) {
        int x, y;
        GetXY(worldPosition, out x, out y);
        SetValue(x, y, value);
    }

    public int GetValue(int x, int y) {
        if (x >= 0 && y >= 0 && x < width && y < height) {
            return grid_array[x, y];
        }else {
            return 0;
        }
    }

    public int GetValue(Vector3 worldPosition) {
        int x, y;
        GetXY(worldPosition, out x, out y);
        return GetValue(x, y);
    }
}