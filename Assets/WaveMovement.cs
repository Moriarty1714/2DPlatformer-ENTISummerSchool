using UnityEngine;

public class WaveMovement : MonoBehaviour
{
    public float amplitude = 1.0f;  // Amplitud del movimiento de la ola
    public float frequency = 1.0f;  // Frecuencia del movimiento de la ola
    private Vector3 startPos;  // Posición inicial del objeto

    void Start()
    {
        // Guardar la posición inicial del objeto
        startPos = transform.position;
    }

    void Update()
    {
        // Calcular el nuevo valor de Y usando una función senoidal
        float newY = startPos.y + Mathf.Sin(Time.time * frequency) * amplitude;

        // Aplicar la nueva posición al objeto
        transform.position = new Vector3(startPos.x, newY, startPos.z);
    }
}
