using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamMovement : MonoBehaviour {

    [SerializeField]
    Transform m_target;

	void Update ()
    {
        transform.RotateAround(m_target.position, Vector3.up * 2 + Vector3.forward, 20 * Time.deltaTime);
    }
}
