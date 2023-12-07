using UnityEngine;

public class CollisionData
{
    public Vector3 ContactPoint { get; private set; }
    public Vector3 DirectionToContactPoint { get; private set; }
    public Vector3 Impulse { get; private set; }
    public Collider Collider { get; private set; }

    public CollisionData(Vector3 contactPoint, Vector3 directionToContactPoint, Vector3 impulse, Collider collider)
    {
        ContactPoint = contactPoint;
        DirectionToContactPoint = directionToContactPoint;
        Impulse = impulse;
        Collider = collider;
    }
}
