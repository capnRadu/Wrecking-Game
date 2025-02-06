using System.Collections.Generic;
using UnityEngine;

public class ArtefactPiece : MonoBehaviour
{
    [SerializeField] private List<GameObject> spawnPoints = new List<GameObject>();

    private void Start()
    {
        int randomIndex = Random.Range(0, spawnPoints.Count);

        while (spawnPoints[randomIndex].transform.childCount != 0)
        {
            randomIndex = Random.Range(0, spawnPoints.Count);
        }

        transform.position = spawnPoints[randomIndex].transform.position;
        transform.SetParent(spawnPoints[randomIndex].transform, true);
        spawnPoints.RemoveAt(randomIndex);
    }
}
