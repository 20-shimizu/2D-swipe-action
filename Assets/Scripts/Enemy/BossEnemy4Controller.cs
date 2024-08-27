using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossEnemy4Controller : EnemyController
{
    private enum BossState
    {
        IDLE,
        MOVE,
        PREPARE_ATTACK,
        ATTACK,
        DIE,
    }
    private BossState state = BossState.IDLE;
    [SerializeField]
    private GameObject dropItem;
    private Slider hpBar;
    private StageManager stageManager;
    private GameObject mainCamera;
    private MainCameraController cameraController;
    private GameObject attackCollider;
    private GameObject blackHole;
    private float count = 0.0f;

    private bool isFacingRight = false;
    private Vector3 scale;
    private Vector2 pos;
    private Vector2 nextPos;
    private Collider2D moveArea;
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        audioManager = GameObject.Find("AudioSource").GetComponent<AudioManager>();
        hpBar = transform.Find("Canvas/HPBar").gameObject.GetComponent<Slider>();
        hpBar.maxValue = hp;
        hpBar.value = hp;
        stageManager = GameObject.Find("StageManager").GetComponent<StageManager>();
        anim = GetComponent<Animator>();
        mainCamera = GameObject.Find("Main Camera");
        blackHole = transform.Find("BlackHole").gameObject;
        attackCollider = transform.Find("AttackCollider").gameObject;
        blackHole.SetActive(false);
        attackCollider.SetActive(false);
        scale = transform.localScale;
        pos = transform.position;
        moveArea = GameObject.Find("BossBattleArea").GetComponent<Collider2D>();
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case BossState.IDLE:
                count += Time.deltaTime;
                if (count > 3.0f)
                {
                    count = 0.0f;
                    state = BossState.MOVE;
                    GenerateNextPos();
                    isFacingRight = nextPos.x > pos.x;
                    if (isFacingRight) transform.localScale = scale;
                    else transform.localScale = new Vector3(-scale.x, scale.y, scale.z);
                }
                break;
            case BossState.MOVE:
                count += Time.deltaTime;
                pos += (nextPos - pos) * 0.05f;
                transform.position = pos;
                if (count > 3.0f)
                {
                    count = 0.0f;
                    anim.SetTrigger("PrepareAttack");
                    audioManager.PlaySE("BlackHole");
                    blackHole.SetActive(true);
                    state = BossState.PREPARE_ATTACK;
                    isFacingRight = player.transform.position.x > pos.x;
                    if (isFacingRight) transform.localScale = scale;
                    else transform.localScale = new Vector3(-scale.x, scale.y, scale.z);
                }
                break;
            case BossState.PREPARE_ATTACK:
                count += Time.deltaTime;
                if (count > 4.0f)
                {
                    count = 0.0f;
                    anim.SetTrigger("Attack");
                    state = BossState.ATTACK;
                }
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

    // animation event から実行
    protected override void Attack()
    {
        audioManager.PlaySE("SlashAttack");
        attackCollider.SetActive(true);
    }
    private void EndAttack()
    {
        blackHole.SetActive(false);
        attackCollider.SetActive(false);
        state = BossState.IDLE;
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
        audioManager.PlaySE("BossEnemyFall");
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
