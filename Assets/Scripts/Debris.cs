using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debris : MonoBehaviour{
    public void Destroy(){
        if (!gameObject.CompareTag("Player")){
            Destroy(gameObject);
        }
    }
    
}
