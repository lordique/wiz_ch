using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessSquare : MonoBehaviour
{
    public string col;
    public int row;
    private Color color;
    private ChessBoard chessBoard;

    private void Start()
    {
        color = gameObject.GetComponent<Renderer>().material.color;
    }

    public void RegisterBoard(ChessBoard board)
    {
        chessBoard = board;
    }

    public void Click() {
        if (chessBoard.selectedPiece != null) // TODO && chessBoard.selectedPiece.MoveOptions.Contains(this))
        {
            chessBoard.PlayerMove(chessBoard.selectedPiece, this);
        }
    }

    public void Highlight(Color hightlightColor)
    {
            gameObject.GetComponent<Renderer>().material.color = hightlightColor;
    }

    public void UnHighlight()
    {
        gameObject.GetComponent<Renderer>().material.color = color;
    }

    public void LaserHighlightIfShould()
    {
        if (chessBoard.selectedPiece != null)
        {
            if (true) // TODO (chessBoard.selectedPiece.MoveOptions.Contains(this))
            {
                Highlight(Color.blue);
            }
        }
    }

    public void LaserUnHighlightIfShould()
    {
        if (chessBoard.selectedPiece != null)
        {
            if (chessBoard.selectedPiece.currentPosition != this)
            {
                UnHighlight();
            }
        }
    }


}
