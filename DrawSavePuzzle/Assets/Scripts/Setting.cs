using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Setting : MonoBehaviour
{
    public GameObject musicOn, musicOff;

    public string policyUrl, termUrl;

    // Start is called before the first frame update
    void Start()
    {
        LoadData();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Close()
    {
        AudioManager.instance.btnSfx.Play();
        gameObject.SetActive(false);
    }

    public void ToggleMusic()
    {
        AudioManager.instance.btnSfx.Play();
        int musicState = PlayerPrefs.GetInt("Music");
        if (musicState == 0)
        {

            musicOn.SetActive(false);
            musicOff.SetActive(true);
            PlayerPrefs.SetInt("Music", 1);
            AudioManager.instance.ToogleMusic(false);
        }
        else
        {

            musicOn.SetActive(true);
            musicOff.SetActive(false);

            PlayerPrefs.SetInt("Music", 0);
            AudioManager.instance.ToogleMusic(true);
        }
    }

    public void LoadData()
    {
        int musicState = PlayerPrefs.GetInt("Music");
        if (musicState == 1)
        {

            musicOn.SetActive(false);
            musicOff.SetActive(true);

            AudioManager.instance.ToogleMusic(false);
        }
        else
        {

            musicOn.SetActive(true);
            musicOff.SetActive(false);
            AudioManager.instance.ToogleMusic(true);
        }

       
    }

    public void OpenPolicy()
    {
        Application.OpenURL(policyUrl);
    }

    public void OpenTerm()
    {
        Application.OpenURL(termUrl);
    }
}
