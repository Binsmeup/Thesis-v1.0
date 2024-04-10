using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickUp : MonoBehaviour{
    private void OnTriggerEnter2D(Collider2D other){
        if (other.CompareTag("Player")){
            HealthManager script = other.GetComponent<HealthManager>();
            script.HealUp();
            Destroy(gameObject);
        }
    }
}
