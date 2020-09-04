using UnityEngine;

public class MoveCamera : MonoBehaviour {

    public Transform player;
    public Camera actualCamera;
    public float acceleration = 1;

    void Start()
    {
        actualCamera.transform.parent = null;
    }
    
    void Update() {
        transform.position = player.transform.position;
        actualCamera.transform.position = transform.position;
        
        actualCamera.transform.rotation = Quaternion.Slerp(actualCamera.transform.rotation, transform.rotation, acceleration * Time.deltaTime);
    }
}
