using UnityEngine;
using System.Collections;

public class HorizontalCube : GenerateMovingCubes
{
    protected new Vector3 startingPosition = new Vector3(-22, 0, 0);

    protected override Vector2 GetMovingForce()
    {
        return transform.right * 500;
    }

    protected override Vector3 GetStartingPosition()
    {
        int height = Random.Range(-1, 1);

        var actualStartingPos = startingPosition;
        actualStartingPos.y = height * 2;
        return actualStartingPos;
    }
}
