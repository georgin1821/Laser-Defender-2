using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthSlider : MonoBehaviour
{
    [SerializeField] Slider healthSlider;
    [SerializeField] Gradient gradient;
    [SerializeField] Image fill;
    private void Awake()
    {
        gameObject.GetComponent<Enemy>().OnGettingDamage += OnGettingDamageHandler;
    }
    private void OnDestroy()
    {
        gameObject.GetComponent<Enemy>().OnGettingDamage -= OnGettingDamageHandler;

    }
    private void OnGettingDamageHandler(float health)
    {
        UpdateSlider(health);
    }

    void Start()
    {
        float health = gameObject.GetComponent<Enemy>().health;
        SetHealth(health);
    }

    private void SetHealth(float health)
    {
        healthSlider.maxValue = health;
        healthSlider.value = health;
        fill.color = gradient.Evaluate(1f);
    }

    public void UpdateSlider(float health)
    {
        healthSlider.value = health;
        fill.color = gradient.Evaluate(healthSlider.normalizedValue);
    }
}
