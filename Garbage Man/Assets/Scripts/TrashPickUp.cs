using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashPickUp : MonoBehaviour
{
    [Header("Components")]
    public GameManager gm;
    public Animator anim;
    public Transform firePos;
    public GameObject trashProjectile;

    [Header("Inputs")]
    public KeyCode pickupButton = KeyCode.Space;

    [Header("Variables")]
    public string trashTag;
    public float pickUpTime = 2f;

    //Can be private
    public bool isPickUp = false;
    public bool canPickUp = true;

    [Header("Pick up Item")]
    public int scoreValue;
    public Collider pickUpItem;

    private void Start()
    {
        //find gamemanager
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void Update()
    {
        //Pick up trash bin
        PickUpTrash();

        //Animations
        HandleAnimation();
    }

    public void PickUpTrash()
    {
        if(Input.GetKeyDown(pickupButton) && canPickUp)
        {
            if(pickUpItem != null && !isPickUp)
            {
                //Picking up object
                isPickUp = true;
                canPickUp = false;

                //Add points
                scoreValue = pickUpItem.GetComponent<PickUpItem>().pointValue;
                gm.UpdateScore(scoreValue, 1);

                //Destroy objects
                Destroy(pickUpItem.gameObject);

                //wait until pick up is done
                Invoke("ResetPickUp", pickUpTime);
                Invoke("ThrowTrash", pickUpTime - 1f);
            }
        }
    }

    public void ResetPickUp()
    {
        isPickUp = false;
        canPickUp = false;
        pickUpItem = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == trashTag)
        {
            //Trash collider has been entered
            canPickUp = true;
            pickUpItem = other;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        canPickUp = false;
        pickUpItem = null;
    }

    public void ThrowTrash()
    {
        Instantiate(trashProjectile, firePos.position, firePos.rotation);
        //projectile.GetComponent<Rigidbody>().AddForce();
    }

    public void HandleAnimation()
    {
        anim.SetBool("isPickingUp", isPickUp);
    }
}
