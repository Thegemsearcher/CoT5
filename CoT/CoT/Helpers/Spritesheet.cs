using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CoT
{
    public class Spritesheet
    {
        public string Texture { get; set; }
        public int StartFrame { get; set; }
        public Point FrameCount { get; set; }
        public int TotalFrames { get; set; }
        public Rectangle SourceRectangle { get; set; }
        public int CurrentFrame { get; set; }
        public Point Offset { get; set; }
        public Point DefaultOffset { get; set; }
        public float FrameTimer { get; set; }
        public float Interval { get; set; }
        public bool TimerEnabled { get; set; } = true;

        public Spritesheet(string texture, Point frameCount, Rectangle sourceRectangle, float interval = 1000)
        {
            Texture = texture;
            FrameCount = frameCount;
            SourceRectangle = sourceRectangle;
            Interval = interval;

            Offset = new Point(SourceRectangle.X, SourceRectangle.Y);
            DefaultOffset = new Point(Offset.X, Offset.Y);
            TotalFrames = FrameCount.X * FrameCount.Y;
        }

        public void Update()
        {
            if (TimerEnabled) FrameTimer += Time.DeltaTime * 1000;

            if (FrameTimer >= Interval)
            {
                CurrentFrame++;
                if (CurrentFrame >= TotalFrames)
                {
                    CurrentFrame = StartFrame;
                }
                SourceRectangle = CalculateSourceRectangle(CurrentFrame);
                FrameTimer = 0;
            }
        }

        public Spritesheet SetFrameCount(Point frameCount)
        {
            FrameCount = frameCount;
            TotalFrames = frameCount.X * frameCount.Y;
            return this;
        }

        public Spritesheet SetCurrentFrame(int frame)
        {
            CurrentFrame = frame;
            SourceRectangle = CalculateSourceRectangle(CurrentFrame);
            return this;
        }

        private Rectangle CalculateSourceRectangle(int currentFrame)
        {
            return new Rectangle(
                ((currentFrame % FrameCount.X) * SourceRectangle.Width) + Offset.X,
                ((currentFrame / FrameCount.X) * SourceRectangle.Height) + Offset.Y,
                SourceRectangle.Width,
                SourceRectangle.Height);
        }

        public Color[] GetTextureColorData(int currentFrame)
        {
            Color[] data = new Color[SourceRectangle.Width * SourceRectangle.Height];
            Rectangle predictedRec = CalculateSourceRectangle(currentFrame);
            ResourceManager.Get<Texture2D>(Texture).GetData(0, predictedRec, data, 0, data.Length);
            return data;
        }
    }
}
