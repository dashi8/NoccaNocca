using System;
using PiecesBoard;
using UniRx;

namespace NOCCARule
{
    public class NOCCACore
    {
        public readonly static int XRANGE = 6;
        public readonly static int ZRANGE = 5;
        public readonly static int YRANGE = 3;
        public readonly static Point MyGoalPoint = new Point(-2, -2);//自分が目指すMyGoalPoint
        public readonly static Point OppGoalPoint = new Point(-1, -1);


        PlayerIndicatorScript indicatorScript = null;
        //1:自分，-1：相手
        //高さ方向がy
        int[,,] state = new int[XRANGE, YRANGE, ZRANGE];
        private ReactiveProperty<bool> _isMyTurn = new ReactiveProperty<bool>(true);
        public IReactiveProperty<bool> isMyTurn
        {
            get { return _isMyTurn; }
        }

        public bool isGameOver { get; private set; }
        public static int winner { get; private set; }

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
            _isMyTurn.Value = true;

            foreach (int x in new int[] { 0, XRANGE - 1 })
            {
                for (int z = 0; z < ZRANGE; z++)
                {
                    state[x, 0, z] = (x == 0) ? 1 : -1;
                }
            }
        }

        public void InverseTurn(bool oppFirst)
        {
            //oppFirst==trueのとき，1ターン目でisMyTurn = false
            _isMyTurn.Value = !oppFirst;
        }

        public int Move(Point pre, Point next)
        {
            //動かせるPointが渡されること前提
            //呼び出す側で確認する

            var preTopState = TopState(pre);
            var nextTopState = TopState(next);
            if (preTopState[0] != (_isMyTurn.Value ? 1 : -1))
            {
                //現ターンではないの駒を移動しようとした
                return -2;
            }

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
                if (preTopState[0] != (_isMyTurn.Value ? 1 : -1))
                {
                    return -2;
                }
                state[next.x, nextTopState[1] + 1, next.z] = preTopState[0];
                state[pre.x, preTopState[1], pre.z] = 0;

                _isMyTurn.Value = !_isMyTurn.Value;

                CheckAllPieceCannotMove();

                return nextTopState[1] + 1;
            }
        }

        //cpuが利用
        //プロパティは変更しない
        //駒が動かせないときはnullを返す
        //nextにgoalを渡すのは禁止
        public int[,,] CheckNextState(Point pre, Point next)
        {
            //動かせるPointが渡されること前提
            //呼び出す側で確認する
            var preTopState = TopState(pre);
            var nextTopState = TopState(next);
            if (preTopState[0] == 0)
            {
                //preに駒がない
                return null;
            }

            if (next == MyGoalPoint)
            {
                return null;
            }
            else if (next == OppGoalPoint)
            {
                return null;
            }

            int[,,] returnState = GetState();
            returnState[next.x, nextTopState[1] + 1, next.z] = preTopState[0];
            returnState[pre.x, preTopState[1], pre.z] = 0;

            return returnState;
        }

        void CheckAllPieceCannotMove()
        {
            bool cannotMove = true;
            for(int x = 0; x < XRANGE; x++)
            {
                for(int z = 0; z < ZRANGE; z++)
                {
                    Point tempPoint = new Point(x, z);
                    if (TopState(tempPoint)[0] == (_isMyTurn.Value ? 1 : -1))
                    {
                        if (CanMovePointsFrom(tempPoint).Length > 0)
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
                winner = _isMyTurn.Value ? -1 : 1; //逆のターンを返す
            }
        }

        //csを設定するときは必ずctも設定する．
        public Point[] CanMovePointsFrom(Point p)
        {
            var canMovePoints = new Point[] { };
            //ゴール
            if (IsGoal(p) != Goal.None)
            {
                return canMovePoints;
            }

            //手番と駒が一致
            if ((TopState(p)[0] == 1 && _isMyTurn.Value) || (TopState(p)[0] == -1 && !_isMyTurn.Value)) {
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
                if(TopState(p)[0] == 1 && p.x == XRANGE - 1)
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

        public Point[] CanMovePiecesPoints()
        {
            var canMovePoints = new Point[] { };
            for(int x = 0; x < XRANGE; x++)
            {
                for(int z = 0; z < ZRANGE; z++)
                {
                    var checkingPoint = new Point(x, z);
                    if(TopState(checkingPoint)[0] == (_isMyTurn.Value ? 1 : -1))
                    {
                        Array.Resize(ref canMovePoints, canMovePoints.Length + 1);
                        canMovePoints[canMovePoints.Length - 1] = checkingPoint;
                    }
                }
            }
            return canMovePoints;
        }

        //{topstate, step}を返す
        public int[] TopState(Point p, int[,,] sc = null)
        {
            int[,,] checkState = new int[,,] { };
            if(sc == null)
            {
                checkState = state;
            }
            else
            {
                checkState = sc;
            }
            int topState = 0;
            int step = -1;
            //ゴールの時
            if(p == MyGoalPoint || p == OppGoalPoint)
            {
                return new int[] { 0, 0 };
            }
            //手番と駒が一致
            for (int y = YRANGE - 1; y >= 0; y--)
            {
                if (checkState[p.x, y, p.z] != 0)
                {
                    topState = checkState[p.x, y, p.z];
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

        public int[,,] GetState()
        {
            //deepcopyする便利な機能ないの？
            int[,,] copiedState = new int[XRANGE, YRANGE, ZRANGE];

            for (int x = 0; x < XRANGE; x++)
            {
                for (int y = 0; y < YRANGE; y++)
                {
                    for (int z = 0; z < ZRANGE; z++)
                    {
                        copiedState[x, y, z] = state[x, y, z];
                    }
                }
            }

            return copiedState;
        }

        public static bool getWinner()
        {
            //result画面への受け渡し用
            return winner == 1 ? true : false;
        }
    }
}
