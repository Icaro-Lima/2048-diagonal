using UnityEngine;

public class Board : MonoBehaviour
{
    public GameObject piecePrefab;

    public RectTransform corner;

    public Color[] pieceBackgroundColors;
    public Color[] pieceFontColors;

    public int gridThickness;
    public int gridWidth;
    public int gridHeight;

    private Vector2 _pieceSize;
    private Vector2 _cornerCenterPos;

    private int _occupiedSlots;
    private Piece[,] _grid;

    // Start is called before the first frame update
    private void Start()
    {
        _pieceSize = corner.sizeDelta;
        _cornerCenterPos = corner.anchoredPosition;

        _occupiedSlots = 0;
        _grid = new Piece[gridWidth, gridHeight];

        SpawnPiece2Random();
        SpawnPiece2Random();
    }

    private void SpawnPiece2Random()
    {
        var slots = _grid.GetLength(0) * _grid.GetLength(1);

        var freeSlots = slots - _occupiedSlots;

        var chance = 1.0f / freeSlots;
        var accChance = chance;

        var spawned = false;
        for (var x = 0; x < _grid.GetLength(0); x++)
        {
            for (var y = 0; y < _grid.GetLength(1); y++)
            {
                if (_grid[x, y] != null)
                {
                    continue;
                }

                if (Random.value < accChance)
                {
                    SpawnPiece(new Vector2Int(x, y), 8);
                    spawned = true;
                    break;
                }

                accChance += chance;
            }

            if (spawned)
            {
                break;
            }
        }
    }

    private void SpawnPiece(Vector2Int gridPos, int value)
    {
        var piece = Instantiate(piecePrefab, transform).GetComponent<Piece>();

        var boardPos = GridPosToBoardPos(gridPos);
        piece.Init(value, boardPos);

        _grid[gridPos.x, gridPos.y] = piece;

        _occupiedSlots++;
    }

    private Vector2 GridPosToBoardPos(Vector2Int gridPos)
    {
        return _cornerCenterPos +
               Vector2.Scale(gridPos, _pieceSize + new Vector2(gridThickness, gridThickness));
    }
}