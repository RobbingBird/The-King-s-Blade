using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class store : MonoBehaviour
{
    //variables
    public GameObject Steel; 
    private GameObject steelInstance; 

    private int steelAmount = 0; 
    public float steelDistance = 0;

    private void OnEnable(){
        Mining.mineSteel += spawnSteel; 
    }
    void spawnSteel(){
        steelInstance = Instantiate(Steel, transform);
        steelInstance.transform.position = transform.position;
        steelInstance.transform.position.y += steelAmount * steelDistance;
        steelAmount += 1;
    }
}
