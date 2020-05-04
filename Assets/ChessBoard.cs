using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessBoard : MonoBehaviour
{

    public ChessBoard() {}

    public List<MoveLog> movesLog;
    public List<Piece> pieces;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public class MoveLog : Object { }

public interface Piece {

    ChessSquare currentPosition
    {
        get;
        set;
    }

    void Move(ChessSquare newSquare);

    List<ChessSquare> MoveOptions();
}