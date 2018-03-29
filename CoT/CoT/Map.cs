using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CoT
{
    public class Map
    {
        public int tileHeight = 80;

        public Map()
        {
        }

        public void Update()
        {
        }

        public void Draw()
        {
            Vector2 cartesianTileWorldPos =
                new Vector2(Camera.ScreenToWorld(Input.CurrentMousePosition).X / Game1.Game.map.tileHeight,
                    Camera.ScreenToWorld(Input.CurrentMousePosition).Y / Game1.Game.map.tileHeight);

            Point isometricScreenTile = (cartesianTileWorldPos.ToScreen() + new Vector2(-0.5f, 0.5f)).ToPoint();

            for (int i = 0; i < 100; i++)
            {
                for (int j = 0; j < 100; j++)
                {
                    if (isometricScreenTile == new Point(i, j))
                    {
                        Game1.Game.SpriteBatch.Draw(ResourceManager.Get<Texture2D>("tile1"), new Vector2(i * tileHeight, j * tileHeight).ToWorld(), Color.Red);
                    }
                    else
                    {
                        Game1.Game.SpriteBatch.Draw(ResourceManager.Get<Texture2D>("tile1"), new Vector2(i * tileHeight, j * tileHeight).ToWorld(), Color.White);
                    }
                }
            }
        }
    }
}