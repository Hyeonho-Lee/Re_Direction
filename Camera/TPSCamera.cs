using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPSCamera : MonoBehaviour {
    [Header("Camera Status")]
    public Transform Player_Transform;
    public Transform Camera_Arm;
    public Transform Reset_Transform;
    public bool is_joystick;
    public GameObject box;

    public float mouse_horizontal;
    public float mouse_vertical;
    public float whell_value;
    public float camera_distance;

    public Vector3 camera_dir;
    public Vector3 player_dir;
    public Vector3 camera_angle;
    public Vector3 dir_nor;

    private RaycastHit camera_hit;

    void Start() {
        whell_value = 1f;
    }

    void Update() {
        Look_Movement();
        Input_whell();
        Raycast_Camera();
    }
    public float test;
    void Look_Movement() {
        if (!is_joystick) {
            mouse_horizontal = Input.GetAxis("Mouse X");
            mouse_vertical = Input.GetAxis("Mouse Y");
        } else {
            mouse_horizontal = Input.GetAxis("J_Horizontal") * 3f;
            mouse_vertical = -Input.GetAxis("J_Vertical") * 3f;
            if (Mathf.Abs(mouse_horizontal) < 0.03f * 3f && Mathf.Abs(mouse_vertical) < 0.03f * 3f) {
                mouse_horizontal = 0f;
                mouse_vertical = 0f;
            }
        }

        camera_angle = Camera_Arm.rotation.eulerAngles;
        float x = camera_angle.x - mouse_vertical;
        test = x;

        if (x < 180.0f) {
            x = Mathf.Clamp(x, -1.0f, 50.0f);
        }else {
            x = Mathf.Clamp(x, 335.0f, 361.0f);
        }

        Camera_Arm.rotation = Quaternion.Euler(x, camera_angle.y + mouse_horizontal, camera_angle.z);

        camera_dir = Player_Transform.transform.position - this.transform.position;
        dir_nor = camera_dir.normalized;
        player_dir.Set(camera_dir.x, 0, camera_dir.z);
        Debug.DrawRay(Player_Transform.transform.position, camera_dir * 0.3f, Color.red);
    }

    void Raycast_Camera() {
        Debug.DrawRay(this.transform.position, camera_dir, Color.blue);
        camera_distance = Vector3.Distance(Player_Transform.transform.position, this.transform.position);

        if (Physics.Linecast(Player_Transform.transform.position, this.transform.position, out camera_hit)) {
            camera_distance = Vector3.Distance(Player_Transform.transform.position, camera_hit.point);
            if (camera_hit.point != Vector3.zero && camera_distance >= 3.5f) {
                box.transform.position = camera_hit.point + (dir_nor * 2f);
            }
        }else {
            box.transform.position = Reset_Transform.transform.position + (dir_nor * whell_value);
        }
    }

    void Input_whell() {
        float whell = Input.GetAxisRaw("Mouse ScrollWheel");
        if (whell > 0) {
            if (whell_value < 9)
                whell_value = whell_value + 0.5f;
        } else if (whell < 0) {
            if (whell_value > 1)
                whell_value = whell_value - 0.5f;
        }
    }
}
