using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ObstacleTreeCross : ObstacleSpawner
{
    public float treeAngle = -15f;
    public override void Spawn()
    {
        Transform leftTree = transform.GetChild(1);
        Transform rightTree = transform.GetChild(0);
        rightTree.DORotate(new Vector3(-treeAngle, 0, 0f), .25f);
        leftTree.DORotate(new Vector3(-treeAngle,-180f, 0f),.25f);
    }
}
