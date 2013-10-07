using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace TanksDrop.Projectiles
{
	/// <summary>
	/// A game object that moves according to its own physics (normally in straight lines) and its goal is to kill tanks.
	/// </summary>
	abstract class ProjectileObject
	{
		public Vector2 Position;
		protected float xAdvance;
		protected float yAdvance;
		protected TimeSpan originalTime;
		protected int width;
		protected int height;
		protected float _angle;
		protected float angle
		{
			get
			{
				return _angle;
			}
			set
			{
				_angle = value;
				ReCalcAngle();
			}
		}
		public float Angle
		{
			get { return _angle; }
		}
		public abstract float Scale { get; }
		public int bWidth
		{
			get
			{
				return ( int )( 32 * Scale );
			}
		}
		public int bHeight
		{
			get
			{
				return ( int )( 32 * Scale );
			}
		}
		protected float speed;
		public TankObject Owner;
		public Texture2D tex;
		protected float duration;
		public bool doesCount;

		public ProjectileObject( Vector2 position, float angle, TimeSpan gameTime, int width, int height, float factor, TankObject owner )
		{
			this.Position = position;
			this.speed = factor;
			this.angle = angle;
			originalTime = gameTime;
			this.width = width;
			this.height = height;
			Owner = owner;
			duration = Owner.ShotTime;
			doesCount = true;
		}

		protected ProjectileObject()
		{
			doesCount = true;
		}

		protected bool UpdatePhysics( TimeSpan gameTime, TankObject[] Tanks, HashSet<FenceObject> Fences )
		{
			if ( Position.X + bWidth >= width || Position.X <= 0 )
			{
				angle = 540 - angle;
			}
			if ( Position.Y + bHeight >= height || Position.Y <= 0 )
			{
				angle = 360 - angle;
			}
			Position.X += xAdvance;
			Position.Y += yAdvance;
			Rectangle boundingBox = new Rectangle( ( int )( Position.X ), ( int )( Position.Y ), bWidth, bHeight );
			foreach ( TankObject Tank in Tanks )
			{
				if ( Tank.IsInGame )
				{
					bool intersects = GameTools.RotRectIntersectRect( Tank.Position, Tank.Origin, Tank.Width, Tank.Height, Tank.Rotation, boundingBox );
					if ( intersects )
					{
						if ( Tank.BulletHit( this ) ) return true;
					}
				}
			}

			Rectangle newBoundingBox = new Rectangle( ( int )( Position.X + ( xAdvance ) ), ( int )( Position.Y + yAdvance  ), bWidth, bHeight );
			Rectangle averageBoundingBox = new Rectangle( ( int )( Position.X + xAdvance / 2 ), ( int )( Position.Y + yAdvance / 2 ), bWidth, bHeight );
			foreach ( FenceObject Fence in Fences )
			{
				bool intersects = GameTools.LineIntersectsRect( Fence.Point1, Fence.Point2, newBoundingBox ) || GameTools.LineIntersectsRect( Fence.Point1, Fence.Point2, averageBoundingBox );
				if ( intersects )
				{
					float fangle = Fence.angle < 180 ? Fence.angle + 180 : Fence.angle;
					angle = ( ( 2 * fangle ) - angle ) % 360;
					Position.X += xAdvance * 2;
					Position.Y += yAdvance * 2;
				}
			}
			return false;
		}

		public virtual bool Update( TimeSpan gameTime, TankObject[] Tanks, HashSet<FenceObject> Fences, HashSet<Pickup> Pickups )
		{
			return ( ( gameTime - originalTime ).TotalMilliseconds > duration && duration > 0 );
		}

		public abstract Texture2D LoadTex( ContentManager Content );

		public abstract void Draw( TimeSpan gameTime, SpriteBatch spriteBatch );

		public void Twist( float newAngle )
		{
			angle = newAngle;
			angle %= 360;
		}

		public void Turn( float TurnAngle )
		{
			angle += TurnAngle;
			angle %= 360;
		}

		protected void ReCalcAngle()
		{
			xAdvance = ( float )Math.Cos( MathHelper.ToRadians( _angle ) ) * speed;
			yAdvance = ( float )Math.Sin( MathHelper.ToRadians( _angle ) ) * speed;
			_angle = _angle % 360;
		}

		public void ChangeDuration( float newDuration )
		{
			duration = newDuration;
		}
	}
}
