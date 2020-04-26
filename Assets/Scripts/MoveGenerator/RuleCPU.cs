using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using NOCCARule;


namespace MoveGenerator
{
    public class RuleCPU : CPUMoveBase
    {
        //Pcandidateの中から動かす駒と行き先を選んで，それぞれmovingPointとdestinationPointへ
        override protected void SelectPolicy()
        {
            int maxValue = int.MinValue;
            Point[] maxPreList = new Point[] { };
            Point[] maxNextList = new Point[] { };


            // 動かせる駒
            Point[] preCandidate = nocca.CanMovePiecesPoints();
            foreach (Point pre in preCandidate)
            {
                Point[] nextCandidate = nocca.CanMovePointsFrom(pre);
                foreach (Point next in nextCandidate)
                {
                    //OppGoalに入れるならそこを選んで終わり
                    if (next == (player == -1 ? NOCCACore.OppGoalPoint : NOCCACore.MyGoalPoint))
                    {
                        movingPoint = pre;
                        destinationPoint = next;
                        return;
                    }

                    int tmpValue = StateEvaluatedValue(nocca.CheckNextState(pre, next));
                    if (tmpValue > maxValue)
                    {
                        maxValue = tmpValue;
                        maxPreList = new Point[] { pre };
                        maxNextList = new Point[] { next };
                    }
                    else if (tmpValue == maxValue)
                    {
                        Array.Resize(ref maxPreList, maxPreList.Length + 1);
                        maxPreList[maxPreList.Length - 1] = pre;
                        Array.Resize(ref maxNextList, maxNextList.Length + 1);
                        maxNextList[maxNextList.Length - 1] = next;
                    }
                }
            }

            int randIndex = UnityEngine.Random.Range(0, maxPreList.Length);
            movingPoint = maxPreList[randIndex];
            destinationPoint = maxNextList[randIndex];
        }

        //盤面を評価 適当
        int StateEvaluatedValue(int[,,] state)
        {
            var weightList = new int[] { };
            var valueList = new int[] { };

            //自己コマのxが相手のゴールに近い(xが小さい)
            Array.Resize(ref weightList, weightList.Length + 1);
            weightList[weightList.Length - 1] = 1;
            Array.Resize(ref valueList, valueList.Length + 1);
            valueList[valueList.Length - 1] = SumOfX(state);

            //相手の駒の上にのっている-自分の駒にのっている
            Array.Resize(ref weightList, weightList.Length + 1);
            weightList[weightList.Length - 1] = 3;
            Array.Resize(ref valueList, valueList.Length + 1);
            valueList[valueList.Length - 1] = NumOfOnPieces(state);

            //-相手の駒の隣にいる
            Array.Resize(ref weightList, weightList.Length + 1);
            weightList[weightList.Length - 1] = 4;
            Array.Resize(ref valueList, valueList.Length + 1);
            valueList[valueList.Length - 1] = -NumOfAdjacentPieces(state);

            int sum = 0;
            for (int i = 0; i < weightList.Length; i++)
            {
                sum += weightList[i] * valueList[i];
            }
            return sum;
        }

        int SumOfX(int[,,] state)
        {
            int ans = 0;
            for (int x = 0; x < NOCCACore.XRANGE; x++)
            {
                for (int y = 0; y < NOCCACore.YRANGE; y++)
                {
                    for (int z = 0; z < NOCCACore.ZRANGE; z++)
                    {
                        if (state[x, y, z] == player)
                        {
                            ans += player == 1 ? x + 1 : 6 - x;
                        }
                    }
                }
            }
            return ans;
        }

        int NumOfOnPieces(int[,,] state)
        {
            int ans = 0;
            for (int x = 0; x < NOCCACore.XRANGE; x++)
            {
                for (int z = 0; z < NOCCACore.ZRANGE; z++)
                {
                    int topState = nocca.TopState(new Point(x, z), state)[0];
                    if (topState == player)
                    {
                        ans += player == 1 ? x + 1 : 6 - x;
                        if (x == (player == -1 ? 0 : 5))
                        {
                            ans += 6;
                        }
                    }
                    if (topState == -player)
                    {
                        ans -= player == 1 ? x + 1 : 6 - x;
                        if (x == (player == -1 ? 5 : 0))
                        {
                            ans -= 6;
                        }
                    }
                }
            }
            return ans;
        }

        int NumOfAdjacentPieces(int[,,] state)
        {
            int ans = 0;
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
            for (int x = 0; x < NOCCACore.XRANGE; x++)
            {
                for (int z = 0; z < NOCCACore.ZRANGE; z++)
                {
                    foreach (Point pi in piteration)
                    {
                        Point tmpP = new Point(x, z);
                        //範囲内
                        if (0 <= (tmpP + pi).x && (tmpP + pi).x < NOCCACore.XRANGE && 0 <= (tmpP + pi).z && (tmpP + pi).z < NOCCACore.ZRANGE)
                        {
                            int topState = nocca.TopState(tmpP + pi, state)[0];
                            if (topState == -player)
                            {
                                ans += 1;
                            }
                        }
                    }

                }
            }
            return ans;
        }

    }
}
