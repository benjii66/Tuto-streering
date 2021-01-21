using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using Random = UnityEngine.Random;
public class B_Boid : MonoBehaviour
{
    public event Action OnBoidUpdate = null;
    public event Action OnBoidBehaviour = null;
    [SerializeField] Vector3 cohesion = Vector3.zero, separation = Vector3.zero, alignement = Vector3.zero, velocity = Vector3.zero;

    B_Boid[] closeBoids = null;
    B_BoidSettings settings = null;
    Transform anchor = null;
    int separationCount = 0;
    public Vector3 Velocity => velocity;
    public Vector3 Separation { get => separation; set => separation = value; }

    public bool IsValid => anchor;

    private void Start()
    {
        InvokeRepeating("BoidBehaviour", Random.value * 2f, 2f);
    }

    private void Awake()
    {
        OnBoidBehaviour += () =>
        {
            closeBoids = GetCloseBoids();
            ResetBoidBehaviour();
            SetInitPosition();
            SetCohesion();
            SetSeparation();
            SetAlignement();
            SetVelocity();
        };

        OnBoidUpdate += () =>
        {
            UpdatePosition();
        };
    }

    private void Update()
    {
        OnBoidUpdate?.Invoke();
    }

    public void InitBoid(Transform _anchor, B_BoidSettings _settings)
    {
        anchor = _anchor;
        settings = _settings;

    }

    void BoidBehaviour() => OnBoidBehaviour?.Invoke();
    B_Boid[] GetCloseBoids() => Physics.OverlapSphere(transform.position, settings.CohesionRadius).Select(b => b.GetComponent<B_Boid>()).ToArray();

    void ResetBoidBehaviour()
    {
        cohesion = Vector3.zero;
        separation = Vector3.zero;
        alignement = Vector3.zero;
        velocity = Vector3.zero;
        separationCount = 0;
    }

    void SetInitPosition()
    {
        if (closeBoids.Length == 0) return;
        closeBoids.ToList().ForEach(b =>
        {
            cohesion += b.transform.position;
            alignement += b.Velocity;
            Vector3 _separationVector = transform.position - b.transform.position;
            if (_separationVector.sqrMagnitude > 0 && _separationVector.sqrMagnitude < Math.Pow(settings.SeparationDistance, 2))
            {
                separation += _separationVector / _separationVector.sqrMagnitude;
                separationCount++;
            }
        });
    }

    void SetCohesion()
    {
        cohesion = cohesion / (closeBoids.Length > 20 ? 20 : closeBoids.Length);

        cohesion = Vector3.ClampMagnitude(cohesion - transform.position, settings.MaxSpeed);
        //cohesion *= factorCohesion;

    }


    void SetSeparation()
    {
        if (separationCount > 0)
        {
            separation = separation / separationCount;
            separation = Vector3.ClampMagnitude(separation, settings.MaxSpeed);
        }
        //separation *= factorSeparation;
    }

    void SetAlignement()
    {
        alignement = alignement / (closeBoids.Length > 20 ? 20 : closeBoids.Length);
        alignement = Vector3.ClampMagnitude(alignement, settings.MaxSpeed);
        //alignement *= factorSeparation;
    }

    void SetVelocity() => velocity = Vector3.ClampMagnitude(cohesion + separation + alignement, settings.MaxSpeed);

    void UpdatePosition()
    {
        if (!IsValid) return;
        float _maxDistance = Vector3.Distance(transform.position, anchor.position);
        bool _needNormalizeVelocity = _maxDistance > settings.MaxDistance;

        if (_needNormalizeVelocity)
            velocity += (anchor.position - transform.position) / settings.MaxDistance;
        transform.position += velocity * Time.deltaTime;
    }
}

