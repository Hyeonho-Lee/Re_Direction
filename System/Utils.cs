using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utils {
    public class Utils {
        // TextMesh 문자를 오브젝트로 출력
        public static TextMesh CreateTextMesh(string text = null, Transform parent = null, Vector3 localPosition = default(Vector3), float cellSize = 0.0f, int fontSize = 40, Color? color = null, TextAnchor textAnchor = TextAnchor.UpperLeft, TextAlignment textAllgnment = TextAlignment.Left, int sortingOrder = 5000) {
            if (color == null)
                color = Color.white;

            return CreateTextMesh(text, parent, localPosition, cellSize, fontSize, (Color)color, textAnchor, textAllgnment, sortingOrder);
        }

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

        // 마우스 스크린 좌표
        public static Vector3 GetScreenMousePosition() {
            Vector3 screenPosition = GetWorldMousePosition(Input.mousePosition, Camera.main);
            screenPosition.z = 0.0f;
            return screenPosition;
        }

        // 마우스 월드 좌표
        public static Vector3 GetWorldMousePosition(Vector3 screenPosition, Camera worldCamera) {
            Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
            return worldPosition;
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
    }
}
