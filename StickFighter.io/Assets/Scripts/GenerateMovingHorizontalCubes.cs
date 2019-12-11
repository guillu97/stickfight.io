using UnityEngine;

public class GenerateMovingHorizontalCubes : MonoBehaviour
{
    public int nbrWantedCubes;
    public GameObject prefab;

    private int currentCubes = 0;

    // Use this for initialization
    void Start()
    {
        spawnCube();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(currentCubes < nbrWantedCubes)
        {
            spawnCube();
            currentCubes++;
        }
    }

    private void spawnCube()
    {
        int height = Random.Range(-1, 1);

        var obj = Instantiate(prefab, new Vector3(-22, height * 2, 0), Quaternion.identity);
        obj.tag = "Moving-Cube";
        var rb = obj.gameObject.AddComponent<Rigidbody2D>();
        rb.mass = 9000;
        rb.AddForce(transform.right * 5000000);
        rb.gravityScale = 0;
    }
}
