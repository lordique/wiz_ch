using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessSquare : MonoBehaviour
{
    public string col;
    public int row;

    public void Click() {
        Debug.Log("clicked square at" + col + row.ToString());
    }
}
