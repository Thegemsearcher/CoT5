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
    public class Item : GameObject
    {
        //protected Texture2D texItemDrop, texItemInv;
        protected Texture2D texItem;
        public Rectangle rectItemDrop, rectItemInv, sourceRectSprite;

        public int verticalTileSlotSize;
        public bool isInBag;

        public Item(Spritesheet texture, Vector2 position, Rectangle sourceRectangle, bool putInBag) : base(texture, position)
        {
            if (putInBag == true)
                Pickup();
            else
                isInBag = false;
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
        }

        public virtual void Drag()
        {
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

            if (Input.CurrentKeyboard.IsKeyDown(Keys.S)
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

            if (Input.CurrentKeyboard.IsKeyDown(Keys.F)
                && Input.LastKeyboard.IsKeyUp(Keys.F)
                && rectItemInv.Contains(Input.CurrentMousePosition)
                && isInBag
                && Inventory.Instance.IsActive)
            {
                Console.WriteLine("Detected Item consumption attempt");
                Use();
            }
        }

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);
        }
    }
}
