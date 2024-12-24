using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeManager : MonoBehaviour
{
    public GameObject settingPanel;


    // Start is called before the first frame update
    void Start()
    {
        //AdsControl.Instance.ShowBannerAd();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowSetting()
    {
        AudioManager.instance.btnSfx.Play();
        settingPanel.SetActive(true);
    }

    public void Play()
    {
        AudioManager.instance.btnSfx.Play();
        SceneManager.LoadScene("Game");
    }
}
