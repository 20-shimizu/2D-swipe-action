using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    protected Rigidbody2D rb;
    [SerializeField]
    private float damage;

    public virtual void Initialize(float speed, float angle, bool reverse = false, float gravity = 0.0f)
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public float GetDamage() { return damage; }
}
