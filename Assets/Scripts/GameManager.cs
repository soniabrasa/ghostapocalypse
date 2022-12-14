using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Los prefabs
    public GameObject ghostPrefab;
    public GameObject gameOverPrefab;
    public GameObject vidasPrefab;

    // El tablero de juego
    public Transform[] spawnPoints;
    public Transform[] marcadorPoints;
    public Transform[] board;

    // Los clones de prefabs
    GameObject ghostClone;
    GameObject vidaClone;
    List<GameObject> ghostClones = new List<GameObject>();
    string nameGhost, nameVida;

    // Marcadores
    int totalGhosts, barreraPoints, perdidos, vidasBarrera;

    // Bordes del tablero
    float borderTop, borderRight, borderBottom;

    // Fin del juego
    bool gameOver;

    // Booleano publico de sólo lectura
    public bool GameOver {
        get { return gameOver; }
    }

    // Instancia pública para el patrón Singleton
    public static GameManager instance;

    void Awake() {
        instance = this;
    }

    void Start() {
        gameOver = false;
        vidasBarrera = 4;
        totalGhosts = 0;
        barreraPoints = 0;
        perdidos = 0;

        nameGhost = "Ghost_";
        nameVida = "Vida_";

        borderTop = board[0].position.y;
        borderRight = board[1].position.x;
        borderBottom = board[2].position.y;

        InicioMarcador ();
    }

    // En cada frame
    void Update()
    {
        if( GameOver ) { return; }

        if ( vidasBarrera > 0 ) {
            // Probabilidad del 0.001
            if ( Random.Range(0f, 1f) < 0.001f ) {
                SpawnGhost();
            }

            UpdateGhosts();
        }

        // Condición 1 para el GameOver (Vidas == 0)
        else {
            SetGameOver();
        }
    }

    void InicioMarcador ()
    {
        // Enunciado marcadorX = [-0.75f, -0.25f, 0.25f, 0.75f];

        for ( int i = 0; i < vidasBarrera; i++ ) {
            Transform marcadorPoint = marcadorPoints[i];
            Vector3 posicion = marcadorPoint.position;
            Quaternion rotacion = Quaternion.identity;

            vidaClone = Instantiate( vidasPrefab, posicion, rotacion );
            vidaClone.name = nameVida + i;
        }
    }

    // Método para instanciar un gosthPrefab
    // en uno de los 5 SpawnPoints de espaneo al azar
    void SpawnGhost()
    {
        // Random.Range devuelve un valor de un rango
        // que incluye el 1er parámetro [0]
        // pero excluye el segundo [5]

        // n resulta un entero al azar entre 0 y 4
        int n = Random.Range(0, spawnPoints.Length);

        // Uno de los 5 objetos del array spawPoints
        Transform spawnPoint = spawnPoints[n];

        // Los objetos de tipo Transform tienen 3 propiedades
        // position, rotation y scale

        Vector3 posicion = spawnPoint.position;

        // Quaternion.identity
        // Obtiene un cuaternión que no representa nunguna rotación.
        // Cuaternión cuyos valores son (0, 0, 0, 1)

        Quaternion rotacion = Quaternion.identity;

        // Instantiate( prefab, Vector3, rotacion );
        ghostClone = Instantiate( ghostPrefab, posicion, rotacion );
        ghostClone.name = nameGhost + totalGhosts;
        totalGhosts++;

        ghostClones.Add( ghostClone );

    }

    void UpdateGhosts()
    {
        for ( int i=0; i < ghostClones.Count; i++ ) {
            GameObject clon = ghostClones[i];

            if ( clon != null ) {

                float x = clon.transform.position.x;
                float y = clon.transform.position.y;

                // print( "Position X GhostClone_" + totalGhosts + ": " + x );

                if ( x > borderRight ) {
                    ghostClones.RemoveAt(i);
                    Destroy( clon );
                    BarreraFlop();
                }
            }
        }
    }

    void BarreraFlop()
    {
        vidasBarrera--;
        print("Vidas restantes: " + vidasBarrera );

        string vidaPrefabName = nameVida + vidasBarrera;

        GameObject vida = GameObject.Find(vidaPrefabName);
        Destroy( vida );
    }

    public void BarreraHit() {
        barreraPoints++;
    }

    void ErrorEnemigo()
    {
        Debug.Log("Enemigo perdido");
    }

    void SetGameOver()
    {
        Debug.Log( "GAME OVER" );
        Instantiate( gameOverPrefab);

        gameOver = true;
    }

    void OnGUI()
    {
        GUI.skin.label.fontSize = 40;

        GUI.Label( new Rect(10, 1000, 400, 100),
            "Fantasmas: " + totalGhosts
        );

        GUI.Label( new Rect(400, 1000, 500, 100),
            "Neutralizados: " + barreraPoints
        );

        GUI.Label( new Rect(800, 1000, 500, 100),
            "Perdidos: " + perdidos
        );

        GUI.Label( new Rect(1500, 1000, 500, 100),
            "Vidas: " + vidasBarrera
        );
    }
}
