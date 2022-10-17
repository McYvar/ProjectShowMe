using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    private enum InteractableTarget { self, other, both, none }
    [SerializeField] private InteractableTarget target;
    [SerializeField] SO_InteractableBase action;

    [SerializeField] private InteractableMenu menu;


    private Dictionary<Object, SO_InteractableBase> EnterActionsDict;
    private Dictionary<Object, SO_InteractableBase> ExitActionsDict;

    private List<SO_InteractableBase> EnterActionsList;
    private List<SO_InteractableBase> ExitActionsList;

    private void OnEnable()
    {
        EnterActionsDict = new Dictionary<Object, SO_InteractableBase>();
        ExitActionsDict = new Dictionary<Object, SO_InteractableBase>();

        EnterActionsList = new List<SO_InteractableBase>();
        ExitActionsList = new List<SO_InteractableBase>();
    }

    private void OnDisable()
    {
        EnterActionsDict.Clear();
        ExitActionsDict.Clear();

        EnterActionsList.Clear();
        ExitActionsList.Clear();
    }

    private void FixedUpdate()
    {
        foreach (var action in EnterActionsDict)
        {
            if (action.Key != null)
            {
                if (action.Value.EnterAction(action.Key))
                {
                    StartCoroutine(RemoveFromDict(EnterActionsDict, action.Key));
                }
            }
            else
            {
                if (action.Value.EnterAction())
                {
                    StartCoroutine(RemoveFromDict(EnterActionsDict, action.Key));
                }
            }
        }

        foreach (var action in ExitActionsDict)
        {
            if (action.Key != null)
            {
                if (EnterActionsDict.ContainsKey(action.Key)) continue;

                if (action.Value.ExitAction(action.Key))
                {
                    StartCoroutine(RemoveFromDict(ExitActionsDict, action.Key));
                }
            }
            else
            {
                if (action.Value.ExitAction())
                {
                    StartCoroutine(RemoveFromDict(ExitActionsDict, action.Key));
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            switch (target)
            {
                case InteractableTarget.self:
                    if (player == PlayerManager.controllers[0]) StartCoroutine(AddToDict(EnterActionsDict, ExitActionsDict, PlayerManager.controllers[0], action));
                    else StartCoroutine(AddToDict(EnterActionsDict, ExitActionsDict, PlayerManager.controllers[1], action));
                    break;

                case InteractableTarget.other:
                    if (player == PlayerManager.controllers[0]) EnterActionsDict.Add(PlayerManager.controllers[1], action);
                    else if (player == PlayerManager.controllers[1]) StartCoroutine(AddToDict(EnterActionsDict, ExitActionsDict, PlayerManager.controllers[0], action));
                    break;

                case InteractableTarget.both:
                    StartCoroutine(AddToDict(EnterActionsDict, ExitActionsDict, PlayerManager.controllers[0], action));
                    StartCoroutine(AddToDict(EnterActionsDict, ExitActionsDict, PlayerManager.controllers[1], action));
                    break;
                case InteractableTarget.none:
                    break;
            }

            foreach (var submenu in menu.otherActions)
            {
                if (submenu.anything != null) StartCoroutine(AddToDict(EnterActionsDict, ExitActionsDict, submenu.anything, submenu.otherActions));
                else EnterActionsList.Add(submenu.otherActions);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            switch (target)
            {
                case InteractableTarget.self:
                    if (player == PlayerManager.controllers[0]) StartCoroutine(AddToDict(ExitActionsDict, EnterActionsDict, PlayerManager.controllers[0], action));
                    else StartCoroutine(AddToDict(ExitActionsDict, EnterActionsDict, PlayerManager.controllers[1], action));
                    break;

                case InteractableTarget.other:
                    if (player == PlayerManager.controllers[0]) ExitActionsDict.Add(PlayerManager.controllers[1], action);
                    else if (player == PlayerManager.controllers[1]) StartCoroutine(AddToDict(ExitActionsDict, EnterActionsDict, PlayerManager.controllers[0], action));
                    break;

                case InteractableTarget.both:
                    StartCoroutine(AddToDict(ExitActionsDict, EnterActionsDict, PlayerManager.controllers[0], action));
                    StartCoroutine(AddToDict(ExitActionsDict, EnterActionsDict, PlayerManager.controllers[1], action));
                    break;
                case InteractableTarget.none:
                    break;
            }

            foreach (var submenu in menu.otherActions)
            {
                if (submenu.anything != null) StartCoroutine(AddToDict(ExitActionsDict, EnterActionsDict, submenu.anything, submenu.otherActions));
                else ExitActionsList.Add(submenu.otherActions);
            }
        }
    }

    private IEnumerator AddToDict(Dictionary<Object, SO_InteractableBase> dict1, Dictionary<Object, SO_InteractableBase> dict2, Object obj, SO_InteractableBase action)
    {
        yield return new WaitUntil(() => !dict2.ContainsKey(obj));
        yield return new WaitForFixedUpdate();
        if (!dict1.ContainsKey(obj)) dict1.Add(obj, action);
    }

    private IEnumerator RemoveFromDict(Dictionary<Object, SO_InteractableBase> dict, Object obj)
    {
        yield return new WaitForFixedUpdate();
        if (dict.ContainsKey(obj)) dict.Remove(obj);
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
