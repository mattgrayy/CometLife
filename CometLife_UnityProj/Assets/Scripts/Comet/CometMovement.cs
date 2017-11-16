using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CometMovement : MonoBehaviour {

    [SerializeField] Planet m_targetPlanet;

    Rigidbody m_RB;
    Vector3 m_rotation;
    Vector3 m_force;

    float m_maxVelocity = 300;
    float m_maxVelocitySqr;

    public void setPlanet(Planet _planet)
    {
        m_targetPlanet = _planet;
        Start();
    }

    public void addPull(float _force, Vector3 _pos, float _radius)
    {
        m_RB.AddExplosionForce(-_force, _pos, _radius);
    }

    public void stopMovement(bool _explode = false)
    {
        m_rotation = Vector3.zero;
        m_force = Vector3.zero;
        m_RB.velocity = Vector3.zero;
        m_RB.freezeRotation = true;
    }

    void Start ()
    {
        m_RB = GetComponent<Rigidbody>();
        m_rotation = new Vector3(Random.Range(0.0f, 0.05f), Random.Range(0.0f, 0.05f), Random.Range(0.0f, 0.05f));
        m_maxVelocitySqr = m_maxVelocity * m_maxVelocity;

        if(m_targetPlanet != null)
            m_force = Vector3.Normalize(new Vector3(m_targetPlanet.transform.position.x + Random.Range(0, m_targetPlanet.getRadius()/4),
                                                    m_targetPlanet.transform.position.y + Random.Range(0, m_targetPlanet.getRadius()/4),
                                                    m_targetPlanet.transform.position.z + Random.Range(0, m_targetPlanet.getRadius()/4)) - transform.position) * 20;
    }
	
	void FixedUpdate ()
    {
        m_RB.AddRelativeTorque(m_rotation);
        m_RB.AddForce(m_force);

        Vector3 rb_V = m_RB.velocity; 
        if(rb_V.sqrMagnitude > m_maxVelocitySqr)
            m_RB.velocity = rb_V.normalized * m_maxVelocity;
	}
}
