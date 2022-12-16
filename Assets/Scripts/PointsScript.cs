using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsScript : MonoBehaviour
{
    float timeToLive, speed;
    Vector3 velocity;

    SpriteRenderer spriteRenderer;
    Color spriteColor;

    // Start is called before the first frame update
    void Start()
    {
        speed = 2.5f;

        // El cartel se desplazará hacia arriba a una velocidad de 2.5 m/s.
        velocity = Vector3.up * speed;

        // hasta desaparecer completamente al cabo de 1.2 segundos
        timeToLive = 1.2f;
    }

    // Update is called once per frame
    void Update()
    {
        // Aplicando la dirección y velocidad al desplazmto. en cada frame
        transform.position += velocity * Time.deltaTime;

        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteColor = spriteRenderer.color;

        // Restando el canal alfa del color del Sprite Renderer
        // en la cantidad que corresponde al deltaTime de este frame

        spriteColor.a -= Time.deltaTime / timeToLive;

        // Asignando el nuevo valor de color si no he alcanzado la invisibilidad
        if( spriteColor.a > 0 )
        {
            spriteRenderer.color = spriteColor;
        }
        else {
            Destroy(gameObject);
        }
    }
}
