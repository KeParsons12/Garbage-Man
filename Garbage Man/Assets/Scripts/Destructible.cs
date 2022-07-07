using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    [SerializeField] [Tooltip("The object to be switched when this object is hit")] private GameObject _destroyedVersion;
    [SerializeField] [Tooltip("The tag of the object that can make contact")] private string _colTag;
    [SerializeField] [Tooltip("The speed needed by the impact to cause the object to explode. Lower number = lower speed for impact explosion")] private float _colImpactVelocity;

    private void OnCollisionEnter(Collision collision)
    {
        //Check the tag
        if(collision.gameObject.CompareTag(_colTag))
        {
            //Check if the collision is greater than the collision impact variable needed to destroy the object
            if(collision.relativeVelocity.magnitude > _colImpactVelocity)
            {
                //Spawn in broken version
                Instantiate(_destroyedVersion, transform.position, transform.rotation);

                //Destroy old verion
                Destroy(gameObject);
            }
        }
    }
}
