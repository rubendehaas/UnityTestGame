using UnityEngine;
using System.Collections;

public class MissileMover : MonoBehaviour {

    public float speed;
    private Rigidbody2D rb2d;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.velocity = new Vector2(rb2d.velocity.x, 1.0f * speed);
        //rigidbody.velocity = transform.forward * speed;
    }
}
