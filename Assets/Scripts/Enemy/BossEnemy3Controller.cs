using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossEnemy3Controller : EnemyController
{
    private enum BossState
    {
        ATTACK,
        IDLE,
        MOVE,
        DIE,
    }
    private BossState state = BossState.IDLE;
    private GameObject attackCollider;
    [SerializeField]
    private GameObject shotPoint;
    [SerializeField]
    private GameObject dropItem;
    private Slider hpBar;
    private StageManager stageManager;
    private GameObject mainCamera;
    private MainCameraController cameraController;
    private float count = 0.0f;
    private Vector2 pos;

    private GameObject player;
    private bool isMovingToRight = false;
    private Vector3 scale;
    private Vector2 velocity = Vector2.zero;
    private float moveSpeed = 3.0f;

    private GameObject pivot;
    // Start is called before the first frame update
    void Start()
    {
        attackCollider = transform.Find("AttackCollider").gameObject;
        player = GameObject.Find("Player");
        attackCollider.SetActive(false);
        hpBar = transform.Find("Canvas/HPBar").gameObject.GetComponent<Slider>();
        hpBar.maxValue = hp;
        hpBar.value = hp;
        stageManager = GameObject.Find("StageManager").GetComponent<StageManager>();
        anim = GetComponent<Animator>();
        mainCamera = GameObject.Find("Main Camera");
        scale = transform.localScale;
        rb = GetComponent<Rigidbody2D>();
        pivot = transform.Find("Pivot").gameObject;
        pos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(pivot.transform.position.x - transform.position.x);
        switch (state)
        {
            case BossState.IDLE:
                count += Time.deltaTime;
                if (count > 3.0f)
                {
                    count = 0.0f;
                    // 画像の中心位置がずれているので反転時に調整
                    pos = transform.position;
                    if (isMovingToRight != pivot.transform.position.x < player.transform.position.x)
                    {
                        pos.x += 2.0f * (pivot.transform.position.x - transform.position.x);
                        transform.position = pos;
                        isMovingToRight = !isMovingToRight;
                        transform.localScale = isMovingToRight ? new Vector3(-scale.x, scale.y, scale.z) : scale;
                    }
                    velocity.x = isMovingToRight ? moveSpeed : -moveSpeed;
                    anim.SetTrigger("Move");
                    state = BossState.MOVE;
                }
                break;
            case BossState.MOVE:
                count += Time.deltaTime;
                rb.velocity = velocity;
                if (count > 2.0f)
                {
                    count = 0.0f;
                    rb.velocity = Vector2.zero;
                    if (Random.Range(0.0f, 1.0f) > 0.8f) anim.SetTrigger("Shot");
                    else anim.SetTrigger("Attack");
                    state = BossState.ATTACK;
                }
                break;
            case BossState.ATTACK:
            case BossState.DIE:
            default:
                break;
        }
    }

    // animation event から実行
    private void ActivateAttack() { attackCollider.SetActive(true); }
    private void ActivateShotPoint()
    {
        Instantiate(shotPoint, (Vector2)pivot.transform.position + new Vector2(Random.Range(5.0f, 8.0f), Random.Range(8.0f, 12.0f)), transform.rotation);
        Instantiate(shotPoint, (Vector2)pivot.transform.position + new Vector2(Random.Range(-8.0f, -5.0f), Random.Range(8.0f, 12.0f)), transform.rotation);
    }
    private void EndAttack()
    {
        attackCollider.SetActive(false);
        state = BossState.IDLE;
        anim.SetTrigger("Idle");
    }

    public override void Damage(float damage)
    {
        hp -= damage;
        hpBar.value = hp;
        if (hp <= 0.0f) Die();
    }
    protected override void Die()
    {
        state = BossState.DIE;
        stageManager.DieBossEnemy();
        CancelInvoke();
        anim.SetTrigger("Die");
    }
    protected override void FinishDieAnimation()
    {
        stageManager.AppearGoalItem();
        Instantiate(dropItem, new Vector2(mainCamera.transform.position.x, mainCamera.transform.position.y + 8.0f), Quaternion.identity);
        Destroy(gameObject);
    }
}
