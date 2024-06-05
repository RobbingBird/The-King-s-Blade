using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int points = 0; 
    public int winPoints = 6;

    void OnEnable(){
        Forge2.onPoint += managePoint;
    }

    void OnDisable(){
        Forge2.onPoint -= managePoint;
    }

    void managePoint(int point){
        points += point;

        if (points >= winPoints){
            Debug.Log("Win");
        }
    }
}
