using System.Linq;
using UnityEngine;

namespace Character
{
    public class FieldOfView : MonoBehaviour
    {
        [SerializeField, Range(0.1f, 10f)] private float radius = 1;
    [SerializeField, Range(1.0f, 360f)] private int fov = 90;

    private Vector2 direction = Vector2.down;
    private Vector2 leftLineFOV;
    private Vector2 rightLineFOV;

    public Character.FacingDirection FacingDirection
    {
        get;
        set;
    }
    
    private void Update()
    {
        direction = FacingDirection switch
        {
            Character.FacingDirection.Down => Vector2.down,
            Character.FacingDirection.Up => Vector2.up,
            Character.FacingDirection.Left => Vector2.left,
            Character.FacingDirection.Right => Vector2.right,
            _ => direction
        };

        rightLineFOV = RotatePointAroundTransform(direction.normalized * radius, -fov / 2.0f);
        leftLineFOV = RotatePointAroundTransform(direction.normalized * radius, fov / 2.0f);
    }

    public GameObject[] GetAllEnemiesInsideFOV() => GameObject.FindGameObjectsWithTag("Enemy").Where(enemy => InsideFOV(enemy.transform.position)).ToArray();

    public GameObject GetPlayerInsideFOV()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        
        return InsideFOV(player.transform.position) ? player : null;
    }
    
    public bool InsideFOV(Vector2 enemyPos)
    {
        var position = transform.position;
        float squaredDistance = ((enemyPos.x - position.x) * (enemyPos.x - position.x)) +
                                ((enemyPos.y - position.y) * (enemyPos.y - position.y));

        if (!(radius * radius >= squaredDistance))
            return false;
        
        float signLeftLine = (leftLineFOV.x) * (enemyPos.y - position.y) -
                             (leftLineFOV.y) * (enemyPos.x - position.x);
        float signRightLine = (rightLineFOV.x) * (enemyPos.y - position.y) -
                              (rightLineFOV.y) * (enemyPos.x - position.x);
        if (fov <= 180)
        {
            if (signLeftLine <= 0 && signRightLine >= 0)
                return true;
        }
        else
        {
            if (!(signLeftLine >= 0 && signRightLine <= 0))
                return true;
        }

        return false;
    }

    private Vector2 RotatePointAroundTransform(Vector2 pointToRotate, float angles)
    {
        return new Vector2(
            Mathf.Cos((angles) * Mathf.Deg2Rad) * (pointToRotate.x) -
            Mathf.Sin((angles) * Mathf.Deg2Rad) * (pointToRotate.y),
            Mathf.Sin((angles) * Mathf.Deg2Rad) * (pointToRotate.x) +
            Mathf.Cos((angles) * Mathf.Deg2Rad) * (pointToRotate.y));
    }

    private void OnDrawGizmos()
    {
        var position = transform.position;
        
        Gizmos.color = Color.green;
        Gizmos.DrawRay(position, direction.normalized * radius);

        rightLineFOV = RotatePointAroundTransform(direction.normalized * radius, -fov / 2.0f);
        leftLineFOV = RotatePointAroundTransform(direction.normalized * radius, fov / 2.0f);

        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(position, rightLineFOV);
        Gizmos.DrawRay(position, leftLineFOV);

        Vector2 point1 = rightLineFOV;

        for (int i = 1; i <= 20; i++)
        {
            float step = fov / 20.0f;
            Vector2 point2 = RotatePointAroundTransform(direction.normalized * radius, -fov / 2.0f + step * (i));
            Gizmos.DrawRay(new Vector2(transform.position.x, position.y) + point1, point2 - point1);
            point1 = point2;
        }

        Gizmos.DrawRay(new Vector2(transform.position.x, position.y) + point1, leftLineFOV - point1);
    }
    }
}