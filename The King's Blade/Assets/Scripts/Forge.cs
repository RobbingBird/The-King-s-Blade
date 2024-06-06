using UnityEngine;
using System.Collections;

public class Forge : MonoBehaviour
{
    public GameObject Card; 
    public GameObject steel; 
    public int steelAmount = 5; 
    private bool hasOrcish = false; 
    private bool hasDwarven = false; 
    private bool hasElvish = false; 
    private int whatCard = 1; 

    public int worth = 0; 
    

    public float forgeTime = 10f;

    public GameObject player;

    public delegate void onCompleteAction(int points);
    public delegate void onKeepAction();
    public static event onCompleteAction onComplete;
    public static event onKeepAction keep;

    private bool canAddSteel = true; // To control when steel can be added
    private bool playerInTrigger = false; // To check if player is in the trigger area

    void OnEnable(){
        PickupDrop.onForge += addSteel;
    }

    void OnDisable(){
        PickupDrop.onForge -= addSteel;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Card.SetActive(true);
            playerInTrigger = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Card.SetActive(false);
            playerInTrigger = false;
        }
    }

    private void addSteel(GameObject Steel)
    {
        if (canAddSteel && steelAmount < 11 && whatCard == 1 && playerInTrigger) // Check if player is in the trigger area
        {
            Card.transform.GetChild(steelAmount).gameObject.SetActive(true);
            steelAmount += 1;
            Destroy(Steel); // Destroy the added steel

            if (Steel.name == "orcishSteel(Clone)" && !hasOrcish)
            {
                Card.transform.GetChild(3).gameObject.SetActive(true);
                hasOrcish = true;

                if (this.name == "Forge (2)"){
                    worth += 1;
                }

                if (this.name == "Forge (1)"){

                }
            }
            else if (Steel.name == "dwarvenSteel(Clone)" && !hasDwarven)
            {
                Card.transform.GetChild(4).gameObject.SetActive(true);
                hasDwarven = true;

                if (this.name == "Forge (2)"){
                    worth += 2;
                }

                if (this.name == "Forge (1)"){

                }
            }
            else if (Steel.name == "elvishSteel(Clone)" && !hasElvish)
            {
                Card.transform.GetChild(5).gameObject.SetActive(true);
                hasElvish = true;

                if (this.name == "Forge (2)"){
                    worth += 3;
                }

                if (this.name == "Forge (1)"){

                }
            }

            if (steelAmount >= 11 && whatCard == 1)
            {
                whatCard += 1;
                if (this.name == "Forge (2)"){
                    worth += 3;
                }

                onComplete?.Invoke(worth);
                Card.transform.position = new Vector2(100, 100);
                worth = 0;
            }

            StartCoroutine(SteelAdditionCooldown());
        }
        else if (whatCard == 1 && playerInTrigger)
        {
            keep?.Invoke();
            Debug.Log("Can't add steel");
        }
    }

    private IEnumerator SteelAdditionCooldown()
    {
        canAddSteel = false; // Disable adding steel
        yield return new WaitForSeconds(forgeTime); // Wait for the specified time
        canAddSteel = true; // Re-enable adding steel
    }
}
