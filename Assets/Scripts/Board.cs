using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Board : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
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

    private Vector2 _beginDragPos;

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

    public void OnBeginDrag(PointerEventData eventData)
    {
        _beginDragPos = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Vector2 diff = eventData.position - _beginDragPos;

        MoveBoardToDir(diff.x >= 0 ? 1 : -1, diff.y >= 0 ? 1 : -1);
        SpawnPiece2Random();
    }

    private void MoveBoardToDir(int dirx, int diry)
    {
        Queue<Vector2Int> q = new Queue<Vector2Int>();

        if (dirx == 1 && diry == 1)
        {
            q.Enqueue(new Vector2Int(_grid.GetLength(0) - 1, _grid.GetLength(1) - 1));
        }
        else if (dirx == -1 && diry == 1)
        {
            q.Enqueue(new Vector2Int(0, _grid.GetLength(1) - 1));
        }
        else if (dirx == -1 && diry == -1)
        {
            q.Enqueue(new Vector2Int(0, 0));
        }
        else
        {
            q.Enqueue(new Vector2Int(_grid.GetLength(0) - 1, 0));
        }

        int idx = 0;
        int[,] order = new int[_grid.GetLength(0), _grid.GetLength(1)];
        bool[,] visited = new bool[_grid.GetLength(0), _grid.GetLength(1)];
        while (q.Count > 0)
        {
            Vector2Int act = q.Dequeue();

            if (act.x < 0 || act.y < 0 || act.x >= _grid.GetLength(0) || act.y >= _grid.GetLength(1))
            {
                continue;
            }

            if (visited[act.x, act.y])
            {
                continue;
            }

            order[act.x, act.y] = idx++;

            visited[act.x, act.y] = true;

            if (_grid[act.x, act.y] != null)
            {
                MovePieceToDir(act.x, act.y, dirx, diry);
            }

            q.Enqueue(new Vector2Int(act.x - dirx, act.y - diry));
            q.Enqueue(new Vector2Int(act.x, act.y - diry));
            q.Enqueue(new Vector2Int(act.x - dirx, act.y));
        }
    }

    private void MovePieceToDir(int x, int y, int dirx, int diry)
    {
        int initialx = x;
        int initialy = y;
        while (x + dirx >= 0 && x + dirx < _grid.GetLength(0) &&
               y + diry >= 0 && y + diry < _grid.GetLength(1) &&
               _grid[x + dirx, y + diry] == null)
        {
            x += dirx;
            y += diry;
        }

        if (x + dirx < 0 || x + dirx >= _grid.GetLength(0) ||
            y + diry < 0 || y + diry >= _grid.GetLength(1))
        {
            if (initialx != x || initialy != y)
            {
                MovePieceTo(_grid[initialx, initialy], x, y);
                _grid[x, y] = _grid[initialx, initialy];
                _grid[initialx, initialy] = null;
            }
        }
        else
        {
            print("Parou de mover por causa de uma peça.");
        }
    }

    private void MovePieceTo(Piece piece, int x, int y)
    {
        Vector2 boardPos = GridPosToBoardPos(new Vector2Int(x, y));

        piece.MoveTo(boardPos);
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