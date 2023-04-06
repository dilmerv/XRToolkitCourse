using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRSocketInteractor))]
public class SocketRelease : MonoBehaviour
{
    [SerializeField]
    private float releaseAfterSeconds = 0.15f;

    [SerializeField]
    private float frequencyCheckForIntersects = 0.5f;

    private XRSocketInteractor socketInteractor;

    private IXRSelectInteractable lastSelected;

    private Collider socketCollider;

    private void Start()
    {
        socketInteractor = GetComponent<XRSocketInteractor>();
        socketCollider = GetComponent<Collider>();

        socketInteractor.selectEntered.AddListener((s) =>
        {
            lastSelected = s.interactableObject;
            // coroutine to release objects with physics
            StartCoroutine(Release(lastSelected));
        });

        // if object was selected and it no longer intercects
        // then re-activate socket
        StartCoroutine(MonitorIntersectsBetweenColliders());
    }

    private IEnumerator Release(IXRSelectInteractable selectInteractable)
    {
        yield return new WaitForSeconds(releaseAfterSeconds);
        socketInteractor.socketActive = false;
        selectInteractable.transform.GetComponent<Rigidbody>().isKinematic = false;
    }

    private IEnumerator MonitorIntersectsBetweenColliders()
    {
        while (true)
        {
            if (lastSelected != null && !AnyColliderIntercects(lastSelected.colliders))
            {
                socketInteractor.socketActive = true;
            }
            yield return new WaitForSeconds(frequencyCheckForIntersects);
        }
    }

    private bool AnyColliderIntercects(IEnumerable<Collider> targetColliders)
    {
        foreach (Collider collider in targetColliders)
        {
            if (socketCollider.bounds.Intersects(collider.bounds))
            {
                return true;
            }
        }
        return false;
    }
}
