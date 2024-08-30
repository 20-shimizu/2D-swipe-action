using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotPoint : MonoBehaviour
{
    AudioManager audioManager;
    [SerializeField]
    private GameObject bullet;
    [SerializeField]
    private float bulletSpeed;
    Color32 bulletColor = new Color32(0, 60, 160, 255);

    void Start()
    {
        audioManager = GameObject.Find("AudioSource").GetComponent<AudioManager>();
    }

    // animation event から実行
    private void GenerateBullets()
    {
        audioManager.PlaySE("FireBullet");
        GameObject b1 = Instantiate(bullet, transform.position, transform.rotation);
        GameObject b2 = Instantiate(bullet, transform.position, transform.rotation);
        GameObject b3 = Instantiate(bullet, transform.position, transform.rotation);
        b1.GetComponent<BulletController>().Initialize(bulletSpeed, 245.0f);
        b2.GetComponent<BulletController>().Initialize(bulletSpeed, 270.0f);
        b3.GetComponent<BulletController>().Initialize(bulletSpeed, 295.0f);
        b1.GetComponent<SpriteRenderer>().color = bulletColor;
        b2.GetComponent<SpriteRenderer>().color = bulletColor;
        b3.GetComponent<SpriteRenderer>().color = bulletColor;
    }
    private void DestroyObject()
    {
        Destroy(gameObject);
    }
}
