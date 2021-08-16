using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Check : MonoBehaviour {

    public CanvasGroup ui_chat;
    [Range(0.0f, 1.0f)]
    public float ui_chat_alpha;
    public float ui_chat_time;

    PlayerMovement playermovement;

    void Awake() {
        playermovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
    }

    void Start() {
        ui_chat_time = 0.75f;
        ui_chat_alpha = 0.0f;
        ui_chat.alpha = ui_chat_alpha;
    }

    void Update() {
    }

    public IEnumerator UI_Fade_In(float time, CanvasGroup canvas, bool ui_chat) {
        if (ui_chat) {
            while (ui_chat_alpha < 1.0f) {
                ui_chat_alpha += Time.deltaTime / time;
                if (ui_chat_alpha >= 1.0f) {
                    ui_chat_alpha = 1.0f;
                }
                canvas.alpha = ui_chat_alpha;
                yield return null;
            }
        }else {
            ui_chat_alpha = 0.0f;
        }
    }

    public IEnumerator UI_Fade_Hold(float alpha, CanvasGroup canvas) {
        ui_chat_alpha = alpha;
        canvas.alpha = ui_chat_alpha;
        yield return null;
    }

    public IEnumerator UI_Fade_Out(float time, CanvasGroup canvas, bool ui_chat) {
        while (ui_chat_alpha > 0.0f) {
            ui_chat_alpha -= Time.deltaTime / time;
            if (ui_chat_alpha <= 0.0f) {
                ui_chat_alpha = 0.0f;
            }
            canvas.alpha = ui_chat_alpha;
            yield return null;
        }
    }
}
