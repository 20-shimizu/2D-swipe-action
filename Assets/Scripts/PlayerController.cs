using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private enum PLAYERSTATE
    {
        IDLE, // 移動速度が一定以下、攻撃判定なし
        AIM, // タップ中、時間の流れが遅くなる
        MOVE, // 移動速度が一定以上、攻撃判定あり
        ATTACK, // 敵と重なってる状態、素早く敵の判定領域を抜ける
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
    // ダメージを受けたときに押される力
    [SerializeField]
    private float pushedPower;
    // 敵に攻撃してすり抜けるときのスピード
    [SerializeField]
    private float attackingSpeed;
    // 攻撃後に敵のコライダーを抜けたときのスピード
    [SerializeField]
    private float finishAttackingSpeed;
    [SerializeField]
    private float hp;
    private Slider hpBar;

    private PLAYERSTATE state = PLAYERSTATE.IDLE;

    private TimeManager timeManager;
    private Animator anim;
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private GroundCheck groundCheck;

    private Vector2 pushForce = Vector2.zero;
    private Vector2 prevTouchPos = Vector2.zero;
    private Vector2 maxSwipeSpeedVec = Vector2.zero;
    private float prevVelocity = 0.0f;
    // 敵に攻撃した時点の速度ベクトル
    private Vector2 enteringEnemyVelocityVec = Vector2.zero;
    // スワイプ後移動速度が閾値を超えるまではtrue
    private bool isSpeedUping = false;

    // 右向いてるときはtrue
    private bool isFacingRight = true;
    void Start()
    {
        timeManager = GameObject.Find("TimeManager").GetComponent<TimeManager>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        groundCheck = transform.Find("GroundCheck").gameObject.GetComponent<GroundCheck>();
        hpBar = transform.Find("Canvas/HPBar").gameObject.GetComponent<Slider>();
        hpBar.maxValue = hp;
        hpBar.value = hp;
    }

    void Update()
    {
        isAiming = state == PLAYERSTATE.AIM;
        switch (state)
        {
            case PLAYERSTATE.IDLE:
                anim.SetBool("Idle", true);
                anim.SetBool("Aim", false);
                anim.SetBool("Move", false);
                break;
            case PLAYERSTATE.AIM:
                anim.SetBool("Idle", false);
                anim.SetBool("Aim", true);
                anim.SetBool("Move", false);
                break;
            case PLAYERSTATE.MOVE:
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
            state = PLAYERSTATE.AIM;
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
        else if (state != PLAYERSTATE.ATTACK)
        {
            // 速度上昇中にIDLEにならないための処理, isSpeedUpingはAddForce時にtrue
            if (rb.velocity.magnitude > thresMoveSpeed)
            {
                isSpeedUping = false;
            }
            else if (!isSpeedUping)
            {
                state = PLAYERSTATE.IDLE;
            }
        }

        if (isFacingRight)
        {
            hpBar.transform.localScale = new Vector3(0.01f, 0.01f, 1f);
        }
        else
        {
            hpBar.transform.localScale = new Vector3(-0.01f, 0.01f, 1f);

        }
        prevVelocity = rb.velocity.magnitude;
    }

    void FixedUpdate()
    {
        if (pushForce != Vector2.zero)
        {
            state = PLAYERSTATE.MOVE;
            rb.velocity = Vector2.zero;
            rb.AddForce(pushForce);
            pushForce = Vector2.zero;
            isSpeedUping = true;
        }
    }

    public void Damage(float damage)
    {
        hp -= damage;
        hpBar.value = hp;
        if (hp <= 0.0f) Die();
    }

    private void Die()
    {
        // (TODO)死亡演出
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "NormalEnemy")
        {
            timeManager.HitStop(0.1f);
            if (state == PLAYERSTATE.MOVE)
            {
                state = PLAYERSTATE.ATTACK;
                enteringEnemyVelocityVec = rb.velocity;
                anim.SetTrigger("Attack");
                other.gameObject.GetComponent<EnemyController>().Damage(attackDamage);
            }
            else if (state != PLAYERSTATE.ATTACK)
            {
                Damage(1.0f);
                rb.AddForce((transform.position - other.transform.position) * pushedPower);
            }
        }
        else if (other.gameObject.tag == "BossEnemy")
        {
            timeManager.HitStop(0.1f);
            if (state == PLAYERSTATE.MOVE)
            {
                state = PLAYERSTATE.ATTACK;
                enteringEnemyVelocityVec = rb.velocity;
                anim.SetTrigger("Attack");
                other.gameObject.GetComponent<BossController>().Damage(attackDamage);
            }
            else if (state != PLAYERSTATE.ATTACK)
            {
                Damage(1.0f);
                rb.AddForce((transform.position - other.transform.position) * pushedPower);
            }
        }
        else if (other.gameObject.tag == "Bullet")
        {
            // (TODO) 弾によってダメージ変更
            Damage(2.0f);
            Destroy(other.gameObject);
        }
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "NormalEnemy")
        {
            if (state == PLAYERSTATE.ATTACK)
            {
                rb.velocity = enteringEnemyVelocityVec.normalized * attackingSpeed;
            }
        }
        else if (other.gameObject.tag == "BossEnemy")
        {
            if (state == PLAYERSTATE.ATTACK)
            {
                rb.velocity = enteringEnemyVelocityVec.normalized * attackingSpeed;
            }
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "NormalEnemy")
        {
            if (state == PLAYERSTATE.ATTACK)
            {
                rb.velocity = enteringEnemyVelocityVec.normalized * finishAttackingSpeed;
                enteringEnemyVelocityVec = Vector2.zero;
                state = PLAYERSTATE.MOVE;
            }
        }
        else if (other.gameObject.tag == "BossEnemy")
        {
            if (state == PLAYERSTATE.ATTACK)
            {
                rb.velocity = enteringEnemyVelocityVec.normalized * finishAttackingSpeed;
                enteringEnemyVelocityVec = Vector2.zero;
                state = PLAYERSTATE.MOVE;
            }
        }
    }
}
