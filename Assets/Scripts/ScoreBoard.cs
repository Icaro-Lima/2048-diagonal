using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBoard : MonoBehaviour
{
    public Text text;

    public ScoreUpdatedEvent scoreUpdatedEvent;

    private int score;

    private void Awake()
    {
        if (scoreUpdatedEvent == null)
        {
            scoreUpdatedEvent = new ScoreUpdatedEvent();
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
        text.text = score.ToString();
    }

    public void PieceMerged(int valueA, int valueB)
    {
        score += valueA + valueB;

        scoreUpdatedEvent.Invoke(score);

        text.text = score.ToString();
    }
}
