using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : Singleton<ScoreManager>


{

    public  int current_level;
    public int m_currentScore = 0;
    int m_counterValue = 5;
    int m_increment = 5;
    public static int moves_left;
    public Text scoreText;
    public float countTime = 1f;
    public Text Moves_Left;
    public Text Highest;



    
    void Start()
    {
        UpdateScoreText(m_currentScore);
        
    }

    public void UpdateScoreText(int scoreValue)
    {
        if (scoreText != null)
        {
            scoreText.text = scoreValue.ToString();
        }

       
    }


    public void UpdateHighest(int k ) //updating highest score 
    {
        if (Highest != null)
        {
            Highest.text = "Highest:" + k.ToString();
        }
    }
    public void UpdateMovesCount(int m)
    { 
        Moves_Left.text = "Moves Left:"+(m).ToString();

    }
    public void AddScore(int value) // adding score to the current score
    {
        m_currentScore += value;
        StartCoroutine(CountScoreRoutine());

    }

    IEnumerator CountScoreRoutine() // we need very short pause everytime the score is updated in order to do it, coroutine is needed
    {
        int iterations = 0;

        while (m_counterValue < m_currentScore && iterations < 100000) //<10000 part is defensive programming
        {
            m_counterValue += m_increment; //increase showed value 
            UpdateScoreText(m_counterValue);
            iterations++;
            yield return null; //making a frame, every frame we count up a small increment

            m_counterValue = m_currentScore;
            UpdateScoreText(m_currentScore);
        }
    }
}
