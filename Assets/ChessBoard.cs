﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class ChessBoard : MonoBehaviour
{

    public ChessBoard() { }

    public List<Piece> WhitePieces;
    public List<Piece> BlackPieces;
    private ChessAPI ChessAPI;
    private Dictionary<(string, int), ChessSquare> Squares = new Dictionary<(string, int), ChessSquare>();
    private bool IsWhiteTurn = true;
    public bool IsPlayerWhite = true;
    public Piece selectedPiece = null;

    // Start is called before the first frame update
    void Start()
    {
        ChessAPI = new ChessAPI();
        foreach (Transform child in GetComponentsInChildren<Transform>())
        {
            ChessSquare sq = child.gameObject.GetComponent<ChessSquare>();
            if (sq != null)
            {
                sq.RegisterBoard(this);
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
        ChessAPI.AddMove(fromPosition, toPosition);
        piece.Move(newPosition);
        IsWhiteTurn = !IsWhiteTurn;
        piece.MoveOptions = getMoveOptions(piece);
        ChessAPI.RequestAiMove();
    }

    public void ProcessAIMove(string fromPosition, string toPosition)
    {
        Debug.Log("Received move from API! " + fromPosition + toPosition);
        Piece piece = GetPieceFromSquare(false, fromPosition);
        
        ChessSquare newPosition = Squares[(toPosition[0].ToString(), int.Parse(toPosition[1].ToString()))];
        piece.Move(newPosition);
               
        IsWhiteTurn = !IsWhiteTurn;
        selectedPiece = null;
    
       
    }

    private Piece GetPieceFromSquare(bool isPlayerPiece, string position)
    {
        ChessSquare positionSquare = Squares[(position[0].ToString(), int.Parse(position[1].ToString()))];
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
        return null; //todo
    }

    public void DetermineCollision(Piece piece1, Piece piece2)
    {
        Debug.Log("determining collision");
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
        if (ChessAPI.AIMove != null)
        {
            ProcessAIMove(ChessAPI.AIMove.from, ChessAPI.AIMove.to);
            ChessAPI.AIMove = null;
        }
    }
}

