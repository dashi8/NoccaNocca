using System;
using UnityEngine;
using PiecesBoard;

namespace NOCCARule
{
    public class NOCCACore
    {
        public readonly static int XRANGE = 6;
        public readonly static int ZRANGE = 5;
        public readonly static int YRANGE = 3;
        public readonly static Point MyGoalPoint = new Point(-2, -2);
        public readonly static Point OppGoalPoint = new Point(-1, -1);


        PlayerIndicatorScript indicatorScript = null;
        //1:自分，-1：相手
        //高さ方向がy
        int[,,] state = new int[XRANGE, YRANGE, ZRANGE];
        private bool _isMyTurn;
        public bool isMyTurn
        {
            get {
                return _isMyTurn;
            }
            set {
                _isMyTurn = value;
                indicatorScript.ChangeTurn(value);
            }
        }

        public bool isGameOver;
        public int winner;

        enum Goal
        {
            None = 0,
            MyGoal = 1,
            OppGoal = 2,
        }

        public NOCCACore()
        {
            isGameOver = false;
            winner = 0;

            foreach (int x in new int[] { 0, XRANGE - 1 })
            {
                for (int z = 0; z < ZRANGE; z++)
                {
                    state[x, 0, z] = (x == 0) ? 1 : -1;
                }
            }
        }

        public void RegistIndicator()
        {
            indicatorScript = GameObject.FindGameObjectWithTag("IndicatorTag").GetComponent<PlayerIndicatorScript>();
            isMyTurn = true;
        }

        public int Move(Point pre, Point next)
        {
            if (isGameOver)
            {
                return -1;
            }

            if (next == MyGoalPoint)
            {
                //終了
                winner = 1;
                isGameOver = true;
                return 0;
            }
            else if (next == OppGoalPoint)
            {
                //終了
                winner = -1;
                isGameOver = true;
                return 0;
            }
            else
            {
                var preTopState = TopState(pre);
                var nextTopState = TopState(next);
                state[next.x, nextTopState[1] + 1, next.z] = preTopState[0];
                state[pre.x, preTopState[1], pre.z] = 0;

                isMyTurn = !isMyTurn;

                CheckAllPieceCannotMove();

                return nextTopState[1] + 1;
            }
        }

        void CheckAllPieceCannotMove()
        {
            bool cannotMove = true;
            for(int x = 0; x < XRANGE; x++)
            {
                for(int z = 0; z < ZRANGE; z++)
                {
                    Point tempPoint = new Point(x, z);
                    if (TopState(tempPoint)[0] == (isMyTurn ? 1 : -1))
                    {
                        if (CanMovePoints(tempPoint).Length > 0)
                        {
                            cannotMove = false;
                            break;
                        }
                    }
                }
            }

            if (cannotMove)
            {
                isGameOver = true;
                winner = isMyTurn ? -1 : 1;
            }
        }

        public Point[] CanMovePoints(Point p)
        {
            Debug.Log(isMyTurn);
            var canMovePoints = new Point[] { };
            //ゴール
            if (IsGoal(p) != Goal.None)
            {
                return canMovePoints;
            }

            //手番と駒が一致
            if ((TopState(p)[0] == 1 && isMyTurn) || (TopState(p)[0] == -1 && !isMyTurn)) {
                //周囲8箇所を確認
                var piteration = new Point[] {
                    new Point(0,1),
                    new Point(0,-1),
                    new Point(1,0),
                    new Point(1,1),
                    new Point(1,-1),
                    new Point(-1,0),
                    new Point(-1,1),
                    new Point(-1,-1),
                };
                foreach (Point pi in piteration)
                {
                    //範囲内
                    if (0 <= (p + pi).x && (p + pi).x < XRANGE && 0 <= (p + pi).z && (p + pi).z < ZRANGE)
                    {
                        //飽和してない
                        if (TopState(p + pi)[1] != YRANGE-1)
                        {
                            Array.Resize(ref canMovePoints, canMovePoints.Length + 1);
                            canMovePoints[canMovePoints.Length - 1] = p + pi;
                        }
                    }
                }
                //ゴールも追加
                if(TopState(p)[0]==1 && p.x == XRANGE - 1)
                {
                    Array.Resize(ref canMovePoints, canMovePoints.Length + 1);
                    canMovePoints[canMovePoints.Length - 1] = MyGoalPoint;
                }else if (TopState(p)[0] == -1 && p.x == 0)
                {
                    Array.Resize(ref canMovePoints, canMovePoints.Length + 1);
                    canMovePoints[canMovePoints.Length - 1] = OppGoalPoint;
                }
            }
            return canMovePoints;
        }

        //{topstate, step}を返す
        int[] TopState(Point p)
        {
            int topState = 0;
            int step = -1;
            //手番と駒が一致
            for (int y = YRANGE - 1; y >= 0; y--)
            {
                if (state[p.x, y, p.z] != 0)
                {
                    topState = state[p.x, y, p.z];
                    step = y;
                    break;
                }
            }
            return new int[] { topState, step };
        }

        Goal IsGoal(Point p)
        {
            Goal g;
            if (p == MyGoalPoint)
            {
                g = Goal.MyGoal;
            }
            else if (p == OppGoalPoint)
            {
                g = Goal.OppGoal;
            }
            else { g = Goal.None; }

            return g;
        }
    }
}
