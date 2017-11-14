using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    List<CometMovement> comets = new List<CometMovement>();

    float m_pullForce;
    float m_radius;

    public float getRadius()
    {
        if (m_radius == 0)
            Start();

        return m_radius;
    }

    public void removeComet(CometMovement _comet)
    {
        comets.Remove(_comet);
    }

    void Start()
    {
        m_pullForce = 500;
        m_radius = 400;
    }

    void FixedUpdate()
    {
        foreach (CometMovement comet in comets)
            comet.addPull(m_pullForce, transform.position, m_radius);
    }

    void OnTriggerEnter(Collider _Collider)
    {
        CometMovement comet = _Collider.GetComponent<CometMovement>();

        if (comet != null && !comets.Contains(comet))
            comets.Add(comet);
    }
}
