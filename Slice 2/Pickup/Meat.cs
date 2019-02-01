using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meat : MonoBehaviour {

    private PlayerDeath playerDeath;

	// Use this for initialization
	void Start () {
        playerDeath = FindObjectOfType<PlayerDeath>();
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {
            playerDeath.Heal(10);
            Destroy(gameObject);
        }
    }
}
