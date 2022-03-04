using System.Collections;
using UnityEngine;

public class ActivatePlatformCollider : MonoBehaviour
{
    [SerializeField] private Collider2D plaformCollider;
    [SerializeField] private Transform player;
    [SerializeField] private bool reverse;
    private void Start()
    {
        StartCoroutine(CheckforActivate());
    }

    private IEnumerator CheckforActivate()
    {
        while (true)
        {
            if (player.position.y >= transform.position.y + plaformCollider.bounds.size.y/2.0f)
            {
                plaformCollider.isTrigger = reverse ? true : false;
            }
            else if (player.position.y < transform.position.y - plaformCollider.bounds.size.y)
            {
                plaformCollider.isTrigger = reverse ? false : true;
            }
            yield return null;
        }
    }
}
