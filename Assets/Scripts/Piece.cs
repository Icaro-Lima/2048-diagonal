using UnityEngine;
using UnityEngine.UI;

public class Piece : MonoBehaviour
{
    private int _value;
    private Color _backgroundColor;
    private Color _fontColor;
    private Vector2 _anchoredPos;

    public void Init(int value, Color backgroundColor, Color fontColor, Vector2 anchoredPos)
    {
        _value = value;
        _backgroundColor = backgroundColor;
        _fontColor = fontColor;
        _anchoredPos = anchoredPos;
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