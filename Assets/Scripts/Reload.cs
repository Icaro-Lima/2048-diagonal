using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Reload : MonoBehaviour
{
    public Sprite spriteOnGameOver;

    public UnityEvent onReloadConfirmation;

    private Image image;
    private Button button;

    private bool gameOver;

    private void Awake()
    {
        if (onReloadConfirmation == null)
        {
            onReloadConfirmation = new UnityEvent();
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
        image = GetComponent<Image>();
        button = GetComponent<Button>();

        button.onClick.AddListener(() =>
        {
            if (gameOver)
            {
                SceneManager.LoadScene("SampleScene");
            }
            else
            {
                onReloadConfirmation.Invoke();
            }
        });
    }

    public void OnGameOver()
    {
        gameOver = true;
        image.overrideSprite = spriteOnGameOver;
    }

    public void OnConfirmReload()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
