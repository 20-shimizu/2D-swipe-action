using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy0Controller : EnemyController
{
    private bool isFacingRight;
    private float moveSpeed = 3.0f;
    private float stopDuration = 2.0f;
    private float moveDuration = 2.0f;
    private Vector2 velocity = Vector2.zero;
    private Vector3 scale;
    private float moveCount = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        isFacingRight = transform.localScale.x > 0.0f;
        scale = transform.localScale;
        StartCoroutine(MoveLoop());
    }

    private IEnumerator MoveLoop()
    {
        while (true)
        {
            velocity.x = isFacingRight ? moveSpeed : -moveSpeed;
            anim.SetBool("Walk", true);
            while (moveCount < moveDuration)
            {
                rb.velocity = velocity;
                moveCount += Time.deltaTime;
                yield return null;
            }
            moveCount = 0.0f;
            velocity.x = 0.0f;
            rb.velocity = velocity;
            anim.SetBool("Walk", false);
            yield return new WaitForSeconds(stopDuration);
            isFacingRight = !isFacingRight;
            scale.x = -scale.x;
            transform.localScale = scale;
        }
    }

    protected override void Die()
    {
        anim.SetTrigger("Die");
    }
}
