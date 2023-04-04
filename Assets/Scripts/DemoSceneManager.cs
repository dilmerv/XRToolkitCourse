using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class DemoSceneManager : MonoBehaviour
{
    [SerializeField]
    private Transform crateTransform = null;

    [SerializeField]
    private Transform coneTransform = null;

    [SerializeField]
    private XRSocketInteractor[] socketInteractors;

    [SerializeField]
    private UnityEvent OnDemoSceneSimulationCompleted;

    [SerializeField]
    private UnityEvent OnResetSceneSimulationCompleted;

    private Vector3 initialCratePosition;
    private Quaternion initialCrateRotation;

    private Vector3 initialConePosition;
    private Quaternion initialConeRotation;

    private void Awake()
    {
        initialCratePosition = crateTransform.position;
        initialCrateRotation = crateTransform.rotation;

        initialConePosition = coneTransform.position;
        initialConeRotation = coneTransform.rotation;
    }

    public void SceneSimulationCompleted() => OnDemoSceneSimulationCompleted?.Invoke();

    public void ResetTransforms()
    {
        StartCoroutine(ResetTransformsAsync());
    }

    private IEnumerator ResetTransformsAsync()
    {
        foreach (var socket in socketInteractors)
        {
            socket.enabled = false;
        }

        yield return new WaitForEndOfFrame();

        SetTransform(crateTransform, initialCratePosition, initialCrateRotation);
        SetTransform(coneTransform, initialConePosition, initialConeRotation);

        yield return new WaitForEndOfFrame();

        foreach (var socket in socketInteractors)
        {
            socket.enabled = true;
        }

        OnResetSceneSimulationCompleted?.Invoke();
    }

    private void SetTransform(Transform transform, Vector3 position, Quaternion rotation)
    {
        transform.position = position;
        transform.rotation = rotation;
    }
}
