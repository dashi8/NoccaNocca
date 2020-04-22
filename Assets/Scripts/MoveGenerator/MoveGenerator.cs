using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveGenerator {
    public abstract class AMoveGenerator: MonoBehaviour
    {
        //NOCCACOre.MyGoalPoint, OppGoalPointとかぶらないように設定しないとだめ．．．
        //というかここで保管するべき？
        public Point clickedNonIMyInputObject = new Point(-10, -10);
        protected int player = 0;

        public void setPlayer(int player)
        {
            if (player == 1 || player == -1)
            {
                this.player = player;
            }
            else
            {
                Debug.Log("ココには来ない");
            }
        }

        abstract public Point? GetInputPoint();
    }
}
