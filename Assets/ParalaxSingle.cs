using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParalaxSingle : MonoBehaviour
{
    public float velocidad = 5f; // Velocidad de movimiento del objeto
    public float offset = 0.5f; // Offset para teletransportar el objeto fuera de la pantalla

    private float limiteIzquierdo; // Límite izquierdo de la pantalla con offset
    private float limiteDerecho; // Límite derecho de la pantalla con offset

    void Start()
    {
        // Calculamos los límites de la pantalla
        RecalcularLimitesDePantalla();
    }

    void Update()
    {
        // Movemos el objeto hacia la izquierda
        transform.Translate(Vector3.left * velocidad * Time.deltaTime);

        // Si el objeto ha pasado el límite izquierdo de la pantalla
        if (transform.position.x < limiteIzquierdo - (offset*3))
        {
            // Teletransportamos el objeto al límite derecho con offset
            TeletransportarObjeto(limiteDerecho + offset);
        }
        // Si el objeto ha pasado el límite derecho de la pantalla
        else if (transform.position.x > limiteDerecho + (offset * 3))
        {
            // Teletransportamos el objeto al límite izquierdo con offset
            TeletransportarObjeto(limiteIzquierdo - offset);
        }
    }

    void RecalcularLimitesDePantalla()
    {
        // Calculamos los límites de la pantalla con offset
        limiteIzquierdo = Camera.main.ViewportToWorldPoint(new Vector3(-0.1f, 0, 0)).x; // Cambiamos 0 a -0.1 para mover el límite izquierdo más lejos
        limiteDerecho = Camera.main.ViewportToWorldPoint(new Vector3(1.1f, 0, 0)).x; // Cambiamos 1 a 1.1 para mover el límite derecho más lejos
    }

    // Función para teletransportar el objeto al lado opuesto de la pantalla con offset
    void TeletransportarObjeto(float destino)
    {
        Vector3 nuevaPosicion = transform.position;
        nuevaPosicion.x = destino;
        transform.position = nuevaPosicion;
    }

    // Llamamos a esta función cuando la cámara se mueve para recalcular los límites
    void LateUpdate()
    {
        RecalcularLimitesDePantalla();
    }
}
