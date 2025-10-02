using UnityEngine;

public class PathPoint : MonoBehaviour
{
    public float size = 0.1f;

    public float WaitTime = 0.0f;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, size);
    }
}
