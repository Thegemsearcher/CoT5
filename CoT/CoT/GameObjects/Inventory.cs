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
    public class Inventory : GameObject
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

        public static InventoryTile[,] invTiles = new InventoryTile[4, 8];

        public static Inventory Instance { get; set; }

        private int
            //Main layer
            invMainWidth = 400,
            invMainMarginY = 20,
            invMainMarginX = 20,
            //Layer 1
            invLayer1MarginY = 30,
            invLayer1MarginX = 10,
            invLayer1PaddingY = 50,
            invLayer1PaddingX = 50;
        public static int
            //Inventory tile
            invTileSize = 60,
            invTileLeftMargin = 10,
            invTileTopMargin = 10,
            invTileTotalWidth = (invTileSize + invTileLeftMargin) * invTiles.GetLength(0),
            invTileTotalHeight = (invTileSize + invTileTopMargin) * invTiles.GetLength(1);

        public Inventory(string texture, Vector2 position, Rectangle sourceRectangle) : base(texture, position, sourceRectangle)
        {
            pixelMain.SetData(colorDataMain);
            pixelLayer1.SetData(colorDataLayer1);
            pixelInvTile.SetData(colorDataInvTile);
            IsActive = false;
            rectMain = new Rectangle(Game1.ScreenWidth - invMainWidth - invMainMarginX, invMainMarginY, invMainWidth, Game1.ScreenHeight - invMainMarginY * 2);
            rectLayer1 = new Rectangle(rectMain.X + invLayer1MarginX, rectMain.Y + invLayer1MarginY, rectMain.Width - invLayer1MarginX * 2, rectMain.Height - invLayer1MarginY * 2);

            Instance = this;
            CreateTiles();
        }

        public void CreateTiles()
        {
            for (int i = 0; i < invTiles.GetLength(0); i++)
            {
                for (int j = 0; j < invTiles.GetLength(1); j++)
                {
                    invTiles[i, j] = new InventoryTile(
                        new Vector2(rectLayer1.X + rectLayer1.Width / 2 - invTileTotalWidth / 2 + invTileLeftMargin * (i + 1) + invTileSize * i, rectLayer1.Y + rectLayer1.Height / 2 - invTileTotalHeight / 2 + invTileTopMargin * (j + 1) + invTileSize * j/*rectLayer1.X + invLayer1PaddingX + invTileLeftMargin * (i + 1) + invTileSize * i, rectLayer1.Y + invLayer1PaddingY + invTileTopMargin * (j + 1) + invTileSize * j*/),
                        invTileSize,
                        pixelInvTile,
                        null,
                        SourceRectangle);
                }
            }
        }

        public override void Update()
        {
            if (Input.CurrentKeyboard.IsKeyDown(Keys.I) && Input.LastKeyboard.IsKeyUp(Keys.I))
            {
                if (IsActive)
                {
                    IsActive = false;
                }
                else IsActive = true;
            }

            foreach (InventoryTile tile in invTiles)
            {
                tile.Update();
            }
        }

        public override void Draw(SpriteBatch sb)
        {
            if (IsActive)
            {
                sb.Draw(pixelMain, rectMain, null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.1f);
                sb.Draw(pixelLayer1, rectLayer1, null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.2f);
                sb.DrawString(ResourceManager.Get<SpriteFont>("font1"), "Inventory", new Vector2(rectMain.X + 150, rectMain.Y + 5), Color.Black, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.3f);

                foreach (InventoryTile tile in invTiles)
                    tile.Draw(sb);

                foreach (Item item in GameManager.Instance.ItemManager.Items)
                    if (item.isInBag)
                        sb.Draw(ResourceManager.Get<Texture2D>(item.Texture), item.rectItemInv, item.sourceRectSprite, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.5f);
            }
        }

        public override void OnRemove()
        {
            throw new NotImplementedException();
        }
    }
}


