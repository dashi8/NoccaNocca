using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PiecesBoard
{
    public class MasuScript : MonoBehaviour, IMyInput
    {
        private Point point;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Init(Point p)
        {
            point = p;
            transform.position = Point2Coord.point2coord(p, transform.position.y);
        }

        public Point GetPoint()
        {
            return point;
        }
    }
}
