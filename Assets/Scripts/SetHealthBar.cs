using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetHealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;

    private HealthManager healthManager;
    public void Start()
    {
        GameObject enemyObject = GameObject.FindGameObjectWithTag("Enemy");
        healthManager = enemyObject.GetComponent<HealthManager>();

    }
    public void UpdateBar(float currentValue, float maxValue)
    {
        if (gameObject.CompareTag("Enemy"))
        {
            Debug.Log("health = " + healthManager.health);
            slider.value = currentValue;
            slider.maxValue = maxValue;
        }

        
    }

}

