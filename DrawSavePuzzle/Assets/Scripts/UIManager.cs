using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarkSDKSpace;
using TTSDK.UNBridgeLib.LitJson;

public class UIManager : MonoBehaviour
{

    public UIGame uiGame;

    public UIWin uiWin;

    public ParticleSystem winFx1, winFx2;

    public Transform winTick, failTick;

    private StarkAdManager starkAdManager;
    /// <summary>
    /// 播放插屏广告
    /// </summary>
    /// <param name="adId"></param>
    /// <param name="errorCallBack"></param>
    /// <param name="closeCallBack"></param>
    public void ShowInterstitialAd(string adId, System.Action closeCallBack, System.Action<int, string> errorCallBack)
    {
        starkAdManager = StarkSDK.API.GetStarkAdManager();
        if (starkAdManager != null)
        {
            var mInterstitialAd = starkAdManager.CreateInterstitialAd(adId, errorCallBack, closeCallBack);
            mInterstitialAd.Load();
            mInterstitialAd.Show();
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowWin()
    {
        ShowInterstitialAd("3hai1a0e71d53t86r7",
       () => {
           Debug.LogError("--插屏广告完成--");
           var data = new JsonData
           {
               ["event_type"] = "game_addiction",
               ["extra"] = "{product_name: '插屏广告完成'}",
           };
           StarkSDK.API.StarkSendToTAQ(data);
       },
       (it, str) => {
           Debug.LogError("Error->" + str);
           AndroidUIManager.ShowToast("广告加载异常，请稍后再试");
       });
        StartCoroutine(ShowWinIE());
    }

    public void ShowFail()
    {
        ShowInterstitialAd("3hai1a0e71d53t86r7",
       () => {
           Debug.LogError("--插屏广告完成--");
           var data = new JsonData
           {
               ["event_type"] = "game_addiction",
               ["extra"] = "{product_name: '插屏广告完成'}",
           };
           StarkSDK.API.StarkSendToTAQ(data);
       },
       (it, str) => {
           Debug.LogError("Error->" + str);
           AndroidUIManager.ShowToast("广告加载异常，请稍后再试");
       });
        StartCoroutine(ShowFailIE());
    }

    IEnumerator ShowWinIE()
    {
        AudioManager.instance.levelWinSfx.Play();
        winFx1.Play();
        winFx2.Play();
        winTick.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        winTick.gameObject.SetActive(false);
        uiWin.gameObject.SetActive(true);
    }

    IEnumerator ShowFailIE()
    {
       
        failTick.gameObject.SetActive(true);
        AudioManager.instance.levelFailSfx.Play();
        yield return new WaitForSeconds(1.5f);
        failTick.gameObject.SetActive(false);
        GameManager.instance.Replay();
    }

}
