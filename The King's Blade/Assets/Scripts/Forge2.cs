using UnityEngine;
using System.Collections;

public class Forge2 : MonoBehaviour
{
    public GameObject Card; 
    public GameObject steel; 
    public int steelAmount = 5; 
    private bool hasOrcish = false; 
    private bool hasDwarven = false; 
    private bool hasElvish = false; 
    private int whatCard = 1; 

    private int worth = 0; 

    public GameObject player;

    public delegate void onCompleteAction(int points);
    public delegate void onKeepAction();
    public static event onCompleteAction onComplete;
    public static event onKeepAction keep;

    private bool canAddSteel = true; // To control when steel can be added

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
        }
    }

    void OnTriggerExit2D(Collider2D other)    {
        if (other.CompareTag("Player")){
            Card.SetActive(false);
        }
    }

    private void addSteel(GameObject Steel){
        if (canAddSteel && steelAmount < 11 && whatCard == 1){ 

            Card.transform.GetChild(steelAmount).gameObject.SetActive(true);
            steelAmount += 1; 
            Destroy(player.transform.GetChild(0).gameObject);

            if (Steel.name == "orcishSteel(Clone)" && hasOrcish == false){
                Card.transform.GetChild(3).gameObject.SetActive(true);
                hasOrcish = true;
                worth += 1;
            } else if (Steel.name == "dwarvenSteel(Clone)" && hasDwarven == false){
                Card.transform.GetChild(4).gameObject.SetActive(true);
                hasDwarven = true;
                worth += 2;
            } else if (Steel.name == "elvishSteel(Clone)" && hasElvish == false){
                Card.transform.GetChild(5).gameObject.SetActive(true);
                hasElvish = true;
                worth += 3;
            }

            if (steelAmount >= 11 && whatCard == 1){
                whatCard += 1;
                worth += 3;
                onComplete?.Invoke(worth);
                Card.transform.position = new Vector2(100, 100);
                worth = 0;
            }

            StartCoroutine(SteelAdditionCooldown());
        } else if (whatCard == 1){
            keep?.Invoke();
        }
    }

    private IEnumerator SteelAdditionCooldown() {
        canAddSteel = false; // Disable adding steel
        yield return new WaitForSeconds(10); // Wait for 10 seconds
        canAddSteel = true; // Re-enable adding steel
    }
}
