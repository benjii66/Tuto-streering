using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B_BoidGenerator : MonoBehaviour
{
    [SerializeField] B_Boid boidItem = null;
    [SerializeField] B_BoidSettings globalSettings= null;
    [SerializeField, Range(1, 20)] int size= 10;
    [SerializeField, Range(1, 1000)] int boidCount = 300;


    public bool IsValid => boidItem;

    private void Start()
    {
        GenerateBoid();
    }

    void GenerateBoid()
    {
        if (!IsValid) return;

        for (int i = 0; i < boidCount; i++)
        {
            Vector3 _position = Random.insideUnitSphere * size;
            Instantiate(boidItem, _position, Quaternion.identity, transform).InitBoid(transform,globalSettings);
        }
    }
}
