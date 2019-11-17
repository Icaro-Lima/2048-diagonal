using UnityEngine;

public class Board : MonoBehaviour
{
    public GameObject piecePrefab;

    public RectTransform corner;

    public int boardGridThickness;

    public int gridRows;
    public int gridColumns;

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
        _grid = new Piece[gridColumns, gridRows];

        SpawnPiece2Random();
        SpawnPiece2Random();
    }

    private void SpawnPiece2Random()
    {
        int slots = _grid.GetLength(0) * _grid.GetLength(1);

        int freeSlots = slots - _occupiedSlots;

        float chance = 1.0f / freeSlots;
        float accChance = chance;

        bool spawned = false;
        for (int x = 0; x < _grid.GetLength(0); x++)
        {
            for (int y = 0; y < _grid.GetLength(1); y++)
            {
                if (_grid[x, y] != null)
                {
                    continue;
                }

                if (Random.value < accChance)
                {
                    SpawnPiece(new Vector2Int(x, y), 2);
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
        Piece piece = Instantiate(piecePrefab, transform).GetComponent<Piece>();

        Vector2 boardPos = GridPosToBoardPos(gridPos);
        piece.Init(value, boardPos);

        _grid[gridPos.x, gridPos.y] = piece;

        _occupiedSlots++;
    }

    private Vector2 GridPosToBoardPos(Vector2Int gridPos)
    {
        return _cornerCenterPos +
               Vector2.Scale(gridPos, _pieceSize + new Vector2(boardGridThickness, boardGridThickness));
    }
}