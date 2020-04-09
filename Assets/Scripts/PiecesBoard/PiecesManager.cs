using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NOCCARule;
using System;
using UnityEngine.SceneManagement;

public struct Point
{
    public int x;
    public int z;
    public Point(int x, int z)
    {
        this.x = x;
        this.z = z;
    }

    public static bool operator ==(Point p1, Point p2)
    {
        return (p1.x==p2.x && p1.z==p2.z);
    }
    public static bool operator !=(Point p1, Point p2)
    {
        return !(p1 == p2);
    }
    public static Point operator +(Point p1, Point p2)
    {
        return new Point(p1.x+p2.x,p1.z+p2.z);
    }
}

namespace PiecesBoard
{
    interface IMyInput
    {
        Point GetPoint();
    }

    class Point2Coord
    {
        static private float masuPotiDelta = 2f;

        static public Vector3 point2coord(Point p, float y)
        {
            if (p == NOCCACore.MyGoalPoint)
            {
                return new Vector3((NOCCACore.XRANGE - 2.5f) * masuPotiDelta, y, 0);
            }
            else if (p == NOCCACore.OppGoalPoint)
            {
                return new Vector3( -3.5f * masuPotiDelta, y, 0);
            }
            else
            {
                return new Vector3((p.x - 2.5f) * masuPotiDelta, y, (p.z - 2f) * masuPotiDelta);
            }
        }
    }


    public class PiecesManager : MonoBehaviour
    {
        NOCCACore nocca = new NOCCACore();
        GameState gameState;

        PieceScript selectedPieceScript;
        Point[] canMovePoints;

        enum GameState
        {
            noinited,
            waiteForSlectingPiece,
            waiteForMovingPoint,
            gameover
        }


        // Start is called before the first frame update
        void Start()
        {
            gameState = GameState.noinited;
            selectedPieceScript = null;
            canMovePoints = new Point[]{ };
        }

        // Update is called once per frame
        void Update()
        {
            switch (gameState)
            {
                case GameState.noinited:
                    //Start()で呼ぶとなぜかfindできない
                    InitUnirx();
                    break;
                case GameState.waiteForSlectingPiece:
                    SelectPiece();
                    break;
                case GameState.waiteForMovingPoint:
                    SelectMovePoint();
                    break;
                case GameState.gameover:
                    GameOverProcess();
                    break;
                default:
                    break;
            }
        }

        void GameOverProcess()
        {
            Debug.Log("Gameover!");
            SceneManager.LoadScene("MenuScene");
        }

        void SelectPiece()
        {
            //Debug.Log(nocca.isMyTurn.Value.GetType().FullName);
            if (nocca.isMyturn.Value)
            {
                //自分のターンの時
                if (Input.GetMouseButtonUp(0))
                {
                    GameObject clickedGameObject = GetClickedGameObject();
                    if (clickedGameObject != null && clickedGameObject.GetComponent<PieceScript>() != null)
                    {
                        PieceScript clickedPieceScript = clickedGameObject.GetComponent<PieceScript>();
                        Point inputPoint = clickedPieceScript.GetPoint();
                        canMovePoints = nocca.CanMovePoints(inputPoint);
                        if (canMovePoints.Length > 0)
                        {
                            //移動できる駒が選択された
                            gameState = GameState.waiteForMovingPoint;
                            selectedPieceScript = clickedPieceScript;
                            selectedPieceScript.isSelected = true;
                        }
                    }
                }
            }
            else
            {
                //相手のターンの時
                //とりあえずコピペ
                if (Input.GetMouseButtonUp(0))
                {
                    GameObject clickedGameObject = GetClickedGameObject();
                    if (clickedGameObject != null && clickedGameObject.GetComponent<PieceScript>() != null)
                    {
                        PieceScript clickedPieceScript = clickedGameObject.GetComponent<PieceScript>();
                        Point inputPoint = clickedPieceScript.GetPoint();
                        canMovePoints = nocca.CanMovePoints(inputPoint);
                        if (canMovePoints.Length > 0)
                        {
                            //移動できる駒が選択された
                            gameState = GameState.waiteForMovingPoint;
                            selectedPieceScript = clickedPieceScript;
                            selectedPieceScript.isSelected = true;
                        }
                    }
                }
            }
        }

        void SelectMovePoint()
        {
            if (Input.GetMouseButtonUp(0))
            {
                GameObject clickedGameObject = GetClickedGameObject();
                if (clickedGameObject != null && clickedGameObject.GetComponent<IMyInput>() != null)
                {
                    IMyInput clickedIMyInputScript = clickedGameObject.GetComponent<IMyInput>();
                    Point inputPoint = clickedIMyInputScript.GetPoint();
                    if (Array.IndexOf(canMovePoints,inputPoint)!=-1)
                    {
                        //移動先が選択された
                        int step = nocca.Move(selectedPieceScript.GetPoint(), inputPoint);
                        selectedPieceScript.changePoint(inputPoint, step);

                        selectedPieceScript.isSelected = false;
                        selectedPieceScript = null;
                        canMovePoints = new Point[] { };
                        gameState = GameState.waiteForSlectingPiece;

                        if (nocca.isGameOver)
                        {
                            gameState = GameState.gameover;
                        }
                        return;
                    }
                }
                //選択解除
                selectedPieceScript.isSelected = false;
                selectedPieceScript = null;
                canMovePoints = new Point[] { };
                gameState = GameState.waiteForSlectingPiece;
            }
        }

        void InitUnirx()
        {
            var PIScript = GameObject.FindGameObjectWithTag("IndicatorTag").GetComponent<PlayerIndicatorScript>();
            PIScript.RegistTurnReactiveProperty(nocca.isMyturn);
            gameState = GameState.waiteForSlectingPiece;
        }

        GameObject GetClickedGameObject()
        {
            Point? ans = null;
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
