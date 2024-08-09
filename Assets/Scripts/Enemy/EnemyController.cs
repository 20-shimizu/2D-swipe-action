using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private float hp;
    public Transform shotPoint;
    public GameObject bulletPrefab;
    Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetTrigger("Shot");
            Instantiate(bulletPrefab, shotPoint.position, transform.rotation);
        }
    }

    public void Damage(float damage)
    {
        hp -= damage;
        if (hp <= 0.0f) Die();
    }

    private void Die()
    {
        // (TODO)死亡演出
        Destroy(gameObject);
    }
}
