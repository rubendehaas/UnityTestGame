using UnityEngine;
using System.Collections;

public class GroundMover : MonoBehaviour {

    public int speed;
    public float maxPosition;

    private Rigidbody2D rb2d;
    private Transform playerObject;
    
	void Start () {
        rb2d = GetComponent<Rigidbody2D>();
    }
	
	void Update () {

        if (rb2d.transform.position.x == maxPosition)
        {
            maxPosition = -maxPosition;
        }

        Vector3 endPosition = new Vector3(maxPosition, rb2d.transform.position.y);
        rb2d.transform.position = Vector3.MoveTowards(rb2d.transform.position, endPosition, speed * Time.deltaTime);
    }
}
