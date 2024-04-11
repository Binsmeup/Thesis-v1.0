using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour{
    public int coinValue;
    private void OnTriggerEnter2D(Collider2D other){
        if (other.CompareTag("Player")){
            playerScript script = other.GetComponent<playerScript>();
            script.getCoins(coinValue);
            Destroy(gameObject);
        }
    }
}
