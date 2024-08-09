using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossController : MonoBehaviour
{
    private enum BossState
    {
        ATTACK,
        STOP_ATTACK,
    }
    private BossState state = BossState.ATTACK;

    [SerializeField]
    private GameObject bullet;
    [SerializeField]
    private GameObject dropItem;
    [SerializeField]
    private float hp;
    private Slider hpBar;
    private StageManager stageManager;
    private Animator anim;
    private GameObject mainCamera;
    private MainCameraController cameraController;
    private float attackCount = 0.0f;
    private float attackAngleOffset = 0.0f;
    void Start()
    {
        hpBar = transform.Find("Canvas/HPBar").gameObject.GetComponent<Slider>();
        hpBar.maxValue = hp;
        hpBar.value = hp;
        stageManager = GameObject.Find("StageManager").GetComponent<StageManager>();
        anim = GetComponent<Animator>();
        mainCamera = GameObject.Find("Main Camera");

        InvokeRepeating("Attack", 0f, 0.1f);
    }
    void Update()
    {
        switch (state)
        {
            case BossState.ATTACK:
                attackCount += Time.deltaTime;
                if (attackCount > 1.0f)
                {
                    state = BossState.STOP_ATTACK;
                    attackCount = 0.0f;
                    CancelInvoke();
                }
                break;
            case BossState.STOP_ATTACK:
                attackCount += Time.deltaTime;
                if (attackCount > 2.0f)
                {
                    state = BossState.ATTACK;
                    attackCount = 0.0f;
                    InvokeRepeating("Attack", 0f, 0.1f);
                }
                break;
        }
    }

    private void Attack()
    {
        for (int angle = 0; angle < 360; angle += 90)
        {
            ShotBullet((float)angle + attackAngleOffset);
        }
        attackAngleOffset += 5.0f;
    }
    private void ShotBullet(float angleDeg)
    {
        GameObject b = Instantiate(bullet, transform.position, transform.rotation);
        b.GetComponent<BulletManager>().Initialize(5.0f, angleDeg);
    }

    public void Damage(float damage)
    {
        hp -= damage;
        hpBar.value = hp;
        if (hp <= 0.0f) Die();
    }

    // 死亡 → 死亡演出,state:BOSS_DYING → アイテム出現演出,state:GOAL_ITEM_APPEARING → 取得した能力の説明ダイアログ表示,ボタン押してマップへ戻る
    private void Die()
    {
        stageManager.DieBossEnemy();
        anim.SetTrigger("Die");
    }

    // animation event から実行、ゴールアイテムの出現を開始する
    private void FinishDieAnimation()
    {
        stageManager.AppearGoalItem();
        Instantiate(dropItem, new Vector2(mainCamera.transform.position.x, mainCamera.transform.position.y + 8.0f), Quaternion.identity);
        Destroy(gameObject);
    }
}
