using System.Collections;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    [Header("Timing Settings")]
    [SerializeField] private float delayBeforeFall = 2f;
    [SerializeField] private float delayBeforeDestroy = 1f;

    private bool hasStartedFalling = false;
    private Rigidbody2D platformRb;

    private void Awake()
    {
        platformRb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!hasStartedFalling && collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(BeginFallSequence());
        }
    }

    private IEnumerator BeginFallSequence()
    {
        hasStartedFalling = true;
        yield return new WaitForSeconds(delayBeforeFall);

        platformRb.bodyType = RigidbodyType2D.Dynamic;

        yield return new WaitForSeconds(delayBeforeDestroy);
        Destroy(gameObject);
    }
}
