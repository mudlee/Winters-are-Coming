using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] GameObject _enemyPrefab;
    const int NUM_OF_STARTING_ENEMIES = 3;
    const int MIN_ENEMY_SPAWN_RANGE = 10;
    const float TELEPORT_LIMIT_SECS = 3f;
    PlayerController _player;
    GroundController _ground;
    SoundPlayer _soundPlayer;
    int _enemyIndex=0;
    List<GameObject> _walkableBlocks;
    float _lastTeleport=0;

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        _ground = GetComponent<GroundController>();
        _soundPlayer = GetComponent<SoundPlayer>();

        _ground.BuildGround();

        _walkableBlocks = _ground.GetWalkableBlocks();
        SpawnEnemies();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            bool sameColor = _player.currentVariant == _player.sittingBlock.GetComponent<GroundBlockController>().variant;
            if(sameColor)
            {
                if((Time.time-_lastTeleport)>TELEPORT_LIMIT_SECS)
                {
                    Debug.Log("Teleporting...");
                    _soundPlayer.Play(SoundType.TELEPORT);
                    GameObject target = _ground.GetRandomBlockByVariant(_player.currentVariant);
                    _player.transform.position = new Vector3(
                        target.transform.position.x,
                        _player.transform.position.y,
                        target.transform.position.z
                    );
                    _lastTeleport=Time.time;
                }
                else
                {
                    _soundPlayer.Play(SoundType.TELEPORT_REJECTED);
                }
            }
            else
            {
                _soundPlayer.Play(SoundType.TELEPORT_REJECTED);
            }
        }
    }

    void SpawnEnemies()
    {
        for(int i=0;i<NUM_OF_STARTING_ENEMIES;i++)
        {
            SpawnEnemy();
        }
    }

    void SpawnEnemy()
    {
        bool foundSpot = false;
        while(!foundSpot)
        {
            GameObject possibleSpot = _walkableBlocks[Random.Range(0,_walkableBlocks.Count)];
            float distanceFromPlayer = Vector3.Distance(possibleSpot.transform.position, _player.transform.position);
            if(distanceFromPlayer>MIN_ENEMY_SPAWN_RANGE)
            {
                Vector3 at = possibleSpot.transform.position;
                at.y = 1;
                Instantiate(_enemyPrefab, at, Quaternion.identity);
                foundSpot=true;
                _enemyIndex++;
            }
        }
    }
}
