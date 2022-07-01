using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTrash : MonoBehaviour
{
    public GameObject objectToSpawn;
    public Transform spawnPos;

    private void Start()
    {
        SpawnObject();
    }

    public void SpawnObject()
    {
        var spawnedObject = Instantiate(objectToSpawn, spawnPos.position, spawnPos.rotation);
        spawnedObject.transform.parent = spawnPos.transform;
    }
}
