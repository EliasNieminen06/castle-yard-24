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
    [SerializeField] private float baseAngleWithMultipleProjectiles;
    private float nextFire;
    [SerializeField] private float fireRate;

    [SerializeField] private Hero hero;

    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time > nextFire)
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        float fireRateReduction = fireRate * ((float)hero.Stats.CooldownReduction / 100);
        nextFire = Time.time + (fireRate - fireRateReduction);
        //Debug.Log("newxtFire: " + (nextFire - Time.time) + "FireRateReduction: " + fireRateReduction);
        //print(hero.Stats.CooldownReduction);

        if (hero.Stats.Projectiles == 1)
        {
            Bullet arrow = Instantiate(arrowPrefab, gunEnd.position, gunEnd.rotation).GetComponent<Bullet>();
            arrow.Init(hero.Stats.Attack * baseDamage, arrowSpeed, hero.Stats.AreaOfEffect, hero.Stats.Pierce);
            return;
        }

        for (int i = 0; i < hero.Stats.Projectiles; i++)
        {
            float angle = 0;
            Vector3 angleVector = Vector3.zero;

            float startingAngle;
            float angleAdded;

            if (hero.Stats.Projectiles % 2 == 0)
            {
                startingAngle = (float)hero.Stats.Projectiles / 2 * baseAngleWithMultipleProjectiles - (baseAngleWithMultipleProjectiles / 2);
                angleAdded = i * -baseAngleWithMultipleProjectiles;

                angle = startingAngle + angleAdded;
            }
            else
            {
                startingAngle = (float)(hero.Stats.Projectiles - 1) / 2 * baseAngleWithMultipleProjectiles;
                angleAdded = i * -baseAngleWithMultipleProjectiles;

                angle = startingAngle + angleAdded;
            }

            angleVector.y += angle + transform.rotation.eulerAngles.y;
            Quaternion rotation = Quaternion.Euler(angleVector);

            Bullet arrow = Instantiate(arrowPrefab, gunEnd.position, rotation).GetComponent<Bullet>();
            arrow.Init(hero.Stats.Attack * baseDamage, arrowSpeed, hero.Stats.AreaOfEffect, hero.Stats.Pierce);
        }
    }
}