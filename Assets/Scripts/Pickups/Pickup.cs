using UnityEngine;

public abstract class Pickup : Entity, IVisitor
{
    public float DistanceToPlayer => (GameManager.Instance.Player.position - transform.position).magnitude;

    protected abstract void ApplyPickupEffect(Hero hero);

    private bool visited;

    [SerializeField] Rigidbody rb;

    public override void Init()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
            Debug.LogWarning($"Rigidbody on {this} is null");
        }

        visited = false;
        ShootInRandomDirection();
        base.Init();
    }

    public void Visit<T>(T visitable) where T : Component, IVisitable
    {
        if (visited) return;

        if (visitable is Hero hero)
        {
            visited = true;
            ApplyPickupEffect(hero);
            Destroy(gameObject);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground")) rb.drag = 10f;
    }

    public void OnTriggerStay(Collider other)
    {
        other.GetComponentInParent<IVisitable>()?.Accept(this);
    }

    private void ShootInRandomDirection()
    {
        rb.AddForce(Random.insideUnitSphere.normalized * 5f, ForceMode.Impulse);
    }

}
