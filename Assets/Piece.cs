using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
public interface Piece
{

    ChessSquare currentPosition
    {
        get;
        set;
    }

    void Move(ChessSquare newSquare);

    List<ChessSquare> MoveOptions();
}
*/

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
        int row_change = currentPosition.row - newPosition.row;
        int col_change = DistMap[currentPosition.col] - DistMap[newPosition.col];
        gameObject.transform.position = new Vector3(transform.position.x + SquareSpacing * col_change, transform.position.y, transform.position.z + SquareSpacing * row_change);
        currentPosition = newPosition;
    }

    public void Destroy()
    {
        // needs to be idempotent
        gameObject.SetActive(false);
        InPlay = false;
    }

    public void Click()
    {
        Destroy();
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

