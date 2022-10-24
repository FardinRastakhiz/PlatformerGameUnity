using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class RotateTowardsPlayer : MonoBehaviour
{
    public Transform player;
    private Vector3 rotationMask = new Vector3(0, 1, 0); // locks the Y axis so tooltip doesn't tilt up and down

    void LateUpdate()
    {
        Vector3 relativePos = player.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(-1*relativePos, Vector3.up);
        transform.rotation = Quaternion.Euler(Vector3.Scale(rotation.eulerAngles, rotationMask));
    }
}
