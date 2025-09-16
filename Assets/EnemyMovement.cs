using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;

    [Header("Attributes")]
    [SerializeField] private float moveSpeed = 2f;

    private Transform target;
    private int pathIndex = 0;

    void Awake()
    {
        if (!rb) rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        SetTargetFromPathIndex();
    }

    void Update()
    {
        if (!target) return;

        if (Vector2.Distance(transform.position, target.position) <= 0.1f)
        {
            pathIndex++;

            // check BEFORE indexing
            if (!SetTargetFromPathIndex())
            {
                // reached end of path
                Destroy(gameObject);
                return;
            }
        }
    }

    void FixedUpdate()
    {
        if (!target) { rb.velocity = Vector2.zero; return; }
        Vector2 direction = ((Vector2)target.position - rb.position).normalized;
        rb.velocity = direction * moveSpeed;
    }

    // Returns true if a valid target was set
    private bool SetTargetFromPathIndex()
    {
        var p = LevelManager.main?.path;
        if (p == null || p.Length == 0) { Debug.LogError("Path is null/empty."); return false; }
        if (pathIndex >= p.Length) return false;

        target = p[pathIndex];
        return true;
    }
}
