using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHPController : MonoBehaviour
{
    [SerializeField]
    private float hp;
    private Slider hpBar;
    void Start()
    {
        hpBar = transform.Find("Canvas/HPBar").gameObject.GetComponent<Slider>();
        hpBar.maxValue = hp;
        hpBar.value = hp;
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
}
