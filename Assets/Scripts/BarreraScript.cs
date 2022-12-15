using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarreraScript : MonoBehaviour
{
    Rigidbody2D rb;
    float speed, verticalInput;

    // Límites del movimiento vertical del GameManager
    float minY, maxY;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        speed = 8f;

        minY = GameManager.instance.BarreraBottom.y;
        maxY = GameManager.instance.BarreraTop.y;
    }

    void Update()
    {
        // Usando la instancia del patrón Singleton
        if( GameManager.instance.GameOver ) { return; }

        verticalInput = Input.GetAxisRaw("Vertical") * speed;

        if ( verticalInput != 0 ) {
            Vector3 tmpPosition = transform.position;
            // El método Clamp() de Mathf está muy chulo
            // Limita la posición x/y/z entre  un mínimo y un máximo
            tmpPosition.y = Mathf.Clamp( tmpPosition.y, minY, maxY );

            transform.position = tmpPosition;
        }
    }

    void FixedUpdate() {
        rb.velocity = new Vector2( rb.velocity.x, verticalInput );
    }

    void DestroyBarrera () {
        Destroy( gameObject );
    }
}
