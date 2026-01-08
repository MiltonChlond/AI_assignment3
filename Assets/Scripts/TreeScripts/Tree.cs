using UnityEngine;

public class Tree : MonoBehaviour
{
    bool fallen;
    int hp;
    int fallSpeed = 60;
    float currentX = -90;

    public bool Fallen
    {
        get { return fallen; }
    }

    public int HP
    {
        get { return hp; }
    }

    void Start()
    {
        fallen = false;
        hp = 100;
    }

    public void Cut()
    {
        hp -= 25;
        if (hp <= 0)
        {
            fallen = true;
            GetComponent<Collider>().enabled = false;
        }
    }

    void Update()
    {
        if (!fallen)
            return;

        currentX = Mathf.MoveTowards(currentX, -180f, fallSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(currentX, 0f, 0f);

        if (currentX <= -180)
        {
            Destroy(gameObject);
        }
    }
}
