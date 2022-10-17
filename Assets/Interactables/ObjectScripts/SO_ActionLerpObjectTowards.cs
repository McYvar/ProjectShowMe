using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Interactables/Action/Lerp Object Towards", fileName = "LerpObjectTowards")]
public class SO_ActionLerpObjectTowards : SO_InteractableBase
{
    [SerializeField] private Vector3 newRelativeLocation;
    [SerializeField, Range(0f, 1f)] private float lerpSpeed;
    [SerializeField] private bool moveBackWhenNotActive;

    public override bool EnterAction<T>(T obj)
    {
        if (obj == null) return true;

        GameObject gameObject = obj as GameObject;
        if (gameObject == null) return true;

        if (Vector3.Distance(gameObject.transform.localPosition, newRelativeLocation) < 0.1f)
        {
            gameObject.transform.localPosition = newRelativeLocation;
            return true;
        }

        gameObject.transform.localPosition = Vector3.Lerp(gameObject.transform.localPosition, newRelativeLocation, lerpSpeed);
        return false;
    }

    public override bool ExitAction<T>(T obj)
    {
        if (obj == null || !moveBackWhenNotActive) return true;

        GameObject gameObject = obj as GameObject;
        if (gameObject.transform == null) return true;

        if (Vector3.Distance(gameObject.transform.localPosition, Vector3.zero) < 0.1)
        {
            gameObject.transform.localPosition =  Vector3.zero;
            return true;
        }

        gameObject.transform.localPosition = Vector3.Lerp(gameObject.transform.localPosition, Vector3.zero, lerpSpeed);
        return false;
    }
}
