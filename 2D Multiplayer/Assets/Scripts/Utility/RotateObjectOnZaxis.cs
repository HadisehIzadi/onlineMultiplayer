using UnityEngine;
public class RotateObjectOnZaxis : MonoBehaviour
{
    public float m_rotationSpeed = 1f;

    private void Update()
    {

        transform.Rotate(m_rotationSpeed * Time.deltaTime * Vector3.forward);
    }
}
