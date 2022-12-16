using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Los prefabs
    public GameObject ghostPrefab;
    public GameObject gameOverPrefab;
    public GameObject healthPrefab;

    // El tablero de juego
    public Transform[] spawnPoints;
    public Transform[] scoreHealth;
    public Transform[] board;

    // Los clones de prefabs
    GameObject ghostClone;
    GameObject healthClone;
    GameObject gameOverClone;
    List<GameObject> ghostClones = new List<GameObject>();
    string nameGhost, nameHealth;

    // Marcadores
    public ScorePointsSc scorePoints;
    int totalGhosts, barreraPoints, healthCount;

    // Bordes del tablero
    float borderTop, borderRight, borderBottom;

    // Fin del juego
    bool gameOver;

    // Booleano publico de sólo lectura
    public bool GameOver {
        get { return gameOver; }
    }

    // Posiciones públicas del tablero
    public Vector3 BarreraTop    { get { return board[0].position; } }
    public Vector3 BoardRight    { get { return board[1].position; } }
    public Vector3 BarreraBottom { get { return board[2].position; } }

    // Instancia pública para el patrón Singleton
    public static GameManager instance;

    void Awake() {
        instance = this;
    }

    void Start() {
        nameGhost = "Ghost_";
        nameHealth = "Health_";

        InitGame ();
    }

    // En cada frame
    void Update()
    {
        if( GameOver ) {
            // al pulsar la tecla F1 se iniciará una nueva partida.
            if ( Input.GetKeyDown(KeyCode.F1) ) {
                InitGame();
            }
            return;
        }

        if ( healthCount > 0 ) {
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

        if ( Input.GetKeyDown(KeyCode.Space) ) { SuperPower(); }
    }

    void InitGame ()
    {
        gameOver = false;
        healthCount = 4;
        totalGhosts = 0;
        barreraPoints = 0;
        scorePoints.Display(0);

        if ( gameOverClone != null ) {
            Destroy( gameOverClone );
        }

        DestroyAllGhosts();

        // Enunciado marcadorX = [-0.75f, -0.25f, 0.25f, 0.75f];

        for ( int i = 0; i < healthCount; i++ ) {
            Transform healthPoint = scoreHealth[i];
            Vector3 posicion = healthPoint.position;
            Quaternion rotacion = Quaternion.identity;

            healthClone = Instantiate( healthPrefab, posicion, rotacion );
            healthClone.name = nameHealth + i;
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

                if ( x > BoardRight.x ) {
                    ghostClones.RemoveAt(i);
                    Destroy( clon );
                    BarreraFlop();
                }
            }
        }
    }

    void BarreraFlop()
    {
        healthCount--;
        print("Vidas restantes: " + healthCount );

        string healthPrefabName = nameHealth + healthCount;

        GameObject health = GameObject.Find(healthPrefabName);
        Destroy( health );
    }

    public void BarreraPoints( int points ) {
        barreraPoints += points;

        print( "Marcador barrera: " + barreraPoints );
        scorePoints.Display( barreraPoints );
    }

    void SuperPower() {
        // foreach ( GameObject go in ghostClones ) {
        //     go.GetComponent<GhostScript>().Die();
        // }
        for ( int i=0; i < ghostClones.Count; i++ ) {
            GameObject go = ghostClones[i];

            if ( go != null ) {
                go.GetComponent<GhostScript>().Die();
            }
        }
    }

    void SetGameOver()
    {
        Debug.Log( "GAME OVER" );
        gameOverClone = Instantiate( gameOverPrefab);

        gameOver = true;
    }

    void DestroyAllGhosts() {
        foreach ( GameObject go in ghostClones ) {
            Destroy( go );
        }
        ghostClones.Clear();
    }


    void OnGUI()
    {
        GUI.skin.label.fontSize = 40;

        GUI.Label( new Rect(10, 1000, 400, 100),
            "Fantasmas: " + totalGhosts
        );

        GUI.Label( new Rect(400, 1000, 500, 100),
            "Puntuación: " + barreraPoints
        );

        GUI.Label( new Rect(1500, 1000, 500, 100),
            "Vidas: " + healthCount
        );
    }
}
