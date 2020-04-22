using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PiecesBoard;
using Photon.Pun;using Photon.Realtime;
using NOCCARule;
using UniRx;

namespace MoveGenerator
{
    public class MyMoveOnline : AMoveGenerator, IPunObservable
    {
        bool returnLocalInput;
        PhotonView photonView;

        Point? recievedPoint = null;
        public static MyMoveOnline LocalMoveInstance;

        void Awake()
        {
            photonView = PhotonView.Get(this);
            if (photonView.IsMine)
            {
                LocalMoveInstance = this;
            }
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {

            }
            else
            {

            }
        }

        [PunRPC] // Point型は送れない
        void SendMyInputToOpp(bool isNull, int Recievedx, int Recievedz)
        {
            if (!isNull)
            {
                Point convertedPoint;
                // 相手からの視点での座標が送られてくるため，自分の視点での座標に変換．
                if (new Point(Recievedx, Recievedz) == NOCCACore.MyGoalPoint)
                {
                    convertedPoint = NOCCACore.OppGoalPoint;
                }
                else if (new Point(Recievedx, Recievedz) == NOCCACore.OppGoalPoint)
                {
                    convertedPoint = NOCCACore.MyGoalPoint;
                }
                else
                {
                    convertedPoint = new Point(NOCCACore.XRANGE - 1 - Recievedx, NOCCACore.ZRANGE - 1 - Recievedz);
                }
                //LocalMoveInstanceを呼び出すことを明記する！だいぶハマった．．．
                LocalMoveInstance.GetComponent<MyMoveOnline>().SetRecievedPoint(convertedPoint);
            }
        }

        public void SetRecievedPoint(Point rp)
        {
            recievedPoint = rp;
        }

        public void RegistTurnReactiveProperty(IReactiveProperty<bool> _isMyTurn)
        {
            _isMyTurn.Subscribe(isMyTurn =>
            {
                returnLocalInput = isMyTurn;
            });
        }

        public override Point? GetInputPoint()
        {
            Point? p = null;

            if (returnLocalInput)
            {
                p = GetLocalInput();
            }
            else
            {
                p = GetRemoteInput();
            }

            return p;
        }

        Point? GetRemoteInput()
        {
            var ans = recievedPoint;
            recievedPoint = null;
            return ans;
        }

        Point? GetLocalInput()
        {
            //null: クリックされてない
            //clickedNonIMyInputObject: クリックされたがIMyInputでない
            //otherwise: IMyInputがクリックされた
            Point? p = null;

            if (Input.GetMouseButtonUp(0))
            {
                GameObject clickedGameObject = GetClickedGameObject();
                if (clickedGameObject != null && clickedGameObject.GetComponent<IMyInput>() != null)
                {
                    IMyInput clickedPieceScript = clickedGameObject.GetComponent<IMyInput>();
                    p = clickedPieceScript.GetPoint();
                }
                else
                {
                    p = clickedNonIMyInputObject;
                }
            }

            //nullじゃないときだけ相手のSendMyInputToOppにわたす
            if(p != null)
            {
                photonView.RPC("SendMyInputToOpp", RpcTarget.Others, false, p.Value.x, p.Value.z);
            }

            return p;
        }

        GameObject GetClickedGameObject()
        {
            GameObject clickedGameObject = null;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit = new RaycastHit();

            if (Physics.Raycast(ray, out hit))
            {
                clickedGameObject = hit.collider.gameObject;
            }
            return clickedGameObject;
        }
    }
}
