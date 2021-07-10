using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public float horizontal;
    public float vertical;

    public float mass;
    public float jump_speed;
    public float walk_speed;
    public float dash_speed;
    public float move_speed;
    public float velocity;
    public float attack_combo;
    public float attack_time;
    public float roll_wait;
    public float roll_time;
    public float roll_speed;

    public bool is_ground;
    public bool is_roll;
    public bool is_rolls;
    public bool is_roll_timer;
    public bool is_attack;
    public bool is_jump;
    public bool is_dash;
    public bool is_ctrl;
    public bool is_fly;
    public float ground_height;
    public float ground_range;

    public LayerMask ground_hit;

    public Vector3 movement;
    public Vector3 animation_movement;
    public Vector3 player_dir;
    public Vector3 player_forward;
    public Vector3 impact;
    private Vector3 dir_forward, dir_f_right, dir_right, dir_b_right, dir_back, dir_b_left, dir_left, dir_f_left;

    CharacterController cc;
    TPSCamera tpscamera;
    ItemLoot itemloot;

    void Awake() {
        cc = GetComponent<CharacterController>();
        tpscamera = GameObject.Find("TPS_Camera").GetComponent<TPSCamera>();
        itemloot = GetComponent<ItemLoot>();
    }

    void Start() {
        mass = 1.0f;
        jump_speed = 2f;
        walk_speed = 5.0f;
        dash_speed = 5.0f;
        velocity = 0.0f;
        ground_height = -0.08f;
        ground_range = 0.65f;
        attack_time = 0.0f;
        attack_combo = 1.0f;
        roll_wait = 0.0f;
        roll_time = 0.3f;
        roll_speed = 20.0f;
        impact = Vector3.zero;
    }

    void Update() {
        Check_Input();
        Check_Ground();
        Check_Value();
    }

    void FixedUpdate() {
        Move();
    }

    void OnDrawGizmos() {
        Color green = new Color(0.0f, 1.0f, 0.0f, 0.5f);
        Color red = new Color(1.0f, 0.0f, 0.0f, 0.5f);

        Gizmos.color = is_ground ? green : red;
        Gizmos.DrawSphere(transform.position - new Vector3(0, ground_height, 0), ground_range);
    }

    void Check_Input() {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        player_dir = tpscamera.player_dir.normalized;

        if (Input.GetKeyDown(KeyCode.LeftShift) && !is_attack) {
            is_dash = true;
            if (!is_roll_timer) {
                roll_wait = Time.time;
                is_roll_timer = true;
            } else if (is_roll_timer && ((Time.time - roll_wait) < roll_time)) {
                StartCoroutine(Roll());
            }
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
            is_dash = false;

        if (Input.GetKeyDown(KeyCode.Space) && is_ground && !is_attack) {
            StartCoroutine(Jump());
        }

        if (Input.GetKeyDown(KeyCode.F)) {
            itemloot.Check_Area();
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
            is_ctrl = true;

        if (Input.GetKeyUp(KeyCode.LeftControl))
            is_ctrl = false;

        if (Input.GetMouseButtonDown(0) && !is_attack && !is_rolls) {
            is_attack = true;
            attack_time = 0;
            StartCoroutine(Attack());
        }
    }

    void Check_Value() {
        if (is_ground) {
            is_fly = false;
            /*if (velocity < 0.0f) {
                velocity = -2.0f;
            }*/
        } else {
            is_fly = true;
            if (velocity < 53.0f) {
                velocity += -15.0f * Time.deltaTime;
            }
        }

        if (is_dash)
            move_speed = walk_speed + dash_speed;
        else
            move_speed = walk_speed;

        if (is_ctrl)
            move_speed = move_speed / 2;

        if (is_roll_timer && ((Time.time - roll_wait) > roll_time)) {
            is_roll_timer = false;
        }

        if (is_attack) {
            horizontal = 0;
            vertical = 0;
        }
    }

    void Move() {
        movement.Set(horizontal, 0, vertical);
        movement = movement.normalized;

        dir_forward = Quaternion.Euler(0f, 0f, 0f) * player_dir;
        dir_f_right = Quaternion.Euler(0f, 45f, 0f) * player_dir;
        dir_right = Quaternion.Euler(0f, 90f, 0f) * player_dir;
        dir_b_right = Quaternion.Euler(0f, 135f, 0f) * player_dir;
        dir_back = Quaternion.Euler(0f, 180f, 0f) * player_dir;
        dir_b_left = Quaternion.Euler(0f, 225f, 0f) * player_dir;
        dir_left = Quaternion.Euler(0f, 270f, 0f) * player_dir;
        dir_f_left = Quaternion.Euler(0f, 315f, 0f) * player_dir;

        if (movement.z == 0 && movement.x == 0) {
            if (is_roll) {
                StartCoroutine(Add_Impact(transform.forward, roll_speed));
                is_roll = false;
            }else {
                cc.Move(movement * (walk_speed * Time.deltaTime) + new Vector3(0.0f, velocity, 0.0f) * Time.deltaTime);
            }
        }

        // ¾Õ¿À
        if ((movement.z > 0 && movement.z <= 1) && (movement.x > 0 && movement.x <= 1)) {
            if (is_roll) {
                StartCoroutine(Add_Impact(dir_f_right, roll_speed));
                is_roll = false;
            }else {
                Move_Vector(dir_f_right, dir_f_right);
            }
        }

        // ¾Õ¿Þ
        if ((movement.z > 0 && movement.z <= 1) && (movement.x < 0 && movement.x >= -1)) {
            if (is_roll) {
                StartCoroutine(Add_Impact(dir_f_left, roll_speed));
                is_roll = false;
            }else {
                Move_Vector(dir_f_left, dir_f_left);
            }
        }

        // µÚ¿Þ
        if ((movement.z < 0 && movement.z >= -1) && (movement.x < 0 && movement.x >= -1)) {
            if (is_roll) {
                StartCoroutine(Add_Impact(dir_b_left, roll_speed));
                is_roll = false;
            }else {
                Move_Vector(dir_b_left, dir_b_left);
            }
        }

        // µÚ¿À
        if ((movement.z < 0 && movement.z >= -1) && (movement.x > 0 && movement.x <= 1)) {
            if (is_roll) {
                StartCoroutine(Add_Impact(dir_b_right, roll_speed));
                is_roll = false;
            }else {
                Move_Vector(dir_b_right, dir_b_right);
            }
        }

        // ¾Õ
        if ((movement.z > 0 && movement.z <= 1) && (movement.x == 0)) {
            if (is_roll) {
                StartCoroutine(Add_Impact(dir_forward, roll_speed));
                is_roll = false;
            }else {
                Move_Vector(dir_forward, dir_forward);
            }
        }

        // µÚ
        if ((movement.z < 0 && movement.z >= -1) && (movement.x == 0)) {
            if (is_roll) {
                StartCoroutine(Add_Impact(dir_back, roll_speed));
                is_roll = false;
            }else {
                Move_Vector(dir_back, dir_back);
            }
        }

        // ¿À
        if ((movement.x > 0 && movement.x <= 1) && (movement.z == 0)) {
            if (is_roll) {
                StartCoroutine(Add_Impact(dir_right, roll_speed));
                is_roll = false;
            }else {
                Move_Vector(dir_right, dir_right);
            }
        }

        // ¿Þ
        if ((movement.x < 0 && movement.x >= -1) && (movement.z == 0)) {
            if (is_roll) {
                StartCoroutine(Add_Impact(dir_left, roll_speed));
                is_roll = false;
            }else {
                Move_Vector(dir_left, dir_left);
            }
        }

        if (impact.magnitude > 0.2)
            cc.Move(impact * Time.deltaTime);
        impact = Vector3.Lerp(impact, Vector3.zero, 5 * Time.deltaTime);
    }

    void Move_Vector(Vector3 input_vector, Vector3 rotate_vector) {
        Quaternion newRotation = Quaternion.LookRotation(rotate_vector);
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, newRotation, 10.0f * Time.deltaTime);
        cc.Move(input_vector * (move_speed * Time.deltaTime) + new Vector3(0.0f, velocity, 0.0f) * Time.deltaTime);
    }

    IEnumerator Jump() {
        is_jump = true;
        velocity = Mathf.Sqrt(jump_speed * -2f * -15.0f);
        yield return new WaitForSeconds(0.2f);
        is_jump = false;
    }

    IEnumerator Roll() {
        is_roll = true;
        is_rolls = true;
        yield return new WaitForSeconds(0.5f);
        is_roll = false;
        is_rolls = false;
    }

    IEnumerator Attack() {
        yield return null;
        while (!(Input.GetMouseButtonDown(0) || attack_time >= 0.7f)) {
            attack_time += Time.deltaTime;
            yield return null;
        }

        if (attack_time >= 0.1f && attack_time <= 0.6f) {
            attack_combo++;
            if (attack_combo < 4) {
                attack_time = 0;
                StartCoroutine(Attack());
            }else {
                attack_combo = 1;
                is_attack = false;
            }
        }else {
            attack_combo = 1;
            is_attack = false;
        }

        attack_time = 0.0f;
    }

    void Check_Ground() {
        Vector3 ground_pos = transform.position - new Vector3(0, ground_height, 0);
        is_ground = Physics.CheckSphere(ground_pos, ground_range, ground_hit, QueryTriggerInteraction.Ignore);
    }

    IEnumerator Add_Impact(Vector3 dir, float force) {
        dir.Normalize();
        if (dir.y < 0)
            dir.y = -dir.y;
        impact += dir.normalized * force / mass;
        yield return 0;
    }
}
