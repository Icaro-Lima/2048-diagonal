using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slots
{
    public int freeSlots { get; private set; }

    public Vector2Int size
    {
        get { return new Vector2Int(_slots.GetLength(0), _slots.GetLength(1)); }
    }

    private readonly Slot[,] _slots;

    public Slots(int width, int height)
    {
        _slots = new Slot[width, height];

        freeSlots = width * height;
    }

    public Slot this[Vector2Int position]
    {
        get
        {
            return _slots[position.x, position.y];
        }

        set
        {
            if (_slots[position.x, position.y] == null && value != null)
            {
                freeSlots--;
            }
            else if (_slots[position.x, position.y] != null && value == null)
            {
                freeSlots++;
            }

            _slots[position.x, position.y] = value;
        }
    }

    public Slot this[int x, int y]
    {
        get
        {
            return _slots[x, y];
        }

        set
        {
            if (_slots[x, y] == null && value != null)
            {
                freeSlots--;
            }
            else if (_slots[x, y] != null && value == null)
            {
                freeSlots++;
            }

            _slots[x, y] = value;
        }
    }

    public bool IsInside(Vector2Int pos)
    {
        return pos.x >= 0 && pos.y >= 0 && pos.x < size.x && pos.y < size.y;
    }
}
