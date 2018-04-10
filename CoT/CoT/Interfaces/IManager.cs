using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace CoT
{
    public interface IManager
    {
        void Initialize();
        void LoadContent();
        void Update();
        void Draw(SpriteBatch spriteBatch);
        void DrawToWorldAdditiveBlend(SpriteBatch spriteBatch);
        void DrawUserInterface(SpriteBatch spriteBatch);
    }
}
