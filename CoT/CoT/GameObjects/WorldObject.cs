using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CoT.GameObjects
{
    public class WorldObject : GameObject
    {
        public Vector2 DepthSortingOffset { get; set; }

        public WorldObject(string texture, Vector2 position, Rectangle sourceRectangle, Vector2 depthSortingOffset) : base(texture, position, sourceRectangle)
        {
            DepthSortingOffset = depthSortingOffset;
        }

        public override void Update()
        {
            LayerDepth = (Position.Y + DepthSortingOffset.Y).Normalize(short.MinValue, short.MaxValue);
            //base.Update();

            if (GameplayScreen.Instance.Player.Hitbox.Intersects(Hitbox) && GetType().Name == "WorldObject")
            {
                Transparency = 0.1f;
            }
            else
            {
                Transparency = 1f;
            }
        }

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);
            if (GameDebugger.Debug)
            {
                sb.Draw(ResourceManager.Get<Texture2D>("rectangle"), new FloatRectangle(Position + DepthSortingOffset, new Vector2(10, 10)), null, Color.White * 0.5f, Rotation, Vector2.Zero, SpriteEffects.None, 1f);
                sb.DrawString(ResourceManager.Get<SpriteFont>("font1"), "Depth: " + LayerDepth, Position + DepthSortingOffset, Color.LightGreen, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
            }
        }
    }
}
