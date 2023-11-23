using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : MonoBehaviour
{
    [Header("Height Settings")]
    [SerializeField] float _minHeight = 0.5f;
    [SerializeField] float _maxHeight = 2.0f;



    public void SetTableHeight(float heightPercent)
    {
        Vector3 newHeight = transform.localPosition;
        newHeight.y = Mathf.Lerp(_minHeight, _maxHeight, heightPercent);
        transform.localPosition = newHeight;
    }

}
