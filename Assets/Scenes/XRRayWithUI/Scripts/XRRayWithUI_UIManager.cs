using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XRRayWithUI_UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject uiToControl;

    public void ToggleUI()
    {
        bool isActive = uiToControl.activeSelf;
        isActive = !isActive;
        uiToControl.SetActive(isActive);
    }
}
