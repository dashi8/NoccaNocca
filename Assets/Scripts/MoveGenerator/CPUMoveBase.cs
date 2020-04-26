using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NOCCARule;
using System;


namespace MoveGenerator
{
    public abstract class CPUMoveBase : AMoveGenerator
    {
        protected NOCCACore nocca = null;

        //動かす駒をこれから選択する状況:true
        //選択した駒を動かす場所を選択する状況:false
        bool willSelectMovingPiece = true;

        protected Point? movingPoint = null;
        protected Point? destinationPoint = null;

        //isWaiting_flame用
        int waitFlameCounter = 0;

        public void SetNoccaObject(NOCCACore nocca)
        {
            this.nocca = nocca;
        }

        bool isWaiting_flame(int waitFlames)
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
            if (willSelectMovingPiece)
            {
                Debug.Log((int)(3f / (4f * Time.deltaTime)));
                if (isWaiting_flame((int)(3f / (4f * Time.deltaTime))))
                {
                    return null;
                }
                else
                {
                    //前回選択した駒を削除
                    movingPoint = null;
                    destinationPoint = null;

                    //手を選択
                    SelectPolicy();
                    if(movingPoint != null)
                    {
                        willSelectMovingPiece = false;
                    }
                    return movingPoint;
                }
            }
            else
            {
                willSelectMovingPiece = true;
                return destinationPoint;
            }
        }

        //Pcandidateの中から動かす駒と行き先を選んで，それぞれmovingPointとdestinationPointへ
        abstract protected void SelectPolicy();
    }
}