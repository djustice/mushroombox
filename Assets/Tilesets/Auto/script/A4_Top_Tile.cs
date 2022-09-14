using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace IGL_Tech.RPGM.Auto_Tile_Importer
{
    public class A4_Top_Tile : A2_Tile
    {
        /// <summary>
        /// Compute the near cell status in the form of a byte mask
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="map"></param>
        /// <returns></returns>
        protected override byte Compute_Neighbours(Vector3Int pos, ITilemap map)
        {
            int res = 0;

            Vector3Int tmp_pos = pos + Vector3Int.up;
            Auto_Tile_Base tmp_Tile = map.GetTile(tmp_pos) as Auto_Tile_Base;
            if (tmp_Tile != null && tmp_Tile.id_tile_type == this.id_tile_type)
                res |= (1 << 0);

            tmp_pos = pos + Vector3Int.right + Vector3Int.up;
            tmp_Tile = map.GetTile(tmp_pos) as Auto_Tile_Base;
            if (tmp_Tile != null && tmp_Tile.id_tile_type == this.id_tile_type)
                res |= (1 << 1);

            tmp_pos = pos + Vector3Int.right;
            tmp_Tile = map.GetTile(tmp_pos) as Auto_Tile_Base;
            if (tmp_Tile != null && tmp_Tile.id_tile_type == this.id_tile_type)
                res |= (1 << 2);

            tmp_pos = pos + Vector3Int.down + Vector3Int.right;
            tmp_Tile = map.GetTile(tmp_pos) as Auto_Tile_Base;
            if (tmp_Tile != null && tmp_Tile.id_tile_type == this.id_tile_type)
                res |= (1 << 3);

            tmp_pos = pos + Vector3Int.down;
            tmp_Tile = map.GetTile(tmp_pos) as Auto_Tile_Base;
            if (tmp_Tile != null && tmp_Tile.id_tile_type == this.id_tile_type)
                res |= (1 << 4);

            tmp_pos = pos + Vector3Int.down + Vector3Int.left;
            tmp_Tile = map.GetTile(tmp_pos) as Auto_Tile_Base;
            if (tmp_Tile != null && tmp_Tile.id_tile_type == this.id_tile_type)
                res |= (1 << 5);

            tmp_pos = pos + Vector3Int.left;
            tmp_Tile = map.GetTile(tmp_pos) as Auto_Tile_Base;
            if (tmp_Tile != null && tmp_Tile.id_tile_type == this.id_tile_type)
                res |= (1 << 6);

            tmp_pos = pos + Vector3Int.up + Vector3Int.left;
            tmp_Tile = map.GetTile(tmp_pos) as Auto_Tile_Base;
            if (tmp_Tile != null && tmp_Tile.id_tile_type == this.id_tile_type)
                res |= (1 << 7);

            return (byte)res;
        }
    }
}