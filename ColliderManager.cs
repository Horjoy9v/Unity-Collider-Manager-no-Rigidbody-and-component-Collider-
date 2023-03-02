using System.Collections.Generic;
using UnityEngine;

public class ColliderManager : DrawCapsule
{
    public static CollisionType currentCollisionType = CollisionType.air;
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
        air
    }

    [Space(5)]
    [Header("Options Capsule Collider")]
    public Vector2 capsuleSize;
    private float capsuleAngle;
    private Vector2 capsuleDirection;
    [Space(5)]
    [Header("Сollision pre-check")]
    public float raycastDistance = 1f;

    private void Update()
    {
        currentCollisionType = ReturnCollisionName();
        Debug.Log(currentCollisionType);
        /*  test Gravity
        if (currentCollisionType == CollisionType.air)
        {
            transform.position += new Vector3(0f, gravityVector * Time.deltaTime, 0f);
        }
        */
    }

    public List<CollisionType> OverlapCapsuleAll(Vector2 center, Vector2 size, float angle, Vector2 direction)
    {
        List<CollisionType> collidedTypes = new List<CollisionType>();

        Collider2D[] colliders = Physics2D.OverlapCapsuleAll(center, size, CapsuleDirection2D.Vertical, angle, layerMask, 0f);
        foreach (Collider2D collider in colliders)
        {
            collidedTypes.AddRange(OverlapColliderAll(center, size, collider));
        }

        return collidedTypes;
    }

    public List<CollisionType> OverlapColliderAll(Vector2 center, Vector2 size, Collider2D collider)
    {
        List<CollisionType> collidedTypes = new List<CollisionType>();

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

        return collidedTypes;
    }

    public CollisionType ReturnCollisionName()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, raycastDistance, layerMask);
        List<CollisionType> collidedTypes = OverlapCapsuleAll(transform.position, capsuleSize, capsuleAngle, capsuleDirection);

        if (hit.collider != null && hit.point.y + capsuleSize.y / 2f > transform.position.y)
        {
            transform.position = new Vector2(transform.position.x, hit.point.y + capsuleSize.y / 2f);
            return CollisionType.Ground;
        }
        else if (collidedTypes.Count > 0)
        {
            return collidedTypes[0];
        }
        else
        {
            return CollisionType.air;
        }
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
