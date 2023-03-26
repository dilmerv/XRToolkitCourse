using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DemoSceneManager : MonoBehaviour
{
    [SerializeField]
    private Transform crateTransform = null;

    [SerializeField]
    private Transform coneTransform = null;

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

    public void ResetTransforms()
    {
        SetTransform(crateTransform, initialCratePosition, initialCrateRotation);
        SetTransform(coneTransform, initialConePosition, initialConeRotation);
    }

    private void SetTransform(Transform transform, Vector3 position, Quaternion rotation)
    {
        transform.position = position;
        transform.rotation = rotation;
    }
}
