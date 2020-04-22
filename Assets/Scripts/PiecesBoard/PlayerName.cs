using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

namespace PiecesBoard
{
    public class PlayerName : MonoBehaviour
    {
        public GameObject MyNameText;
        public GameObject OppNameText;

        // Start is called before the first frame update
        void Start()
        {
            if(MenuSceneScript.myPlayerSetting == PlayerSetting.MyOnline)
            {
                gameObject.SetActive(true);
                GetPlayerName();
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        void GetPlayerName()
        {
            //oter playerは一人だけ
            string myName = PhotonNetwork.LocalPlayer.NickName;
            string oppName = PhotonNetwork.PlayerListOthers[0].NickName;

            MyNameText.GetComponent<Text>().text = myName;
            OppNameText.GetComponent<Text>().text = oppName;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
