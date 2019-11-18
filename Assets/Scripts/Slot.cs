using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot
{
    public Piece piece;
    public int value;

    public Slot(Piece piece, int value)
    {
        this.piece = piece;
        this.value = value;
    }
}
