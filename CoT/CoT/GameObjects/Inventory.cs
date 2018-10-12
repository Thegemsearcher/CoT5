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
            colorDataDGray = { Color.DarkGray },
            colorDataGray = { Color.Gray },
            colorDataBlack = { Color.Black },
            colorDataSanBrown = { Color.SandyBrown },
            colorDataSadBrown = { Color.SaddleBrown };
        public static Rectangle
            rectMain,
            rectLayer1;

        public static InventoryTile[,] invTiles = new InventoryTile[4, 8];

        public static Inventory Instance { get; set; }

        public static bool DragMode { get; set; }
        public float Opacity { get; set; }

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

        public Inventory(Spritesheet spritesheet, Vector2 position) : base(spritesheet, position)
        {
            spritesheet.SetFrameCount(new Point(5, 1));
            spritesheet.Interval = 100;

            pixelMain.SetData(colorDataSadBrown);
            pixelLayer1.SetData(colorDataDGray);
            pixelInvTile.SetData(colorDataBlack);
            IsActive = false;
            DragMode = false;
            Opacity = 0.8f;
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
                        new Vector2(rectLayer1.X + rectLayer1.Width / 2 - invTileTotalWidth / 2 + invTileLeftMargin * (i + 1) + invTileSize * i, rectLayer1.Y + rectLayer1.Height / 2 - invTileTotalHeight / 2 + invTileTopMargin * (j + 1) + invTileSize * j),
                        invTileSize,
                        pixelInvTile,
                        Spritesheet);
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
                    SoundManager.Instance.PlaySound("invClose", 1.0f, 0.0f, 0.5f);
                }
                else
                {
                    IsActive = true;
                    SoundManager.Instance.PlaySound("invOpen", 1.0f, 0.0f, 0.5f);
                }
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
                sb.Draw(ResourceManager.Get<Texture2D>("rectangle"), rectMain, null, Color.Chocolate * Opacity, 0f, Vector2.Zero, SpriteEffects.None, 0.1f);
                sb.Draw(ResourceManager.Get<Texture2D>("rectangle"), rectLayer1, null, Color.SaddleBrown * Opacity, 0f, Vector2.Zero, SpriteEffects.None, 0.2f);
                sb.DrawString(ResourceManager.Get<SpriteFont>("font1"), "Inventory", new Vector2(rectMain.X + 150, rectMain.Y + 5), Color.Black * Opacity, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.3f);

                foreach (InventoryTile tile in invTiles)
                    tile.Draw(sb);

                foreach (Item item in GameManager.Instance.ItemManager.Items)
                    if (item.isInBag)
                    {
                        sb.Draw(item.texItem, item.rectItemInv, item.sourceRectSprite, Color.White * Opacity, 0f, Vector2.Zero, SpriteEffects.None, 0.5f);
                        if (item.rectItemInv.Contains(Input.CurrentMousePosition) && Instance.IsActive && !DragMode)
                        {
                            string stringItemdesc = item.itemName + "\n" + item.itemDescription + "\n<Right click to use>";
                            Vector2 stringdim = ResourceManager.Get<SpriteFont>("font1").MeasureString(stringItemdesc);
                            Vector2 posItemdesc = new Vector2(Input.CurrentMousePosition.X - stringdim.X, Input.CurrentMousePosition.Y - stringdim.Y);
                            int borderT = 3;
                            int padding = 5;

                            Rectangle rectBackground = new Rectangle((int)posItemdesc.X - padding, (int)posItemdesc.Y - padding, (int)stringdim.X + padding * 2, (int)stringdim.Y + padding * 2);
                            Rectangle rectBorder = new Rectangle(rectBackground.X - borderT, rectBackground.Y - borderT, rectBackground.Width + borderT * 2, rectBackground.Height + borderT * 2);
                            sb.Draw(ResourceManager.Get<Texture2D>("rectangle"), rectBorder, null, Color.Black, 0f, Vector2.Zero, SpriteEffects.None, 0.6f);
                            sb.Draw(ResourceManager.Get<Texture2D>("rectangle"), rectBackground, null, Color.SaddleBrown, 0f, Vector2.Zero, SpriteEffects.None, 0.7f);
                            sb.DrawString(ResourceManager.Get<SpriteFont>("font1"), stringItemdesc, posItemdesc, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.8f);
                        }
                    }
                        
            }
        }

        public override void OnRemove()
        {
            throw new NotImplementedException();
        }
    }
}


