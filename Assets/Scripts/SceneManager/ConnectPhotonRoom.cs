﻿using UnityEngine;
    #region Private Serializable Fields
    /// <summary>

    #endregion

    #region Private Fields
    /// <summary>

    /// <summary>

    #endregion

    #region Public Fields
    [Tooltip("The Ui Panel to let the user enter name, connect and play")]
    #endregion

    #region MonoBehaviour CallBacks

    /// <summary>
        // #Critical
        // this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
        PhotonNetwork.AutomaticallySyncScene = false;

    /// <summary>

    #endregion

    #region Public Methods

    /// <summary>
        // we check if we are connected or not, we join if we are , else we initiate the connection to the server.
        if (PhotonNetwork.IsConnected)
            // #Critical we need at this point to attempt joining a Random Room. If it fails, we'll get notified in OnJoinRandomFailed() and we'll create one.
            PhotonNetwork.JoinRandomRoom();
            // keep track of the will to join a room, because when we come back from the game we will get a callback that we are connected, so we need to know what to do then
            isConnecting = PhotonNetwork.ConnectUsingSettings();




    #endregion
    #region MonoBehaviourPunCallbacks Callbacks

    public override void OnConnectedToMaster()
        // we don't want to do anything if we are not attempting to join a room.
        // this case where isConnecting is false is typically when you lost or quit the game, when this level is loaded, OnConnectedToMaster will be called, in that case
        // we don't want to do anything.
        if (isConnecting)
            // #Critical: The first we try to do is to join a potential existing room. If there is, good, else, we'll be called back with OnJoinRandomFailed()
            PhotonNetwork.JoinRandomRoom();

        // #Critical: we failed to join a random room, maybe none exists or they are all full. No worries, we create a new room.
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
        // 先行後攻を決める．ランダムに決めたい
        {
            PiecesManager.oppFirst = false;
        }
        else if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            PiecesManager.oppFirst = true;
        }

        //部屋に一人のときは待機，二人揃ったらPlaySceneへ遷移
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            progressLabel.GetComponent<Text>().text = "Waiting for Player...";
        }
        else if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            MenuSceneScript.ChangeToPlayScene(PlayerSetting.MyOnline, PlayerSetting.MyOnline);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("OnPlayerEnteredRoom() called by PUN. Now a new client entered this room.");

        //部屋に一人のときは待機，二人揃ったらPlaySceneへ遷移
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            progressLabel.GetComponent<Text>().text = "Waiting for Player...";
        }
        else if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            MenuSceneScript.ChangeToPlayScene(PlayerSetting.MyOnline, PlayerSetting.MyOnline);
        }
    }



    #endregion

}