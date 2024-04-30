using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] int damage;
    [SerializeField] float speed;
    [SerializeField] float lifeTime;
    [SerializeField] Rigidbody rb;
    [SerializeField] GameObject player;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        rb.AddForce(player.transform.forward, ForceMode.Impulse);
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0) Destroy(this.gameObject);
    }
}
