using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class SpawnPlayers : MonoBehaviour
{
    public GameObject playerPrefab;

    public List<Transform> spawnPoints = new List<Transform>();

    private void Start()
    {
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count - 1)];
        PhotonNetwork.Instantiate(playerPrefab.name, spawnPoint.position, spawnPoint.rotation);
        spawnPoints.Remove(spawnPoint);
    }
}
