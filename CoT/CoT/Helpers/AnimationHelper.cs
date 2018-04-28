
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CoT.Helpers
{
    static class AnimationHelper
    {
        //static public Rectangle AnimationPlayer(Rectangle sourceRectangle, float time , string animationType)
        //{
        //    if (animationType.Equals("stationary"))
        //    {
        //        sourceRectangle = PlayerStationary(sourceRectangle, time);
        //    }

        //    return sourceRectangle;
        //}
        //static public Rectangle PlayerStationary(Rectangle sourceRectangle, float time)
        //{
        //    int startX = 0;
        //    int xOffset = 25;
        //    int frames = 5;
        //    int x = sourceRectangle.X / frames;

        //    if (x < frames)
        //    {
        //        sourceRectangle.X = (x + 1) * xOffset;
        //    } else
        //    {
        //        sourceRectangle.X = startX;
        //    }


        //    return sourceRectangle;
        //}
        static public Rectangle Animation(Rectangle sourceRectangle, ref float frameTimer, float frameInterval, ref int frame, int animationStarts, int amountOfFrames, int animationOffset)
        {
            frameTimer -= (float)(Time.GameTime.ElapsedGameTime.TotalMilliseconds);
            if (frameTimer <= 0)
            {
                frameTimer = frameInterval;
                frame++;
                sourceRectangle.X = animationStarts + (frame % amountOfFrames) * animationOffset;
            }
            
            return sourceRectangle;
        }
    }
}
