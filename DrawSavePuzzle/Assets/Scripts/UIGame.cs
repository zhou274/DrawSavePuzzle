using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TTSDK.UNBridgeLib.LitJson;
using TTSDK;
using StarkSDKSpace;

public class UIGame : MonoBehaviour
{
    public Text timerText;

    public Text questionTxt;

    public Text gemsTxt;

    public Text levelTxt;

    public float cownDownTimer;

    private float timer;

    public bool startTimer, isWin;

    public GameObject GetGemPanel;

    private StarkAdManager starkAdManager;

    public string clickid;
    // Start is called before the first frame update
    void Start()
    {
        startTimer = false;
        isWin = false;
        timer = cownDownTimer;
    }

    // Update is called once per frame
    void Update()
    {
        if (!startTimer)
            return;

        if (isWin)
            return;

        timer -= Time.deltaTime;
        timerText.text = Mathf.CeilToInt(timer).ToString();
        if(timer <= 0.0f)
        {
            isWin = true;
            timer = 0.0f;
            timerText.gameObject.SetActive(false);
            GameManager.instance.CheckResult();
        }
    }

    public void StartTimer()
    {
        startTimer = true;
        timerText.gameObject.SetActive(true);
    }

    public void ShowQuestion(string _question)
    {
        if (_question != "")
            questionTxt.text = _question;
    }

    public void Skip()
    {
        ShowVideoAd("192if3b93qo6991ed0",
            (bol) => {
                if (bol)
                {
                    Debug.Log("xxx");
                    SceneManager.LoadScene("Game");
                    clickid = "";
                    getClickid();
                    apiSend("game_addiction", clickid);
                    apiSend("lt_roi", clickid);
                }
                else
                {
                    StarkSDKSpace.AndroidUIManager.ShowToast("观看完整视频才能获取奖励哦！");
                }
            },
            (it, str) => {
                Debug.LogError("Error->" + str);
                //AndroidUIManager.ShowToast("广告加载异常，请重新看广告！");
            });
        //SceneManager.LoadScene("Game");
        //AdsControl.Instance.ShowRewardedAd(AdsControl.REWARD_TYPE.SKIP_LEVEL);
    }

    public void ShowHint()
    {
        if (GameManager.instance.gems >= 100)
        {
            AudioManager.instance.coinSfx.Play();
            GameManager.instance.gems -= 100;
            GameManager.instance.SaveGems();
            ShowGems(GameManager.instance.gems);
            GameManager.instance.ShowHint();
        }
        else
        {
            GetGemPanel.SetActive(true);
        }
    }
    

    public void ShowGems(int _value)
    {
        gemsTxt.text = _value.ToString();
    }

    public void ShowLevel(int _value)
    {
        levelTxt.text = "当前关卡 " + _value.ToString();
    }
    public void getClickid()
    {
        var launchOpt = StarkSDK.API.GetLaunchOptionsSync();
        if (launchOpt.Query != null)
        {
            foreach (KeyValuePair<string, string> kv in launchOpt.Query)
                if (kv.Value != null)
                {
                    Debug.Log(kv.Key + "<-参数-> " + kv.Value);
                    if (kv.Key.ToString() == "clickid")
                    {
                        clickid = kv.Value.ToString();
                    }
                }
                else
                {
                    Debug.Log(kv.Key + "<-参数-> " + "null ");
                }
        }
    }

    public void apiSend(string eventname, string clickid)
    {
        TTRequest.InnerOptions options = new TTRequest.InnerOptions();
        options.Header["content-type"] = "application/json";
        options.Method = "POST";

        JsonData data1 = new JsonData();

        data1["event_type"] = eventname;
        data1["context"] = new JsonData();
        data1["context"]["ad"] = new JsonData();
        data1["context"]["ad"]["callback"] = clickid;

        Debug.Log("<-data1-> " + data1.ToJson());

        options.Data = data1.ToJson();

        TT.Request("https://analytics.oceanengine.com/api/v2/conversion", options,
           response => { Debug.Log(response); },
           response => { Debug.Log(response); });
    }


    /// <summary>
    /// </summary>
    /// <param name="adId"></param>
    /// <param name="closeCallBack"></param>
    /// <param name="errorCallBack"></param>
    public void ShowVideoAd(string adId, System.Action<bool> closeCallBack, System.Action<int, string> errorCallBack)
    {
        starkAdManager = StarkSDK.API.GetStarkAdManager();
        if (starkAdManager != null)
        {
            starkAdManager.ShowVideoAdWithId(adId, closeCallBack, errorCallBack);
        }
    }
}
