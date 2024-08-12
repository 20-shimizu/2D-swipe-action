using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyController : MonoBehaviour
{
    [SerializeField]
    protected float hp;
    protected Animator anim;

    public virtual void Damage(float damage)
    {
        hp -= damage;
        if (hp <= 0.0f) Die();
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }

    protected virtual void Attack() { }
}
