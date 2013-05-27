using OpenTKTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using System.Drawing;

namespace GOTYE
{
    abstract class SpaceJunk : IRenderable
    {
        protected Sprite Sprite;

        public Vector2 Position
        {
            get { return Sprite.Position; }
        }

        public Vector2 Size
        {
            get { return Sprite.Size; }
        }

        public float Scale
        {
            get { return Sprite.Scale.X; }
        }

        public abstract Vector2 Velocity
        {
            get;
        }

        public Vector2 Position
        {
            get
            {
                return Sprite.Position;
            }
        }

        public float Scale
        {
            get
            {
                return Sprite.Scale.X;
            }
        }

        public abstract float Depth
        {
            get;
        }

        public Program Scene
        {
            get;
            set;
        }

        public SpaceJunk(float x, float miny, float maxy, BitmapTexture2D texture, float scale)
        {            
            Sprite = new Sprite(texture, scale);
            Sprite.UseCentreAsOrigin = true;
            Sprite.X = x;
            Sprite.Y = Program.Rand.NextSingle() * (maxy - miny) + miny;
        }

        public SpaceJunk(float x, float y, BitmapTexture2D texture, float scale)
        {
            Sprite = new Sprite(texture, scale);
            Sprite.UseCentreAsOrigin = true;
            Sprite.X = x;
            Sprite.Y = y;
        }

        public virtual void Draw(SpriteShader shader)
        {
            Sprite.Render(shader);
        }

        public virtual void Update(IEnumerable <SpaceJunk> junkage)
        {
            Sprite.X = Sprite.X + Velocity.X;
            Sprite.Y = Sprite.Y + Velocity.Y;
        }

        public bool IsTouching(SpaceJunk that)
        {
            Vector2 center = that.Position;
            float thatradius = Math.Min(that.Sprite.Width, that.Sprite.Height) / 2;
            float thisradius = Math.Min(Sprite.Width,Sprite.Height) / 2;
            float distance = (center - Sprite.Position).Length;
            return distance <= thatradius + thisradius;
        }

        public bool IsHit(Vector2 pos)
        {
            Vector2 hit;
            return IsHit(pos, pos, out hit);
        }

        public virtual bool IsHit(Vector2 start, Vector2 end, out Vector2 hit)
        {
            hit = new Vector2();
            return false;
        }

        public virtual bool ShouldRemove(Rectangle bounds)
        {
            bool outofbounds = false;

            if (Velocity.X >= 0)
            {
                outofbounds = Sprite.X - Sprite.Width / 2 > bounds.Right;
            }
            else
            {
                outofbounds = Sprite.X + Sprite.Width / 2 < bounds.Left;
            }

            if (Velocity.Y >= 0)
            {
                outofbounds = outofbounds || Sprite.Y - Sprite.Height / 2 > bounds.Bottom;
            }
            else
            {
                outofbounds = outofbounds || Sprite.Y + Sprite.Height / 2 < bounds.Top;
            }
            return outofbounds;
        }
    }
}
