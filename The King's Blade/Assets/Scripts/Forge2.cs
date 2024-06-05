using UnityEngine;

public class Forge2 : MonoBehaviour
{
    public GameObject Card; 
    public GameObject steel; 
    public int steelAmount = 5; 
    private bool hasOrcish = false; 
    private bool hasDwarven = false; 
    private bool hasElvish = false; 
    private int whatCard = 1; 

    public delegate void onPointAction(int point);
    public static event onPointAction onPoint;


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

            if (Steel.name == "orcishSteel(Clone)" && hasOrcish == false){
                Card.transform.GetChild(3).gameObject.SetActive(true);
                onPoint?.Invoke(1);
                hasOrcish = true;
            } else if (Steel.name == "dwarvenSteel(Clone)" && hasDwarven == false){
                Card.transform.GetChild(4).gameObject.SetActive(true);
                onPoint?.Invoke(2);
                hasDwarven = true;
            } else if (Steel.name == "elvishSteel(Clone)" && hasElvish == false){
                Card.transform.GetChild(5).gameObject.SetActive(true);
                onPoint?.Invoke(3);
                hasElvish = true;
            }

            if (steelAmount >= 11 && whatCard == 1){
            whatCard += 1;
            onPoint?.Invoke(3);
            Card.transform.position = new Vector2(100, 100);
            }
        }
    }
}
