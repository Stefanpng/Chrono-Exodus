using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header("Platform Movement Points")]
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;

    [Header("Movement Settings")]
    [SerializeField] private float speed = 2f;

    private Transform _currentTarget;

    private void Awake()
    {
        _currentTarget = pointB;
    }

    private void Update()
    {
        MovePlatform();
        CheckArrival();
    }

    private void MovePlatform()
    {
        transform.position = Vector3.MoveTowards(transform.position, _currentTarget.position, speed * Time.deltaTime);
    }

    private void CheckArrival()
    {
        if (Vector3.Distance(transform.position, _currentTarget.position) < 0.01f)
        {
            _currentTarget = (_currentTarget == pointA) ? pointB : pointA;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.transform.SetParent(null);
        }
    }
}