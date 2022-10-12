using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Interactables/Action/Change Player Gravity", fileName = "ChangePlayerGravity")]
public class SO_ActionChangeGravityDirectionOfPlayer : SO_InteractableBase
{
    [SerializeField] private Vector3 gravityDirection;

    public override void Execute<T>(T obj)
    {
        PlayerController player = obj as PlayerController;
        player.ChangeGravityDirection(gravityDirection);
    }
}
