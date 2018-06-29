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
            if (child.name != "SplitChild")
                continue;

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
        _radius *= 3;// Random.Range(1.0f, 3.0f);
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