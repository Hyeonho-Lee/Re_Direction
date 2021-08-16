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
}

[System.Serializable]
public class PlayerInformation {
    public string nickname;
    public string career;
    public string achievements;
}

[System.Serializable]
public class PlayerEquipment {
    public int head_skin;
    public int body_skin;
    public int leg_skin;
    public int knee_skin;
    public int arm_skin;
    public int hand_skin;
    public int foot_skin;
    public int head_equip;
    public int body_equip;
    public int pant_equip;
    public int hand_equip;
    public int foot_equip;
    public int weapon_equip;
    public int spell_equip;
    public int accessory_1_equip;
    public int accessory_2_equip;
    public int bag_equip;
    public int safe_bag_equip;
}

public class PlayerStatus : MonoBehaviour {
    public PlayerStat stat;
    public PlayerEquipment equip;
    public Slider stamina_ui;

    void Start() {
        if(File.Exists(Application.dataPath + "/Project_R/Scripts/Database/Player/playerstat.json") && 
            File.Exists(Application.dataPath + "/Project_R/Scripts/Database/Player/playerequip.json")) {
            Stat_Load();
            Equip_Load();
        }else {
            Stat_Create();
            Equip_Create();
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
            new JProperty("critical_damage", 0),
            new JProperty("mass", 0),
            new JProperty("jump_speed", 0),
            new JProperty("walk_speed", 0),
            new JProperty("dash_speed", 0),
            new JProperty("move_speed", 0),
            new JProperty("velocity", 0),
            new JProperty("velocity_downhill", 0),
            new JProperty("attack_combo", 0),
            new JProperty("attack_time", 0),
            new JProperty("roll_time", 0),
            new JProperty("roll_delay", 0),
            new JProperty("jump_delay", 0),
            new JProperty("roll_speed", 0),
            new JProperty("idle_stamina", 0),
            new JProperty("jump_stamina", 0),
            new JProperty("downhill_stamina", 0),
            new JProperty("dash_stamina", 0),
            new JProperty("attack_stamina", 0),
            new JProperty("roll_stamina", 0),
            new JProperty("ground_height", 0),
            new JProperty("ground_range", 0)
        );

        File.WriteAllText(Application.dataPath + "/Project_R/Scripts/Database/Player/playerstat.json", playerstat.ToString());
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
        stat.mass = float.Parse(data["mass"].ToString());
        stat.jump_speed = float.Parse(data["jump_speed"].ToString());
        stat.walk_speed = float.Parse(data["walk_speed"].ToString());
        stat.dash_speed = float.Parse(data["dash_speed"].ToString());
        stat.velocity = float.Parse(data["velocity"].ToString());
        stat.velocity_downhill = float.Parse(data["velocity_downhill"].ToString());
        stat.attack_combo = float.Parse(data["attack_combo"].ToString());
        stat.attack_time = float.Parse(data["attack_time"].ToString());
        stat.roll_time = float.Parse(data["roll_time"].ToString());
        stat.roll_delay = float.Parse(data["roll_delay"].ToString());
        stat.jump_delay = float.Parse(data["jump_delay"].ToString());
        stat.roll_speed = float.Parse(data["roll_speed"].ToString());
        stat.idle_stamina = float.Parse(data["idle_stamina"].ToString());
        stat.jump_stamina = float.Parse(data["jump_stamina"].ToString());
        stat.downhill_stamina = float.Parse(data["downhill_stamina"].ToString());
        stat.dash_stamina = float.Parse(data["dash_stamina"].ToString());
        stat.attack_stamina = float.Parse(data["attack_stamina"].ToString());
        stat.roll_stamina = float.Parse(data["roll_stamina"].ToString());
        stat.ground_height = float.Parse(data["ground_height"].ToString());
        stat.ground_range = float.Parse(data["ground_range"].ToString());
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
            new JProperty("critical_damage", stat.critical_damage),
            new JProperty("mass", stat.mass),
            new JProperty("jump_speed", stat.jump_speed),
            new JProperty("walk_speed", stat.walk_speed),
            new JProperty("dash_speed", stat.dash_speed),
            new JProperty("move_speed", stat.move_speed),
            new JProperty("velocity", stat.velocity),
            new JProperty("velocity_downhill", stat.velocity_downhill),
            new JProperty("attack_combo", stat.attack_combo),
            new JProperty("attack_time", stat.attack_time),
            new JProperty("roll_time", stat.roll_time),
            new JProperty("roll_delay", stat.roll_delay),
            new JProperty("jump_delay", stat.jump_delay),
            new JProperty("roll_speed", stat.roll_speed),
            new JProperty("idle_stamina", stat.idle_stamina),
            new JProperty("jump_stamina", stat.jump_stamina),
            new JProperty("downhill_stamina", stat.downhill_stamina),
            new JProperty("dash_stamina", stat.dash_stamina),
            new JProperty("attack_stamina", stat.attack_stamina),
            new JProperty("roll_stamina", stat.roll_stamina),
            new JProperty("ground_height", stat.ground_height),
            new JProperty("ground_range", stat.ground_range)
        );

