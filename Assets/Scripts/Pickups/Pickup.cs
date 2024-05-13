using UnityEngine;

public abstract class Pickup : Entity, IVisitor
{
    public float DistanceToPlayer => (GameManager.Instance.Player.position - transform.position).magnitude;

    protected abstract void ApplyPickupEffect(Hero hero);

    private bool visited;

    public override void Init()
    {
        visited = false;
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
        other.GetComponentInParent<IVisitable>()?.Accept(this);
    }
}
