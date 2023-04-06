using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRSocketInteractor))]
public class SocketUsed : MonoBehaviour
{
    [SerializeField]
    private float releaseAfterSeconds = 0.5f;

    [SerializeField]
    private float selectionCheckerSeconds = 0.5f;

    private XRSocketInteractor socketInteractor;

    private IXRSelectInteractable lastSelected;

    private Collider socketCollider;

    void Start()
    {
        socketInteractor = GetComponent<XRSocketInteractor>();
        socketCollider = GetComponent<Collider>();

        socketInteractor.selectEntered.AddListener((s) =>
        {
            lastSelected = s.interactableObject;
            StartCoroutine(Release(s.interactableObject));
        });
        
        // if object was selected and it no longer intercepts then
        // re-activate socket
        StartCoroutine(MonitorSelectionColliders());
    }

    private IEnumerator Release(IXRSelectInteractable selectInteractable)
    {
        yield return new WaitForSeconds(releaseAfterSeconds);
        socketInteractor.socketActive = false;

        selectInteractable.transform
            .GetComponent<Rigidbody>().isKinematic = false;
    }

    private IEnumerator MonitorSelectionColliders()
    {
        while (true)
        {
            if (lastSelected != null && !AnyColliderIntercects(lastSelected.colliders))
            {
                socketInteractor.socketActive = true;
            }
            yield return new WaitForSeconds(selectionCheckerSeconds);
        }
    }

    private bool AnyColliderIntercects(IEnumerable<Collider> targetColliders)
    {
        foreach (Collider targetCollider in targetColliders)
        {
            if (socketCollider.bounds.Intersects(targetCollider.bounds))
            {
                return true;
            }
        }
        return false;
    }
}
