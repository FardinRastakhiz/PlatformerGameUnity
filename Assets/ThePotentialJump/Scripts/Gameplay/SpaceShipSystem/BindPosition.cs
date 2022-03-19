using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BindPosition : MonoBehaviour
{
    [SerializeField] private Transform targetBinding;
    [SerializeField] bool bindX;
    [SerializeField] bool bindY;
    [SerializeField] bool bindZ;
    private void Start()
    {
        StartCoroutine(UpdatePosition());
    }

    IEnumerator UpdatePosition()
    {
        var targetBasePosition = targetBinding.position;
        var basePosition = transform.position;
        while (true)
        {
            transform.position = new Vector3(
                bindX ? targetBinding.position.x - targetBasePosition.x : 0,
                bindY ? targetBinding.position.y - targetBasePosition.y : 0,
                bindZ ? targetBinding.position.z - targetBasePosition.z : 0
                ) + basePosition;
            yield return null;
        }
    }
}
