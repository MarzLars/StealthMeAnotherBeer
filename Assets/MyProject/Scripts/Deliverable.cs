using UnityEngine;
using System.Collections;

public class Deliverable : MonoBehaviour
{
    [SerializeField]
    protected DeliverableModel data;

    public DeliverableModel Data
    {
        get { return data; }
    }

    private float wasThrownCooldown = 1;

    public void Throw()
    {
        wasThrownCooldown = 3;
        Destroy(GetComponent<FixedJoint>());
    }

    private void OnTriggerEnter(Collider coll)
    {
        if (wasThrownCooldown <= 0 && coll.CompareTag("PlayerInteration"))
        {
            InventoryManager.Instance.AddToInventory(data, 1);
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (wasThrownCooldown > 0)
        {
            wasThrownCooldown -= Time.deltaTime;
        }
    }
}
