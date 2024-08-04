using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyWeapon : MonoBehaviour
{
    public Transform shotPoint;
    public GameObject bulletPrefab;
    Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetTrigger("Shot");
            Instantiate(bulletPrefab,shotPoint.position,transform.rotation);
        }
    }
}
