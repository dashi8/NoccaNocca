using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PiecesBoard
{
    public class PlayerIndicatorScript : MonoBehaviour
    {
        Vector3 oppIndicatorPotision = new Vector3(8.23f, 0.55f, 4f);
        Vector3 myIndicatorPotision = new Vector3(-8.23f, 0.55f, -4f);

        float isIncreasing = 1f;
        float RcolorStep = 500f;
        float nowR = 0;

        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            GetComponent<Renderer>().material.color = new Color32((byte)nowR, 0, 0, 1);
            nowR += isIncreasing * Time.deltaTime * RcolorStep;

            if (nowR >= 255f)
            {
                isIncreasing = -1f;
                nowR = 255f;
            }else if(nowR <= 0)
            {
                isIncreasing = 1f;
                nowR = 0f;
            }

        }

        public void ChangeTurn(bool isMyTurn)
        {
            if (isMyTurn)
            {
                transform.position = myIndicatorPotision;
            }
            else
            {
                transform.position = oppIndicatorPotision;
            }
        }
    }
}
