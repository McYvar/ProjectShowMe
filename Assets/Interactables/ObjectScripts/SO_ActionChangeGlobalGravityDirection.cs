using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Interactables/Action/Change Global Gravity", fileName = "ChangeGlobalGravity")]
public class SO_ActionChangeGlobalGravityDirection : SO_InteractableBase
{
    [SerializeField] private Vector3 gravityDirection;

    public override void Execute()
    {
        Physics.gravity = gravityDirection;
    }
}
