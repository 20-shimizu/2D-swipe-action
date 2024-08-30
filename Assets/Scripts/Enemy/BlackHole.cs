using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole : MonoBehaviour
{
    private PlayerController playerCtrl;
    [SerializeField]
    private float absorbPower;
    private float maxForce = 50.0f;
    // Start is called before the first frame update
    void Start()
    {
        playerCtrl = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 diff = transform.position - playerCtrl.transform.position;
        float force = Mathf.Clamp(absorbPower / (diff.magnitude * diff.magnitude), 0.0f, maxForce);
        playerCtrl.AddForce(force, diff);
    }
}
