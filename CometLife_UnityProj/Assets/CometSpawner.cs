using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CometSpawner : MonoBehaviour
{

    [SerializeField]
    Transform comet;

    float timer = 0;

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            timer = 1f;
            spawnComet();
        }
    }

    public void spawnComet()
    {
        Vector3 newPos = transform.position + new Vector3(Random.Range(-600, 600), Random.Range(-600, 600), Random.Range(-600, 600));

        Transform newComet = Instantiate(comet, newPos, Quaternion.identity);
        newComet.GetComponent<CometMovement>().setPlanet(GetComponent<Planet>());
        float randScale = Random.Range(0.05f, 0.3f);
        newComet.localScale = new Vector3(randScale, randScale, randScale);
    }
}