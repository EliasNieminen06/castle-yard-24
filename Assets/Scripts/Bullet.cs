using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float damage;
    [SerializeField] float lifeTime;
    [SerializeField] private Rigidbody rb;

    void Update()
    {
        rb.AddForce(transform.forward, ForceMode.Impulse);
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0) Destroy(this.gameObject);
    }

    public void Init(float damage, float projectileSpeed)
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
            Debug.LogWarning("Rigidbody on " + this + " is null");
        }

        rb.AddForce(transform.forward * projectileSpeed, ForceMode.Impulse);

        this.damage = damage;
    }

    private void OnCollisionEnter(Collision collision)
    {
        collision.gameObject.GetComponent<IDamageable>()?.TakeDamage(damage);
        Debug.Log("Dealt " + damage + " damage");
        Destroy(gameObject);
    }
}
