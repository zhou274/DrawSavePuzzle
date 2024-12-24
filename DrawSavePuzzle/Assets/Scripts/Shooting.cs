using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    private Rigidbody2D mRigidbody;

    public Vector2 mVelocity;
    // Start is called before the first frame update
    void Start()
    {
        mRigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(mRigidbody.bodyType == RigidbodyType2D.Dynamic)
          mRigidbody.velocity = mVelocity;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
       
       // Destroy(gameObject);
    }
}
