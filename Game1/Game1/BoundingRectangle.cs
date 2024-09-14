using System;
using Microsoft.Xna.Framework;

namespace Game1
{
    public struct BoundingRectangle
    {
        public float X;

        public float Y;

        public float Width;

        public float Height;

        public float Left => X - Width / 2;

        public float Right => X + Width / 2;

        public float Top => Y - Height / 2;

        public float Bottom => Y + Height / 2;

        public BoundingRectangle(Vector2 position, float width, float height)
        {
            X = position.X;
            Y = position.Y;
            Width = width;
            Height = height;
        }

        public bool CollidesWith(BoundingRectangle other)
        {
            return CollisionHelper.Collides(this, other);
        }

        public bool CollidesWith(BoundingCircle other)
        {
            return CollisionHelper.Collides(other, this);
        }
    }
}
