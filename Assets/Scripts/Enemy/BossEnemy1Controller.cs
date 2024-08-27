using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossEnemy1Controller : EnemyController
{
    private enum BossState
    {
        ATTACK,
        MOVE,
        DIE,
    }
    private BossState state = BossState.ATTACK;

    private GameObject shotPoint;
    [SerializeField]
    private GameObject bullet;
    [SerializeField]
    private GameObject dropItem;
    private Slider hpBar;
    private StageManager stageManager;
    private GameObject mainCamera;
    private MainCameraController cameraController;
    private float attackCount = 0.0f;
    private float attackAngleOffset = 0.0f;

    private Vector2 pos;
    private Vector2 nextPos;
    private Collider2D moveArea;

    void Start()
    {
        shotPoint = transform.Find("ShotPoint").gameObject;
        hpBar = transform.Find("Canvas/HPBar").gameObject.GetComponent<Slider>();
        hpBar.maxValue = hp;
        hpBar.value = hp;
        stageManager = GameObject.Find("StageManager").GetComponent<StageManager>();
        anim = GetComponent<Animator>();
        mainCamera = GameObject.Find("Main Camera");
        pos = transform.position;
        moveArea = GameObject.Find("BossBattleArea").GetComponent<Collider2D>();
        InvokeRepeating("Attack", 0f, 0.3f);
        anim.SetTrigger("Attack");
    }
    void Update()
    {
        switch (state)
        {
            case BossState.ATTACK:
                attackCount += Time.deltaTime;
                if (attackCount > 1.4f)
                {
                    state = BossState.MOVE;
                    attackCount = 0.0f;
                    CancelInvoke();
                    GenerateNextPos();
                }
                break;
            case BossState.MOVE:
                attackCount += Time.deltaTime;
                pos += (nextPos - pos) * 0.05f;
                transform.position = pos;
                if (attackCount > 3.0f)
                {
                    state = BossState.ATTACK;
                    attackCount = 0.0f;
                    InvokeRepeating("Attack", 0f, 0.3f);
                    anim.SetTrigger("Attack");
                }
                break;
            case BossState.DIE:
            default:
                break;
        }

        if (transform.localScale.x > 0.0f)
        {
            hpBar.transform.localScale = new Vector3(0.01f, 0.01f, 1.0f);
        }
        else
        {
            hpBar.transform.localScale = new Vector3(-0.01f, 0.01f, 1.0f);
        }
    }

    protected override void Attack()
    {
        for (int angle = 0; angle < 360; angle += 90)
        {
            ShotBullet((float)angle + attackAngleOffset);
        }
        attackAngleOffset += 30.0f;
    }
    private void ShotBullet(float angleDeg)
    {
        GameObject b = Instantiate(bullet, shotPoint.transform.position, transform.rotation);
        b.GetComponent<BulletController>().Initialize(10.0f, angleDeg);
    }

    private void GenerateNextPos()
    {
        nextPos = new Vector2(transform.position.x + Random.Range(-8.0f, 8.0f), transform.position.y + Random.Range(-8.0f, 8.0f));
        while (!moveArea.OverlapPoint(nextPos))
        {
            nextPos = new Vector2(transform.position.x + Random.Range(-8.0f, 8.0f), transform.position.y + Random.Range(-8.0f, 8.0f));
        }
    }

    public override void Damage(float damage)
    {
        hp -= damage;
        hpBar.value = hp;
        if (hp <= 0.0f) Die();
    }

    // 死亡 → 死亡演出,state:BOSS_DYING → アイテム出現演出,state:GOAL_ITEM_APPEARING → 取得した能力の説明ダイアログ表示,ボタン押してマップへ戻る
    protected override void Die()
    {
        // ボス死亡後は全体の時間を止めるが、アニメーションは止めない
        anim.updateMode = AnimatorUpdateMode.UnscaledTime;
        state = BossState.DIE;
        stageManager.DieBossEnemy();
        CancelInvoke();
        anim.SetTrigger("Die");
    }

    // animation event から実行、ゴールアイテムの出現を開始する
    protected override void FinishDieAnimation()
    {
        stageManager.AppearGoalItem();
        Instantiate(dropItem, new Vector2(mainCamera.transform.position.x, mainCamera.transform.position.y + 8.0f), Quaternion.identity);
        Destroy(gameObject);
    }
}
