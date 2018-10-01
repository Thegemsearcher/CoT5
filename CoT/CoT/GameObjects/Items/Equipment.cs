using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CoT
{
    public class Equipment : Item
    {
        public enum GearType { WoodenSword, WoodenShield, WoodenHelmet, WoodenLeggings, WoodenBracers, WoodenBoots, WoolGloves}

        public Equipment(Spritesheet spritesheet, Vector2 position, Rectangle sourceRectangle, bool putInBag, GearType gearType) : base(spritesheet, position, sourceRectangle, putInBag)
        {

        }
    }
}
