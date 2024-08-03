using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float pushPower = 1.0f;
    private Rigidbody2D rb;
    private Vector2 pushForce = Vector2.zero;
    private Vector2 startTouchPos = Vector2.zero;
    private Vector2 endTouchPos = Vector2.zero;
    float swipingTime = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startTouchPos = Input.mousePosition;
            Debug.Log("start : (" + startTouchPos.x + ", " + startTouchPos.y);
        }
        else if (Input.GetMouseButton(0))
        {
            swipingTime += Time.deltaTime;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            endTouchPos = Input.mousePosition;
            Debug.Log("end : (" + endTouchPos.x + ", " + endTouchPos.y);
            Vector2 swipeSpeed = (endTouchPos - startTouchPos) / swipingTime;
            pushForce = pushPower * swipeSpeed;
            swipingTime = 0.0f;
        }
    }

    void FixedUpdate()
    {
        if (pushForce != Vector2.zero)
        {
            rb.velocity = Vector2.zero;
            rb.AddForce(pushForce);
            pushForce = Vector2.zero;
        }
    }
}
