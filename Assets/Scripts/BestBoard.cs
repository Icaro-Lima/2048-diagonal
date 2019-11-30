using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BestBoard : MonoBehaviour
{
    public Text text;

    private int best;

    // Start is called before the first frame update
    private void Start()
    {
        if (PlayerPrefs.HasKey("best"))
        {
            best = PlayerPrefs.GetInt("best");
        }
        else
        {
            best = 0;
        }

        text.text = best.ToString();
    }

    public void ScoreUpdated(int newScore)
    {
        if (newScore > best)
        {
            best = newScore;
            PlayerPrefs.SetInt("best", best);
        }

        text.text = best.ToString();
    }
}
