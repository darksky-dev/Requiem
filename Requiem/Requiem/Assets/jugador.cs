using UnityEngine;

public class jugador : MonoBehaviour
{
    public float speed = 10;

    public float minX = -6f;

    public float maxX = 6f;

    public Rigidbody rb;

    public float input;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Vector3 newPosition = rb.position + Vector3.right * input * speed * Time.deltaTime;

        newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);

        rb.MovePosition(newPosition);
    }
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        input = Input.GetAxis("Horizontal");
    }
}
