using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveGenerator
{
    public class RLCPU : CPUMoveBase
    {
        public GameObject RLAgentPrefub;

        private void Start()
        {
            if(RLAgentPrefub != null)
            {
                RLAgent RLAgentScript = Instantiate(RLAgentPrefub).GetComponent<RLAgent>();
                RLAgentScript.Init(this);
            }
        }

        override protected void SelectPolicy()
        {
            //RLAgentScriptにmovingPointとdestinationPointを更新してもらう
            //ここでは何もしない
        }
    }
}
