using UnityEngine;

public class WaveMovement : MonoBehaviour
{
    public float amplitude = 1.0f;  // Amplitud del movimiento de la ola
    public float frequency = 1.0f;  // Frecuencia del movimiento de la ola
    private Vector3 startPos;  // Posici�n inicial del objeto

    void Start()
    {
        // Guardar la posici�n inicial del objeto
        startPos = transform.position;
    }

    void Update()
    {
        // Calcular el nuevo valor de Y usando una funci�n senoidal
        float newY = startPos.y + Mathf.Sin(Time.time * frequency) * amplitude;

        // Aplicar la nueva posici�n al objeto
        transform.position = new Vector3(startPos.x, newY, startPos.z);
    }
}
