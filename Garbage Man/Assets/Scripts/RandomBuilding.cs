using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomBuilding : MonoBehaviour
{
    private void Start()
    {
        // pick a random color
        Color newColor = new Color(Random.value, Random.value, Random.value, 1.0f);
        // apply it on current object's material
        Renderer renderer = GetComponent<Renderer>();
        renderer.material.color = newColor;

        //Random Y scale
        transform.localScale = new Vector3(transform.localScale.x, Random.Range(5f, 15f), transform.localScale.z);

        //Touch Ground
        transform.position = new Vector3(transform.position.x, (transform.localScale.y / 2f), transform.position.z);
    }
}
