using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public int points = 0; 
    public int winPoints = 6;

    public GameObject win;

    public TextMeshProUGUI timerText;
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI pointsText;
    public float remainingTime = 300f; // 5 minutes in seconds
    private bool isGameOver = false;

    void Start()
    {
        gameOverText.gameObject.SetActive(false);
        StartCoroutine(StartTimer());
    }

    IEnumerator StartTimer()
    {
        while (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
            UpdateTimerDisplay();
            yield return null;
        }

        GameOver();
    }

    void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        pointsText.text = points + "/" + winPoints;
    }

    void GameOver()
    {
        isGameOver = true;
        timerText.gameObject.SetActive(false);
        gameOverText.gameObject.SetActive(true);
    }

    void OnEnable(){
        Forge.onComplete += manageComplete;
    }

    void OnDisable(){
        Forge.onComplete -= manageComplete;
    }

    void manageComplete(int point, int trash, int mine, int store, int run){ 
        points += point; 

        if (points >= winPoints){
            Debug.Log("Win");
            win.SetActive(true);
        }
    }
}
