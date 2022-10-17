using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SO_InteractableBase : ScriptableObject
{
    public virtual bool EnterAction() { return true; }
    public virtual bool EnterAction<T>(T obj) where T : Object { return true; }
    public virtual bool ExitAction() { return true; }
    public virtual bool ExitAction<T>(T obj) where T: Object { return true; }
}