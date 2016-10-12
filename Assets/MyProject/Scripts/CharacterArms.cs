using UnityEngine;
using System.Collections;

public class CharacterArms : MonoBehaviour
{
    [SerializeField]
    protected Rigidbody[] leftArm, rightArm;
    [SerializeField]
    protected Rigidbody leftHand, rightHand;

    [SerializeField]
    protected float liftForce = 100;

    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Vector3 horizontalVelocity = GetComponent<CharacterLegs>().chestBody.transform.forward.normalized;
            horizontalVelocity.y = 0;
            leftArm[1].AddForce(horizontalVelocity * liftForce, ForceMode.Force);
        }
    }
}
