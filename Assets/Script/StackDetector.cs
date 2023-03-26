using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StackDetector : MonoBehaviour
{
    [SerializeField]
    private GameObject topObject = null;

    [SerializeField]
    private GameObject bottomObject = null;

    [SerializeField]
    private Text feedbackText = null;

    RaycastHit hit;

    void Update()
    {
        if (Physics.Raycast(bottomObject.transform.position, Vector3.up, out hit, 0.1f))
        {
            if (hit.collider.gameObject == topObject)
            {
                feedbackText.text = "Success!";
            }
            else
            {
                feedbackText.text = "Keep trying!";
            }
        }
    }
}
