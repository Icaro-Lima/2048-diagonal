using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Piece : MonoBehaviour
{
    private struct Action
    {
        public Vector2 targetPos;
        public Piece pieceToMerge;

        public Action(Vector2 targetPos, Piece pieceToMerge)
        {
            this.targetPos = targetPos;
            this.pieceToMerge = pieceToMerge;
        }
    }

    public Color[] bgColors;
    public Color[] fontColors;

    // Init
    public int value { get; private set; }

    private Color _backgroundColor;
    private Color _fontColor;
    private Vector2 _anchoredPos;

    // Start
    private RectTransform _rectTransform;

    private Queue<Action> _targetPositions;

    public void Init(int value, Vector2 anchoredPos)
    {
        this.value = value;
        _anchoredPos = anchoredPos;
    }

    // Start is called before the first frame update
    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();

        _rectTransform.anchoredPosition = _anchoredPos;

        _targetPositions = new Queue<Action>();

        UpdateColorsAndText();
    }

    private void Update()
    {
        const float speed = 10;

        if (_targetPositions.Count > 0)
        {
            Vector2 targetPos = _targetPositions.Peek().targetPos;
            Piece targetPiece = _targetPositions.Peek().pieceToMerge;

            _rectTransform.anchoredPosition = Vector2.MoveTowards(_rectTransform.anchoredPosition, targetPos, speed);

            if (targetPos == _rectTransform.anchoredPosition)
            {
                if (targetPiece != null)
                {
                    Destroy(targetPiece.gameObject);

                    UpdateColorsAndText();
                }

                _targetPositions.Dequeue();
            }
        }
    }

    public void MoveTo(Vector2 boardPos)
    {
        _targetPositions.Enqueue(new Action(boardPos, null));
    }

    public void Merge(Piece other, Vector2 boardPos)
    {
        // This is the logical value. Can be separated with another matrix.
        value = value + other.value;
        _targetPositions.Enqueue(new Action(boardPos, other));
    }

    private void UpdateColorsAndText()
    {
        int log = (int)Mathf.Log(value, 2);
        Color backgroundColor = bgColors[Mathf.Clamp(log - 1, 0, bgColors.Length - 1)];
        Color fontColor = fontColors[Mathf.Clamp(log - 1, 0, fontColors.Length - 1)];

        GetComponent<Image>().color = backgroundColor;

        Text text = GetComponentInChildren<Text>();
        text.text = value.ToString();
        text.color = fontColor;
    }
}