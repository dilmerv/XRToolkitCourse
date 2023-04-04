using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ExperienceManager : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The parent object to use when looking for all interactables")]
    private Transform parentObject;

    private List<Transform> transforms = new List<Transform>();

    private Dictionary<int, TrackableTransform> cachedTransforms = 
        new Dictionary<int, TrackableTransform>();

    [SerializeField]
    private UnityEvent OnExperienceSimulationCompleted;

    [SerializeField]
    private UnityEvent OnResetExperiencedCompleted;

    private void Awake()
    {
        foreach (Transform transform in parentObject)
        {
            transforms.Add(transform);
            cachedTransforms.Add(transform.GetInstanceID(), new TrackableTransform
            {
                position = transform.position,
                rotation = transform.rotation,
            });
        }
    }

    public void ExperienceCompleted() => OnExperienceSimulationCompleted?.Invoke();

    public void Reset() => StartCoroutine(ResetTransforms());

    private IEnumerator ResetTransforms()
    {
        foreach (Transform updatedTransform in transforms)
        { 
            var updatedTransformRigidBody = updatedTransform.GetComponent<Rigidbody>();
            if (updatedTransformRigidBody != null)
            {
                updatedTransformRigidBody.velocity = Vector3.zero;
                updatedTransformRigidBody.angularVelocity = Vector3.zero;
            }

            var cachedTransform = cachedTransforms[updatedTransform.GetInstanceID()];
            updatedTransform.position = cachedTransform.position;
            updatedTransform.rotation = cachedTransform.rotation;
        }
        OnResetExperiencedCompleted?.Invoke();
        yield return null;
    }
}
