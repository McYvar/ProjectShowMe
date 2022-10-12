using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SO_InteractableBase : ScriptableObject
{
    public virtual void Execute() { }
    public virtual void Execute<T>(T obj) where T : Object { }

}