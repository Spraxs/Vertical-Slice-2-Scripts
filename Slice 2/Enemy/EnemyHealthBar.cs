using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour {

    private Enemy enemy;
    private Slider slider;

    [SerializeField]
    private bool damage;

	// Use this for initialization
	void Start () {
        slider = GetComponent<Slider>();
        enemy = GetComponentInParent<Enemy>();

        enemy.enemyDamageAction += UpdateHealthBar;
        slider.fillRect.GetComponent<Image>().color = Color.green;
	}

    private void UpdateHealthBar(float health, float damage)
    {
        slider.value = health / enemy.maxHealth;

        Image image = slider.fillRect.GetComponent<Image>();

        if (slider.value <= 0.25)
        {
            image.color = Color.red;
        } else if (slider.value <= 0.5)
        {
            image.color = Color.yellow;
        } else
        {
            image.color = Color.green;
        }
    }

    void Update()
    {
        if (damage)
        {
            damage = false;

            enemy.Damage(10);
        }
    }
}
