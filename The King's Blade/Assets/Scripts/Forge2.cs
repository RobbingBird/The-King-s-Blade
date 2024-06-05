using UnityEngine;

public class Forge2 : MonoBehaviour
{
    public GameObject Card; 
    public GameObject steel; 
    public int steelAmount = 5; 
    //private bool hasOrcish = false; 
    //private bool hasDwarven = false; 
    //private bool hasElvish = false; 
    private int whatCard = 1; 

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

    void OnTriggerExit2D(Collider2D other)	{
        if (other.CompareTag("Player")){
            Card.SetActive(false);
        }
    }

    private void addSteel(GameObject Steel){
        if (steelAmount < 11 && whatCard == 1){ 
            Card.transform.GetChild(steelAmount).gameObject.SetActive(true);
            steelAmount += 1; 

            if (Steel.name == "orcishSteel(Clone)"){
                Card.transform.GetChild(3).gameObject.SetActive(true);
            } else if (Steel.name == "dwarvenSteel(Clone)"){
                Card.transform.GetChild(4).gameObject.SetActive(true);
            } else if (Steel.name == "elvishSteel(Clone)"){
                Card.transform.GetChild(5).gameObject.SetActive(true);
            }
        } else if (steelAmount >= 11 && whatCard == 1){
            whatCard += 1;
        }
    }
}
