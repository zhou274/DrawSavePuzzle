using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Vector2 jumpForce;

    private float attackTimer;

    public float attackDuration;

    private Rigidbody2D mRigidbody;

    // Start is called before the first frame update
    void Start()
    {
        attackTimer = 0.0f;
        mRigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        attackTimer += Time.deltaTime;

        if(attackTimer >= attackDuration)
        {
            attackTimer = 0.0f;
            mRigidbody.AddForce(jumpForce);
            Debug.Log("JUMP");
        }
    }
}
