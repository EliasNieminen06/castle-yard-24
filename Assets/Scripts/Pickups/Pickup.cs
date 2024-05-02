using UnityEngine;

public abstract class Pickup : MonoBehaviour, IVisitor
{
    protected abstract void ApplyPickupEffect(Hero hero);

    public void Visit<T>(T visitable) where T : Component, IVisitable
    {
        if (visitable is Hero hero)
        {
            ApplyPickupEffect(hero);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        other.GetComponent<IVisitable>()?.Accept(this);
        Destroy(gameObject);
    }
}
