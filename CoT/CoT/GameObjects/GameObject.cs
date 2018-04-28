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
        public string Texture { get; set; }

        [DataMember]
        public Vector2 Position { get; set; }

        [DataMember]
        public Color Color { get; set; }

        [DataMember]
        public float Rotation { get; set; }

        [DataMember]
        public Rectangle SourceRectangle { get; set; }

        [DataMember]
        public float Scale { get; set; }

        [DataMember]
        public FloatRectangle Hitbox { get; set; }

        [DataMember]
        public bool Remove { get; set; } = false;

        [DataMember]
        public Vector2 Offset { get; set; }

        [DataMember]
        public float Transparency { get; set; }

        [DataMember]
        public bool IsActive { get; set; }

        [DataMember]
        public float LayerDepth { get; set; }

        public GameObject(string texture, Vector2 position, Rectangle sourceRectangle)
        { 
            Texture = texture;
            Position = position;
            SourceRectangle = sourceRectangle;

            Transparency = 1f;
            Scale = 1f;
            IsActive = true;

            Color = Color.White;
            Hitbox = new FloatRectangle(Position, new Vector2(SourceRectangle.Width * Scale, SourceRectangle.Height * Scale));
            //Offset = new Vector2((float)SourceRectangle.Width / 2, (float)SourceRectangle.Height / 2);
            Offset = new Vector2(0,0);
        }

        public virtual void OnRemove() { }

        public virtual void Update()
        {
            Hitbox = new FloatRectangle(Position, new Vector2(SourceRectangle.Width * Scale, SourceRectangle.Height * Scale));
        }

        public virtual void Draw(SpriteBatch sb)
        {
            if (Camera.VisibleArea.Contains(Position))
            {
                sb.Draw(ResourceManager.Get<Texture2D>(Texture), Position + Offset, SourceRectangle, Color * Transparency, Rotation, Vector2.Zero, Scale, SpriteEffects.None, LayerDepth);

                if (GameDebugger.Debug)
                    sb.Draw(ResourceManager.Get<Texture2D>("rectangle"), Hitbox, null, Color.Magenta * 0.3f, Rotation, Vector2.Zero, SpriteEffects.None, 0.9f);
            }
        }
    }
}
