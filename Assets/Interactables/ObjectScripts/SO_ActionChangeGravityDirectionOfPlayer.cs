using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Interactables/Action/Change Player Gravity", fileName = "ChangePlayerGravity")]
public class SO_ActionChangeGravityDirectionOfPlayer : SO_InteractableBase
{
    [SerializeField] private Vector3 gravityDirection;

    public override bool EnterAction<T>(T obj)
    {
        if (obj == null) return true;

        PlayerController player = obj as PlayerController;
        if (player == null) return true;

        player.ChangeGravityDirection(gravityDirection);
        return true;
    }
}
