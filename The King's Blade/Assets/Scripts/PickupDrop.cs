using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupDrop : MonoBehaviour
{
    public float pickupRadius = 1f;
    public float forgeCheckRadius = 2f; // Radius to check for the Forge

    public delegate void onPickupAction();
    public static event onPickupAction onPickup;

    private bool inventoryFull = false;
    private bool inventoryCheck = true;

    public LayerMask itemLayer;
    public LayerMask forgeLayer;
    
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
    
    Collider2D[] colliders2 = Physics2D.OverlapCircleAll(transform.position, forgeCheckRadius, forgeLayer);
        foreach (Collider2D collider in colliders2)
        {
            if (collider.CompareTag("Forge") && inventoryFull && inventoryCheck) 
            {
                Debug.Log("Near Forge: Destroying inventory items");
                foreach (Transform child in transform)
                {
                    Destroy(child.gameObject);
                }
                inventoryFull = false;
                break;
            }
        }

    Collider2D[] colliders3 = Physics2D.OverlapCircleAll(transform.position, pickupRadius, itemLayer);
    foreach (Collider2D collider in colliders3)
    {
        if (collider.CompareTag("Storage") && inventoryFull && inventoryCheck)
        {
            transform.GetChild(0).SetParent(collider.transform);
            collider.transform.GetChild(0).position = collider.transform.position;
            inventoryFull = false;
        }
    }

    inventoryCheck = true;
}

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, pickupRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, forgeCheckRadius);
    }
}