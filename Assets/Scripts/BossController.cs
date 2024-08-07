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
    void Start()
    {
        hpBar = transform.Find("Canvas/HPBar").gameObject.GetComponent<Slider>();
        hpBar.maxValue = hp;
        hpBar.value = hp;
        stageManager = GameObject.Find("StageManager").GetComponent<StageManager>();
    }
    public void Damage(float damage)
    {
        hp -= damage;
        hpBar.value = hp;
        if (hp <= 0.0f) Die();
    }

    private void Die()
    {
        stageManager.AppearGoalItem();
        // (TODO)死亡演出
        Instantiate(dropItem, new Vector2(transform.position.x, transform.position.y + 8.0f), Quaternion.identity);
        Destroy(gameObject);
    }
}
