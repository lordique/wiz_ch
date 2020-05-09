using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Piece : MonoBehaviour
{
    public ChessSquare startingPosition;
    public ChessSquare currentPosition;
    private Vector3 desiredCoordinates;
    private float speed = 0.1f;
    public ChessBoard chessBoard;
    public List<ChessSquare> MoveOptions;
    public bool InPlay = true;
    private bool isMoving = false;

    private Dictionary<string, int> DistMap = new Dictionary<string, int> {
            {"a", 1}, {"b", 2}, {"c", 3}, {"d", 4}, {"e", 5}, {"f", 6}, {"g", 7}, {"h", 8}
        };
    private int SquareSpacing = 2;

    void Start()
    {
        desiredCoordinates = gameObject.transform.position;
        chessBoard = startingPosition.GetComponentsInParent<ChessBoard>()[0];
    }

    public void Move(ChessSquare newPosition)
    {
        int row_change = newPosition.row - currentPosition.row;
        int col_change = DistMap[newPosition.col] - DistMap[currentPosition.col];
        desiredCoordinates = new Vector3(SquareSpacing * col_change, 0, SquareSpacing * row_change)+ desiredCoordinates;
        currentPosition.UnHighlight();
        currentPosition = newPosition;
        isMoving = true;
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

    void OnTriggerEnter(Collider collider)
    {
        Debug.Log("Trigger overlap");
        Piece otherPiece = collider.gameObject.GetComponent<Piece>();
        if (otherPiece != null && isMoving)
        {
            otherPiece.Destroy();
        }
    }

    void OnCollisionEnter(Collision collider)
    {
        Debug.Log("Collision overlap");
        Piece otherPiece = collider.gameObject.GetComponent<Piece>();
        if (otherPiece != null && isMoving)
        {
            otherPiece.Destroy();
        }
    }

    void Update()
    {
        if (isMoving)
        {
            if ((gameObject.transform.position - desiredCoordinates).sqrMagnitude > 0.05)
            {
                Vector3 diff = desiredCoordinates - gameObject.transform.position;
                gameObject.transform.position = speed * (new Vector3(Math.Sign(diff.x), Math.Sign(diff.y), Math.Sign(diff.z))) + gameObject.transform.position;
            } else
            {
                isMoving = false;
                chessBoard.moveNextPiece();
            }
        }
    }


}

