using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chicken : Enemy {

    private bool attached;

    [SerializeField]
    private float destroyWhenAttachedInSeconds;

    public override void Attack()
    {
        if (attached) return;
        if (!CanAttack()) return;
        if (player == null) return;

        attached = true;

        player.GetComponent<TestMove>().chickens += 1;

        gameObject.transform.parent = player.gameObject.transform;

        Destroy(gameObject, destroyWhenAttachedInSeconds);

    }

    void Update()
    {
        if (IsPlayerClose())
            Attack();
    }

    void FixedUpdate()
    {
        if (IsPlayerClose()) return;
        if (player != null)
            Move(player.transform.position);
    }

    void OnDestroy()
    {
        player.GetComponent<TestMove>().chickens -= 1;
    }
}
