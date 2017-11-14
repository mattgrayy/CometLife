using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CometSpawner : MonoBehaviour {

    [SerializeField]Transform comet;

    public void spawnComet()
    {
        //400
        Vector3 newPos = transform.position + new Vector3(Random.Range(-600, 600), Random.Range(-600, 600), Random.Range(-600, 600));

        Instantiate(comet, newPos, Quaternion.identity).GetComponent<CometMovement>().setPlanet(GetComponent<Planet>());
    }
}
