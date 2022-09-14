using UnityEngine;
using UnityEngine.Tilemaps;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace IGL_Tech.RPGM.Auto_Tile_Importer
{
    public class A1_WaterFall_Tile : A1_Water_Tile
    {
        public override bool GetTileAnimationData(Vector3Int position, ITilemap tilemap, ref TileAnimationData tileAnimationData)
        {
            //return base.GetTileAnimationData(position, tilemap, ref tileAnimationData);

            //need to update the current sprite of this tile based on the near tile
            byte status = Compute_Neighbours(position, tilemap);
            tileAnimationData.animatedSprites = new Sprite[] { frame1[status], frame2[status], frame3[status] };
            tileAnimationData.animationStartTime = this.animation_Start_time;
            tileAnimationData.animationSpeed = this.animation_Speed;

            return true;
        }

        protected override byte Compute_Neighbours(Vector3Int pos, ITilemap map)
        {
            int res = 0;
            Vector3Int tmp_pos = pos + Vector3Int.right;

            Auto_Tile_Base tmp_Tile = map.GetTile(tmp_pos) as Auto_Tile_Base;
            tmp_Tile = map.GetTile(tmp_pos) as Auto_Tile_Base;
            if (tmp_Tile != null && tmp_Tile.id_tile_type == this.id_tile_type)
                res |= (1 << 2);

            tmp_pos = pos + Vector3Int.left;
            tmp_Tile = map.GetTile(tmp_pos) as Auto_Tile_Base;
            if (tmp_Tile != null && tmp_Tile.id_tile_type == this.id_tile_type)
                res |= (1 << 6);

            return (byte)res;
        }

    }
}