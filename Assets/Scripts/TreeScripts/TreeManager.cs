using UnityEngine;

public class TreeManager : MonoBehaviour
{
    [SerializeField] Tree[] trees;

    public Tree FindClosestTree(Vector3 agentPos)
    {
        float closest = 0;
        Tree tree = null;
        for (int i = 0; i < trees.Length; i++)
        {
            if (trees[i] != null)
            {
                if (!trees[i].Fallen)
                {
                    float distance = Vector3.Distance(agentPos, trees[i].transform.position);
                    if (distance < closest || tree == null)
                    {
                        closest = distance;
                        tree = trees[i];
                    }
                }
            }
        }
        return tree;
    }
}
