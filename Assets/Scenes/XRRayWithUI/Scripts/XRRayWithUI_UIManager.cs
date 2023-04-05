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

    public void ClearAllSettings()
    {
        Logger.Instance.LogInfo("Clearning all settings");
    }
}
