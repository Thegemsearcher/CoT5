using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoT
{
    public class Tile
    {
        public TileType TileType { get; set; }
        public Spritesheet Spritesheet { get; set; }
        public string Tag { get; set; }

        public Tile(TileType tileType, Spritesheet spritesheet)
        {
            TileType = tileType;
            Spritesheet = spritesheet;
        }

        public Tile Clone()
        {
            return new Tile(TileType, new Spritesheet(Spritesheet.Texture, Spritesheet.FrameCount, Spritesheet.SourceRectangle, Spritesheet.Interval));
        }
    }
}
