using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable{
    public float health { set; get; }

    public bool Targettable { set; get; }

    public bool damageCooldown { set; get; }

    public void OnHit(float baseDamage, float DamageMulti, Vector2 knockback);

    public void OnHit(float baseDamage, float DamageMulti);


    public void OnObjectDestroyed();
    

}
