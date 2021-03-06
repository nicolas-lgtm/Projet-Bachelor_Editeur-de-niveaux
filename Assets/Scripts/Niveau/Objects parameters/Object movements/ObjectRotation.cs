using UnityEngine;

public class ObjectRotation : MonoBehaviour
{
    [SerializeField] bool rotateOnX, rotateOnY, rotateOnZ, 
        startWithRandomRotationX, startWithRandomRotationY, startWithRandomRotationZ;
    [SerializeField] Direction direction;

    [SerializeField] float maxSpeed, minSpeed;
    float rotationSpeed, rotation;

    void Start()
    {
        Quaternion startingRotation = transform.rotation;

        if (startWithRandomRotationX) transform.Rotate(Random.Range(0, 180), startingRotation.y, startingRotation.z);
        if (startWithRandomRotationY) transform.Rotate(startingRotation.x, Random.Range(0, 180), startingRotation.z);
        if (startWithRandomRotationZ) transform.Rotate(startingRotation.x, startingRotation.y, Random.Range(0, 180));

        rotationSpeed = Random.Range(minSpeed, maxSpeed);

        if (direction == Direction.CanBeBoth)
            do { direction = (Direction)Random.Range(-1, 2); } while (direction == 0);

        rotation = rotationSpeed * (int)direction;
    }

    void Update()
    {
        float rotationMovement = rotation * Time.deltaTime;

        if (rotateOnX) transform.Rotate(Vector3.right, rotationMovement);
        if (rotateOnY) transform.Rotate(Vector3.up, rotationMovement);
        if (rotateOnZ) transform.Rotate(Vector3.forward, rotationMovement);
    }

    enum Direction
    {
        Clockwise = 1,
        AntiClockwise = -1,
        CanBeBoth
    }
}
