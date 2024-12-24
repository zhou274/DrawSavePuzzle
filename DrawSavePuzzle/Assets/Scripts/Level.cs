using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public int levelIndex;

    public Transform[] physicObject;

    public Transform[] colliderObject;

    public Transform[] forceObject;

    public Transform[] spawnItemList;

    public Player mainPlayer;

    public FailTrigger failTrigger;

    public string levelQuestion;

    public GameObject hintObj;

    // Start is called before the first frame update
    void Start()
    {
        DeactivePhysicObject();
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    void DeactivePhysicObject()
    {
        for (int i = 0; i < physicObject.Length; i++)
        {
            physicObject[i].GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            
        }

        for (int i = 0; i < colliderObject.Length; i++)
        {
            colliderObject[i].GetComponent<Collider2D>().enabled = false;

        }

       

    }

    public void ActivePhysicObject()
    {
        for (int i = 0; i < physicObject.Length; i++)
        {
            physicObject[i].GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            
        }
        for (int i = 0; i < colliderObject.Length; i++)
        {
            colliderObject[i].GetComponent<Collider2D>().enabled = true;

        }

        for (int i = 0; i < forceObject.Length; i++)
            forceObject[i].GetComponent<AddForcing>().AddForce();

        for (int i = 0; i < spawnItemList.Length; i++)
            spawnItemList[i].gameObject.SetActive(true);

        mainPlayer.AddForce();
        mainPlayer.Walk();
    }
}
