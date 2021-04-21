using UnityEngine;

public class CameraStabilizer : MonoBehaviour
{
    public Camera _Camera;
    public Vector3 local;
    void Start()
    {
        _Camera = Camera.main;
    }
    
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.One))
        {
            OVRManager.display.RecenterPose();
        }
    }
}
