using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShootingHandler : MonoBehaviour
{
    [SerializeField] private Transform gunEnd;
    [SerializeField] private GameObject bullet;
    private float nextFire;
    private float fireRate;

    void Start()
    {
        fireRate = 0.25f;
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1") && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            Instantiate(bullet, new Vector3(gunEnd.position.x, gunEnd.position.y, gunEnd.position.z), Quaternion.identity);
        }
    }
}