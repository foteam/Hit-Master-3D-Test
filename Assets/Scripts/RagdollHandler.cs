using UnityEngine;

public class RagdollHandler : MonoBehaviour
{
    public GameObject parentObject;

    private void OnCollisionEnter(Collision collision)
    {
        parentObject.SendMessage("OnRagdollCollisionEnter", collision, SendMessageOptions.DontRequireReceiver);
    }
        
}