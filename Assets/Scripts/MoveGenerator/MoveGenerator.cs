using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveGenerator {
    public abstract class AMoveGenerator
    {
        //NOCCACOre.MyGoalPoint, OppGoalPointとかぶらないように設定しないとだめ．．．
        //というかここで保管するべき？
        public Point clickedNonIMyInputObject = new Point(-10, -10);

        abstract public Point? GetInputPoint();
    }
}
