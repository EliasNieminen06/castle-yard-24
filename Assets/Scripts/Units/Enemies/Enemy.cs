using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Unit, IVisitor
{
    [SerializeField] List<itemDrop> dropTable;
    [SerializeField] private Rigidbody rb;

    private Transform player;

    public override void Init()
    {
        player = GameManager.Instance.Player;
        if (rb == null)
        {
            Debug.LogWarning("Rigidbody On " + this.gameObject + " is null");
            rb = GetComponent<Rigidbody>();
        }
        base.Init();
    }

    private void FixedUpdate()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        rb.velocity = direction * Stats.Speed;

        //transform.position = Vector3.MoveTowards(transform.position, player.position, Stats.Speed);
    }

    public void Visit<T>(T visitable) where T : Component, IVisitable
    {
        if (visitable is Hero hero)
        {
            hero.TakeDamage(Stats.Attack);
            base.OnDeath();
            Destroy(gameObject);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        other.GetComponentInParent<IVisitable>()?.Accept(this);
    }

    private void OnCollisionEnter(Collision collision)
    {
        collision.transform.GetComponentInParent<IVisitable>()?.Accept(this);
    }

    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            TakeDamage(10f);
        }

        base.Update();
    }

    protected override void OnDeath()
    {
        // drop items before destroying
        foreach (var item in dropTable)
        {
            int rng = Random.Range(0, 100);
            if (rng < item.chance)
            {
                Instantiate(item.prefab, transform.position, Quaternion.identity);
            }
        }

        base.OnDeath();

        Destroy(gameObject);
    }

    [System.Serializable]
    private struct itemDrop
    {
        public GameObject prefab;
        public int chance;
    }
}
