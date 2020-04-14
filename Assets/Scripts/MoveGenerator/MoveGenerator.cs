using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveGenerator {
    public abstract class AMoveGenerator
    {
        //NOCCACOre.MyGoalPoint, OppGoalPointとかぶらないように設定しないとだめ．．．
        //というかここで保管するべき？
        public Point clickedNonIMyInputObject = new Point(-10, -10);

        //動かす駒を選択する状況:false
        //選択した駒を動かす場所を選択する状況:true
        bool isSelectingMovingPiece = false;

        abstract public Point? GetInputPoint();
    }
}
