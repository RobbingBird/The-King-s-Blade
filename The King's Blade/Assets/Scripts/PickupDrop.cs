using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PickupDrop : MonoBehaviour
{
    public float pickupRadius = 1f;
    public float trashTime = 10f; // Timer delay in seconds
    public TextMeshProUGUI trashTimerText; // Text element for displaying trash timer

    public delegate void onPickupAction();
    public delegate void onPlaceAction();
    public delegate void onForgeAction(GameObject Steel);
    public static event onPickupAction onPickup;
    public static event onPlaceAction onPlace;
    public static event onForgeAction onForge;

    private bool inventoryFull = false;
    private bool inventoryCheck = true;
    private bool nearForge = false; // Tracks if the player is near the forge
    private bool canTrash = true;

    public LayerMask itemLayer;

    void OnEnable()
    {
        Forge.keep += keep2;
        Forge.onComplete += trashSpeedChange;
    }

    void OnDisable()
    {
        Forge.keep -= keep2;
        Forge.onComplete += trashSpeedChange;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PickupItem();
        }
    }

    void PickupItem()
    {
        // Pickup
        Collider2D[] colliders1 = Physics2D.OverlapCircleAll(transform.position, pickupRadius, itemLayer);
        foreach (Collider2D collider in colliders1)
        {
            if (collider.CompareTag("Steel") && !inventoryFull)
            {
                collider.transform.SetParent(transform);
                collider.transform.position = transform.position + new Vector3(0, 1, 0);
                collider.GetComponent<PolygonCollider2D>().enabled = false;
                inventoryFull = true;
                inventoryCheck = false;
                onPickup?.Invoke();
                break;
            }

            if (collider.CompareTag("Trash") && inventoryFull && inventoryCheck && canTrash)
            {
                inventoryCheck = false; 
                Destroy(transform.GetChild(0).gameObject); 
                inventoryFull = false; 
                Debug.Log("Can't destroy steel"); 
                StartCoroutine(ResetInventoryCheckAfterDelay()); 
            }
        }

        // Forge
        if (nearForge && inventoryFull && inventoryCheck)
        {
            foreach (Transform child in transform)
            {
                inventoryFull = false;
                onForge?.Invoke(child.gameObject);
                break;
            }
        }

        // Place back in storage
        Collider2D[] colliders3 = Physics2D.OverlapCircleAll(transform.position, pickupRadius, itemLayer);
        foreach (Collider2D collider in colliders3)
        {
            if (collider.CompareTag("Storage") && inventoryFull && inventoryCheck)
            {
                bool steelAtPosition = false;
                Collider2D[] nearbyColliders = Physics2D.OverlapCircleAll(collider.transform.position, 0.1f, itemLayer);
                foreach (Collider2D nearbyCollider in nearbyColliders)
                {
                    if (nearbyCollider.CompareTag("Steel") && nearbyCollider.transform.position - new Vector3 (0, 0.3f, 0) == collider.transform.position)
                    {
                        steelAtPosition = true;
                        break;
                    }
                }

                if (!steelAtPosition)
                {
                    Transform child = transform.GetChild(0);
                    child.SetParent(collider.transform);
                    child.position = collider.transform.position;
                    child.position += new Vector3(0, 0.3f, 0);
                    child.GetComponent<PolygonCollider2D>().enabled = true;
                    onPlace?.Invoke();
                    inventoryFull = false;
                    break;
                }
            }
        }

        inventoryCheck = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Forge"))
        {
            nearForge = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Forge"))
        {
            nearForge = false;
        }
    }

    void keep2()
    {
        inventoryFull = true;
    }

    void trashSpeedChange(int points, int trash, int mine, int store, int run)
    {
        trashTime -= trash;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, pickupRadius);
    }

    IEnumerator ResetInventoryCheckAfterDelay()
    {
        canTrash = false;
        StartCoroutine(UpdateTrashTimer());
        yield return new WaitForSeconds(trashTime);
        canTrash = true;
    }

    private IEnumerator UpdateTrashTimer()
    {
        float remainingTime = trashTime;
        while (remainingTime > 0)
        {
            trashTimerText.text = remainingTime.ToString("F2");
            yield return null;
            remainingTime -= Time.deltaTime;
        }
        trashTimerText.text = " ";
    }
}
