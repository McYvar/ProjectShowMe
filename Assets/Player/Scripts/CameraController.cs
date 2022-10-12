using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform[] players;
    [SerializeField] private Transform mainCamera;
    [SerializeField, Range(0.0f, 10.0f)] float cameraOffsetDistance;
    [SerializeField, Range(1.0f, 10.0f)] float distanceOffsetReduction;
    [SerializeField, Range(0.01f, 1.0f)] float cameraLerpSpeed;

    private void Start()
    {
        players = new Transform[2];
    }

    private void LateUpdate()
    {
        foreach (Transform player in players)
        {
            if (player == null) return;
        }

        Vector3 nextPostion = transform.position;

        for (int i = 0; i < players.Length; i++)
        {
            if (i == 0)
            {
                nextPostion = players[i].position;
            }
            else
            {
                nextPostion += (players[i].position - nextPostion) / 2;
            }
        }
        transform.position = Vector3.Lerp(transform.position, nextPostion, cameraLerpSpeed);

        mainCamera.localPosition = Vector3.Lerp(mainCamera.localPosition, 
            -mainCamera.transform.forward * Vector3.Distance(players[0].position, players[1].position) / distanceOffsetReduction - mainCamera.transform.forward * cameraOffsetDistance,
            cameraLerpSpeed);
    }

    public void ConnectPlayerToInstances(Transform player)
    {
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i] == null)
            {
                players[i] = player;
                return;
            }
        }
    }
}
