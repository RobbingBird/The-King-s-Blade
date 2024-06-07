using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Mining : MonoBehaviour
{
    public delegate void onMineAction(); 
    public static event onMineAction mineSteel;

    private bool mine = false;
    
    public int mineAmount = 0;
    public int mineMax = 20;

    public void OnEnable(){
        Forge.onComplete += increaseMineSpeed;
    }

    public void OnDisable(){
        Forge.onComplete -= increaseMineSpeed;
    }

    public void OnTriggerEnter2D(){
        mine = true;
    }

    public void OnTriggerExit2D(){
        mine = false;
    } 

    public void Update(){
        if (mine && Input.GetKeyDown(KeyCode.Space)){
            mineAmount++;
            Debug.Log(mineAmount);
        }

        if (mineAmount >= mineMax){
            mineSteel?.Invoke();
            mineAmount = 0;
        }
    }

    public void increaseMineSpeed(int point, int trash, int mine){
        mineMax -= mine;
    }
}