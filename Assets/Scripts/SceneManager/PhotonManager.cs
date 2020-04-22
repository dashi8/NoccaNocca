using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;


public class PhotonManager : MonoBehaviourPunCallbacks
{
    #region Public Fields

    public static PhotonManager Instance;

    #endregion


    #region Photon Callbacks


    /// <summary>
    /// Called when the local player left the room. We need to load the launcher scene.
    /// </summary>
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("MenuScene");
    }

    public override void OnPlayerLeftRoom(Player other)
    {
        Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName); // seen when other disconnects

        LeaveRoom();
    }


    #endregion

    #region MonoBehaviour Callbacks

    void Start()
    {
        Instance = this;
    }

    #endregion

    #region Public Methods


    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public void BacktoMenu()
    {
        if (PhotonNetwork.InRoom)
        {
            LeaveRoom();
        }
        SceneManager.LoadScene("MenuScene");
    }


    #endregion

}
