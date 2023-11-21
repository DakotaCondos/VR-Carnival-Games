using UnityEngine;

public class SetLocalTransform : MonoBehaviour
{
    // Public methods to modify the Position, Rotation, and Scale.

    public void SetPosition(Vector3 newPosition)
    {
        transform.localPosition = newPosition;
    }

    public void SetRotation(Vector3 newRotation)
    {
        transform.localRotation = Quaternion.Euler(newRotation);
    }

    public void SetScale(Vector3 newScale)
    {
        transform.localScale = newScale;
    }

    // Public methods to individually modify the X, Y, and Z values.

    public void SetXPosition(float newX)
    {
        Vector3 newPosition = transform.localPosition;
        newPosition.x = newX;
        transform.localPosition = newPosition;
    }

    public void SetYPosition(float newY)
    {
        Vector3 newPosition = transform.localPosition;
        newPosition.y = newY;
        transform.localPosition = newPosition;
    }

    public void SetZPosition(float newZ)
    {
        Vector3 newPosition = transform.localPosition;
        newPosition.z = newZ;
        transform.localPosition = newPosition;
    }

    public void SetXRotation(float newAngleX)
    {
        Vector3 newRotation = transform.localEulerAngles;
        newRotation.x = newAngleX;
        transform.localRotation = Quaternion.Euler(newRotation);
    }

    public void SetYRotation(float newAngleY)
    {
        Vector3 newRotation = transform.localEulerAngles;
        newRotation.y = newAngleY;
        transform.localRotation = Quaternion.Euler(newRotation);
    }

    public void SetZRotation(float newAngleZ)
    {
        Vector3 newRotation = transform.localEulerAngles;
        newRotation.z = newAngleZ;
        transform.localRotation = Quaternion.Euler(newRotation);
    }

    public void SetXScale(float newScaleX)
    {
        Vector3 newScale = transform.localScale;
        newScale.x = newScaleX;
        transform.localScale = newScale;
    }

    public void SetYScale(float newScaleY)
    {
        Vector3 newScale = transform.localScale;
        newScale.y = newScaleY;
        transform.localScale = newScale;
    }

    public void SetZScale(float newScaleZ)
    {
        Vector3 newScale = transform.localScale;
        newScale.z = newScaleZ;
        transform.localScale = newScale;
    }
}
