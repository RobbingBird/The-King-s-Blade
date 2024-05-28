using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Mining : MonoBehaviour
{
    //variables
    public float mineCountdown;
    private float currentTime = 0;


    // Define the states using an enum
    public enum MineState
    {
        None,
        Mining
    }

    // Start with the initial state as None
    private MineState currentState = MineState.None;

    void Start()
    {
        // Ensure the initial state is set
        SetState(MineState.None);
    }

    void Update()
    {
        // Log the current state every frame
        switch (currentState)
        {
            case MineState.None:
                break;
            case MineState.Mining:
            if (currentTime >= 0f){
                currentTime -= Time.deltaTime;
            }
            else if (Input.GetKey(KeyCode.Space) & currentTime < 0f){
                Debug.Log("Mining");
                currentTime = mineCountdown;
            }
                break;
        }
    }

    // This function changes the state
    private void SetState(MineState newState)
    {
        currentState = newState;
    }

    // When the player enters the trigger, change state to Mining
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            currentTime = mineCountdown;
            SetState(MineState.Mining);
        }
    }

    // When the player exits the trigger, change state to None
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SetState(MineState.None);
        }
    }
}