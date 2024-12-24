using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    //[HideInInspector]
    public Level mLevel;

    public int levelIndex;

    public int totalLevel;

    public UIManager uiManager;

    public int gems;

    private void Awake()
    {
        instance = this;

        Application.targetFrameRate = 60;
    }
    // Start is called before the first frame update
    void Start()
    {
        
        AdsControl.Instance.ShowBannerAd();
        if(mLevel == null)
          LoadLevel();

        uiManager.uiGame.ShowQuestion(mLevel.levelQuestion);
        gems = PlayerPrefs.GetInt("Gems");
        uiManager.uiGame.ShowGems(gems);
        uiManager.uiGame.ShowLevel(levelIndex + 1);
    }

    public void LoadLevel()
    {
        levelIndex = PlayerPrefs.GetInt("CurrentLevel");
        //levelIndex = 1;

        if (levelIndex >= totalLevel)
        {
            levelIndex = 0;
            PlayerPrefs.SetInt("CurrentLevel", 0);
        }


        GameObject lvObj = Instantiate(Resources.Load("prefabs/level/LV_" + (levelIndex + 1).ToString())) as GameObject;
        mLevel = lvObj.GetComponent<Level>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartToCheckWin()
    {
        uiManager.uiGame.StartTimer();
    }

    public void AddGems(int _value)
    {
        gems += _value;
        uiManager.uiGame.ShowGems(gems);
    }

    public void SaveGems()
    {
        PlayerPrefs.SetInt("Gems", gems);
    }

    public void CheckResult()
    {
        if(mLevel.failTrigger != null)
        {
            if(mLevel.failTrigger.failLevel)
            {
                uiManager.ShowFail();
                return;
            }
               
        }

        if (!mLevel.mainPlayer.isDie)
       
            uiManager.ShowWin();

        

        else
            uiManager.ShowFail();
        
    }

    public void Replay()
    {
        AudioManager.instance.btnSfx.Play();
        SceneManager.LoadScene("Game");
    }

    public void Back()
    {
        AudioManager.instance.btnSfx.Play();
        SceneManager.LoadScene("Main");
    }

    public void ShowHint()
    {
        AudioManager.instance.btnSfx.Play();
        if (mLevel.hintObj != null)
        mLevel.hintObj.SetActive(true);
    }

    public void SkipLevel()
    {
        AudioManager.instance.btnSfx.Play();
        levelIndex++;
        PlayerPrefs.SetInt("CurrentLevel", levelIndex);
        Replay();
    }
}
