using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CoT
{
    public class Inventory
    {
        private Texture2D
            pixelMain = new Texture2D(Game1.Game.GraphicsDevice, 1, 1),     //Placeholder textures
            pixelLayer1 = new Texture2D(Game1.Game.GraphicsDevice, 1, 1),
            pixelInvTile = new Texture2D(Game1.Game.GraphicsDevice, 1, 1);
        private Color[]
            colorDataMain = { Color.DarkGray },
            colorDataLayer1 = { Color.Gray },
            colorDataInvTile = { Color.Black };
    public static Rectangle
            rectMain,
            rectLayer1;

        InvTile[,] invTiles = new InvTile[4, 8];
        
        public static bool active = false;

        int
            //Main layer
            invMainWidth = 400,
            invMainMarginY = 20,
            invMainMarginX = 20,
            //Layer 1
            invLayer1MarginY = 30,
            invLayer1MarginX = 10,
            //Inventory tile
            invTileSize = 60,
            invTileLeftMargin = 4,
            invTileTopMargin = 4;

        public Inventory()
        {
            pixelMain.SetData(colorDataMain);
            pixelLayer1.SetData(colorDataLayer1);
            pixelInvTile.SetData(colorDataInvTile);

            rectMain = new Rectangle(Game1.ScreenWidth - invMainWidth - invMainMarginX, invMainMarginY, invMainWidth, Game1.ScreenHeight - invMainMarginY * 2);
            rectLayer1 = new Rectangle(rectMain.X + invLayer1MarginX, rectMain.Y + invLayer1MarginY, rectMain.Width - invLayer1MarginX * 2, rectMain.Height - invLayer1MarginY * 2);

            CreateTiles();
        }

        public void CreateTiles()
        {
            for (int i = 0; i < invTiles.GetLength(0); i++)
            {
                for (int j = 0; j < invTiles.GetLength(1); j++)
                {
                    invTiles[i, j] = new InvTile(new Vector2(rectLayer1.X + invTileLeftMargin * (i + 1) + invTileSize * i, rectLayer1.Y + invTileTopMargin * (j + 1) + invTileSize * j), invTileSize, pixelInvTile);
                }
            }
        }

        public void Update()
        {
            if (Input.CurrentKeyboard.IsKeyDown(Keys.I) && Input.LastKeyboard.IsKeyUp(Keys.I))
            {
                if (active)
                {
                    active = false;
                }
                else active = true;
            }

            foreach (InvTile tile in invTiles)
            {
                tile.Update();
            }
        }

        public void Draw(SpriteBatch sb)
        {
            if (active)
            {
                sb.Draw(pixelMain, rectMain, Color.White);
                sb.Draw(pixelLayer1, rectLayer1, Color.White);
                sb.DrawString(ResourceManager.Get<SpriteFont>("font1"), "Inventory", new Vector2(rectMain.X + 150, rectMain.Y + 5), Color.Black);

                //for (int i = 0; i < invTiles.GetLength(0); i++)
                //{
                //    for (int j = 0; j < invTiles.GetLength(1); j++)
                //    {
                //        invTiles[i, j].Draw(sb);
                //    }
                //}

                foreach (InvTile tile in invTiles)
                {
                    tile.Draw(sb);
                }
            }
        }
    }

    class InvTile
    {
        Texture2D pixel;
        Rectangle rectangle;

        public InvTile(Vector2 position, int tileSize, Texture2D pixel)
        {
            this.pixel = pixel;
            rectangle = new Rectangle((int)position.X, (int)position.Y, tileSize, tileSize);
        }

        public void Update()
        {
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(pixel, rectangle, Color.White);
        }
    }
}


