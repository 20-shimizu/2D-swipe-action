using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossController : MonoBehaviour
{
    [SerializeField]
    private GameObject dropItem;
    [SerializeField]
    private float hp;
    private Slider hpBar;
    private StageManager stageManager;
    private Animator anim;
    private GameObject mainCamera;
    private MainCameraController cameraController;
    void Start()
    {
        hpBar = transform.Find("Canvas/HPBar").gameObject.GetComponent<Slider>();
        hpBar.maxValue = hp;
        hpBar.value = hp;
        stageManager = GameObject.Find("StageManager").GetComponent<StageManager>();
        anim = GetComponent<Animator>();
        mainCamera = GameObject.Find("Main Camera");
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
