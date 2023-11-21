using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

[Serializable]
public class UnityEventFloat : UnityEvent<float> { }

[Serializable]
public class XRSliderInteractable : XRGrabInteractable
{
    public enum Axis { X, Y, Z }
    [SerializeField] private Axis _movementAxis = Axis.X;
    [SerializeField] private Transform _sliderVisual;
    [SerializeField] private Transform _lowSliderBound;
    [SerializeField] private Transform _highSliderBound;
    [SerializeField] private bool _moveSliderVisual = true;

    private readonly float _minSliderValue = 0;
    private readonly float _maxSliderValue = 1;
    [SerializeField] private float _currentValue;
    private float _maxSliderDistance;

    private bool _isGrabbed = false;
    private Transform _parentTransform;

    [SerializeField]
    private UnityEventFloat _onValueChanged = new UnityEventFloat();
    public UnityEventFloat OnValueChanged => _onValueChanged;


    protected override void Awake()
    {
        base.Awake();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        _parentTransform = transform.parent.transform;

        #region SafetyChecks
        if (_sliderVisual == null)
        {
            throw new MissingReferenceException("Missing reference to slider visual object transform");
        }

        if (_lowSliderBound == null || _highSliderBound == null)
        {
            throw new MissingReferenceException("Missing reference to slider bound transform");
        }
        else
        {
            float lowValue = 0;
            float highValue = 0;

            switch (_movementAxis)
            {
                case Axis.X:
                    lowValue = _lowSliderBound.position.x;
                    highValue = _highSliderBound.position.x;
                    break;
                case Axis.Y:
                    lowValue = _lowSliderBound.position.y;
                    highValue = _highSliderBound.position.y;
                    break;
                case Axis.Z:
                    lowValue = -_lowSliderBound.position.z;
                    highValue = -_highSliderBound.position.z;
                    break;
            }

            if (highValue - lowValue <= 0)
            {
                throw new InvalidOperationException("High slider bound value is not greater than Low slider bound value.\nCheck that bounds are not flipped, and the correct slider axis is selected");
            }

            _maxSliderDistance = _movementAxis switch
            {
                Axis.X => _highSliderBound.localPosition.x - _lowSliderBound.localPosition.x,
                Axis.Y => _highSliderBound.localPosition.y - _lowSliderBound.localPosition.y,
                Axis.Z => _highSliderBound.localPosition.z - _lowSliderBound.localPosition.z,
                _ => throw new NotImplementedException(),
            };
        }
        #endregion

        DetermineSliderValue();
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        transform.SetParent(_parentTransform);
        _isGrabbed = true;
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        _isGrabbed = false;
        MoveToSliderPosition();
    }

    private void MoveToSliderPosition()
    {
        transform.SetLocalPositionAndRotation(_sliderVisual.localPosition, _sliderVisual.localRotation);
    }

    private void MoveSliderVisual()
    {
        Vector3 newPosition = _sliderVisual.localPosition;
        switch (_movementAxis)
        {
            case Axis.X:
                newPosition.x = Mathf.Clamp(transform.localPosition.x, _lowSliderBound.localPosition.x, _highSliderBound.localPosition.x);
                break;
            case Axis.Y:
                newPosition.y = Mathf.Clamp(transform.localPosition.y, _lowSliderBound.localPosition.y, _highSliderBound.localPosition.y);
                break;
            case Axis.Z:
                newPosition.z = Mathf.Clamp(transform.localPosition.z, _lowSliderBound.localPosition.z, _highSliderBound.localPosition.z);
                break;
        }
        _sliderVisual.localPosition = newPosition;
    }

    private void Update()
    {
        if (_isGrabbed)
        {
            //Vector3 newPosition = _initialPosition;

            //switch (_movementAxis)
            //{
            //    case Axis.X:
            //        newPosition.x = Mathf.Clamp(transform.localPosition.x, _lowSliderBound.localPosition.x, _highSliderBound.localPosition.x);
            //        break;
            //    case Axis.Y:
            //        newPosition.y = Mathf.Clamp(transform.localPosition.y, _lowSliderBound.localPosition.y, _highSliderBound.localPosition.y);
            //        break;
            //    case Axis.Z:
            //        newPosition.z = Mathf.Clamp(transform.localPosition.z, _lowSliderBound.localPosition.z, _highSliderBound.localPosition.z);
            //        break;
            //}

            //transform.position = newPosition;
            if (_moveSliderVisual) { MoveSliderVisual(); }
            UpdateCurrentValue();
        }
    }

    private void UpdateCurrentValue()
    {
        var initialValue = _currentValue;
        _currentValue = DetermineSliderValue();
        if (initialValue != _currentValue) { OnValueChanged?.Invoke(_currentValue); }
    }

    public float GetValue()
    {
        return _currentValue;
    }

    private float DetermineSliderValue()
    {
        float currentDistance = _movementAxis switch
        {
            Axis.X => transform.localPosition.x - _lowSliderBound.localPosition.x,
            Axis.Y => transform.localPosition.y - _lowSliderBound.localPosition.y,
            Axis.Z => transform.localPosition.z - _lowSliderBound.localPosition.z,
            _ => throw new NotImplementedException(),
        };

        float sliderValue = currentDistance / _maxSliderDistance;

        if (_minSliderValue <= sliderValue && sliderValue <= _maxSliderValue) { return sliderValue; }


        if (sliderValue < 0)
        {
            sliderValue = 0;
        }
        else
        {
            sliderValue = 1;
        }
        return sliderValue;
    }
}
