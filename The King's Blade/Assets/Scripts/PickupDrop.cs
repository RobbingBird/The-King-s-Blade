using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public float pickupRadius = 1f;

    public delegate void onPickupAction();
    public static event onPickupAction onPickup;

    private bool inventoryFull = false;
    private bool inventoryCheck = true;

    public LayerMask itemLayer;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PickupItem();
        }
    }

    void PickupItem()
{
    //Pickup
    Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, pickupRadius, itemLayer);
    foreach (Collider2D collider in colliders)
    {
        if (collider.CompareTag("Steel") && !inventoryFull)
        {
            Debug.Log("Picked up: " + collider.gameObject.name);
            collider.transform.SetParent(transform);
            collider.GetComponent<PolygonCollider2D>().enabled = false; 
            inventoryFull = true;
            inventoryCheck = false; 
            onPickup?.Invoke(); 
            break;
        }
    }
    
    // Destroy children if inventory is full
    if (inventoryFull && inventoryCheck)
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
            inventoryFull = false;
        }
    }

    inventoryCheck = true;
}

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, pickupRadius);
    }
}