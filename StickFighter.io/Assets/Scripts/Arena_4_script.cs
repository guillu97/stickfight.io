using UnityEngine;

public class Arena_4_script : MonoBehaviour
{
    public GameObject prefab;

    // Update is called once per frame
    void Start()
    {
        spawnCube();
    }

    private void spawnCube()
    {
        int height = Random.Range(-1, 1);

        var obj = Instantiate(prefab, new Vector3(-20, height * 2, 0), Quaternion.identity);
        obj.tag = "Moving-Cube";
        var rb = obj.gameObject.AddComponent<Rigidbody2D>();
        rb.mass = 9000;
        rb.AddForce(transform.right * 5000000);
        rb.gravityScale = 0;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Destroy(collision.gameObject);
        spawnCube();
    }
}
