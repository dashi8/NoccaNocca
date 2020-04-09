using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NOCCARule;

namespace PiecesBoard
{
    public class GoalScript : MonoBehaviour, IMyInput
    {
        private bool isMine;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Init(bool isMine)
        {
            this.isMine = isMine;
            if (this.isMine)
            {
                transform.position = new Vector3(7, transform.position.y, transform.position.z);
                GetComponent<Renderer>().material.color = Color.black;
            }
            else
            {
                transform.position = new Vector3(-7, transform.position.y, transform.position.z);
                GetComponent<Renderer>().material.color = Color.white;
            }
        }

        public Point GetPoint()
        {
            if (isMine)
            {
                return NOCCACore.MyGoalPoint;
            }
            else
            {
                return NOCCACore.OppGoalPoint;
            }
        }
    }
}