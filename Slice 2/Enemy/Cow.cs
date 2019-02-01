using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cow : Enemy {

    [SerializeField]
    private GameObject projectile;
    
    public override void Attack()
    {
        if (!CanAttack()) return;
            if (player == null) return;
                CowMilk projectile = Instantiate(this.projectile, transform.position, this.projectile.transform.rotation).GetComponent<CowMilk>();
                projectile.playerDeath = player;
                projectile.baseDamage = baseDamage;
        
    }

    private void FixedUpdate()
    {
        if (IsPlayerClose()) return;
        if (player != null)
            Move(player.transform.position);
    }

    // Update is called once per frame
    void Update () {
		if (IsPlayerClose())
        {
            Attack();
        }
	}
}
