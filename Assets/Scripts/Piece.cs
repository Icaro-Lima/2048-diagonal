using UnityEngine;
using UnityEngine.UI;

public class Piece : MonoBehaviour
{
    public Color[] bgColors;
    public Color[] fontColors;

    private int _value;
    private Color _backgroundColor;
    private Color _fontColor;
    private Vector2 _anchoredPos;

    public void Init(int value, Vector2 anchoredPos)
    {
        _value = value;
        _anchoredPos = anchoredPos;

        int log = (int)Mathf.Log(value, 2);
        _backgroundColor = bgColors[Mathf.Clamp(log - 1, 0, bgColors.Length - 1)];
        _fontColor = fontColors[Mathf.Clamp(log - 1, 0, fontColors.Length - 1)];
    }

    // Start is called before the first frame update
    private void Start()
    {
        GetComponent<Image>().color = _backgroundColor;

        var text = GetComponentInChildren<Text>();
        text.text = _value.ToString();
        text.color = _fontColor;
        GetComponent<RectTransform>().anchoredPosition = _anchoredPos;
    }
}