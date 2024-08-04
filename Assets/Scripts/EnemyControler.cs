using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControler : MonoBehaviour
{
    public GameObject bulletPrefab; // 弾のプレハブ
    public float bulletSpeed = 10f; // 弾の速度

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(transform.right); // オブジェクトの右方向ベクトルをログに出力
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // スペースキーが押されたら
        {
            Shoot();
        }
    }

    void Shoot()
    {
        // 弾を生成
        GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
        // 弾に右方向の力を加える
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * bulletSpeed; // 右方向に飛ばす
    }
}
