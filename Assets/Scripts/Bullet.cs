using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float damage;
    [SerializeField] float lifeTime;
    [SerializeField] private Rigidbody rb;

    private int pierce;

    void Update()
    {
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0) Destroy(this.gameObject);
    }

    public void Init(float damage, float projectileSpeed, float AreaOfEffect, int pierce)
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
            Debug.LogWarning("Rigidbody on " + this + " is null");
        }

        rb.AddForce(transform.forward * projectileSpeed, ForceMode.Impulse);

        transform.localScale *= AreaOfEffect / 100;

        this.damage = damage;
        this.pierce = pierce;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            Destroy(gameObject);
            return;
        }

        IDamageable damageable = other.gameObject.GetComponentInParent<IDamageable>();
        if (damageable == null)
        {
            Debug.LogWarning($"{this} collided with a non damageable");
            return;
        }

        damageable.TakeDamage(damage);
        pierce--;

        if (pierce <= 0) DestroySelf();

        //Debug.Log("Dealt " + damage + " damage");
    }

    private void DestroySelf()
    {
        Destroy(gameObject);
    }
}
