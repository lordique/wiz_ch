using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Valve.VR.Extras;
using System.Collections.Generic;

public class LaserController : MonoBehaviour

{

    private SteamVR_LaserPointer laserPointer;

   // private SteamVR_TrackedController trackedController;

        

    private void OnEnable()

    {

        laserPointer = GetComponent<SteamVR_LaserPointer>();

        laserPointer.PointerIn -= HandlePointerIn;

        laserPointer.PointerIn += HandlePointerIn;

        laserPointer.PointerOut -= HandlePointerOut;

        laserPointer.PointerOut += HandlePointerOut;

        laserPointer.PointerClick -= HandlePointerClick;

        laserPointer.PointerClick += HandlePointerClick;

    }




    private void HandlePointerIn(object sender, PointerEventArgs e)
    {
        if (e.target.gameObject.GetComponent<Piece>() != null)
        {
            e.target.gameObject.GetComponent<Piece>().Highlight();
        }
        else if (e.target.gameObject.GetComponent<ChessSquare>() != null)
        {
            e.target.gameObject.GetComponent<ChessSquare>().LaserHighlightIfShould();
        }
    }

    private void HandlePointerClick(object sender, PointerEventArgs e)
    {
        if (e.target.gameObject.GetComponent<Piece>() != null)
        {
            e.target.gameObject.GetComponent<Piece>().Click();
        } else if (e.target.gameObject.GetComponent<ChessSquare>() != null)
        {
            e.target.gameObject.GetComponent<ChessSquare>().Click();
        }
    }


    private void HandlePointerOut(object sender, PointerEventArgs e)
    {
        if (e.target.gameObject.GetComponent<Piece>() != null)
        {
            e.target.gameObject.GetComponent<Piece>().UnHighlight();
        }
        else if (e.target.gameObject.GetComponent<ChessSquare>() != null)
        {
            e.target.gameObject.GetComponent<ChessSquare>().LaserUnHighlightIfShould();
        }
    }


}