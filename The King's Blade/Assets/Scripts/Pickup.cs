using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public float pickupRadius = 1f;

    public delegate void onPickupAction();
    public static event onPickupAction onPickup;

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
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, pickupRadius, itemLayer);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Steel"))
            {
                Debug.Log("Picked up: " + collider.gameObject.name);
                Destroy(collider.gameObject);
                onPickup?.Invoke(); 
                break;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, pickupRadius);
    }
}