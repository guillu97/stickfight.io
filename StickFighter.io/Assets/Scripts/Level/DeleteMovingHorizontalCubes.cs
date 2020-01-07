using System;
using UnityEngine;

public class DeleteMovingHorizontalCubes : MonoBehaviour
{
    public GameObject prefab;
    private GenerateMovingCubes movingCubeScript;

    public void Start()
    {
        var go = GameObject.FindGameObjectWithTag("StartingLimit");

        try
        {
            movingCubeScript = go.GetComponent<HorizontalCube>();

            if (movingCubeScript == null)
            {
                movingCubeScript = go.GetComponent<VerticalCube>();
            }
        }
        catch (Exception e)
        {
            movingCubeScript = go.GetComponent<VerticalCube>();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(collision.gameObject);
        movingCubeScript.spawnCube();
    }
}
