using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRSocketInteractor))]
public class SocketValidator : MonoBehaviour
{
    private XRSocketInteractor socketInteractor;

    void Start()
    {
        socketInteractor = GetComponent<XRSocketInteractor>();
        socketInteractor.hoverEntered.AddListener(Validator());
    }

    private UnityAction<HoverEnterEventArgs> Validator()
    {
        return (hoverEvent) =>
        {
            if (socketInteractor.hasSelection)
            {
                Logger.Instance.LogError("There is already an object on this socket");
            }
        };
    }
}
