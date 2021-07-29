using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class PlayerStat {
    public float health_point;
    public float stamina_point;
    public float experience_point;
    public float karma_point;
    public float attack_power;
    public float attack_speed;
    public float defence;
    public float critical_chance;
    public float critical_damage;
}

[System.Serializable]
public class PlayerInformation{
    public string nickname;
    public string career;
    public string achievements;
}

public class PlayerStatus : MonoBehaviour {
    public PlayerStat stat;
    public Slider stamina_ui;
    void Start() {
        if(File.Exists(Application.dataPath + "/Project_R/Scripts/Database/Player/playerstat.json")) {
            Stat_Load();
        }else {
            Stat_Create();
        }
    }

    
    void Update() {
        stamina_ui.value = stat.stamina_point / 100;
    }

    [ContextMenu("Stat_Create")]
    void Stat_Create() {
        JObject playerstat = new JObject(
            new JProperty("health_point", 0),
            new JProperty("stamina_point", 0),
            new JProperty("experience_point", 0),
            new JProperty("karma_point", 0),
            new JProperty("attack_power", 0),
            new JProperty("attack_speed", 0),
            new JProperty("defence", 0),
            new JProperty("critical_chance", 0),
            new JProperty("critical_damage", 0)
        );

        File.WriteAllText(Application.dataPath + "/Project_R/Scripts/Database/Player/playerstat.json", playerstat.ToString());
        //Debug.Log("스텟 생성되었습니다.");
    }

    [ContextMenu("Stat_Load")]
    void Stat_Load() {
        string text_data = File.ReadAllText(Application.dataPath + "/Project_R/Scripts/Database/Player/playerstat.json");
        JObject data = JObject.Parse(text_data);
        stat.health_point = float.Parse(data["health_point"].ToString());
        stat.stamina_point = float.Parse(data["stamina_point"].ToString());
        stat.experience_point = float.Parse(data["experience_point"].ToString());
        stat.karma_point = float.Parse(data["karma_point"].ToString());
        stat.attack_power = float.Parse(data["attack_power"].ToString());
        stat.attack_speed = float.Parse(data["attack_speed"].ToString());
        stat.defence = float.Parse(data["defence"].ToString());
        stat.critical_chance = float.Parse(data["critical_chance"].ToString());
        stat.critical_damage = float.Parse(data["critical_damage"].ToString());
        //Debug.Log("스텟이 불러왔습니다.");
    }

    [ContextMenu("Stat_Save")]
    void Stat_Save() {
        JObject playerstat = new JObject(
            new JProperty("health_point", stat.health_point),
            new JProperty("stamina_point", stat.stamina_point),
            new JProperty("experience_point", stat.experience_point),
            new JProperty("karma_point", stat.karma_point),
            new JProperty("attack_power", stat.attack_power),
            new JProperty("attack_speed", stat.attack_speed),
            new JProperty("defence", stat.defence),
            new JProperty("critical_chance", stat.critical_chance),
            new JProperty("critical_damage", stat.critical_damage)
        );

        File.WriteAllText(Application.dataPath + "/Project_R/Scripts/Database/Player/playerstat.json", playerstat.ToString());
        //Debug.Log("스텟이 저장되었습니다.");
    }
}
