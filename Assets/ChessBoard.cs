using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChessBoard : MonoBehaviour
{

    public ChessBoard() { }

    public List<MoveLog> MovesLog;
    public List<Piece> WhitePieces;
    public List<Piece> BlackPieces;
    private ChessAPI ChessAPI = new ChessAPI();
    private Dictionary<(string, int), ChessSquare> Squares = new Dictionary<(string, int), ChessSquare>();
    private bool IsWhiteTurn = true;
    public bool IsPlayerWhite = true;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in GetComponentsInChildren<Transform>())
        {
            ChessSquare sq = child.gameObject.GetComponent<ChessSquare>();
            if (sq != null)
            {
                Debug.Log("Getting square . . ."+sq.col+ sq.row);
                Squares.Add((sq.col, sq.row), sq);
            }
        }

        if (IsPlayerWhite){
            foreach (Piece piece in WhitePieces)
            { 
                piece.MoveOptions = getMoveOptions(piece);
            }
        } else
        {
            foreach (Piece piece in BlackPieces)
            {
                piece.MoveOptions = getMoveOptions(piece);
            }
        }
     
    }


    private List<ChessSquare> getMoveOptions(Piece piece)
    {
        string fromPosition = piece.currentPosition.col + piece.currentPosition.row.ToString();
        List<string> moveOptions = ChessAPI.GetMoveOptions(fromPosition).moves;
        Debug.Log(fromPosition);
        Debug.Log(moveOptions);
        var moveOptionSquares = from x in moveOptions select Squares[(x[0].ToString(),(int)x[1])];
        return (List<ChessSquare>)moveOptionSquares;

    }

    public void determineCollision(Piece piece1, Piece piece2)
    {
        bool piece1Black = BlackPieces.Contains(piece1);
        if ((IsWhiteTurn && piece1Black) || (!IsWhiteTurn && !piece1Black))
        {
            piece1.Destroy();
        } else {
            piece2.Destroy();
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public class MoveLog : System.Object { }
