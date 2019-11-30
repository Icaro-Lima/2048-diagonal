using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBoard : MonoBehaviour
{
    public Text text;

    private float score;

    // Start is called before the first frame update
    private void Start()
    {
        text.text = score.ToString();
    }

    public void PieceMerged(int valueA, int valueB)
    {
        score += valueA + valueB;

        text.text = score.ToString();
    }
}
