using UnityEngine;

public abstract class Pickup : Entity, IVisitor
{
    public float DistanceToPlayer => (GameManager.Instance.Player.position - transform.position).magnitude;

    protected abstract void ApplyPickupEffect(Hero hero);

    public override void Init()
    {
        base.Init();
    }

    public void Visit<T>(T visitable) where T : Component, IVisitable
    {
        if (visitable is Hero hero)
        {
            ApplyPickupEffect(hero);
            Destroy(gameObject);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        other.GetComponent<IVisitable>()?.Accept(this);
    }
}
