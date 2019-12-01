using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Reload : MonoBehaviour
{
    public Sprite spriteOnGameOver;

    private Image image;
    private Button button;

    // Start is called before the first frame update
    private void Start()
    {
        image = GetComponent<Image>();
        button = GetComponent<Button>();

        button.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("SampleScene");
        });
    }

    public void OnGameOver()
    {
        image.overrideSprite = spriteOnGameOver;
    }
}
