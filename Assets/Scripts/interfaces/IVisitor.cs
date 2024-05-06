using System;
using UnityEngine;

public interface IVisitor
{
    public void Visit<T>(T visitable) where T : Component, IVisitable;
}
