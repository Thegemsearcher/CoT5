using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CoT
{
    public abstract class Item : GameObject
    {
        public enum ItemType { EquipmentItem, PotionItem };
        //protected Texture2D texItemDrop, texItemInv;
        public Texture2D texItem;
        protected Rectangle rectCurrentSprite, rectFirstSprite;
        public Rectangle rectItemDrop, rectItemInv, sourceRectSprite, oldRectBeforeDrag;
        public string itemName, itemDescription;
        
        public int verticalTileSlotSize;
        public int identityRange = 200;
        public bool isInBag, dragMode, isInIdentityRange;

        public Item(Spritesheet spritesheet, Vector2 position, Rectangle sourceRectangle, bool putInBag) : base(spritesheet, position)
        {
            spritesheet.SetFrameCount(new Point(1, 1));
            spritesheet.Interval = 100;

            if (putInBag == true)
                Pickup();
            else
                isInBag = false;

            dragMode = false;
        }


        public virtual void Pickup()
        {
            bool roomAvailable = false;

            for (int i = 0; i < Inventory.invTiles.GetLength(0); i++)
            {
                for (int j = 0; j < Inventory.invTiles.GetLength(1); j++)
                {
                    InventoryTile currentTile = Inventory.invTiles[i, j];

                    if (!currentTile.occupied)
                    {
                        if (verticalTileSlotSize == 2 && j != 7)
                        {
                            if (!Inventory.invTiles[i, j + 1].occupied)
                            {
                                roomAvailable = true;
                                rectItemInv = new Rectangle(currentTile.rectangle.X, currentTile.rectangle.Y, Inventory.invTileSize, Inventory.invTileSize * 2 + Inventory.invTileTopMargin);
                                currentTile.occupied = true;
                                Inventory.invTiles[i, j + 1].occupied = true;
                            }
                        } else if (verticalTileSlotSize == 1)
                        {
                            roomAvailable = true;
                            rectItemInv = currentTile.rectangle;
                            currentTile.occupied = true;
                        }
                    }
                    if (roomAvailable)
                    {
                        Inventory.invTiles[i, j] = currentTile;
                        break;
                    }
                }
                if (roomAvailable)
                {
                    isInBag = true;
                    break;
                }
            }
        }

        public virtual void Drop()
        {
            isInBag = false;
            foreach (Player player in CreatureManager.Instance.Creatures.OfType<Player>())
            {
                rectItemDrop.X = (int)player.GroundPosition.X;
                rectItemDrop.Y = (int)player.GroundPosition.Y;
                this.LayerDepth = player.LayerDepth;
            }

            if (this is Potion)
                SoundManager.Instance.PlaySound("dropPotion", 0.8f, 0.0f, 0.0f);
        }

        public virtual void Drag()
        {
            rectItemInv = new Rectangle((int)Input.CurrentMousePosition.X - rectItemInv.Width / 2, (int)Input.CurrentMousePosition.Y - rectItemInv.Height / 2,
                    rectItemInv.Width, rectItemInv.Height);

            if (Input.IsLeftClickReleased)
            {
                bool draggedToEmptyTile = false;
                foreach (InventoryTile tile in Inventory.invTiles)
                {
                    if (!tile.occupied && tile.rectangle.Contains(Input.CurrentMousePosition))
                    {
                        rectItemInv = tile.rectangle;
                        draggedToEmptyTile = true;

                        if (this is Potion)
                            SoundManager.Instance.PlaySound("invPotion", 0.5f, 0.0f, 0.0f);

                        break;
                    }
                }

                if (!Inventory.rectMain.Contains(Input.CurrentMousePosition))
                    Drop();
                else if (!draggedToEmptyTile)
                    rectItemInv = oldRectBeforeDrag;
                
                dragMode = false;
            }
        }

        public virtual void Use()
        {
        }

        public override void OnRemove()
        {
        }

        public override void Update()
        {
            base.Update();

            if (GameDebugger.Debug
                && Input.CurrentKeyboard.IsKeyDown(Keys.S)
                && Input.LastKeyboard.IsKeyUp(Keys.S)
                && rectItemDrop.Contains(Input.CurrentMousePosition.ScreenToWorld())
                && !isInBag) 
            {
                Console.WriteLine("Detected Pickup Attempt");
                Pickup();
            }

            if (Input.CurrentKeyboard.IsKeyDown(Keys.D)
                && Input.LastKeyboard.IsKeyUp(Keys.D)
                && rectItemInv.Contains(Input.CurrentMousePosition)
                && isInBag
                && Inventory.Instance.IsActive)
            {
                Console.WriteLine("Detected Item deletion attempt");
                IsActive = false;
            }

            if (Input.IsRightClickPressed
                && rectItemInv.Contains(Input.CurrentMousePosition)
                && isInBag
                && Inventory.Instance.IsActive)
            {
                Console.WriteLine("Detected Item consumption attempt");
                Use();
            }

            if (!dragMode && rectItemInv.Contains(Input.CurrentMousePosition)
                && Input.LastMouse.LeftButton == ButtonState.Released && Input.IsLeftClickPressed)
            {
                dragMode = true;
                oldRectBeforeDrag = rectItemInv;
                //SoundManager.Instance.PlaySound("invGrab", 0.5f, 0.0f, 0.0f);
            }

            if (dragMode)
                Drag();

            foreach (Creature player in CreatureManager.Instance.Creatures.OfType<Player>())
            {
                if (rectItemDrop.Contains(Input.CurrentMousePosition.ScreenToWorld())
                    && rectItemDrop.Intersects(player.Hitbox)
                    && Input.IsRightClickPressed
                    && !isInBag)
                {
                    Console.WriteLine("Detected Pickup Attempt");
                    Pickup();
                }

                if (Vector2.Distance(player.Position + player.Center, new Vector2(rectItemDrop.X + rectItemDrop.Width / 2, rectItemDrop.Y + rectItemDrop.Height / 2)) <= identityRange)
                    isInIdentityRange = true;
                else
                    isInIdentityRange = false;
            }
        }

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);

            if (!isInBag)
                sb.Draw(texItem, rectItemDrop, rectCurrentSprite, Color.White, 0f, Vector2.Zero, SpriteEffects.None, LayerDepth + 0.1f);
            if (isInIdentityRange && !isInBag)
            {
                sb.DrawString(ResourceManager.Get<SpriteFont>("font1"), itemName, new Vector2(rectItemDrop.X, rectItemDrop.Y), Color.White, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 1f);
            }
        }
    }
}
