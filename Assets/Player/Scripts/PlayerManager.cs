using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private Material[] materials;

    public void PlayerJoined(PlayerInput input)
    {
        PlayerController[] controllers = FindObjectsOfType<PlayerController>();
        for (int i = 0; i < controllers.Length; i++)
        {
            controllers[i].GetComponent<MeshRenderer>().material = materials[i];
        }
    }
}
