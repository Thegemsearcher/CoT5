﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CoT
{
    public class PhysicsObject : GameObject
    {
        public Vector2 Direction { get; set; }
        public float Speed { get; set; }

        public PhysicsObject(Spritesheet spritesheet, Vector2 position, Vector2 direction, float speed) : base(spritesheet, position)
        {
            Direction = direction;
            Speed = speed;

            Direction = Vector2.Normalize(Direction);

            Console.WriteLine(Direction);
        }

        public override void OnRemove()
        {
        }

        public override void Update()
        {
            Position += Direction * Speed * Time.DeltaTime;

            base.Update();
        }

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);
        }
    }
}
