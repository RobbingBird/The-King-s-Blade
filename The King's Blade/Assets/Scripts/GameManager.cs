using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int points = 0; 
    public int winPoints = 6;

    void OnEnable(){
        Forge.onComplete += manageComplete;
    }

    void OnDisable(){
        Forge.onComplete -= manageComplete;
    }

    void manageComplete(int point, int trash){
        points += point;

        if (points >= winPoints){
            Debug.Log("Win");
        }
    }
}
