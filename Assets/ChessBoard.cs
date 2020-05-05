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
    private ChessAPI ChessAPI;
    private Dictionary<(string, int), ChessSquare> Squares = new Dictionary<(string, int), ChessSquare>();
    private bool IsWhiteTurn = true;
    public bool IsPlayerWhite = true;

    // Start is called before the first frame update
    void Start()
    {
        ChessAPI = new ChessAPI();
        foreach (Transform child in GetComponentsInChildren<Transform>())
        {
            ChessSquare sq = child.gameObject.GetComponent<ChessSquare>();
            if (sq != null)
            {
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

    public void PlayerMove(Piece piece, ChessSquare newPosition)
    {
        string fromPosition = piece.currentPosition.col + piece.currentPosition.row.ToString();
        string toPosition = newPosition.col + newPosition.row.ToString();
        ChessAPI.PlayerMove(fromPosition, toPosition);
        piece.Move(newPosition);
        IsWhiteTurn = !IsWhiteTurn;
        piece.MoveOptions = getMoveOptions(piece);
    }

    private void AIMove()
    {
        AIMoveResponse aIMoveResponse = ChessAPI.AiMove();
        Piece piece = GetPieceFromSquare(false, aIMoveResponse.from);
        piece.Move(Squares[(aIMoveResponse.to[0].ToString(), Int32.Parse(aIMoveResponse.to[1].ToString()))]);
        IsWhiteTurn = !IsWhiteTurn;
    }

    private Piece GetPieceFromSquare(bool isPlayerPiece, string position)
    {
        ChessSquare positionSquare = Squares[(position[0].ToString(), Int32.Parse(position[1].ToString()))];
        if ((isPlayerPiece && IsPlayerWhite) || (!isPlayerPiece && !IsPlayerWhite)) {
            foreach (Piece piece in WhitePieces)
            {
                if (piece.currentPosition == positionSquare) {
                    return piece;
                } 
            }
            
        } else {
            foreach (Piece piece in BlackPieces)
            {
                if (piece.currentPosition == positionSquare)
                {
                    return piece;
                }
            }
        }
        return null;
    }

    private List<ChessSquare> getMoveOptions(Piece piece)
    {
        string fromPosition = piece.currentPosition.col + piece.currentPosition.row.ToString();
        List<string> moveOptions = ChessAPI.GetMoveOptions(fromPosition).moves;
        List<ChessSquare> moveOptionSquares = new List<ChessSquare>();
        foreach (string option in moveOptions)
        {
            var key = (option[0].ToString(), Int32.Parse(option[1].ToString()));
            moveOptionSquares.Add(Squares[key]);
        }
        return moveOptionSquares;

    }

    public void DetermineCollision(Piece piece1, Piece piece2)
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
