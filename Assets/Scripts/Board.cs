using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Serialization;

public class Board : MonoBehaviour
{
    public GameObject piece;

    public RectTransform corner;

    public Color[] pieceBackgroundColors;
    public Color[] pieceFontColors;

    public int gridThickness;
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

        var first = Instantiate(piece, transform);
        var second = Instantiate(piece, transform);

        var firstGridPos = new Vector2(Random.Range(0, gridWidth), Random.Range(0, gridHeight));

        var secondGridPos = firstGridPos;
        while (secondGridPos == firstGridPos)
        {
            secondGridPos = new Vector2(Random.Range(0, gridWidth), Random.Range(0, gridHeight));
        }

        var firstAnchoredPos = GridPosToBoardPos(firstGridPos);
        var secondAnchoredPos = GridPosToBoardPos(secondGridPos);

        first.GetComponent<RectTransform>().anchoredPosition = firstAnchoredPos;
        second.GetComponent<RectTransform>().anchoredPosition = secondAnchoredPos;
    }

    private Vector2 GridPosToBoardPos(Vector2 gridPos)
    {
        return _cornerCenterPos +
               Vector2.Scale(gridPos, _pieceSize + new Vector2(gridThickness, gridThickness));
    }
}