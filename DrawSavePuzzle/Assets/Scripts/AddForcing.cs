using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddForcing : MonoBehaviour
{

    public Vector2 mForce;

    private Rigidbody2D mRigidbody;

    // Start is called before the first frame update
    void Awake()
    {
        mRigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddForce()
    {
        mRigidbody.AddForce(mForce);
    }
}
