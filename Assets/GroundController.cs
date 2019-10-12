using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GroundController : MonoBehaviour
{
    const int MAP_SIZE = 40;
    const int MAP_BLOCK_SPACING = 2;
    [SerializeField] GameObject _walkableParent = default;
    [SerializeField] GameObject _obstacleParent = default;
    [SerializeField] GameObject _groundBlockPrefab = default;
    [SerializeField] List<Variant> _variants = default;
    [SerializeField] Variant _defaultVariant = default;
    Dictionary<VariantType, List<GameObject>> _blocks = new Dictionary<VariantType, List<GameObject>>();

    struct TeleportableBlock
    {
        public GameObject block;
        public VariantType variantType;
    }

    public GameObject GetRandomBlockByVariant(VariantType type)
    {
        List<GameObject> blocks;
        if(_blocks.TryGetValue(type, out blocks))
        {
            return blocks[Random.Range(0,blocks.Count)];
        }

        return null;
    }

    public List<GameObject> GetWalkableBlocks()
    {
        List<GameObject> walkables = new List<GameObject>();

        foreach(Transform walkable in _walkableParent.transform)
        {
            walkables.Add(walkable.gameObject);
        }

        return walkables;
    }
    
    public void BuildGround()
    {
        for(int x = 0; x < MAP_SIZE; x+=MAP_BLOCK_SPACING) 
        {
            for(int z = 0; z < MAP_SIZE; z+=MAP_BLOCK_SPACING)
            {
                bool obstacle = x==0 || z==0 || // left col and bottom row
                    x==MAP_SIZE-MAP_BLOCK_SPACING || z==MAP_SIZE-MAP_BLOCK_SPACING || // top row and right col
                    (Random.value >= 0.8f && x!=MAP_BLOCK_SPACING && z!=MAP_BLOCK_SPACING);

                Vector3 position = new Vector3(x,0,z);
                
                
                if(obstacle) {
                    position.y = 1;
                    CreateBlock(position, false);
                }
                else
                {
                    TeleportableBlock? teleportableBlock = CreateBlock(position, true);
                    if(teleportableBlock.HasValue) 
                    {
                        AddAsTeleportPoint(teleportableBlock.Value.variantType, teleportableBlock.Value.block);
                    }
                }
            }
        }

        NavMeshSurface navMeshSurface = _walkableParent.GetComponent<NavMeshSurface>();
        navMeshSurface.collectObjects = CollectObjects.Children;
        navMeshSurface.BuildNavMesh();
    }

    TeleportableBlock? CreateBlock(Vector3 at, bool walkable)
    {
        at.x+=0.1f;
        at.z+=0.1f;
        GameObject block = GameObject.Instantiate(_groundBlockPrefab, at, Quaternion.identity) as GameObject;
        block.transform.parent = walkable ? _walkableParent.transform : _obstacleParent.transform;

        if(walkable)
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

            return new TeleportableBlock{block = block, variantType = variant.type};
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
