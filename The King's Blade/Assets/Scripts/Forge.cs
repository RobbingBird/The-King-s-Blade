using UnityEngine;
using System.Collections;

public class Forge : MonoBehaviour
{
    public GameObject Card; 
    public GameObject Card1;
    public GameObject steel; 
    public GameObject storage;
    public GameObject storage1;
    public int steelAmount = 5; 
    private bool hasOrcish = false; 
    private bool hasDwarven = false; 
    private bool hasElvish = false; 
    private int whatCard = 1; 

    public int worth = 0; 
    public int increaseForgeSpeed = 0; 
    public int increaseTrashSpeed = 0; 
    public int increaseMineSpeed = 0;
    public int increaseStorage = 0;

    public bool doubleWorth = false;
    public bool completeBonus = false;

    public static float forgeTime = 20f; 
    public static float cardsComplete = 0f;

    public GameObject player; 

    public delegate void onCompleteAction(int points, int trash, int mine);
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
            if (whatCard == 1){
                Card.SetActive(true);
            }

            if (whatCard == 2){
                Card1.SetActive(true);
            }
            playerInTrigger = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (whatCard == 1){
                Card.SetActive(false);
            }

            if (whatCard == 2){
                Card1.SetActive(false);
            }
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
                    increaseTrashSpeed += 5;
                }

                if (this.name == "Forge"){
                    increaseMineSpeed += 3;
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
                    increaseForgeSpeed += 2;
                }

                if (this.name == "Forge"){
                    increaseMineSpeed += 7;
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
                    increaseForgeSpeed += 5; 
                }

                if (this.name == "Forge"){
                    worth += 3;
                }
            }

            if (steelAmount >= 11 && whatCard == 1)
            {
                whatCard += 1;
                steelAmount = 6; 

                if (this.name == "Forge (2)"){
                    worth += 3;
                }

                if (this.name == "Forge (1)" || this.name == "Forge"){
                    worth += 2;
                }

                forgeTime -= increaseForgeSpeed;
                onComplete?.Invoke(worth, increaseTrashSpeed, increaseMineSpeed);
                Card.transform.position = new Vector2(100, 100);
                worth = 0;
                Debug.Log(forgeTime);	
            }

            StartCoroutine(SteelAdditionCooldown());
        } else if (canAddSteel && steelAmount < 11 && whatCard == 2 && playerInTrigger) // Check if player is in the trigger area
        {
            Card1.transform.GetChild(steelAmount).gameObject.SetActive(true);
            steelAmount += 1;
            Destroy(Steel); // Destroy the added steel

            if (Steel.name == "orcishSteel(Clone)" && !hasOrcish)
            {
                Card1.transform.GetChild(3).gameObject.SetActive(true);
                hasOrcish = true;

                if (this.name == "Forge (2)"){
                    worth += 1;
                }

                if (this.name == "Forge (1)"){
                    storage.SetActive(true);
                    increaseStorage += 1;
                }

                if (this.name == "Forge"){
                    increaseMineSpeed += 3;
                }
            }
            else if (Steel.name == "dwarvenSteel(Clone)" && !hasDwarven)
            {
                Card1.transform.GetChild(4).gameObject.SetActive(true);
                hasDwarven = true;

                if (this.name == "Forge (2)"){
                    worth += 2;
                }

                if (this.name == "Forge (1)"){
                    storage1.SetActive(true);
                    increaseStorage += 1;
                }

                if (this.name == "Forge"){
                    increaseMineSpeed += 7;
                }
            }
            else if (Steel.name == "elvishSteel(Clone)" && !hasElvish)
            {
                Card1.transform.GetChild(5).gameObject.SetActive(true);
                hasElvish = true;

                if (this.name == "Forge (2)"){
                    doubleWorth = true;
                }

                if (this.name == "Forge (1)"){
                    completeBonus = true; 
                }

                if (this.name == "Forge"){
                    worth += 3;
                }
            }

            if (steelAmount >= 11 && whatCard == 2)
            {
                whatCard += 1;

                if (this.name == "Forge (2)" || this.name == "Forge (1)"){
                    worth += 1;
                    if (doubleWorth){
                        doubleWorth = false;
                        worth *= 2;
                    }

                    if (completeBonus){
                        worth += cardsComplete;
                    }
                }

                forgeTime -= increaseForgeSpeed;
                onComplete?.Invoke(worth, increaseTrashSpeed, increaseMineSpeed);
                Card.transform.position = new Vector2(100, 100);
                worth = 0;
                Debug.Log(forgeTime);	
            }

            StartCoroutine(SteelAdditionCooldown());
        }
        else if (playerInTrigger)
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

