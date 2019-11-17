using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Piece : MonoBehaviour
{
    public Color[] bgColors;
    public Color[] fontColors;

    // Init
    private int _value;
    private Color _backgroundColor;
    private Color _fontColor;
    private Vector2 _anchoredPos;

    // Start
    private RectTransform _rectTransform;

    private Queue<Vector2> _targetPositions;

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
        _rectTransform = GetComponent<RectTransform>();

        _rectTransform.anchoredPosition = _anchoredPos;

        GetComponent<Image>().color = _backgroundColor;

        Text text = GetComponentInChildren<Text>();
        text.text = _value.ToString();
        text.color = _fontColor;

        _targetPositions = new Queue<Vector2>();
    }

    private void Update()
    {
        const float speed = 90;

        if (_targetPositions.Count > 0)
        {
            Vector2 targetPos = _targetPositions.Peek();

            _rectTransform.anchoredPosition = Vector2.MoveTowards(_rectTransform.anchoredPosition, targetPos, speed);

            if (targetPos == _rectTransform.anchoredPosition)
            {
                _targetPositions.Dequeue();
            }
        }
    }

    public void MoveTo(Vector2 boardPos)
    {
        _targetPositions.Enqueue(boardPos);
    }
}