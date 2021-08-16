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
    public float velocity_downhill;
    public float attack_combo;
    public float attack_time;
    public float roll_time;
    public float roll_delay;
    public float jump_delay;
    public float roll_speed;
    public float idle_stamina;
    public float jump_stamina;
    public float downhill_stamina;
    public float dash_stamina;
    public float attack_stamina;
    public float roll_stamina;
    public float ground_height;
    public float ground_range;
    private float roll_wait;

    public bool is_joystick;
    public bool is_ground;
    public bool is_roll;
    public bool is_rolls;
    public bool is_roll_timer;
    public bool is_roll_lock;
    public bool is_jump_delay_lock;
    public bool is_attack;
    public bool is_jump;
    public bool is_jump_lock;
    public bool is_dash;
    public bool is_downhill;
    public bool is_ctrl;
    public bool is_fly;
    public bool is_wall;
    public bool is_stamina;
    public bool is_idle;
    public bool is_drop;
    public bool is_ui_chat;

    public LayerMask ground_hit;
    private RaycastHit wall_hit;

    public Vector3 movement;
    public Vector3 animation_movement;
    public Vector3 player_dir;
    public Vector3 player_forward;
    public Vector3 impact;
    private Vector3 dir_forward, dir_f_right, dir_right, dir_b_right, dir_back, dir_b_left, dir_left, dir_f_left;

    CharacterController cc;
    TPSCamera tpscamera;
    ItemLoot itemloot;
    PlayerStatus playerstatus;
    UI_Check ui_check;

    void Awake() {
        cc = GetComponent<CharacterController>();
        itemloot = GetComponent<ItemLoot>();
        tpscamera = GameObject.Find("TPS_Camera").GetComponent<TPSCamera>();
        playerstatus = GameObject.Find("System").GetComponent<PlayerStatus>();
        ui_check = GameObject.Find("System").GetComponent<UI_Check>();
    }

    void Start() {
        mass = playerstatus.stat.mass;
        jump_speed = playerstatus.stat.jump_speed;
        walk_speed = playerstatus.stat.walk_speed;
        dash_speed = playerstatus.stat.dash_speed;
        velocity = playerstatus.stat.velocity;
        velocity_downhill = playerstatus.stat.velocity_downhill;
        ground_height = playerstatus.stat.ground_height;
        ground_range = playerstatus.stat.ground_range;
        attack_time = playerstatus.stat.attack_time;
        attack_combo = playerstatus.stat.attack_combo;
        roll_time = playerstatus.stat.roll_time;
        roll_delay = playerstatus.stat.roll_delay;
        jump_delay = playerstatus.stat.jump_delay;
        roll_speed = playerstatus.stat.roll_speed;
        idle_stamina = playerstatus.stat.idle_stamina;
        jump_stamina = playerstatus.stat.jump_stamina;
        downhill_stamina = playerstatus.stat.downhill_stamina;
        dash_stamina = playerstatus.stat.dash_stamina;
        attack_stamina = playerstatus.stat.attack_stamina;
        roll_stamina = playerstatus.stat.roll_stamina;
        impact = Vector3.zero;
        is_joystick = true;
        is_stamina = true;
        is_idle = true;
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
        if(!is_joystick) {
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");
        }else {
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");
        }
        player_dir = tpscamera.player_dir.normalized;

        if ((Input.GetKeyDown(KeyCode.LeftShift) && is_stamina) ||
            (Input.GetButtonDown("J_B") && is_stamina)) {
            is_dash = true;
            if (!is_roll_timer) {
                roll_wait = Time.time;
                is_roll_timer = true;
            } else if (is_roll_timer && ((Time.time - roll_wait) < roll_time) && is_stamina && !is_roll_lock && !is_fly) {
                StartCoroutine(Roll());
                StartCoroutine(Roll_Lock(roll_delay));
            }
        }

        if ((Input.GetKeyUp(KeyCode.LeftShift)) ||
            (Input.GetButtonUp("J_B"))) {
            is_dash = false;
        }

        if ((Input.GetKeyDown(KeyCode.Space) && is_stamina) ||
            (Input.GetButtonDown("J_A") && is_stamina)) {
            is_downhill = true;
        }

        if ((Input.GetKeyUp(KeyCode.Space)) ||
            (Input.GetButtonUp("J_A"))) {
            is_downhill = false;
        }

        if ((Input.GetKeyDown(KeyCode.Space) && is_ground && !is_attack && !is_fly && is_stamina && !is_jump_delay_lock) ||
            (Input.GetButtonDown("J_A") && is_ground && !is_attack && !is_fly && is_stamina && !is_jump_delay_lock)) {
            StartCoroutine(Jump(2.0f, 30.0f));
            StartCoroutine(Jump_Lock(roll_delay));
        }

        if ((Input.GetKeyDown(KeyCode.Space) && !is_attack && is_fly && is_stamina && !is_jump_lock) ||
            (Input.GetButtonDown("J_A") && !is_attack && is_fly && is_stamina && !is_jump_lock)) {
            StartCoroutine(Jump(0.75f, 30.0f));
            is_jump_lock = true;
        }

        if ((Input.GetKeyDown(KeyCode.F) && is_ground && !is_attack && !is_drop) ||
            (Input.GetButtonDown("J_X") && is_ground && !is_attack && !is_drop)) {
            //itemloot.Check_Area();
        }

        if ((Input.GetKeyDown(KeyCode.Tab)) ||
            (Input.GetButtonDown("J_Y"))) {
            Debug.Log("ÀÎ¹êÅä¸® ¿°");
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
            is_ctrl = true;

        if (Input.GetKeyUp(KeyCode.LeftControl))
            is_ctrl = false;

        if ((Input.GetMouseButtonDown(0) && !is_attack && !is_dash && !is_roll && is_stamina) ||
            (Input.GetButtonDown("J_R1") && !is_attack && !is_dash && !is_roll && is_stamina)) {
            is_attack = true;
            attack_time = 0;
            playerstatus.stat.stamina_point -= attack_stamina;
            StartCoroutine(Attack());
        }
    }

    void Check_Value() {
        if (is_ground) {
            is_fly = false;
            is_jump_lock = false;
            if (is_dash && is_ground) {
                is_idle = false;
            }else {
                is_idle = true;
            }
            if (velocity < 0.0f) {
                velocity = -2.0f;
            }
            if (is_downhill) {
                is_downhill = false;
            }
        } else {
            is_fly = true;
            if (velocity < 53.0f) {
                velocity += -15.0f * Time.deltaTime;
            }
        }

        if (is_dash) {
            move_speed = walk_speed + dash_speed;
            if (!is_fly) {
                if (is_attack) {
                    is_idle = false;
                }else {
                    playerstatus.stat.stamina_point -= dash_stamina * Time.deltaTime;
                }
            }
            if (is_fly && is_dash) {
                move_speed = walk_speed;
            }
        }else {
            move_speed = walk_speed;
        }

        if (is_ctrl)
            move_speed = move_speed / 2;

        if (is_roll_timer && ((Time.time - roll_wait) > roll_time)) {
            is_roll_timer = false;
        }

        if (is_attack) {
            horizontal = 0;
            vertical = 0;
            velocity = -1.0f;
        }

        if (is_fly) {
            cc.slopeLimit = 0f;
            is_idle = false;
        } else {
            cc.slopeLimit = 60f;
        }

        if (is_downhill) {
            velocity = velocity_downhill;
            move_speed = walk_speed + dash_speed;
            playerstatus.stat.stamina_point -= downhill_stamina * Time.deltaTime;
            if (playerstatus.stat.stamina_point <= 0) {
                is_downhill = false;
            }
        }

        if (is_wall) {
        }

        if (is_drop) {
            horizontal = 0;
            vertical = 0;
        }

        if (alpha > 0) {
            is_ui_chat = true;
        }else {
            is_ui_chat = false;
        }

        if (is_ui_chat) {
            StartCoroutine(ui_check.UI_Fade_Hold(1.0f, ui_check.ui_chat));
        }else {
            StartCoroutine(ui_check.UI_Fade_Hold(0.0f, ui_check.ui_chat));
        }

        if (is_joystick) {
            tpscamera.is_joystick = true;
        }else {
            tpscamera.is_joystick = false;
        }

        if (playerstatus.stat.stamina_point <= 0) {
            is_stamina = false;
            is_dash = false;
        } else {
            is_stamina = true;
        }

        if (playerstatus.stat.stamina_point < 100 && is_idle) {
            playerstatus.stat.stamina_point += idle_stamina * Time.deltaTime;
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
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, newRotation, 6.0f * Time.deltaTime);
        cc.Move(input_vector * (move_speed * Time.deltaTime) + new Vector3(0.0f, velocity, 0.0f) * Time.deltaTime);
    }

    IEnumerator Jump(float forward, float force) {
        is_jump = true;
        //velocity = Mathf.Sqrt(jump_speed * -2.0f * -15.0f);
        playerstatus.stat.stamina_point -= jump_stamina;
        velocity = -2.0f;
        if (movement.z == 0 && movement.x == 0) {
            StartCoroutine(Add_Impact(transform.up * jump_speed, force));
        }else {
            StartCoroutine(Add_Impact(transform.forward / forward + (transform.up * jump_speed), force));
        }
        yield return new WaitForSeconds(0.2f);
        is_jump = false;
    }

    IEnumerator Jump_Lock(float delay) {
        is_jump_delay_lock = true;
        yield return new WaitForSeconds(delay);
        is_jump_delay_lock = false;
    }

    IEnumerator Roll() {
        is_roll = true;
        is_rolls = true;
        playerstatus.stat.stamina_point -= roll_stamina;
        yield return new WaitForSeconds(0.5f);
        is_roll = false;
        is_rolls = false;
    }

    IEnumerator Roll_Lock(float delay) {
        is_roll_lock = true;
        yield return new WaitForSeconds(delay);
        is_roll_lock = false;
    }

    public IEnumerator Drop_Item(float delay) {
        is_drop = true;
        yield return new WaitForSeconds(delay);
        is_drop = false;
    }

    [Range(0.0f, 1.0f)]
    public float alpha = 0.0f;

    public IEnumerator UI_Chat(float delay) {
        alpha = 1.0f;
        if (!is_ui_chat) {
            while (alpha > 0.0f) {
                alpha -= Time.deltaTime / delay;
                if (alpha <= 0.0f) {
                    alpha = 0.0f;
                }
                yield return null;
            }
        }
    }

    IEnumerator Attack() {
        yield return null;
        while (!(Input.GetMouseButtonDown(0) || Input.GetButtonDown("J_R1") || attack_time >= 0.7f)) {
            attack_time += Time.deltaTime;
            yield return null;
        }
        
        if (attack_time >= 0.1f && attack_time <= 0.6f) {
            attack_combo++;
            if (attack_combo < 4) {
                attack_time = 0;
                playerstatus.stat.stamina_point -= attack_stamina;
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
        Vector3 wall_pos = transform.position + new Vector3(0, 1f, 0);
        is_ground = Physics.CheckSphere(ground_pos, ground_range, ground_hit, QueryTriggerInteraction.Ignore);
        Debug.DrawRay(wall_pos, transform.forward * 1.2f, Color.blue);
        if (Physics.Raycast(wall_pos, transform.forward, out wall_hit, 1.2f)){
            if(wall_hit.transform.tag == "Ground") {
                is_wall = true;
            }
        }else {
            is_wall = false;
        }
    }

    IEnumerator Add_Impact(Vector3 dir, float force) {
        dir.Normalize();
        if (dir.y < 0)
            dir.y = -dir.y;
        impact += dir.normalized * force / mass;
        yield return 0;
    }
}
