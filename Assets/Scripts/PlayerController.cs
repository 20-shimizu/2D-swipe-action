using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private enum STATE
    {
        NEUTRAL, // 移動速度が一定以下、攻撃判定なし
        AIM, // タップ中、時間の流れが遅くなる
        MOVE, // 移動速度が一定以上、攻撃判定あり
    }

    [HideInInspector]
    public bool isAiming = false;

    [SerializeField]
    private float pushPower;
    // スワイプスピードの閾値、超えるとプレイヤーが動く
    [SerializeField]
    private float thresSwipeSpeed;
    // プレイヤーのスピードの閾値、超えているとき攻撃判定あり
    [SerializeField]
    private float thresMoveSpeed;

    private STATE state = STATE.NEUTRAL;

    private Rigidbody2D rb;
    private Vector2 pushForce = Vector2.zero;

    private Vector2 prevTouchPos = Vector2.zero;
    private Vector2 maxSwipeSpeedVec = Vector2.zero;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        isAiming = state == STATE.AIM;

        if (Input.GetMouseButtonDown(0))
        {
            state = STATE.AIM;
            prevTouchPos = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0))
        {
            Vector2 speedVec = ((Vector2)Input.mousePosition - prevTouchPos) / Time.deltaTime;
            if (speedVec.magnitude > maxSwipeSpeedVec.magnitude && speedVec.magnitude > thresSwipeSpeed) maxSwipeSpeedVec = speedVec;
            prevTouchPos = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            pushForce = pushPower * maxSwipeSpeedVec;
            maxSwipeSpeedVec = Vector2.zero;
        }
        else
        {
            if (rb.velocity.magnitude > thresMoveSpeed) state = STATE.MOVE;
            else state = STATE.NEUTRAL;
        }
    }

    void FixedUpdate()
    {
        if (pushForce != Vector2.zero)
        {
            rb.velocity = Vector2.zero;
            rb.AddForce(pushForce);
            pushForce = Vector2.zero;
        }
    }
}
