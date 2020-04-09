using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NOCCARule;

namespace PiecesBoard
{
    public class PiecesBoardGenerator : MonoBehaviour
    {
        public GameObject masu;
        public GameObject goal;
        public GameObject piece;
        public GameObject indicator;


        // Start is called before the first frame update
        void Start()
        {
            InitBoard();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void InitBoard()
        {
            GenerateMasu();
            GenerateGoal();
            GeneratePiece();
            GenerateIndicator();
        }

        private void GenerateIndicator()
        {
            var ind =  Instantiate(indicator, indicator.transform.position, indicator.transform.rotation);
            ind.name = indicator.name;
            ind.tag = "IndicatorTag";
        }

        private void GenerateMasu()
        {
            for (int x = 0; x < NOCCACore.XRANGE; x++)
            {
                for (int z = 0; z < NOCCACore.ZRANGE; z++)
                {
                    MasuScript masuScript = Instantiate(masu, masu.transform.position, masu.transform.rotation).GetComponent<MasuScript>();
                    masuScript.Init(new Point(x, z));
                }
            }
        }

        private void GenerateGoal()
        {
            GoalScript goalScript = Instantiate(goal, goal.transform.position, goal.transform.rotation).GetComponent<GoalScript>();
            goalScript.Init(true);
            goalScript = Instantiate(goal, goal.transform.position, goal.transform.rotation).GetComponent<GoalScript>();
            goalScript.Init(false);
        }

        private void GeneratePiece()
        {
            foreach(int x in new int[] {0, NOCCACore.XRANGE - 1 })
            {
                for(int z = 0; z < NOCCACore.ZRANGE; z++)
                {
                    PieceScript pieceScript = Instantiate(piece, piece.transform.position, piece.transform.rotation).GetComponent<PieceScript>();
                    pieceScript.Init(new Point(x, z), x == 0);
                }
            }
        }
    }
}
 