using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace PiecesBoard
{
    public class PieceScript : MonoBehaviour, IMyInput
    {
        Point point;
        int step;
        private bool isMine;
        bool isMoving;
        public bool _isSelected;

        public bool isSelected {
            set {
                _isSelected = value;
                if (!value)
                {
                    transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                }
            }
            get {return _isSelected; }
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

            var fromPosition = transform.position;
            var toPosition = Point2Coord.point2coord(point, defaultPosition.y + step * 0.4f);
            //transform.position = Point2Coord.point2coord(point, defaultPosition.y + step*0.4f);
            StartCoroutine(MoveAnimation(fromPosition, toPosition));
        }

        IEnumerator MoveAnimation(Vector3 fromP, Vector3 toP)
        {
            isMoving = true;
            const int moveStep = 10;
            Vector3 deltaPosition = toP - fromP;
            float diffx = deltaPosition.x;
            float diffy = deltaPosition.y;
            float diffz = deltaPosition.z;

            float delx = diffx / (float)moveStep;
            float delz = diffz / (float)moveStep;


            float topy = Math.Max(diffy, -diffy) + 1f;
            float a = -(4*topy) / (moveStep*moveStep);

            for (int s = 0; s < moveStep; s++)
            {
                var newy = a * s * (s - moveStep);
                transform.position = new Vector3(fromP.x+delx*s,fromP.y+newy,fromP.z+delz*s);
                yield return null;
            }
            transform.position = toP;
            isMoving = false;
        }

        public Point GetPoint()
        {
            return point;
        }
    }
}
