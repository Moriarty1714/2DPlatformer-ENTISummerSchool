using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParalaxSingle : MonoBehaviour
{
    public float velocidad = 5f; // Velocidad de movimiento del objeto
    public float offset = 0.5f; // Offset para teletransportar el objeto fuera de la pantalla

    private float limiteIzquierdo; // L�mite izquierdo de la pantalla con offset
    private float limiteDerecho; // L�mite derecho de la pantalla con offset

    void Start()
    {
        // Calculamos los l�mites de la pantalla
        RecalcularLimitesDePantalla();
    }

    void Update()
    {
        // Movemos el objeto hacia la izquierda
        transform.Translate(Vector3.left * velocidad * Time.deltaTime);

        // Si el objeto ha pasado el l�mite izquierdo de la pantalla
        if (transform.position.x < limiteIzquierdo - (offset*3))
        {
            // Teletransportamos el objeto al l�mite derecho con offset
            TeletransportarObjeto(limiteDerecho + offset);
        }
        // Si el objeto ha pasado el l�mite derecho de la pantalla
        else if (transform.position.x > limiteDerecho + (offset * 3))
        {
            // Teletransportamos el objeto al l�mite izquierdo con offset
            TeletransportarObjeto(limiteIzquierdo - offset);
        }
    }

    void RecalcularLimitesDePantalla()
    {
        // Calculamos los l�mites de la pantalla con offset
        limiteIzquierdo = Camera.main.ViewportToWorldPoint(new Vector3(-0.1f, 0, 0)).x; // Cambiamos 0 a -0.1 para mover el l�mite izquierdo m�s lejos
        limiteDerecho = Camera.main.ViewportToWorldPoint(new Vector3(1.1f, 0, 0)).x; // Cambiamos 1 a 1.1 para mover el l�mite derecho m�s lejos
    }

    // Funci�n para teletransportar el objeto al lado opuesto de la pantalla con offset
    void TeletransportarObjeto(float destino)
    {
        Vector3 nuevaPosicion = transform.position;
        nuevaPosicion.x = destino;
        transform.position = nuevaPosicion;
    }

    // Llamamos a esta funci�n cuando la c�mara se mueve para recalcular los l�mites
    void LateUpdate()
    {
        RecalcularLimitesDePantalla();
    }
}
