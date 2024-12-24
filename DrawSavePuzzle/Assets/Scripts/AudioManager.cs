using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public AudioSource coinSfx;

    public AudioSource btnSfx;

    public AudioSource levelWinSfx;

    public AudioSource levelFailSfx;

    public AudioSource[] audioArr;

    public AudioSource backgroundMusic;

    public static AudioManager instance;

    private void Awake()
    {
        int musicState = PlayerPrefs.GetInt("Music");
        if (musicState == 0)
        {
          
            ToogleMusic(true);
           

        }
        else
        {
           
            ToogleMusic(false);
          

        }

        int soundState = PlayerPrefs.GetInt("Sound");
        if (soundState == 0)
        {
           
            ToogleSound(true);
           
        }
        else
        {
         
            ToogleSound(false);
            
        }

        if (FindObjectsOfType(typeof(AudioManager)).Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

       
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToogleMusic(bool toogle)
    {
        if(toogle)
          backgroundMusic.volume = 1.0f;
        else
            backgroundMusic.volume = 0.0f;
    }

    public void ToogleSound(bool toogle)
    {
        if (toogle)
        {

            for (int i = 0; i < audioArr.Length; i++)
                audioArr[i].volume = 1.0f;
        }

        else
        {

            for (int i = 0; i < audioArr.Length; i++)
                audioArr[i].volume = 0.0f;
        }
    }
}
