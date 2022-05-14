using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmashObject : MonoBehaviour
{
    [Header("Components")]
    public GameManager gm;
    public Rigidbody rb;
    public BoxCollider boxCol;

    [Header("Variables")]
    public int value;
    public float forceApplied;
    public bool isSmashed;

    private void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isSmashed && other.gameObject.tag == "Player")
        {
            boxCol.enabled = false;
            rb.constraints = RigidbodyConstraints.None;

            Vector3 dir = transform.position - other.transform.position;

            dir.Normalize();

            rb.AddForce(dir * forceApplied * 1000f);

            gm.UpdateMailSmashed(value);

            isSmashed = true;
        }
    }
}
