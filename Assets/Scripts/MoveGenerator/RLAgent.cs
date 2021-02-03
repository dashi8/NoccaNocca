using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;
using MLAgents.Sensors;
using PiecesBoard;

namespace MoveGenerator
{
    public class RLAgent : Agent
    {
        RLCPU rlcpu = null;
        // Start is called before the first frame update
        void Start()
        {
        }

        public void RegistRLCPU(RLCPU rlcpu)
        {
            this.rlcpu = rlcpu;
        }

        public override void CollectObservations(VectorSensor sensor)
        {
            sensor.AddObservation(1.0f);
        }

        public override void OnEpisodeBegin()
        {
            //base.OnEpisodeBegin();
            var piecesManagerScript = GameObject.FindGameObjectWithTag("PieceManager").GetComponent<PiecesManager>();
            //これはヤバそう
            //これを呼ぶときのpiecesManagerの状態要確認
            piecesManagerScript.RestartGame();
        }
    }
}