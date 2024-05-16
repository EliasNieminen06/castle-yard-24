using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShootingHandler : MonoBehaviour
{
    [Tooltip("base damage is multiplied by attack")]
    [SerializeField] private float baseDamage;
    [SerializeField] private float arrowSpeed;
    [SerializeField] private Transform gunEnd;
    [SerializeField] private GameObject arrowPrefab;
    private float nextFire;
    private float fireRate;

    [SerializeField] private Hero hero;

    void Start()
    {
        fireRate = 0.25f;
    }

    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time > nextFire)
        {
            float fireRateReduction = fireRate * ((float)hero.Stats.CooldownReduction / 100);
            nextFire = Time.time + (fireRate - fireRateReduction);
            //Debug.Log("newxtFire: " + (nextFire - Time.time) + "FireRateReduction: " + fireRateReduction);
            print(hero.Stats.CooldownReduction);
            Bullet arrow = Instantiate(arrowPrefab, gunEnd.position, gunEnd.rotation).GetComponent<Bullet>();
            arrow.Init(hero.Stats.Attack * baseDamage, arrowSpeed);
        }
    }
}