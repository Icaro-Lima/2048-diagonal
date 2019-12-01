using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Reload : MonoBehaviour
{
    private Button button;

    // Start is called before the first frame update
    private void Start()
    {
        button = GetComponent<Button>();

        button.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("SampleScene");
        });
    }

    // Update is called once per frame
    private void Update()
    {

    }
}
