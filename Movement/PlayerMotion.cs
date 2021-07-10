using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotion : MonoBehaviour {

    private Vector3 movement;

    PlayerMovement playermovement;
    Animator animator;

    void Awake() {
        playermovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
        animator = GameObject.Find("Body").GetComponent<Animator>();
    }

    
    void Update() {
        Input_Motion();
    }

    void Input_Motion() {
        if (playermovement.is_dash == true) {
            animator.SetFloat("movement_x", playermovement.horizontal * 2f);
            animator.SetFloat("movement_y", playermovement.vertical * 2f);
        } else {
            animator.SetFloat("movement_x", playermovement.horizontal);
            animator.SetFloat("movement_y", playermovement.vertical);
        }

        if (playermovement.is_jump == true) {
            animator.SetBool("is_jump", true);
        } else {
            animator.SetBool("is_jump", false);
        }

        if (playermovement.is_ground == true) {
            animator.SetBool("is_ground", true);
        } else {
            animator.SetBool("is_ground", false);
        }

        if (playermovement.is_fly == true) {
            animator.SetBool("is_fly", true);
        } else {
            animator.SetBool("is_fly", false);
        }

        if (playermovement.is_rolls == true) {
            animator.SetBool("is_roll", true);
        } else {
            animator.SetBool("is_roll", false);
        }

        if (playermovement.is_attack == true) {
            animator.SetBool("is_attack", true);
            animator.SetFloat("attack_combo", playermovement.attack_combo);
        } else {
            animator.SetBool("is_attack", false);
        }

        if(playermovement.is_attack == true && playermovement.attack_combo == 1) {
            animator.Play("sword_attack_1");
        } else if (playermovement.is_attack == true && playermovement.attack_combo == 2) {
            animator.Play("sword_attack_2");
        } else if (playermovement.is_attack == true && playermovement.attack_combo == 3) {
            animator.Play("sword_attack_3");
        }
    }
}
