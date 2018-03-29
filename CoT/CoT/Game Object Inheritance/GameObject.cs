using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace CoT
{
    public abstract class GameObject
    {
        public string Texture { get; set; }
        public Vector2 Position { get; set; }
        public Color Color { get; set; }
        public float Rotation { get; set; }
        public Rectangle SourceRectangle { get; set; }
        public float Scale { get; set; }
        public FloatRectangle Hitbox { get; set; }
        public bool Remove { get; set; }
        public Vector2 Offset { get; set; }
        public float Transparency { get; set; }
        public bool IsActive { get; set; }

        protected GameObject(string texture, Vector2 position, Rectangle sourceRectangle)
        {
            Texture = texture;
            Position = position;
            SourceRectangle = sourceRectangle;

            Transparency = 1f;
            Scale = 1f;
            IsActive = true;

            Color = Color.White;
            Hitbox = new FloatRectangle(Position, new Vector2(SourceRectangle.Width * Scale, SourceRectangle.Height * Scale));
            Offset = new Vector2((float)SourceRectangle.Width / 2, (float)SourceRectangle.Height / 2);
        }

        public abstract void OnRemove();

        public virtual void Update()
        {
            Hitbox = new FloatRectangle(Position, new Vector2(SourceRectangle.Width * Scale, SourceRectangle.Height * Scale));
        }

        public virtual void Draw()
        {
            Game1.Game.SpriteBatch.Draw(ResourceManager.Get<Texture2D>(Texture), Position, SourceRectangle, Color * Transparency, Rotation, Vector2.Zero, Scale, SpriteEffects.None, 0f);

            // Debug
            Game1.Game.SpriteBatch.Draw(ResourceManager.Get<Texture2D>("rectangle"), new Rectangle((int)Hitbox.Position.X, (int)Hitbox.Position.Y, (int)Hitbox.Size.X, (int)Hitbox.Size.Y), Color.Red * 0.5f);
        }
    }
}
