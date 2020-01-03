using UnityEngine;

public class GenerateMovingCubes : MonoBehaviour
{
    public int nbrWantedCubes;
    public GameObject prefab;

    private int currentCubes = 0;
    protected Vector3 startingPosition = new Vector3(0, 0, 0);

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

    protected virtual Vector2 GetMovingForce()
    {
        return transform.right * 500;
    }

    protected virtual Vector3 GetStartingPosition()
    {
        int height = Random.Range(-1, 1);

        var actualStartingPos = startingPosition;
        actualStartingPos.y = height * 2;
        return actualStartingPos;
    }

    public void spawnCube()
    {
        var obj = Instantiate(prefab, GetStartingPosition(), Quaternion.identity);
        obj.tag = "Moving-Cube";
        var rb = obj.gameObject.AddComponent<Rigidbody2D>();
        rb.AddForce(GetMovingForce());
        rb.gravityScale = 0;
    }
}
