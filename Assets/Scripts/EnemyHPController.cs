using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHPController : MonoBehaviour
{
    [SerializeField]
    private float hp;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

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
