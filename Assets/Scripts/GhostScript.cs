using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostScript : MonoBehaviour {

    // Variables globales de tipo audio del prefab
    public AudioClip[] audioClips;
    AudioClip audioSpawn, audioBoom;

    // Variables físicas de velocidad del prefab
    float speed;
    Vector3 velocity;

    // El prefab tiene varias puntuaciones
    public GameObject points100Prefab;
    public GameObject points150Prefab;
    int points;

    void Start() {
        speed = 6f;

        // Puntuación por defecto
        points = 100;

        // Vector3.right = Vector3(1, 0, 0) de magnitud 1
        velocity = Vector3.right * speed;

        // Cada fantasma podrá decidir moverse oblicuamente
        // con una probabilidad de 0.1.
        if( Random.Range( 0f, 1f ) <= 0.1f )
        {
            Vector3 diagonalDirection = DiagonalDirection();

            // Su velocidad de desplazamiento seguirá siendo la misma
            // Aplicando la misma velocidad al vector normalizado
            velocity = diagonalDirection * speed;

            // Puntuación de 150 para los fantasmas que se mueven en diagonal.
            points = 150;
        }

        Spawn();
    }

    void Update() { }

    // FixedUpdate() es una función que se llama 50 veces de forma Konstante
    // O sea, cada 20 ms o 0.02 segundos
    void FixedUpdate()
    {
        // Usando la instancia del patrón Singleton
        if( GameManager.instance.GameOver ) { return; }

        // Dentro de este método Time.deltaTime = Time.fixedDeltaTime = 0.02
        Vector3 movement = velocity * Time.deltaTime;
        transform.position += movement;
    }

    // private void OnCollisionEnter2D( Collision2D other ) {
    //     BarreraScript player = other.collider.GetComponent<BarreraScript>();
    //
    //     if ( player != null ) {
    //         player.Hit();
    //         Explotar();
    //         velocity = Vector3.zero;
    //     }
    //
    //     // DestroyGhost();
    // }

    void OnTriggerEnter2D( Collider2D collision )
    {
        // bool player = collision.gameObject.tag == "Player";
        BarreraScript barrera = collision.GetComponent<BarreraScript>();

        if ( barrera != null ){
            Die();
        }
    }

    public void Die() {
        // Stop
        velocity = Vector3.zero;

        // Puntos para la barrera
        GameManager.instance.BarreraPoints( points );

        if( points == 100 ) {
            Instantiate(points100Prefab, transform.position + Vector3.up, Quaternion.identity);
        } else {
            Instantiate(points150Prefab, transform.position + Vector3.up, Quaternion.identity);
        }

        Explotar();
    }

    Vector3 DiagonalDirection () {
        // el pivote del fantasma oblicuo debe estar entre los límites
        // inferior y superior alcanzables por el cuerpo de la barrera

        // Altura del fantasma
        float hGhost = transform.localScale.y;

        // Eje x de la Barrera
        float toPosX = GameManager.instance.BarreraTop.x;

        // Límites de la Barrera +/- la altura del fantasma
        float toMinPosY = GameManager.instance.BarreraBottom.y - hGhost;
        float toMaxPosY = GameManager.instance.BarreraTop.y + hGhost;

        // Punto aleatorio en los límites del eje Y de la barrera
        float randomPosY = Random.Range( toMinPosY, toMaxPosY );

        // Vector de posición de destino calculado aleatoriamente
        Vector3 toPoint = new Vector3( toPosX, randomPosY, 0 );

        // La resta de vectores obtiene un vector que va
        // desde la posición actual a la posición de destino
        Vector3 diagonalDirection = toPoint - transform.position;

        // Normalizando la magnitud a 1
        diagonalDirection.Normalize();

        return diagonalDirection;
    }

    void Spawn() {
        audioSpawn = audioClips[0];
        Camera.main.GetComponent<AudioSource>().PlayOneShot(audioSpawn);
    }

    void Explotar() {
        // Transición a la animación Explosion
        Animator animator = GetComponent<Animator>();
        animator.SetBool("exploding", true);

        audioBoom = audioClips[1];
        Camera.main.GetComponent<AudioSource>().PlayOneShot(audioBoom);
    }

    void DestroyGhost() {
        Destroy( gameObject );
    }
}
