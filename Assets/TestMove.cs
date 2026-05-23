using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class RealMove : MonoBehaviour
{
    CharacterController cc;

    void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 move = transform.forward * v + transform.right * h;

        cc.Move(move * 2f * Time.deltaTime);
    }
}