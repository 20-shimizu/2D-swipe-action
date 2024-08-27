using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyController : MonoBehaviour
{
    [SerializeField]
    protected float hp;
    protected Animator anim;
    protected Rigidbody2D rb;
    protected AudioManager audioManager;

    public virtual void Damage(float damage)
    {
        hp -= damage;
        if (hp <= 0.0f) Die();
    }

    // animation eventから呼び出し
    protected virtual void Die()
    {
        anim.SetTrigger("Die");
    }

    // animation event から実行
    protected virtual void FinishDieAnimation()
    {
        Destroy(gameObject);
    }

    protected virtual void Attack() { }
}
