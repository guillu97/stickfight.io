using UnityEngine;
using System.Collections;

public class FallingTile : MonoBehaviour
{
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.constraints = RigidbodyConstraints2D.FreezePositionX;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "FallingTile")
        {
            Destroy(collision.gameObject);
        }
        else
        {
            rb.gravityScale = 1;
        }
    }
}
