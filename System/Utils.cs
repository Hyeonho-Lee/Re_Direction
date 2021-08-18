using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Utils {
    public class Utils {

        public static TextMesh CreateTextMesh(string text, Transform parent, Vector3 localPosition, float cellSize, int fontSize, Color color, TextAnchor textAnchor, TextAlignment textAllgnment, int sortingOrder) {
            GameObject gameObject = new GameObject("test_text", typeof(TextMesh));
            Transform transform = gameObject.transform;
            transform.SetParent(parent, false);
            transform.localPosition = localPosition;
            TextMesh textMesh = gameObject.GetComponent<TextMesh>();
            textMesh.text = text;
            textMesh.fontSize = fontSize;
            textMesh.color = color;
            textMesh.anchor = textAnchor;
            textMesh.alignment = textAllgnment;
            textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
            gameObject.AddComponent<BoxCollider>();
            BoxCollider collider = gameObject.GetComponent<BoxCollider>();
            collider.isTrigger = true;
            collider.size = new Vector3(cellSize - 0.5f, cellSize - 0.5f, 0.1f);
            return textMesh;
        }

        public static GameObject CreateUIGrid(string text, Font font, Transform parent, Vector3 localPosition, float cellSize, int fontSize, Color color, TextAnchor textAllgnment) {
            GameObject gameObject = new GameObject("ui_grid", typeof(RectTransform));
            Transform transform = gameObject.transform;
            transform.SetParent(parent, true);
            transform.localPosition = localPosition;
            RectTransform rect_transform = gameObject.GetComponent<RectTransform>();
            rect_transform.sizeDelta = new Vector2(cellSize, cellSize);
            gameObject.AddComponent<Button>();
            gameObject.AddComponent<Text>();
            Text g_text = gameObject.GetComponent<Text>();
            g_text.text = text;
            g_text.font = font;
            g_text.color = color;
            g_text.fontSize = fontSize;
            g_text.alignment = textAllgnment;
            return gameObject;
        }

        // 마우스 월드오브젝트 좌표
        public static Vector3 GetWorldObjcetPosition() {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) {
                Debug.DrawLine(Camera.main.transform.position, hit.point, Color.red, 1f);
                //Debug.Log(hit.transform.gameObject.name);
            }
            return hit.point;
        }

        // 마우스 스크린 좌표
        public static Vector3 GetScreenPosition() {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Debug.Log("마우스 좌표: " + Input.mousePosition);
            return mousePosition;
        }
    }
}
