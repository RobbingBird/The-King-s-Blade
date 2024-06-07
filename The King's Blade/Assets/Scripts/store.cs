using UnityEngine;

public class Store : MonoBehaviour
{
    // Public variables for different steel types
    public GameObject steel; 
    public GameObject orcishSteel;
    public GameObject dwarvenSteel;
    public GameObject elvishSteel;

    // Private instance variable to keep track of the spawned steel
    private GameObject steelInstance;

    // Configuration variables
    private int steelAmount = 0; 
    public float steelDistance = 0;
    public int steelMax = 4; 
    private int whatSteel = 0;

    // Cached transform component for optimization
    private Transform cachedTransform;

    private void OnEnable()
    {
        Mining.mineSteel += SpawnSteel; 
        PickupDrop.onPickup += RemoveSteel;
        PickupDrop.onPlace += addSteel;
        Forge.onComplete += moreStorage;
    }

    private void OnDisable()
    {
        Mining.mineSteel -= SpawnSteel;
        PickupDrop.onPickup -= RemoveSteel;
        PickupDrop.onPlace -= addSteel;
        Forge.onComplete -= moreStorage;
    }

    private void Start()
    {
        cachedTransform = transform;
    }

    private void SpawnSteel()
{
    if (steelAmount < steelMax)
    {
        whatSteel = Random.Range(1, 61); // 1 to 60 inclusive

        // Spawn the appropriate type of steel based on random value at the adjusted position
        if (whatSteel <= 10)
        {
            steelInstance = Instantiate(elvishSteel, cachedTransform);
        }
        else if (whatSteel <= 23)
        {
            steelInstance = Instantiate(dwarvenSteel, cachedTransform);
        }
        else if (whatSteel <= 40)
        {
            steelInstance = Instantiate(orcishSteel, cachedTransform);
        }
        else
        {
            steelInstance = Instantiate(steel, cachedTransform);
        }

        steelAmount++;

        steelInstance.transform.localScale = new Vector3(0.07f, 0.07f, 0.07f);
        steelInstance.transform.position += new Vector3(0, 0.3f, 0);

        Collider2D[] colliders;
        bool foundSteel = false;
        do
        {
            colliders = Physics2D.OverlapCircleAll(steelInstance.transform.position, 1f); // Adjust the radius as needed
            foundSteel = false;
            foreach (Collider2D collider in colliders)
            {
                if (collider.CompareTag("Steel") && collider.gameObject != steelInstance)
                {
                    steelInstance.transform.position += new Vector3(steelDistance, 0, 0); // Adjust the x position by steelDistance
                    foundSteel = true;
                    break;
                }
            }
        } while (foundSteel);
    } else if (steelAmount > steelMax){
        steelAmount = steelMax;
    }
}

    private void RemoveSteel()
    {
        steelAmount--;
    }

    private void addSteel(){
        steelAmount ++;
    }

    private void moreStorage(int points, int trash, int mine, int store, int run){
        steelMax += store;
    }
}