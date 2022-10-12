using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    private enum InteractableTarget { self, other, both, none }
    [SerializeField] private InteractableTarget target;

    [SerializeField] private SO_InteractableBase action;

    [SerializeField] private InteractableMenu menu;

    private void OnCollisionEnter(Collision collision)
    {
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
        if (player != null)
        {
            switch (target)
            {
                case InteractableTarget.self:
                    if (player == PlayerManager.controllers[0]) action.Execute(player);
                    else action.Execute(PlayerManager.controllers[1]);
                    break;

                case InteractableTarget.other:
                    if (player == PlayerManager.controllers[0]) action.Execute(PlayerManager.controllers[1]);
                    else if (player == PlayerManager.controllers[1]) action.Execute(PlayerManager.controllers[0]);
                    break;

                case InteractableTarget.both:
                    action.Execute(PlayerManager.controllers[0]);
                    action.Execute(PlayerManager.controllers[1]);
                    break;
                case InteractableTarget.none:
                    break;
            }

            foreach(var submenu in menu.otherActions)
            {
                if (submenu.anything != null) submenu.otherActions.Execute(submenu.anything);
                else submenu.otherActions.Execute();
            }
        }
    }
}

[System.Serializable]
public struct InteractableMenu
{
    public OtherActionsMenu[] otherActions;
}

[System.Serializable]
public struct OtherActionsMenu
{
    public SO_InteractableBase otherActions;
    public Object anything;
}
