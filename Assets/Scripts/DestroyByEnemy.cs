using UnityEngine;
using System.Collections;

public class DestroyByEnemy : MonoBehaviour {

    private SimplePlatformController simplePlatformController;

    void Start()
    {
        GameObject gameControllerObject = GameObject.FindWithTag("Player");

            simplePlatformController = gameControllerObject.GetComponent<SimplePlatformController>();

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!simplePlatformController.getBoosted())
        {
            if (other.CompareTag("Player"))
            {
                StartGame.instance.StartScene("Start");
            }
        }

        if (other.CompareTag("Missile"))
        {
            Destroy(gameObject);
        }
    }

}
