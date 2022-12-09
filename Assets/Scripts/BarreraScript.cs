using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarreraScript : MonoBehaviour
{
    private Rigidbody2D Rb;
    private float vertical;
    private float speed = 8f;
    // private int Vidas;
    private int puntos;

    void Start()
    {
        puntos = 0;

        Rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        vertical = Input.GetAxisRaw("Vertical") * speed;
    }

    // FixedUpdate() es una función que se llama cada 20ms
    private void FixedUpdate() {
        Rb.velocity = new Vector2( Rb.velocity.x, vertical );
    }

    public void Hit()
    {
        puntos++;
        print("Neutralizados: " + puntos );
        Puntuacion();
    }

    public int Puntuacion() {
        return this.puntos;
    }

    void DestroyBarrera () {
        Destroy( gameObject );
    }
}
