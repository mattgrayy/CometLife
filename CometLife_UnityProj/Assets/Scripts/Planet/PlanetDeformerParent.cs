using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetDeformerParent : MonoBehaviour
{
    List<PlanetDeformerChild> deformerChilden = new List<PlanetDeformerChild>();

    public void setupChildren()
    {
        foreach(Transform child in transform)
        {
            PlanetDeformerChild deform = child.gameObject.GetComponent<PlanetDeformerChild>();

            if(deform == null)
                deform = child.gameObject.AddComponent<PlanetDeformerChild>();

            deform.setParent(this);

            if(!deformerChilden.Contains(deform))
                deformerChilden.Add(deform);
        }
    }

    public void childHit(Vector3 _hitPos, float _radius = 0)
    {
        foreach (PlanetDeformerChild child in deformerChilden)
        {
            child.deformMesh(_hitPos, _radius);
        }
    }

    public void removeComet(CometMovement _comet)
    {
        GetComponent<Planet>().removeComet(_comet);
    }

    void Awake()
    {
        setupChildren();
    }
}