using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ColliderScorer : MonoBehaviour
{
    [Header("PointValue")]
    [SerializeField] private int _pointValue = 1;

    [Header("GUI")]
    [SerializeField] private bool _useGUI = true;
    [SerializeField] private TextMeshProUGUI _pointText;
    private IColliderReader _colliderReader;

    private void Awake()
    {
        _colliderReader = gameObject.GetComponent<IColliderReader>();
        if (_colliderReader == null)
        {
            // IColliderReader Component must be on ColliderScorer GameObject
            throw new MissingComponentException($"Missing IColliderReader component for _colliderReader in " + gameObject.name);
        }
        if (_useGUI && _pointText == null)
        {
            throw new MissingReferenceException($"Missing TextMeshProUGUI reference for _pointText in " + gameObject.name);
        }
    }

    private void Start()
    {
        DisplayPointValue();
    }

    private void DisplayPointValue()
    {
        if (!_useGUI) { return; }
        string modifier = (_pointValue > 0) ? "+" : "";
        _pointText.text = $"{modifier}{_pointValue}";
    }

    public void UpdatePointValue(int value)
    {
        _pointValue = value;
        DisplayPointValue();
    }

    public int GetScore()
    {
        if (_colliderReader.IsColliding())
        {
            return _pointValue;
        }
        else return 0;
    }
}
