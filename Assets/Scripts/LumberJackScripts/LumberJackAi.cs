using UnityEngine;

public class LumberJackAi : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] Tree closestTree;
    [SerializeField] TreeManager treeManager;

    NeuralNetwork nn;

    float inventory;
    bool lineOfSight;

    int maximumDistance = 60; //used for normalizing distances for input, 60 = plane/ground size
    int maximumInventory = 100;

    float moveSpeed = 4;
    Vector3 targetPosForSearching = Vector3.zero;

    void Start()
    {
        nn = new NeuralNetwork(4, 8, 3);
    }

    int Decide()
    {
        if(nn == null)
        {
            Debug.Log("neuralNetwork null");
            return 0;
        }

        float[] output = nn.Run(GetInputs());

        if (!lineOfSight) //om ingen line of sight vet lumberjack inte om var spelare finns så det är omöjligt att lämna trä (output index 1 motsvara beteende för det)
            output[1] = Mathf.NegativeInfinity;

        if (closestTree == null || closestTree.Fallen)
            closestTree = treeManager.FindClosestTree(transform.position);
        if (closestTree == null || closestTree.Fallen)
            output[0] = Mathf.NegativeInfinity; //if no trees left = cant gather wood

        int index = 0; //index of the highest activated output
        for(int i = 0; i < output.Length; i++)
        {
            if (output[i] > output[index])
            {
                index = i;
            }
        }
        return index;
    }

    float[] GetInputs()
    {
        float distanceToPlayerNormalized;
        float distanceToTreeNormalized = 0;
        float inventory;
        float lineOfSight;

        //player distance
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
        distanceToPlayerNormalized = 1 - Mathf.Clamp(distanceToPlayer / maximumDistance, 0, 1);

        //tree distance
        if(closestTree != null && !closestTree.Fallen)
        {
            float distanceToTree = Vector3.Distance(closestTree.transform.position, transform.position);
            distanceToTreeNormalized = 1 - Mathf.Clamp(distanceToTree / maximumDistance, 0, 1);
        }

        //inventory
        inventory = this.inventory / maximumInventory;
        inventory = Mathf.Min(inventory, 1);

        //lineOfSight
        if(this.lineOfSight)
        {
            lineOfSight = 1;
        }
        else
        {
            lineOfSight = 0;
            distanceToPlayerNormalized = 0;
        }

        float[] inputs = { distanceToPlayerNormalized, distanceToTreeNormalized, inventory, lineOfSight };
        return inputs;
    }

    bool CheckLineOfSight()
    {
        Vector3 direction = player.transform.position - transform.position;
        direction.y = 0;
        Ray ray = new Ray(transform.position, direction);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            if (hit.collider.gameObject == player.gameObject)
            {
                return true;
            }
        }
        return false;
    }

    void Update()
    {
        closestTree = treeManager.FindClosestTree(transform.position);
        lineOfSight = CheckLineOfSight();

        int action = Decide();
        switch(action)
        {
            case 0:
                GetWood();
                break;
            case 1:
                DropOffWood();
                break;
            case 2:
                SearchForPlayer();
                break;
        };
    }

    //helper methods

    void GoToTarget(Vector3 target)
    {
        transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
    }

    Vector3 FindTargetPosition() // returns random position on the map, used when searching
    {
        Vector3 pos = Random.insideUnitSphere * 30;
        pos.y = 0;
        return pos;
    }

    //actions
    void GetWood()
    {
        if (closestTree == null || closestTree.Fallen)
            treeManager.FindClosestTree(transform.position);
        if(Vector3.Distance(transform.position, closestTree.transform.position) < 2)
        {
            closestTree.Cut();
            inventory += 10;
        }
        else
        {
            GoToTarget(closestTree.transform.position);
        }
    }

    void DropOffWood()
    {
        if(Vector3.Distance(transform.position, player.transform.position) < 2)
        {
            //player inventory++
            inventory = 0;
        }
        else
        {
            GoToTarget(player.transform.position);
        }
    }

    void SearchForPlayer()
    {
        if(targetPosForSearching == Vector3.zero)
            targetPosForSearching = FindTargetPosition();

        if (Vector3.Distance(transform.position, targetPosForSearching) < 2)
        {
            targetPosForSearching = FindTargetPosition();
        }

        GoToTarget(targetPosForSearching);
    }
}
