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
        if (Input.GetButtonDown("Fire1") && Time.time > nextFire)
        {
            float fireRateReduction = fireRate * (hero.Stats.CooldownReduction / 100);
            nextFire = Time.time + fireRate - fireRateReduction;
            Bullet arrow = Instantiate(arrowPrefab, new Vector3(gunEnd.position.x, gunEnd.position.y, gunEnd.position.z), Quaternion.identity).GetComponent<Bullet>();
            arrow.Init(hero.Stats.Attack * baseDamage, arrowSpeed);
        }
    }
}