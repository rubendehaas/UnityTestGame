using UnityEngine;
using System.Collections;

public class SpawnManager : MonoBehaviour {

    public static SpawnManager instance;

    public GameObject background;

    public int maxPlatforms = 20;
    public float platformWait = 0.5f;
    public float startWait = 0.5f;
    public GameObject coinObject;
    public GameObject[] pickUps;

    public GameObject[] platforms;
    private float horizontalMin = -6f;
    private float horizontalMax = 6f;
    private float verticalMin = 2f;
    private float verticalMax = 4f;
    
    private float originY;
    private float originYBG;

	void Start () {
        instance = this;

        Spawn(maxPlatforms, 0);
    }
    
    public void Spawn(int spawnRate, float playerPositionY)
    {
        if (originY != 0)
            spawnRate = 1;
        else
            spawnRate = 30;

        for (int i = 0; i < spawnRate; i++)
        {
            if ((originY - playerPositionY) < 30)
            {
                float randomY = originY + Random.Range(verticalMin, verticalMax);
                float randomX = Random.Range(horizontalMin, horizontalMax);

                Vector2 randomPosition = new Vector2(randomX, randomY);
                Instantiate(ChoosePlatformType(), randomPosition, Quaternion.identity);

                originY = randomY;

                SpawnPickUp(randomPosition);
            }
        }
    }

    GameObject ChoosePlatformType()
    {
        int randomInt = Random.Range(0, 100);
        GameObject platform;

        if (randomInt > 20)
        {
            platform = platforms[1];
        }
        else
        {
            platform = platforms[0];
        }

        return platform;
    }

    void SpawnPickUp(Vector2 basePosition)
    {
        int pickUpRand = Random.Range(0, 100);

        if (pickUpRand > 70)
        {
            Vector2 coinPos = new Vector2(basePosition.x, basePosition.y + 1f);
            Instantiate(pickUps[1], coinPos, Quaternion.identity);
        }
        else if (pickUpRand < 3)
        {
            Vector2 coinPos = new Vector2(basePosition.x, basePosition.y + 1f);
            Instantiate(pickUps[0], coinPos, Quaternion.identity);
        }
        else if (pickUpRand > 3 && pickUpRand < 15)
        {
            Vector2 coinPos = new Vector2(basePosition.x, basePosition.y + 1f);
            Instantiate(pickUps[2], coinPos, Quaternion.identity);
        }
    }

    public void SpawnBackground(float playerPositionY)
    {
        if ((originYBG - playerPositionY) < 30)
        {
            if (originYBG == 0)
            {
                originYBG += 41;
            }

            Vector3 newBG = new Vector3(transform.position.x, originYBG, 1f);
            Instantiate(background, newBG, Quaternion.identity);

            if (originYBG != 0)
                originYBG += 12;
        }
    }
}
