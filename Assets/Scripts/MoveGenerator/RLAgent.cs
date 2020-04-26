using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;
using MLAgents.Sensors;

namespace MoveGenerator
{
    public class RLAgent : Agent
    {
        RLCPU rlcpu = null;
        // Start is called before the first frame update
        void Start()
        {
        }

        public void Init(RLCPU rlcpu)
        {
            this.rlcpu = rlcpu;
        }

    }
}