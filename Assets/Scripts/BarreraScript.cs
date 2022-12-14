using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarreraScript : MonoBehaviour
{
    Rigidbody2D Rb;
    float vertical;
    float speed = 8f;

    void Start()
    {
        Rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Usando la instancia del patrón Singleton
        if( GameManager.instance.GameOver ) { return; }

        vertical = Input.GetAxisRaw("Vertical") * speed;
    }

    void FixedUpdate() {
        Rb.velocity = new Vector2( Rb.velocity.x, vertical );
    }

    void DestroyBarrera () {
        Destroy( gameObject );
    }
}