        File.WriteAllText(Application.dataPath + "/Project_R/Scripts/Database/Player/playerstat.json", playerstat.ToString());
    }

    [ContextMenu("Equip_Create")]
    void Equip_Create() {
        JObject playerequip = new JObject(
            new JProperty("head_skin", 0),
            new JProperty("body_skin", 0),
            new JProperty("leg_skin", 0),
            new JProperty("knee_skin", 0),
            new JProperty("arm_skin", 0),
            new JProperty("hand_skin", 0),
            new JProperty("foot_skin", 0),
            new JProperty("head_equip", 0),
            new JProperty("body_equip", 0),
            new JProperty("pant_equip", 0),
            new JProperty("hand_equip", 0),
            new JProperty("foot_equip", 0),
            new JProperty("weapon_equip", 0),
            new JProperty("spell_equip", 0),
            new JProperty("accessory_1_equip", 0),
            new JProperty("accessory_2_equip", 0),
            new JProperty("bag_equip", 0),
            new JProperty("safe_bag_equip", 0)
        );

        File.WriteAllText(Application.dataPath + "/Project_R/Scripts/Database/Player/playerequip.json", playerequip.ToString());
    }

    [ContextMenu("Equip_Load")]
    void Equip_Load() {
        string text_data = File.ReadAllText(Application.dataPath + "/Project_R/Scripts/Database/Player/playerequip.json");
        JObject data = JObject.Parse(text_data);
        equip.head_skin = int.Parse(data["head_skin"].ToString());
        equip.body_skin = int.Parse(data["body_skin"].ToString());
        equip.leg_skin = int.Parse(data["leg_skin"].ToString());
        equip.knee_skin = int.Parse(data["knee_skin"].ToString());
        equip.arm_skin = int.Parse(data["arm_skin"].ToString());
        equip.hand_skin = int.Parse(data["hand_skin"].ToString());
        equip.foot_skin = int.Parse(data["foot_skin"].ToString());
        equip.head_equip = int.Parse(data["head_equip"].ToString());
        equip.body_equip = int.Parse(data["body_equip"].ToString());
        equip.pant_equip = int.Parse(data["pant_equip"].ToString());
        equip.hand_equip = int.Parse(data["hand_equip"].ToString());
        equip.foot_equip = int.Parse(data["foot_equip"].ToString());
        equip.weapon_equip = int.Parse(data["weapon_equip"].ToString());
        equip.spell_equip = int.Parse(data["spell_equip"].ToString());
        equip.accessory_1_equip = int.Parse(data["accessory_1_equip"].ToString());
        equip.accessory_2_equip = int.Parse(data["accessory_2_equip"].ToString());
        equip.bag_equip = int.Parse(data["bag_equip"].ToString());
        equip.safe_bag_equip = int.Parse(data["safe_bag_equip"].ToString());
    }

    [ContextMenu("Equip_Save")]
    void Equip_Save() {
        JObject playerequip = new JObject(
            new JProperty("head_skin", equip.head_skin),
            new JProperty("body_skin", equip.body_skin),
            new JProperty("leg_skin", equip.leg_skin),
            new JProperty("knee_skin", equip.knee_skin),
            new JProperty("arm_skin", equip.arm_skin),
            new JProperty("hand_skin", equip.hand_skin),
            new JProperty("foot_skin", equip.foot_skin),
            new JProperty("head_equip", equip.head_equip),
            new JProperty("body_equip", equip.body_equip),
            new JProperty("pant_equip", equip.pant_equip),
            new JProperty("hand_equip", equip.hand_equip),
            new JProperty("foot_equip", equip.foot_equip),
            new JProperty("weapon_equip", equip.weapon_equip),
            new JProperty("spell_equip", equip.spell_equip),
            new JProperty("accessory_1_equip", equip.accessory_1_equip),
            new JProperty("accessory_2_equip", equip.accessory_2_equip),
            new JProperty("bag_equip", equip.bag_equip),
            new JProperty("safe_bag_equip", equip.safe_bag_equip)
        );

        File.WriteAllText(Application.dataPath + "/Project_R/Scripts/Database/Player/playerequip.json", playerequip.ToString());
    }
}
