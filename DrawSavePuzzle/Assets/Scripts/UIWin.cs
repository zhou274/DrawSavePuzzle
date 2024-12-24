using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIWin : MonoBehaviour
{
    public float speed;

    public int startingPoint;

    public Vector2[] points;

    public Vector2[] gemPoints;

    public GameObject uiGemPrefab;

    public Transform gemRoot;

    private int i;

    public Transform bonusIndify;

    public int bonusCoin;

    public Button nextBtn;

    public Text bonusCoinTxt;

    float ratioBonus;

    // Start is called before the first frame update
    void Start()
    {
        bonusIndify.transform.localPosition = points[startingPoint];
        i = 0;

        AdsControl.Instance.ShowInterstitalRandom();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(bonusIndify.transform.localPosition, points[i]) <= 0.02f)
        {
            i++;

            if (i == points.Length)
                i = 0;
        }

        bonusIndify.transform.localPosition = Vector2.MoveTowards(bonusIndify.transform.localPosition, points[i], speed * Time.deltaTime);

        ratioBonus = (bonusIndify.transform.localPosition.x - points[0].x) / (points[1].x - points[0].x);

       

        if (Mathf.CeilToInt(ratioBonus * 7.0f) == 1)
            ratioBonus = 1.0f;
        else if (Mathf.CeilToInt(ratioBonus * 7.0f) == 2)
            ratioBonus = 1.5f;
        else if (Mathf.CeilToInt(ratioBonus * 7.0f) == 3)
            ratioBonus = 2.0f;
        else if (Mathf.CeilToInt(ratioBonus * 7.0f) == 4)
            ratioBonus = 3.0f;
        else if (Mathf.CeilToInt(ratioBonus * 7.0f) == 5)
            ratioBonus = 2.0f;
        else if (Mathf.CeilToInt(ratioBonus * 7.0f) == 6)
            ratioBonus = 1.5f;
        else if (Mathf.CeilToInt(ratioBonus * 7.0f) == 7)
            ratioBonus = 1.0f;
        UpdateBonusCoin();
    }

    void UpdateBonusCoin()
    {
        bonusCoin = Mathf.CeilToInt(ratioBonus * 10);
        bonusCoinTxt.text = "+" + Mathf.CeilToInt(ratioBonus * 10).ToString();
    }

    public void NextLevel()
    {
        nextBtn.interactable = false;
        speed = 0.0f;
        StartCoroutine(SpawnGemsIE());
    }

    IEnumerator MoveObject(Transform thisTransform, Vector3 startPos, Vector3 endPos, float time)
    {
        var i = 0.0f;
        var rate = 1.0f / time;
        while (i < 1.0f)
        {
            i += Time.deltaTime * rate;
            thisTransform.localPosition = Vector3.Lerp(startPos, endPos, i);
            yield return null;
        }
        AudioManager.instance.coinSfx.Play();
        GameManager.instance.AddGems(1);
        Destroy(thisTransform.gameObject);
    }

    IEnumerator SpawnGemsIE()
    {
        for(int i = 0; i < bonusCoin; i ++)
        {
            GameObject gemObj = Instantiate(uiGemPrefab, gemPoints[0], Quaternion.identity);

            gemObj.transform.parent = gemRoot;

            gemObj.transform.localPosition = gemPoints[0];

            StartCoroutine(MoveObject(gemObj.transform, gemPoints[0], gemPoints[1], 0.15f));

            yield return new WaitForSeconds(0.1f);

        }

        yield return new WaitForSeconds(0.5f);
        GameManager.instance.SaveGems();
        GameManager.instance.SkipLevel();
    }
}
