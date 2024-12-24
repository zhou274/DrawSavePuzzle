using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnItems : MonoBehaviour
{
    public GameObject item;

    protected float mTimer;

    public float spawnDuration;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        mTimer += Time.deltaTime;

        if(mTimer >= spawnDuration)
        {
            mTimer = 0.0f;
            Spawn();
        }
    }

    void Spawn()
    {
       GameObject itemPrefab =  Instantiate(item, transform.position, transform.rotation);
       itemPrefab.GetComponent<AddForcing>().AddForce();
    }
}
