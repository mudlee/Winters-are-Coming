using System.Collections.Generic;
using UnityEngine;

public class GroundController : MonoBehaviour
{
    const int MAP_SIZE = 50;
    const int MAP_BLOCK_SPACING = 2;
    [SerializeField] GameObject _groundParent = default;
    [SerializeField] GameObject _groundBlockPrefab = default;
    [SerializeField] List<Variant> _variants = default;
    [SerializeField] Variant _defaultVariant = default;
    Dictionary<VariantType, List<GameObject>> _blocks = new Dictionary<VariantType, List<GameObject>>();

    public GameObject GetRandomBlockByVariant(VariantType type)
    {
        List<GameObject> blocks;
        if(_blocks.TryGetValue(type, out blocks))
        {
            return blocks[Random.Range(0,blocks.Count)];
        }

        return null;
    }

    struct CreatedBlock
    {
        public GameObject block;
        public VariantType variantType;
    }
    
    void Start()
    {
        for(int x = 0; x < MAP_SIZE; x+=MAP_BLOCK_SPACING) 
        {
            for(int z = 0; z < MAP_SIZE; z+=MAP_BLOCK_SPACING)
            {
                
                Vector3 position = new Vector3(x,0,z);
                CreatedBlock? groundBlock = CreateBlock(position, true);
                
                if(
                    x==0 || z==0 || // left col and bottom row
                    x==MAP_SIZE-MAP_BLOCK_SPACING || z==MAP_SIZE-MAP_BLOCK_SPACING || // top row and right col
                    (Random.value >= 0.8f && x!=MAP_BLOCK_SPACING && z!=MAP_BLOCK_SPACING)
                ) {
                    position.y = 1;
                    CreateBlock(position, false);
                }
                else if(groundBlock.HasValue) 
                {

                    AddAsTeleportPoint(groundBlock.Value.variantType, groundBlock.Value.block);
                }
            }
        }
    }

    CreatedBlock? CreateBlock(Vector3 at, bool attachVariant)
    {
        at.x+=0.1f;
        at.z+=0.1f;
        GameObject block = GameObject.Instantiate(_groundBlockPrefab, at, Quaternion.identity) as GameObject;
        block.transform.parent = _groundParent.transform;

        if(attachVariant)
        {
            Variant variant;
            if(Random.value >= 0.8f)
            {
                variant = _variants[Random.Range(0,_variants.Count)];
            }
            else
            {
                variant = _defaultVariant;
            }

            block.GetComponent<Renderer>().material = variant.material;
            block.GetComponent<GroundBlockController>().variant = variant.type;

            return new CreatedBlock{block = block, variantType = variant.type};
        }

        return null;
    }

    void AddAsTeleportPoint(VariantType variantType, GameObject block)
    {
        if(!_blocks.ContainsKey(variantType))
        {
            _blocks.Add(variantType, new List<GameObject>());
        }

        List<GameObject> variantBlocks;
        if(_blocks.TryGetValue(variantType, out variantBlocks))
        {
            variantBlocks.Add(block);
        }
    }
}
