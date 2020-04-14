using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum PlayerSetting
{
    My,
    Cpu
}

public class MenuSceneScript : MonoBehaviour
{
    static public PlayerSetting myPlayerSetting = PlayerSetting.My;
    static public PlayerSetting oppPlayerSetting = PlayerSetting.My;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    static public void ChangeToPlayScene(PlayerSetting mySetting, PlayerSetting oppSetting)
    {
        myPlayerSetting = mySetting;
        oppPlayerSetting = oppSetting;
        SceneManager.LoadScene("PlayScene");
    }

    public void MYvsMYStart()
    {
        ChangeToPlayScene(PlayerSetting.My, PlayerSetting.My);
    }

    public void MYvsCPUStart()
    {
        ChangeToPlayScene(PlayerSetting.My, PlayerSetting.Cpu);
    }

    public void JumpToNoccaHP()
    {
        Application.OpenURL("http://www.undanoga.com/");
    }
}
