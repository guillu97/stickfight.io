using UnityEngine;
using System.Collections;

public class VerticalCube : GenerateMovingCubes
{
    protected new Vector3 startingPosition = new Vector3(0, 9, 0);

    protected override Vector2 GetMovingForce()
    {
        return transform.up * -500;
    }

    protected override Vector3 GetStartingPosition()
    {
        int height = Random.Range(-3, 4);

        var actualStartingPos = startingPosition;
        actualStartingPos.x = height * 5;
        return actualStartingPos;
    }
}
