using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [HideInInspector]
    public bool isDie;

    private SpriteRenderer playerSpr;

    private Rigidbody2D mRigidboy;

    public Vector2 initForce;

    public Vector2 walkSpeed;

    public bool canWalk;

    // Start is called before the first frame update
    void Start()
    {
        isDie = false;
        playerSpr = GetComponent<SpriteRenderer>();
        mRigidboy = GetComponent<Rigidbody2D>();
       
    }

    public void AddForce()
    {
        mRigidboy.AddForce(initForce);
    }

    public void Walk()
    {
        if(canWalk)
           mRigidboy.velocity = walkSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Kill()
    {
        isDie = true;

        if(playerSpr != null)
          StartCoroutine(FlashSprite(playerSpr, 1.0f, 0.05f));
    }

    IEnumerator FlashSprite(SpriteRenderer sprite, float time, float intervalTime)
    {
        Color[] colors = {Color.white, new Color(1,1,1,0.5f)};

        Color originalColor = sprite.color;

        float elapsedTime = 0f;
        int index = 0;
        while (elapsedTime < time)
        {
            sprite.color = colors[index % 2];

            elapsedTime += Time.deltaTime;
            index++;
            yield return new WaitForSeconds(intervalTime);
        }

        sprite.color = originalColor;

    }
}
