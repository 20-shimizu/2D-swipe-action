using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossEnemy2Controller : EnemyController
{
    private enum BossState
    {
        BEFORE_ATTACK,
        ATTACK,
        MOVE,
        DIE,
    }
    private BossState state = BossState.BEFORE_ATTACK;
    private GameObject shotPoint;
    [SerializeField]
    private GameObject bullet;
    [SerializeField]
    private GameObject dropItem;
    private Slider hpBar;
    private StageManager stageManager;
    private GameObject mainCamera;
    private MainCameraController cameraController;
    private float count = 0.0f;
    private Vector2 pos;
    private Vector2 nextPos;
    private Collider2D moveArea;
    // Start is called before the first frame update
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
        anim.SetBool("Attack", true);
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case BossState.BEFORE_ATTACK:
                count += Time.deltaTime;
                if (count > 2.0f)
                {
                    count = 0.0f;
                    InvokeRepeating("Attack", 0f, 0.1f);
                    state = BossState.ATTACK;
                }
                break;
            case BossState.ATTACK:
                count += Time.deltaTime;
                if (count > 1.5f)
                {
                    count = 0.0f;
                    CancelInvoke();
                    GenerateNextPos();
                    anim.SetBool("Attack", false);
                    state = BossState.MOVE;
                }
                break;
            case BossState.MOVE:
                count += Time.deltaTime;
                pos += (nextPos - pos) * 0.01f;
                transform.position = pos;
                if (count > 5.0f)
                {
                    state = BossState.BEFORE_ATTACK;
                    count = 0.0f;
                    anim.SetBool("Attack", true);
                }
                break;
            case BossState.DIE:
            default:
                break;
        }
    }

    protected override void Attack()
    {
        GameObject b1 = Instantiate(bullet, shotPoint.transform.position, transform.rotation);
        GameObject b2 = Instantiate(bullet, shotPoint.transform.position, transform.rotation);
        b1.GetComponent<BulletController>().Initialize(Random.Range(9.0f, 12.0f), Random.Range(60.0f, 80.0f), gravity: 1.0f);
        b2.GetComponent<BulletController>().Initialize(Random.Range(9.0f, 12.0f), Random.Range(100.0f, 120.0f), gravity: 1.0f);
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
    protected override void Die()
    {
        // ボス死亡後は全体の時間を止めるが、アニメーションは止めない
        anim.updateMode = AnimatorUpdateMode.UnscaledTime;
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
