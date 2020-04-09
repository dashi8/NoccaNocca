﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PiecesBoard
{
    public class PieceScript : MonoBehaviour, IMyInput
    {
        Point point;
        int step;
        private bool isMine;
        bool isMoving;
        public bool __isSelected;

        public bool isSelected {
            set {
                __isSelected = value;
                if (!value)
                {
                    transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                }
            }
            get {return __isSelected; }
                }
        float time = 0;
        float fre = 0.5f;

        Vector3 defaultPosition;

        // Start is called before the first frame update
        void Start()
        {
            defaultPosition = transform.position;
        }

        // Update is called once per frame
        void Update()
        {
            if (isSelected)
            {
                transform.rotation = Quaternion.Euler(10f, Time.deltaTime*10f, 0f);
                time += Time.deltaTime;
            }

        }

        public void Init(Point p, bool mine)
        {
            step = 0;
            isMoving = false;
            isSelected = false;
            point = p;
            isMine = mine;

            transform.position = Point2Coord.point2coord(p, transform.position.y);
            if (isMine)
            {
                GetComponent<Renderer>().material.color = Color.black;
            }
            else
            {
                GetComponent<Renderer>().material.color = Color.white;
            }
        }

        public void changePoint(Point p, int s)
        {
            point = p;
            step = s;

            transform.position = Point2Coord.point2coord(point, defaultPosition.y + step*0.4f);
        }

        public Point GetPoint()
        {
            return point;
        }
    }
}
