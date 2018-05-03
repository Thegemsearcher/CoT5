using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace CoT
{
    [DataContract(Name = "GameObject", Namespace = "CoT")]
    public class GameObject
    {
        [DataMember]
        public Vector2 Position { get; set; }

        [DataMember]
        public Color Color { get; set; }

        [DataMember]
        public float Rotation { get; set; }

        [DataMember]
        public float Scale { get; set; }

        [DataMember]
        public FloatRectangle Hitbox { get; set; }

        [DataMember]
        public bool Remove { get; set; } = false;

        [DataMember]
        public Vector2 Center { get; set; }

        [DataMember]
        public float Transparency { get; set; }

        [DataMember]
        public bool IsActive { get; set; }

        [DataMember]
        public float LayerDepth { get; set; }

        public Spritesheet Spritesheet { get; set; }

        public GameObject(Spritesheet spritesheet, Vector2 position)
        {
            Spritesheet = spritesheet;
            Position = position;

            Transparency = 1f;
            Scale = 1f;
            IsActive = true;

            Color = Color.White;
            Hitbox = new FloatRectangle(Position, new Vector2(spritesheet.SourceRectangle.Width * Scale, spritesheet.SourceRectangle.Height * Scale));
            Center = new Vector2((float)spritesheet.SourceRectangle.Width / 2 * Scale, (float)spritesheet.SourceRectangle.Height / 2 * Scale);
        }

        public virtual void OnRemove() { }

        public virtual void Update()
        {
            Spritesheet.Update();
            Hitbox = new FloatRectangle(Position, new Vector2(Spritesheet.SourceRectangle.Width * Scale, Spritesheet.SourceRectangle.Height * Scale));
            Center = new Vector2((float)Spritesheet.SourceRectangle.Width / 2 * Scale, (float)Spritesheet.SourceRectangle.Height / 2 * Scale);
        }

        public virtual void Draw(SpriteBatch sb)
        {
            if (Camera.VisibleArea.Contains(Position))
            {
                sb.Draw(ResourceManager.Get<Texture2D>(Spritesheet.Texture), Position, Spritesheet.SourceRectangle, Color * Transparency, Rotation, Vector2.Zero, Scale, SpriteEffects.None, LayerDepth);

                if (GameDebugger.Debug)
                {
                    sb.Draw(ResourceManager.Get<Texture2D>("rectangle"), Hitbox, null, Color.Magenta * 0.3f, Rotation, Vector2.Zero, SpriteEffects.None, LayerDepth + 0.01f);
                }
            }
        }
    }
}
