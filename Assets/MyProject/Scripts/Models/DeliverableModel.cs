using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName ="Deliverably", menuName ="ScriptableObjects/Deliverable", order = 0)]
public class DeliverableModel : ScriptableObject
{
    public string Name;
    public float Sweetness;
    public Sprite Icon;
    public GameObject Prefab;
}
