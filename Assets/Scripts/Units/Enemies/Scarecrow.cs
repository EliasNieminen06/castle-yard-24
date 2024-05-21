using System.Collections;
using UnityEngine;

public class Scarecrow : Enemy
{
    [SerializeField] private float rotationSpeed = 1f;
    [SerializeField] private float bulletSpeed = 3f;

    [SerializeField] GameObject bulletPrefab;

    [SerializeField] Transform shootPoint1;
    [SerializeField] Transform shootPoint2;

    private float nextFire;

    public override void Init()
    {
        transform.position = new Vector3(transform.position.x, 1, transform.position.z);
        base.Init();
    }

    private void FixedUpdate()
    {
        transform.Rotate(Vector3.up, rotationSpeed);

        if (nextFire < Time.time)
        {
            Shoot();
            nextFire = Time.time + attackFrequency * (1 - Stats.CooldownReduction / 100) + Random.Range(-attackFrequency / 5, attackFrequency / 5);
        }
    }

    void Shoot()
    {
        GameObject newBullet = Instantiate(bulletPrefab, shootPoint1.position, shootPoint1.rotation);
        newBullet.GetComponent<Bullet>().Init(Stats.Attack, bulletSpeed, 100f, 0);

        newBullet = Instantiate(bulletPrefab, shootPoint2.position, shootPoint2.rotation);
        newBullet.GetComponent<Bullet>().Init(Stats.Attack, bulletSpeed, 100f, 0);
    }
}
