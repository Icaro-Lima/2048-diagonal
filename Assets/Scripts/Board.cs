using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Board : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public GameObject piecePrefab;

    public RectTransform corner;

    public int boardGridThickness;

    public int gridRows;
    public int gridColumns;

    public PieceMergedEvent pieceMergedEvent;
    public UnityEvent onGameOver;

    private Vector2 _pieceSize;
    private Vector2 _cornerCenterPos;

    private Slots _slots;

    private Vector2 _beginDragPos;

    private bool ignoreNextDrag;

    private void Awake()
    {
        if (pieceMergedEvent == null)
        {
            pieceMergedEvent = new PieceMergedEvent();
        }

        if (onGameOver == null)
        {
            onGameOver = new UnityEvent();
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
        _pieceSize = corner.sizeDelta;
        _cornerCenterPos = corner.anchoredPosition;


        _slots = new Slots(gridColumns, gridRows);

        SpawnPiece2Random();
        SpawnPiece2Random();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _beginDragPos = eventData.position;

        ignoreNextDrag = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (ignoreNextDrag)
        {
            return;
        }

        int dragThreshold = EventSystem.current.pixelDragThreshold;
        dragThreshold = 4 * Mathf.Max(dragThreshold,
                                      (int)(dragThreshold * Screen.dpi / 160f));

        Vector2 delta = eventData.position - _beginDragPos;

        if (delta.magnitude < dragThreshold)
        {
            return;
        }

        ignoreNextDrag = true;

        MoveBoard(delta);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (ignoreNextDrag)
        {
            return;
        }

        Vector2 delta = eventData.position - _beginDragPos;

        MoveBoard(delta);
    }

    private void MoveBoard(Vector2 delta)
    {
        bool CheckGameOver()
        {
            bool gameOver = true;
            for (int x = 0; x < _slots.size.x; x++)
            {
                for (int y = 0; y < _slots.size.y; y++)
                {
                    if (_slots.CanMove(new Vector2Int(x, y)))
                    {
                        gameOver = false;
                        break;
                    }
                }

                if (!gameOver)
                {
                    break;
                }
            }

            return gameOver;
        }

        bool movedAnyPiece = MoveBoardToDir(delta.x >= 0 ? 1 : -1, delta.y >= 0 ? 1 : -1);

        if (movedAnyPiece)
        {
            SpawnPiece2Random();
        }

        if (CheckGameOver())
        {
            onGameOver.Invoke();
        }
    }

    private bool MoveBoardToDir(int dirx, int diry)
    {
        bool movedAnyPiece = false;

        Queue<Vector2Int> q = new Queue<Vector2Int>();

        if (dirx == 1 && diry == 1)
        {
            q.Enqueue(_slots.size - new Vector2Int(1, 1));
        }
        else if (dirx == -1 && diry == 1)
        {
            q.Enqueue(new Vector2Int(0, _slots.size.y - 1));
        }
        else if (dirx == -1 && diry == -1)
        {
            q.Enqueue(new Vector2Int(0, 0));
        }
        else
        {
            q.Enqueue(new Vector2Int(_slots.size.x - 1, 0));
        }

        bool[,] merged = new bool[_slots.size.x, _slots.size.y];

        bool[,] visited = new bool[_slots.size.x, _slots.size.y];
        while (q.Count > 0)
        {
            Vector2Int act = q.Dequeue();

            if (act.x < 0 || act.y < 0 || act.x >= _slots.size.x || act.y >= _slots.size.y)
            {
                continue;
            }

            if (visited[act.x, act.y])
            {
                continue;
            }

            visited[act.x, act.y] = true;

            if (_slots[act] != null)
            {
                bool movedThisPiece = MovePieceToDir(new Vector2Int(act.x, act.y), new Vector2Int(dirx, diry), ref merged);

                if (movedThisPiece)
                {
                    movedAnyPiece = true;
                }
            }

            q.Enqueue(new Vector2Int(act.x - dirx, act.y - diry));
            q.Enqueue(new Vector2Int(act.x, act.y - diry));
            q.Enqueue(new Vector2Int(act.x - dirx, act.y));
        }

        return movedAnyPiece;
    }

    private bool MovePieceToDir(Vector2Int pos, Vector2Int dir, ref bool[,] merged)
    {
        bool pieceMoved = false;

        Vector2Int iniPos = pos;
        while (_slots.IsInside(pos + dir) && _slots[pos + dir] == null)
        {
            pos += dir;
        }

        if (!_slots.IsInside(pos + dir))
        {
            if (iniPos.x != pos.x || iniPos.y != pos.y)
            {
                pieceMoved = true;

                MovePieceTo(_slots[iniPos].piece, pos);
                _slots[pos] = _slots[iniPos];
                _slots[iniPos] = null;
            }
        }
        else
        {
            Slot my = _slots[iniPos];
            Slot other = _slots[pos + dir];

            if (other.value == my.value && !merged[pos.x + dir.x, pos.y + dir.y])
            {
                pieceMoved = true;

                MergePiece(my.piece, other.piece, pos + dir);
                pieceMergedEvent.Invoke(my.value, other.value);
                my.value += other.value;
                _slots[pos + dir] = my;
                merged[pos.x + dir.x, pos.y + dir.y] = true;
                _slots[iniPos] = null;
            }
            else
            {
                if (iniPos.x != pos.x || iniPos.y != pos.y)
                {
                    pieceMoved = true;

                    MovePieceTo(my.piece, pos);
                    _slots[pos] = my;
                    _slots[iniPos] = null;
                }
            }
        }

        return pieceMoved;
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
        int freeSlots = _slots.freeSlots;

        float chance = 1.0f / freeSlots;
        float accChance = chance;

        bool spawned = false;
        for (int x = 0; x < _slots.size.x; x++)
        {
            for (int y = 0; y < _slots.size.y; y++)
            {
                if (_slots[x, y] != null)
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

        _slots[gridPos] = new Slot(piece, value);
    }

    private Vector2 GridPosToBoardPos(Vector2Int gridPos)
    {
        return _cornerCenterPos +
               Vector2.Scale(gridPos, _pieceSize + new Vector2(boardGridThickness, boardGridThickness));
    }
}