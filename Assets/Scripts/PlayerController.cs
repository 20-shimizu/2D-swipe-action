using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private enum STATE
    {
        IDLE, // 移動速度が一定以下、攻撃判定なし
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
    [SerializeField]
    private float attackDamage;

    private STATE state = STATE.IDLE;

    private StageManager stageManager;
    private Animator anim;
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private GroundCheck groundCheck;
    private PlayerHPController hpController;
    private GameObject hpBar;

    private Vector2 pushForce = Vector2.zero;
    private Vector2 prevTouchPos = Vector2.zero;
    private Vector2 maxSwipeSpeedVec = Vector2.zero;
    // 右向いてるときはtrue
    private bool isFacingRight = true;
    void Start()
    {
        stageManager = GameObject.Find("StageManager").GetComponent<StageManager>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        groundCheck = transform.Find("GroundCheck").gameObject.GetComponent<GroundCheck>();
        hpController = GetComponent<PlayerHPController>();
        hpBar = transform.Find("Canvas/HPBar").gameObject;
    }

    void Update()
    {
        isAiming = state == STATE.AIM;
        switch (state)
        {
            case STATE.IDLE:
                anim.SetBool("Idle", true);
                anim.SetBool("Aim", false);
                anim.SetBool("Move", false);
                break;
            case STATE.AIM:
                anim.SetBool("Idle", false);
                anim.SetBool("Aim", true);
                anim.SetBool("Move", false);
                break;
            case STATE.MOVE:
                anim.SetBool("Idle", false);
                anim.SetBool("Aim", false);
                anim.SetBool("Move", true);
                break;
            default:
                break;
        }
        anim.SetBool("Fall", !groundCheck.IsGround());

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
            if (isFacingRight != pushForce.x > 0.0f && pushForce.x != 0.0f)
            {
                isFacingRight = !isFacingRight;
                Vector3 scale = transform.localScale;
                scale.x = -scale.x;
                transform.localScale = scale;
            }
            maxSwipeSpeedVec = Vector2.zero;
        }
        else
        {
            if (rb.velocity.magnitude > thresMoveSpeed) state = STATE.MOVE;
            else state = STATE.IDLE;
        }

        if (isFacingRight)
        {
            hpBar.transform.localScale = new Vector3(0.01f, 0.01f, 1f);
        }
        else
        {
            hpBar.transform.localScale = new Vector3(-0.01f, 0.01f, 1f);

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

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            stageManager.HitStop(0.1f);
            if (state == STATE.MOVE)
            {
                anim.SetTrigger("Attack");
                other.gameObject.GetComponent<EnemyHPController>().Damage(attackDamage);
            }
            else
            {
                hpController.Damage(1.0f);
            }
        }
        else if (other.gameObject.tag == "Bullet")
        {
            // (TODO) 弾によってダメージ変更
            hpController.Damage(2.0f);
            Destroy(other.gameObject);
        }
    }
}
