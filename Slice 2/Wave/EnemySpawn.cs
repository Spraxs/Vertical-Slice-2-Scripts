using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour {

    public void Spawn(GameObject enemyObject)
    {
         Instantiate(enemyObject, gameObject.transform.position, enemyObject.transform.rotation);
    }
}
