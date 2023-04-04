using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public struct TrackableTransform
{ 
    public Vector3 position {  get; set; }
    public Quaternion rotation { get; set; }
}

public class ExperienceManager : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The parent object to use when looking for all interactables")]
    private Transform parentObject = null;

    private List<Transform> transforms = new List<Transform>();

    private Dictionary<int, TrackableTransform> cachedTransforms = new Dictionary<int, TrackableTransform>();

    [SerializeField]
    private UnityEvent OnExperienceSimulationCompleted;

    [SerializeField]
    private UnityEvent OnResetExperienceCompleted;

    private void Awake()
    {
        foreach (Transform transform in parentObject)
        {
            transforms.Add(transform);

            cachedTransforms.Add(transform.GetInstanceID(),
                new TrackableTransform
                {
                    position = transform.position,
                    rotation = transform.rotation,
                });
        }
    }

    public void ExperienceCompleted() => OnExperienceSimulationCompleted?.Invoke();

    public void Reset() => StartCoroutine(ResetTransformsAsync());

    private IEnumerator ResetTransformsAsync()
    {
        foreach(var updatedTransform in transforms)
        {
            var updatedTransformRigidBody = updatedTransform.GetComponent<Rigidbody>();
            if(updatedTransformRigidBody != null)
            {
                updatedTransformRigidBody.velocity = Vector3.zero;
                updatedTransformRigidBody.angularVelocity = Vector3.zero;
            }
            var cachedTransform = cachedTransforms[updatedTransform.GetInstanceID()];
            updatedTransform.SetPositionAndRotation(cachedTransform.position, cachedTransform.rotation);
        }

        OnResetExperienceCompleted?.Invoke();

        yield return null;
    }
}
