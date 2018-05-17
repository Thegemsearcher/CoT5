using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RoyT.AStar;

namespace CoT.GameObjects
{
    public class WorldObject : GameObject
    {
        public Vector2 DepthSortingOffset { get; set; }
        public bool Movable { get; set; }

        public WorldObject(Spritesheet spritesheet, Vector2 position, Vector2 depthSortingOffset, bool movable) : base(spritesheet, position)
        {
            DepthSortingOffset = depthSortingOffset;
            Movable = movable;

            LayerDepth = (Position.Y + DepthSortingOffset.Y).Normalize(short.MinValue, short.MaxValue);
        }

        public override void Update()
        {
            if (Movable)
            {
                LayerDepth = (Position.Y + DepthSortingOffset.Y).Normalize(short.MinValue, short.MaxValue);
            }

            for (int i = 0; i < CreatureManager.Instance.Creatures.Count; i++)
            {
                Creature c = CreatureManager.Instance.Creatures[i];
                if (c is Player)
                {
                    if (new FloatRectangle(c.Position, new Vector2(c.Spritesheet.SourceRectangle.Size.X, c.Spritesheet.SourceRectangle.Size.Y)).Intersects(Hitbox) && GetType().Name == "WorldObject")
                    {
                        if (LayerDepth > c.LayerDepth)
                        {
                            if (Transparency > 0.25f) Transparency -= Time.DeltaTime * 2;
                        }
                    }
                }
                if (c.Hitbox.Intersects(Hitbox) && GetType().Name == "WorldObject")
                {
                    if (LayerDepth > c.LayerDepth)
                    {
                        if (Transparency > 0.25f) Transparency -= Time.DeltaTime * 2;
                    }
                }
            }

            if (Transparency < 1f) Transparency += Time.DeltaTime / 2;

            base.Update();
        }

        public override void Draw(SpriteBatch sb)
        {
            if (GameDebugger.Debug)
            {
                sb.Draw(ResourceManager.Get<Texture2D>("rectangle"), new FloatRectangle(Position + DepthSortingOffset, new Vector2(10, 10)), null, Color.White * 0.5f, Rotation, Vector2.Zero, SpriteEffects.None, 1f);
                sb.DrawString(ResourceManager.Get<SpriteFont>("font1"), "Depth: " + LayerDepth, Position + DepthSortingOffset, Color.LightGreen, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
            }

            base.Draw(sb);
        }
    }
}
