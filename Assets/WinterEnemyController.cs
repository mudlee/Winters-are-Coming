using UnityEngine;
using UnityEngine.AI;

public class WinterEnemyController : MonoBehaviour
{
    GameObject _player;

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<NavMeshAgent>().destination = _player.transform.position;
    }
}
