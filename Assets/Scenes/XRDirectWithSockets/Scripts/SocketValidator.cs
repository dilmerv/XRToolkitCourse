using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRSocketInteractor))]
public class SocketValidator : MonoBehaviour
{
    private XRSocketInteractor socketInteractor = null;

    // Start is called before the first frame update
    void Start()
    {
        socketInteractor = GetComponent<XRSocketInteractor>();
        socketInteractor.hoverEntered.AddListener((hoverEvent) =>
        {
            if(socketInteractor.hasSelection)
            {
                Logger.Instance.LogWarning("There is an object already in socket");
            }
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
