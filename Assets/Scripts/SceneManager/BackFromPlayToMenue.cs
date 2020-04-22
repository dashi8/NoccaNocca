﻿using UnityEngine;
using UnityEngine.SceneManagement;

public class BackFromPlayToMenue : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void toMenuScene()
    {
        if (PhotonManager.Instance != null)
        {
            PhotonManager.Instance.BacktoMenu();
        }
        else
        {
            SceneManager.LoadScene("MenuScene");
        }
    }
}
