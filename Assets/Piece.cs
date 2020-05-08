using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Piece : MonoBehaviour
{
    public ChessSquare startingPosition;
    public ChessSquare currentPosition;
    public ChessBoard chessBoard;
    public List<ChessSquare> MoveOptions;
    public bool InPlay = true;

    private Dictionary<string, int> DistMap = new Dictionary<string, int> {
            {"a", 1}, {"b", 2}, {"c", 3}, {"d", 4}, {"e", 5}, {"f", 6}, {"g", 7}, {"h", 8}
        };
    private int SquareSpacing = 2;

    void Start()
    {
        chessBoard = startingPosition.GetComponentsInParent<ChessBoard>()[0];
    }

    public void Move(ChessSquare newPosition)
    {
        int row_change = newPosition.row - currentPosition.row;
        int col_change = DistMap[newPosition.col] - DistMap[currentPosition.col];
        gameObject.transform.position = new Vector3(transform.position.x + SquareSpacing * col_change, transform.position.y, transform.position.z + SquareSpacing * row_change);
        currentPosition.UnHighlight();
        currentPosition = newPosition;
    }

    public void Destroy()
    {
        // needs to be idempotent
        Debug.Log("Destroy! "+currentPosition.col.ToString() +currentPosition.row.ToString());
        gameObject.SetActive(false);
        InPlay = false;
    }

    public void Click()
    {
        chessBoard.selectedPiece = this;
        currentPosition.Highlight(Color.red);
    }

    public void Highlight()
    {
        if (chessBoard.selectedPiece == null) // TODO && MoveOptions.Count > 0)
        {
            currentPosition.Highlight(Color.blue);
        }
    }

    public void UnHighlight()
    {
        if (chessBoard.selectedPiece == null) // TODO && MoveOptions.Count > 0)
        {
            currentPosition.UnHighlight();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision!");
        Piece otherPiece = collision.gameObject.GetComponent<Piece>();
        if (otherPiece != null)
        {
            chessBoard.DetermineCollision(otherPiece, this);
        }
    }


}

