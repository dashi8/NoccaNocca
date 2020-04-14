using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NOCCARule;

namespace MoveGenerator
{
    public class CPUMove : AMoveGenerator
    {
        NOCCACore nocca = null;

        //動かす駒を選択する状況:true
        //選択した駒を動かす場所を選択する状況:false
        bool willSelectMovingPiece = true;
        Point? movingPoint = null;

        int waitFlameCounter = 0;

        public CPUMove(NOCCACore nocca)
        {
            this.nocca = nocca;
        }

        bool waitFlame(int waitFlames)
        {
            //UniTask.Delayが動かない
            if(waitFlameCounter < waitFlames)
            {
                waitFlameCounter += 1;
                return true;
            }
            else
            {
                waitFlameCounter = 0;
                return false;
            }
        }

        public override Point? GetInputPoint()
        {
            Point? ans;
            Point[] Pcandidate = new Point[] { new Point(0,0) };
            if (willSelectMovingPiece)
            {
                if (waitFlame(30))
                {
                    return null;
                }
                else
                {
                    // 動かす駒をランダムに選択
                    movingPoint = null;
                    Pcandidate = nocca.CanMovePiecesPoints();
                }
            }
            else if(movingPoint != null)
            {
                // 動かす先をランダムに選択
                Pcandidate = nocca.CanMovePointsFrom(movingPoint.Value);
            }
            ans = Pcandidate[UnityEngine.Random.Range(0, Pcandidate.Length)];
            movingPoint = ans;
            willSelectMovingPiece = !willSelectMovingPiece;
            return ans;
        }
    }
}