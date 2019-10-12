using UnityEngine;

public class GameController : MonoBehaviour
{
    PlayerController _player;
    GroundController _ground;

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        _ground = GetComponent<GroundController>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            Debug.Log(_player.currentVariant);
            Debug.Log(_player.sittingBlock.GetComponent<GroundBlockController>().variant);
            if(_player.currentVariant == _player.sittingBlock.GetComponent<GroundBlockController>().variant)
            {
                Debug.Log("Teleporting...");
                GameObject target = _ground.GetRandomBlockByVariant(_player.currentVariant);
                _player.transform.position = new Vector3(
                    target.transform.position.x,
                    _player.transform.position.y,
                    target.transform.position.z
                );
            }
        }
    }
}
