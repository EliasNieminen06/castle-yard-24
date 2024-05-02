using System;
public interface IVisitable
{
    public void Accept(IVisitor visitor);
}
