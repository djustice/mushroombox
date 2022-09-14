using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace IGL_Tech.RPGM.Auto_Tile_Importer
{
    public class A2_Tile : Auto_Tile_Base
    {
        [SerializeField]
        public Sprite[] tile_Variants;

        public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
        {
            //need to update the current sprite of this tile based on the near tile
            byte status = Compute_Neighbours(position, tilemap);
            tileData.sprite = tile_Variants[status];
            tileData.colliderType = base.colliderType;

            //if (tileData.gameObject != null)
            //    DestroyImmediate(tileData.gameObject);
            if (this.gameObject != null)
            {
                tileData.gameObject = this.gameObject;
            }
        }

        public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
        {
            if(go != null)
            {
                go.transform.position += new Vector3(go.transform.localScale.x / 2, 0, go.transform.localScale.z / 2);
                go.transform.Rotate(0, Random.Range(0, 180), 0);
            }
            return base.StartUp(position, tilemap, go);
        }
    }
}