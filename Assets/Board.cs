using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public RectTransform corner;

    public Color[] colors;

    private Vector2 _pieceSize;
    private Vector2 _cornerCenterPos;

    // Start is called before the first frame update
    private void Start()
    {
        _pieceSize = corner.sizeDelta;
        _cornerCenterPos = corner.anchoredPosition;
    }
}