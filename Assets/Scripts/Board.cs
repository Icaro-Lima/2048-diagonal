using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Board : MonoBehaviour
{
    public GameObject piece;

    public RectTransform corner;

    public Color[] pieceBackgroundColors;
    public Color[] pieceFontColors;

    public int gridWidth;
    public int gridHeight;

    private Vector2 _pieceSize;
    private Vector2 _cornerCenterPos;

    private GameObject[,] _grid;

    // Start is called before the first frame update
    private void Start()
    {
        _pieceSize = corner.sizeDelta;
        _cornerCenterPos = corner.anchoredPosition;

        _grid = new GameObject[gridWidth, gridHeight];
    }
}