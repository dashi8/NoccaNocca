using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PiecesBoard;

namespace MoveGenerator
{
    public class MyMove : AMoveGenerator
    {
        public override Point? GetInputPoint()
        {
            //null: クリックされてない
            //clickedNonIMyInputObject: クリックされたがIMyInputでない
            //otherwise: IMyInputがクリックされた
            Point? p = null;

            if (Input.GetMouseButtonUp(0))
            {
                GameObject clickedGameObject = GetClickedGameObject();
                if (clickedGameObject != null && clickedGameObject.GetComponent<IMyInput>() != null)
                {
                    IMyInput clickedPieceScript = clickedGameObject.GetComponent<IMyInput>();
                    p = clickedPieceScript.GetPoint();
                }
                else
                {
                    p = clickedNonIMyInputObject;
                }
            }

            return p;
        }

        GameObject GetClickedGameObject()
        {
            GameObject clickedGameObject = null;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit = new RaycastHit();

            if (Physics.Raycast(ray, out hit))
            {
                clickedGameObject = hit.collider.gameObject;
            }
            return clickedGameObject;
        }
    }
}
