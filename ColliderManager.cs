using System.Collections.Generic;
using UnityEngine;

public class CharacterCollision : MonoBehaviour
{
    public static CollisionType currentCollisionType = CollisionType.Air;
    private int layerMask = Physics2D.AllLayers;
    public float gravityVector;

    [Header("Form Collider")]
    public bool isDrawGizmosShperesON = false;

    public enum CollisionType
    {
        Ground,
        Platform,
        UpSteps,
        DownSteps,
        LeftWall,
        RightWall,
        Air
    }

    [Space(5)]
    [Header("Options Capsule Collider")]
    public Vector2 capsuleSize;
    private float capsuleAngle;
    [Space(5)]
    [Header("Сollision pre-check")]
    public float raycastDistance = 1f;

    void FixedUpdate()
    {
        List<CollisionType> collisionTypes = CheckCollision();

        if (collisionTypes.Contains(CollisionType.Ground))
        {
            Debug.Log("движение по земле");
        }
        else if (collisionTypes.Contains(CollisionType.Platform))
        {
            // движение по платформе
        }
        else if (collisionTypes.Contains(CollisionType.UpSteps))
        {
            // движение по лестнице вверх
        }
        else if (collisionTypes.Contains(CollisionType.DownSteps))
        {
            // движение по лестнице вниз
        }
        else if (collisionTypes.Contains(CollisionType.LeftWall))
        {
            // движение по стене влево
        }
        else if (collisionTypes.Contains(CollisionType.RightWall))
        {
            // движение по стене вправо
        }
        else
        {
            // свободное падение
            Debug.Log("свободное падение");
            transform.position += new Vector3(0f, gravityVector * Time.deltaTime, 0f);
        }

    }
    public List<CollisionType> CheckCollision()
    {
        List<CollisionType> collidedTypes = new List<CollisionType>();

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, raycastDistance, layerMask);
        Collider2D[] colliders = Physics2D.OverlapCapsuleAll(transform.position, capsuleSize, CapsuleDirection2D.Vertical, capsuleAngle, layerMask, 0f);

        foreach (Collider2D collider in colliders)
        {
            switch (collider.gameObject.tag)
            {
                case "Ground":
                    collidedTypes.Add(CollisionType.Ground);
                    break;
                case "Platform":
                    collidedTypes.Add(CollisionType.Platform);
                    break;
                case "UpSteps":
                    collidedTypes.Add(CollisionType.UpSteps);
                    break;
                case "DownSteps":
                    collidedTypes.Add(CollisionType.DownSteps);
                    break;
                case "LeftWall":
                    collidedTypes.Add(CollisionType.LeftWall);
                    break;
                case "RightWall":
                    collidedTypes.Add(CollisionType.RightWall);
                    break;
            }
        }

        if (hit.collider != null && hit.point.y + capsuleSize.y / 2f > transform.position.y)
        {
            transform.position = new Vector2(transform.position.x, hit.point.y + capsuleSize.y / 2f);
            collidedTypes.Add(CollisionType.Ground);
        }

        if (collidedTypes.Count == 0)
        {
            collidedTypes.Add(CollisionType.Air);
        }

        return collidedTypes;
    }



    private void OnDrawGizmos()
    {
        if (isDrawGizmosShperesON)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, capsuleSize.x / 2f);
        }

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(raycastDistance, 0f, 0f));
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(0f, raycastDistance, 0f));
        Gizmos.DrawLine(transform.position, transform.position - new Vector3(raycastDistance, 0f, 0f));
        Gizmos.DrawLine(transform.position, transform.position - new Vector3(0f, raycastDistance, 0f));
    }
}
