using UnityEngine;
using System.Collections;

public class StartGame : MonoBehaviour {

    public static StartGame instance;

    void Start()
    {
        instance = this;
    }

	public void StartScene(string scene)
    {
        Application.LoadLevel(scene);
    }
}
