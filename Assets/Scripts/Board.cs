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
                MovePieceToDir(new Vector2Int(act.x, act.y), new Vector2Int(dirx, diry));
            }

            q.Enqueue(new Vector2Int(act.x - dirx, act.y - diry));
            q.Enqueue(new Vector2Int(act.x, act.y - diry));
            q.Enqueue(new Vector2Int(act.x - dirx, act.y));
        }
    }

    private void MovePieceToDir(Vector2Int pos, Vector2Int dir)
    {
        Vector2Int iniPos = pos;
        while (IsOnGridBounds(pos + dir) && _grid[pos.x + dir.x, pos.y + dir.y] == null)
        {
            pos += dir;
        }

        if (!IsOnGridBounds(pos + dir))
        {
            if (iniPos.x != pos.x || iniPos.y != pos.y)
            {
                MovePieceTo(_grid[iniPos.x, iniPos.y], pos);
                _grid[pos.x, pos.y] = _grid[iniPos.x, iniPos.y];
                _grid[iniPos.x, iniPos.y] = null;
            }
        }
        else
        {
            Piece my = _grid[iniPos.x, iniPos.y];
            Piece other = _grid[pos.x + dir.x, pos.y + dir.y];

            if (other.value == my.value)
            {
                MergePiece(my, other, pos + dir);
                _occupiedSlots--;
                _grid[pos.x + dir.x, pos.y + dir.y] = my;
                _grid[iniPos.x, iniPos.y] = null;
            }
            else
            {
                if (iniPos.x != pos.x || iniPos.y != pos.y)
                {
                    MovePieceTo(my, pos);
                    _grid[pos.x, pos.y] = my;
                    _grid[iniPos.x, iniPos.y] = null;
                }
            }
        }
    }

    private bool IsOnGridBounds(Vector2Int pos)
    {
        return pos.x >= 0 && pos.y >= 0 && pos.x < _grid.GetLength(0) && pos.y < _grid.GetLength(1);
    }

    private void MovePieceTo(Piece piece, Vector2Int pos)
    {
        Vector2 boardPos = GridPosToBoardPos(pos);

        piece.MoveTo(boardPos);
    }

    private void MergePiece(Piece piece, Piece other, Vector2Int pos)
    {
        Vector2 boardPos = GridPosToBoardPos(pos);

        piece.Merge(other, boardPos);
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