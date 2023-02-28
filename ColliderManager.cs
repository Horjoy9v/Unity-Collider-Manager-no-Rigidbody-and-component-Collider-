using System.Collections.Generic;
using UnityEngine;

public class ColliderManager : DrawCapsule
{
    [Header("Form Collider")]
    public bool isDrawGizmosCubeON = false;
    public bool isDrawGizmosCapsuleON = false;

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

    [Header("Options Box Collider")]
    public Vector2 boxSize;
    public static CollisionType currentCollisionType = CollisionType.air;
    private int layerMask = Physics2D.AllLayers;

    private void Update()
    {
        currentCollisionType = ReturnCollisionName();
        Debug.Log(currentCollisionType);
    }


    public List<CollisionType> OverlapBoxAll(Vector2 center, Vector2 size)
    {
        List<CollisionType> collidedTypes = new List<CollisionType>();

        Collider2D[] colliders = Physics2D.OverlapBoxAll(center, size, 0f, layerMask);
        foreach (Collider2D collider in colliders)
        {
            collidedTypes.AddRange(OverlapColliderAll(center, size, collider));
        }

        return collidedTypes;
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
        List<CollisionType> collidedTypes = new List<CollisionType>();

        if(isDrawGizmosCubeON)
        collidedTypes.AddRange(OverlapBoxAll(transform.position, boxSize));

        if(isDrawGizmosCapsuleON)
        collidedTypes.AddRange(OverlapCapsuleAll(transform.position, capsuleSize, capsuleAngle, capsuleDirection));

        if (collidedTypes.Count > 0)
        {
            return collidedTypes[0];
        }

        return CollisionType.air;
    }
    private void OnDrawGizmos()
    {
        if (isDrawGizmosCubeON)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position, new Vector3(boxSize.x, boxSize.y, 1));
        }

        if (isDrawGizmosCapsuleON)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, capsuleSize.x / 2f);
        }
    }


}
