using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSkills : MonoBehaviour{
    public int currentFloor;

    public float skillCooldown;

    public bool enableCharge = false;
    public bool enableShotgun = false;
    public bool enableSpray = false;
    public bool enableRage = false;

    public bool chargeActive = false;
    public bool shotgunActive = false;
    public bool sprayActive = false;
    public bool rageActive = false;
    public bool skillActive = false;
    public bool skillReady = true;
    private HealthManager healthManager;
    private EnemyChase enemyChase;
    private EnemyShoot enemyShoot;

    public enum skillType {
        Charge,
        Shotgun,
        Spray
    }
    void Start(){
        healthManager = GetComponent<HealthManager>();
        enemyChase = GetComponent<EnemyChase>();
        enemyShoot = GetComponent<EnemyShoot>();
        GameObject mapGeneratorObject = GameObject.Find("MapGenerator");
        if (mapGeneratorObject != null){
            MapGeneration mapGeneration = mapGeneratorObject.GetComponent<MapGeneration>();
            if (mapGeneration != null){
                currentFloor = mapGeneration.floorCount;
            }
        }
        skillCheck();
    }
    void Update(){
        if (skillReady && !skillActive){
            skillPick();
        }
        if (healthManager.health <= (healthManager.maxHealth/2) && !rageActive && enableRage){
            Enraged();
        }

    }

    public void skillPick(){
        string chosenSkill = ChooseSkill();
        Debug.Log("Chosen Skill: " + chosenSkill);

        if (chosenSkill == "Charge"){
            if (enemyChase != null){
                enemyChase.canCharge = true;
            }
        }
        if (chosenSkill == "Shotgun"){
            if (enemyShoot != null){
                shotgunActive = true;
                enemyShoot.canShoot = true;
            }
        }
        if (chosenSkill == "Spray"){
            if (enemyShoot != null){
                sprayActive = true;
                enemyShoot.canShoot = true;
            }
        }
        skillReady = false;
        skillActive = true;
    }

    public void skillCheck(){
        if (currentFloor >= 5){
            enableCharge = true;
        }
        if (currentFloor >= 10){
            enableShotgun = true;
        }
        if (currentFloor >= 15){
            enableSpray = true;
        }
        if (currentFloor >= 20){
            enableRage = true;
        }
    }

    public string ChooseSkill() {
        List<skillType> enabledTypes = new List<skillType>();

        if (enableCharge) {
            enabledTypes.Add(skillType.Charge);
        }
        if (enableShotgun) {
            enabledTypes.Add(skillType.Shotgun);
        }
        if (enableSpray) {
            enabledTypes.Add(skillType.Spray);
        }

        if (enabledTypes.Count == 0) {
            return "Charge";
        }

        int randomIndex = UnityEngine.Random.Range(0, enabledTypes.Count);
        return enabledTypes[randomIndex].ToString();
    }
    public void Enraged(){
        Debug.Log("Enraged");
        rageActive = true;
        enemyChase.moveForce *= 1.25f;
        skillCooldown = 2.5f;
        enemyChase.lockDuration = 0.5f;
        enemyShoot.bulletSpeed *= 1.5f;
        enemyShoot.fireCooldown = 0.5f;
    }
    public IEnumerator Cooldown(float cooldownTime){
        yield return new WaitForSeconds(cooldownTime);
        skillReady = true;
        skillActive = false;
    }
}
