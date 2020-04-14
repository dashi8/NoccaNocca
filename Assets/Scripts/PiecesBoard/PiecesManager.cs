using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NOCCARule;
using System;
using UnityEngine.SceneManagement;
using UniRx.Async;
using MoveGenerator;

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

        AMoveGenerator myMove = null;
        AMoveGenerator oppMove = null;

        PieceScript selectedPieceScript;
        Point[] canMovePoints;

        enum GameState
        {
            noinited,
            waiteForSlectingPiece,
            waiteForMovingPoint,
            pieceMoving,
            gameover
        }


        // Start is called before the first frame update
        void Start()
        {
            gameState = GameState.noinited;
            selectedPieceScript = null;
            canMovePoints = new Point[]{ };

            myMove = getMoveGenerator("MyMove");
            myMove.setPlayer(1);
            oppMove = getMoveGenerator("OppMove");
            oppMove.setPlayer(-1);
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

        AMoveGenerator getMoveGenerator(string player)
        {
            PlayerSetting playerSetting = MenuSceneScript.myPlayerSetting;
            if (player == "MyMove")
            {
                playerSetting = MenuSceneScript.myPlayerSetting;
            }
            else if(player == "OppMove")
            {
                playerSetting = MenuSceneScript.oppPlayerSetting;
            }
            else
            {
                Debug.Log("ここには来ないはず");
            }

            switch (playerSetting)
            {
                case PlayerSetting.My:
                    //Start()で呼ぶとなぜかfindできない
                    return new MyMove();
                case PlayerSetting.Cpu:
                    return new CPUMove(nocca);
                default:
                    return new MyMove();
            }
        }

        async void GameOverProcess()
        {
            //ゲーム終了後，駒が移動するまで少し待機
            await UniTask.Delay(TimeSpan.FromSeconds(1));

            SceneManager.LoadScene("ResultScene");
            //SceneManager.LoadScene("MenuScene");
        }

        void SelectPiece()
        {
            Point? tmpInput = null;
            if (nocca.isMyTurn.Value)
            {
                tmpInput = myMove.GetInputPoint();
            }
            else
            {
                tmpInput = oppMove.GetInputPoint();
            }
            
            if(tmpInput != null && tmpInput != myMove.clickedNonIMyInputObject)
            {
                canMovePoints = nocca.CanMovePointsFrom(tmpInput.Value);
                if (canMovePoints.Length > 0)
                {
                    //移動できる駒が選択された
                    gameState = GameState.waiteForMovingPoint;
                    selectedPieceScript = getPieceScript(tmpInput.Value);
                    selectedPieceScript.isSelected = true;
                }
            }
        }

        void SelectMovePoint()
        {
            Point? tmpInput = null;
            if (nocca.isMyTurn.Value)
            {
                tmpInput = myMove.GetInputPoint();
            }
            else
            {
                tmpInput = oppMove.GetInputPoint();
            }

            if (tmpInput != null)
            {
                if (tmpInput != myMove.clickedNonIMyInputObject)
                {
                    //IMyInputがクリックされた
                    if (Array.IndexOf(canMovePoints, tmpInput.Value) != -1)
                    {
                        //移動先が選択された
                        int step = nocca.Move(selectedPieceScript.GetPoint(), tmpInput.Value);
                        selectedPieceScript.changePoint(tmpInput.Value, step);
                    }
                }
                //クリックされた常に実行
                selectedPieceScript.isSelected = false;
                selectedPieceScript = null;
                canMovePoints = new Point[] { };

                if (nocca.isGameOver)
                {
                    gameState = GameState.gameover;
                }
                else
                {
                    gameState = GameState.waiteForSlectingPiece;
                }
            }
        }

        void InitUnirx()
        {
            var PIScript = GameObject.FindGameObjectWithTag("IndicatorTag").GetComponent<PlayerIndicatorScript>();
            PIScript.RegistTurnReactiveProperty(nocca.isMyTurn);
            gameState = GameState.waiteForSlectingPiece;
        }

        PieceScript getPieceScript(Point p)
        {
            var pieces = GameObject.FindGameObjectsWithTag("PieceTag");
            var maxStep = -1;
            PieceScript ansPscript = null;
            foreach(GameObject tmpPobj in pieces)
            {
                var tmpPscript = tmpPobj.GetComponent<PieceScript>();
                if(tmpPscript.GetPoint() == p && tmpPscript.GetStep()>maxStep)
                {
                    maxStep = tmpPscript.GetStep();
                    ansPscript = tmpPscript;
                }
            }
            return ansPscript;
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
