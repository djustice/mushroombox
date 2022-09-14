using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace IGL_Tech.RPGM.Auto_Tile_Importer
{
    public class RPGM_XP_Water_Tile : A1_Water_Tile
    {
        [SerializeField]
        public Sprite[] frame4;

        public override bool GetTileAnimationData(Vector3Int position, ITilemap tilemap, ref TileAnimationData tileAnimationData)
        {
            //return base.GetTileAnimationData(position, tilemap, ref tileAnimationData);

            //need to update the current sprite of this tile based on the near tile
            byte status = Compute_Neighbours(position, tilemap);
            tileAnimationData.animatedSprites = new Sprite[] { frame1[status], frame2[status], frame3[status], frame4[status] };
            tileAnimationData.animationStartTime = this.animation_Start_time;
            tileAnimationData.animationSpeed = this.animation_Speed;

            return true;
        }
    }
}