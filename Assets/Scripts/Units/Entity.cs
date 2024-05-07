using System;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    private void Awake()
    {
        // Temporary
        Init();
    }

    public virtual void Init()
    {
        
    }
}
