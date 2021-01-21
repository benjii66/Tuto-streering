using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public class B_BoidSettings 
{
    [SerializeField, Range(1, 25)] int cohesionRadius = 7, separationDistance = 5, maxSpeed = 1, maxDistance = 25; //distanciation sociale

    public int CohesionRadius => cohesionRadius;
    public int SeparationDistance => separationDistance;
    public int MaxSpeed => maxSpeed;
    public int MaxDistance => maxDistance;
    


}
