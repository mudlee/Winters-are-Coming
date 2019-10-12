using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject sittingBlock;
    public VariantType currentVariant;

    [SerializeField] List<Variant> _variants = default;
    const float SPEED = 7f;
    Rigidbody _rigidBody;
    Renderer _renderer;
    Vector3 _moveVector = Vector3.zero;

    void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _renderer = GetComponent<Renderer>();
        SelectRandomVariant();
    }

    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        if(h!=0 || v!=0)
        {
            _moveVector.Set(h,0,v);
            _moveVector =_moveVector.normalized * SPEED * Time.deltaTime;
            _rigidBody.MovePosition(transform.position + _moveVector);
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            SelectRandomVariant();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "GroundBlock")
        {
            sittingBlock = collision.gameObject;
        }
    }

    void SelectRandomVariant()
    {
        bool found=false;
        while(!found)
        {
            Variant selectedVariant = _variants[Random.Range(0,_variants.Count)];
            if(selectedVariant.type == currentVariant)
            {
                continue;
            }
            _renderer.material = selectedVariant.material;
            currentVariant = selectedVariant.type;
            found = true;
            Debug.Log("Current variant is "+currentVariant);
        }
    }
}
