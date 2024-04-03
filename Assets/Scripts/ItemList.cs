using UnityEngine;

[CreateAssetMenu(fileName = "ItemList", menuName = "ScriptableObjects/ItemList", order = 1)]
public class ItemList : ScriptableObject{

    public string itemName,Type,WeaponType;
    public float baseDmg,dmgMulti,critCHA,critDMG,atkSPD,maxHP,KB,MS,HelmetMaxHealth,ChestMaxHealth,LegMaxHealth;
    public GameObject itemObject;
    private float TempArmorHP;
    private float TempArmorHPDrop;

    public void onEquip(){
        HealthManager healthManager = GameObject.Find("Player").GetComponent<HealthManager>();
        playerScript playerStats = GameObject.Find("Player").GetComponent<playerScript>();
        TempArmorHP = playerStats.GetArmorHP();

        switch (Type){
        case "Weapon":
            if (playerStats.currentWeapon != null){
                Vector3 playerPosition = GameObject.Find("Player").transform.position;
                Instantiate(playerStats.currentWeapon, playerPosition, Quaternion.identity);
            }
            playerStats.currentWeapon = itemObject;
            break;

        case "Helmet":
            healthManager.HelmHP += TempArmorHP;
            if (playerStats.currentHelm != null){
                Vector3 playerPosition = GameObject.Find("Player").transform.position;
                ArmorHealth currentArmorHP = Instantiate(playerStats.currentHelm, playerPosition, Quaternion.identity).GetComponent<ArmorHealth>();
                if (currentArmorHP != null){
                    currentArmorHP.armorHealth = TempArmorHPDrop;
                }
            }
            playerStats.currentHelm = itemObject;
            break;

        case "Chestplate":
            healthManager.ChestHP += TempArmorHP;
            if (playerStats.currentChest != null){
                Vector3 playerPosition = GameObject.Find("Player").transform.position;
                ArmorHealth currentArmorHP = Instantiate(playerStats.currentChest, playerPosition, Quaternion.identity).GetComponent<ArmorHealth>();
                if (currentArmorHP != null){
                    currentArmorHP.armorHealth = TempArmorHPDrop;
                }
            }
            playerStats.currentChest = itemObject;
            break;
        
        case "Leggings":
            healthManager.LegHP += TempArmorHP;
            if (playerStats.currentLeg != null){
                Vector3 playerPosition = GameObject.Find("Player").transform.position;
                ArmorHealth currentArmorHP = Instantiate(playerStats.currentLeg, playerPosition, Quaternion.identity).GetComponent<ArmorHealth>();
                if (currentArmorHP != null){
                    currentArmorHP.armorHealth = TempArmorHPDrop;
                }
            }
            playerStats.currentLeg = itemObject;
            break;

        default:
            break;
    }

        healthManager.maxHealth += maxHP;
        healthManager.HelmMaxHP += HelmetMaxHealth;
        healthManager.ChestMaxHP += ChestMaxHealth;
        healthManager.LegMaxHP += LegMaxHealth;

        if (!string.IsNullOrEmpty(WeaponType)){
            playerStats.weaponType = WeaponType;
        }
        playerStats.baseDamage += baseDmg;
        playerStats.damageMulti += dmgMulti;
        playerStats.attackSpeed += atkSPD;
        playerStats.critChance += critCHA;
        playerStats.critDamage += critDMG;
        playerStats.knockbackForce += KB;
        playerStats.moveSpeed += MS;

    }

        public void onUnequip(){
        HealthManager healthManager = GameObject.Find("Player").GetComponent<HealthManager>();
        playerScript playerStats = GameObject.Find("Player").GetComponent<playerScript>();


        switch (Type){
            case "Helmet":
                TempArmorHPDrop = healthManager.HelmHP;
                healthManager.HelmMaxHP = 0;
                healthManager.HelmHP = 0;
            break;

            case "Chestplate":
                TempArmorHPDrop = healthManager.ChestHP;
                healthManager.ChestMaxHP = 0;
                healthManager.ChestHP = 0;
            break;

            case "Leggings":
                TempArmorHPDrop = healthManager.LegHP;
                healthManager.LegMaxHP = 0;
                healthManager.LegHP = 0;
            break;

            default:
            break;
        }

        healthManager.maxHealth -= maxHP;

        playerStats.weaponType = ("");
        playerStats.baseDamage -= baseDmg;
        playerStats.damageMulti -= dmgMulti;
        playerStats.critChance -= critCHA;
        playerStats.critDamage -= critDMG;
        playerStats.attackSpeed -= atkSPD;
        playerStats.knockbackForce -= KB;
        playerStats.moveSpeed -= MS;

    }
}
