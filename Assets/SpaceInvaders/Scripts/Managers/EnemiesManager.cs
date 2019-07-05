using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesManager : MonoBehaviour
{
    [Header("Enemies Creation")]
    [Space(10)]
    public int numEnemiesPerRow = 11;                 // Number enemies in each row.
    public int numberOfRows = 5;

    [System.Serializable]
    public class EnemyData
    {
        public GameObject enemyPrefab;
        public Vector3 startPoint = Vector3.zero;   // Dustancia 'buena' (-5. 3.5, 0)
        public float distance = 1.0f;
    }
    [Space(10)]
    public EnemyData[] enemiesData;

    [Header("Enemies Movement")]
    public float ratio = 1f;            // Ratio inicial de mivimiento (se mueve casa segundo)
    public float speed = 0.5f;          // Desplazamiento horizontal de los enemigos
    [Space(5)]
    public float changeRatioTime = 10f; // Tiempo de espera para jabar el ratio de los enemigos (se moveran mas deprisa)
    public float ratioMult = 0.1f;      // Cantidad a descontar del ratio (cuanto mas deprisa se mueven cada vez)
    public float ratioByDeadMult = 0.01f;

    [Header("Enemies Destruction")]
    public GameObject explosionPrefab;
    public float explosionTime = 1f;

    private EnemyShip[] Enemies;
    [HideInInspector]
    public int enemiesCount = 0;
    private float internalRatio = 0;

    private bool isBoundReached = false;

    #region Singleton
    public static EnemiesManager Instance;

    private void Awake()
    {
        Instance = this;
    }
    #endregion

    public void Reset()
    {
        ratio = 1f;
        internalRatio = changeRatioTime;
        StopCoroutine("GenerateEnemies");
        StopCoroutine("_MoveEnemiesTimed");
    }

    public void DestroyAllEnemies()
    {
        DeleteEnemies();
    }


    public void StartPlaying()
    {
        internalRatio = changeRatioTime;
        DestroyAllEnemies();
        StartCoroutine("GenerateEnemies");
        StartCoroutine("_MoveEnemiesTimed");
    }

    void Start ()
    {
        //StartPlaying();
    }

    private void OnEnable()
    {
        //DeleteEnemies();
    }

    private void Update()
    {
        internalRatio -= Time.deltaTime;
        if(internalRatio <= 0)
        {
            ratio -= ratioMult;
            ratio = Mathf.Clamp(ratio, 0.01f, 1f);
            internalRatio = changeRatioTime;
        }
    }

    private IEnumerator GenerateEnemies()
    {
        Enemies = new EnemyShip[numEnemiesPerRow * numberOfRows];
        enemiesCount = Enemies.Length;

        int n = 0;
        for (int i = 0; i < enemiesData.Length; i++)
        {
            for (int j = 0; j < numEnemiesPerRow; j++)
            {
                Vector3 displ = new Vector3(enemiesData[i].distance, 0, 0) * j;
                GameObject obj = Instantiate(enemiesData[i].enemyPrefab, enemiesData[i].startPoint + displ, Quaternion.identity);
                obj.transform.parent = this.transform;

                Enemies[n++] = obj.GetComponent<EnemyShip>();
            }

            yield return new WaitForSeconds(0.2f);
        }
    }

    private void DeleteEnemies()
    {
        if (Enemies == null) return;
        
        for (int i = Enemies.Length-1; i >= 0; i--)
        {
            if (Enemies[i] != null)
            {
                Destroy(Enemies[i].gameObject);
            }
        }
    }

    // Movimiento de los enemigos con tiempo de espera.
    IEnumerator _MoveEnemiesTimed()
    {
        while(true)
        {
            yield return new WaitForSeconds(ratio);
            if (!isEnemiesMoving)   // Solo se llama a larutina de moverse, si la anterior rutina ya se ha acabado.
            {
                Vector3 dir = GetMovementDirection();
                StartCoroutine("MoveEnemies", dir);
            }
        }
    }

    // Direccion del movimiento de los enemmigos (cambio de direccion al llegar al un borde)
    private Vector3 GetMovementDirection()
    {
        Vector3 dir = Vector3.zero;
        dir.x = speed;

        if (isBoundReached)
        {
            speed = -speed;
            dir.x = 0f;
            dir.y = -0.25f;
            isBoundReached = false;
        }

        return dir;
    }

    // Movimiento lineal de los enemigos (todos a una). Recorremos el array de una sola vez.
    /*private IEnumerator MoveEnemies(Vector3 direction)
    {
        for (int i = Enemies.Length -1 ; i >= 0 ; i--)
        {
            if (Enemies[i] != null)
            {
                if (Enemies[i].MoveEnemy(direction)) { isBoundReached = true; }
            }

            yield return new WaitForSeconds(0.01f); 
        }
    }*/

    // Movimiento imitando el original. Los enemigos se mueven fila a fila con un tiempo de espera entra cada fila.
    // Activacion de un flag para mirar si la rutina esta activa.
    // Solo hace el yield en caso de mover algun enemigo en cada fila (si la fila esta vacio, no hace ningun yield)
    private bool isEnemiesMoving = false;
    private IEnumerator MoveEnemies(Vector3 direction)
    {
        isEnemiesMoving = true;
        // Se busca el indice de cada fila y se pasa para que anime todos los ememigos de esa fila.
        for (int i = 0; i < numberOfRows * numEnemiesPerRow; i += numEnemiesPerRow)
        {
            if (MoveEnemiesRow(i, direction)) // Se anima de fila en fila.
            {
                yield return new WaitForSeconds(ratio * 0.1f);    // El tiempo de espera de cada fila es un decimo del ratio
            }
        }

        isEnemiesMoving = false;
    }

    // Mueve los elemento de una fila empezando de un indice concreto y va hacia adelante recorriendo toda la fila)
    // Chequeamos que realmente se este movendo algo en la fila (no vaya a ser que ya hayamos matado a tdos los enemigos de esa fila)
    private bool MoveEnemiesRow(int index, Vector3 direction)
    {
        bool isMovingSomething = false;
        for (int i = 0; i < numEnemiesPerRow; i++)
        {
            if (Enemies[index+i] != null)
            {
                isMovingSomething = true;
                if (Enemies[index+i].MoveEnemy(direction)) { isBoundReached = true; }
            }
        }

        return isMovingSomething;
    }

    // Destruccion de un enemigo por la bala de player.
    // Se ja desactivado el sprite y el collider del ese enemigo y le ponemos como hijo la explosion (para que sigha moviendose, que no queda mal)
    public void DestroyEnemy(Transform _enemy)
    {
        enemiesCount--;
        GameObject explosion = Instantiate(explosionPrefab, _enemy.position, Quaternion.identity);
        explosion.transform.parent = _enemy;
        Destroy(_enemy.gameObject, explosionTime);
        ratio -= ratioByDeadMult;
        ratio = Mathf.Clamp(ratio, 0.01f, 1f);

        if(enemiesCount <= 0)
        {
            SpaceInvadersManager.Instance.LevelFinished();
        }
    }

    
}
