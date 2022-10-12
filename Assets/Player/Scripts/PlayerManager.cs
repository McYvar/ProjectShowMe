using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    public static PlayerController[] controllers;
    [SerializeField] private Material[] materials;
    [SerializeField] private Vector3[] spawns;

    private void Start()
    {
        controllers = new PlayerController[2];
    }

    public void PlayerJoined(PlayerInput input)
    {
        controllers = FindObjectsOfType<PlayerController>();
        for (int i = 0; i < controllers.Length; i++)
        {
            controllers[i].GetComponent<MeshRenderer>().material = materials[i];
            controllers[i].transform.position = spawns[i];
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        foreach (var spawn in spawns)
        {
            Gizmos.DrawSphere(spawn, 0.3f);
        }
    }
}
