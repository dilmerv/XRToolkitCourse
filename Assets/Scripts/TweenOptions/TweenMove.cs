using UnityEngine;

public class TweenMove : MonoBehaviour
{
    [SerializeField]
    private Vector3 targetDestination = new Vector3(2.0f, 0, 0);

    [SerializeField]
    private iTween.LoopType loopType = iTween.LoopType.pingPong;

    [SerializeField]
    private iTween.EaseType easeType = iTween.EaseType.easeInOutExpo;

    [SerializeField]
    private float speed = 1.0f;

    [SerializeField]
    private float delay = 0.1f;

    void Start()
    {
        iTween.MoveBy(gameObject, iTween.Hash("x", targetDestination.x, "y",
            targetDestination.y, "z", targetDestination.z, "easeType", easeType,
            "loopType", loopType, "delay", delay, "speed", speed));
    }
}
