using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class store : MonoBehaviour
{
    //variables
    public GameObject Steel; 
    public GameObject orcishSteel;
    public GameObject dwarvenSteel;
    public GameObject elvishSteel;
    private GameObject steelInstance; 

    private int steelAmount = 0; 
    public float steelDistance = 0;
    public float steelMax = 4; 
    public int whatSteel = 0;

    private void OnEnable(){
        Mining.mineSteel += spawnSteel; 
    }

    private void OnDisable()
    {
        Mining.mineSteel -= spawnSteel;
    }
    void spawnSteel(){
        if (steelAmount <= steelMax - 1){
            whatSteel = Random.Range(1,60);
            if (whatSteel <= 10){
                steelInstance = Instantiate(elvishSteel, transform);
            } else if (whatSteel > 10 && whatSteel <= 23){
                steelInstance = Instantiate(dwarvenSteel, transform);
            } else if (whatSteel > 23 && whatSteel <= 40){
                steelInstance = Instantiate(orcishSteel, transform);
            } else if (whatSteel > 40 && whatSteel <= 60){
                steelInstance = Instantiate(Steel, transform);
            }
            
            UnityEngine.Vector3 newPosition = transform.position;
            newPosition.x += steelAmount * steelDistance;
            steelInstance.transform.position = newPosition;
            steelAmount += 1;
        }
    }
}
