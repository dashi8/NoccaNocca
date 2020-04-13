using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveGenerator {
    interface IMoveGenerator
    {
        Point? GetInputPoint();
    }

    public abstract class AMoveGenerator
    {
        //NOCCACOre.MyGoalPoint, OppGoalPointとかぶらないように設定しないとだめ．．．
        public Point clickedNonIMyInputObject = new Point(-10, -10);

        abstract public Point? GetInputPoint();
    }
}
