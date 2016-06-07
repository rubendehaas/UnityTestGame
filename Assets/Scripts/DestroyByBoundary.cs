using UnityEngine;
using System.Collections;

public class DestroyByBoundary : MonoBehaviour {

    void OnTriggerExit2D(Collider2D other)
    {
        
        if (other.CompareTag("Player"))
        {
            StartGame.instance.StartScene("Start");
        }
        else
        {
            Destroy(other.gameObject);
        }
    }
}
