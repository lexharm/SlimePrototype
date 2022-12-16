using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    private int score = 0;
    public Text scoreLabel;
    public static ScoreManager instance;

    private void Awake()
    {
        instance = this;
    }

    public void AddScore()
    {
        score++;
        scoreLabel.text = "Score: " + score;
    }

    public bool TakeOffScore(int value)
    {
        if (score >= value)
        {
            score -= value;
            scoreLabel.text = "Score: " + score;
            return true;
        }
        return false;
    }
}
