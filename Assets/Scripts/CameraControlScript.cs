using UnityEngine;

public class CameraControlScript : MonoBehaviour
{
    public GameObject player;

    public float offsetX = -5f;
    public float offsetZ = 0f;
    public float offsetY = 3f;

    public float smoothTime = 0.3f;
        
    public float xRotation = 30f;
    public float yRotation = 30f;
    public float zRotation = 30f;

    private Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {
        if (player is null) return;
            
        Vector3 targetPosition = new Vector3(player.transform.position.x + offsetX, player.transform.position.y + offsetY, player.transform.position.z + offsetZ);

        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
            
        transform.rotation = Quaternion.Euler(xRotation, yRotation, zRotation);
    }
}
