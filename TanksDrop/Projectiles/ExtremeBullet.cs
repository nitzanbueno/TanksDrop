using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace TanksDrop.Projectiles
{
	/// <summary>
	/// A projectile that looks like a huge black oval that destroys all pickups, fences and tanks in its way and despawns when hitting a wall.
	/// </summary>
	class ExtremeBullet : ProjectileObject
	{
		public override float Scale
		{
			get { return 1; }
		}

		public ExtremeBullet()
			: base()
		{
		}

		public ExtremeBullet( Vector2 position, float angle, TimeSpan gameTime, int width, int height, float factor, TankObject owner )
			: base( position, angle, gameTime, width, height, factor, owner )
		{
		}

		public override Texture2D LoadTex( ContentManager Content )
		{
			tex = Content.Load<Texture2D>( "Sprites\\Bullet" );
			Color[] c = new Color[ tex.Width * tex.Height ];
			Color[] nc = new Color[ tex.Width * tex.Height ];
			tex.GetData<Color>( c );
			for(int i = 0; i < c.Length; i++)
			{
				Color cc = c[ i ];
				if ( cc.A > 0 )
				{
					cc.R = 0;
					cc.G = 0;
					cc.B = 0;
				}
				nc[ i ] = cc;
			}
			tex.SetData( nc );
			return tex;
		}

		public override void Draw( TimeSpan gameTime, Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch )
		{
			spriteBatch.Draw( tex, Position, null, Color.Black, 0, new Vector2( 16, 16 ), Scale, SpriteEffects.None, 0.4F );
		}

		public override bool Update( TimeSpan gameTime, TankObject[] Tanks, HashSet<FenceObject> Fences, HashSet<Pickup> Pickups )
		{
			if ( Position.X + bWidth >= width || Position.X <= 0 || Position.Y + bHeight >= height || Position.Y <= 0 )
			{
				return true;
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
						Tank.BulletHit( this );
					}
				}
			}
			boundingBox = new Rectangle( ( int )( Position.X + ( xAdvance ) ), ( int )( Position.Y + ( yAdvance ) ), bWidth, bHeight );
			HashSet<FenceObject> FencesToRemove = new HashSet<FenceObject>();
			foreach ( FenceObject Fence in Fences )
			{
				bool intersects = GameTools.LineIntersectsRect( Fence.Point1, Fence.Point2, boundingBox );
				if ( intersects )
				{
					FencesToRemove.Add( Fence );
				}
			}
			foreach ( FenceObject Fence in FencesToRemove )
			{
				Fences.Remove( Fence );
			}

			HashSet<Pickup> PickupsToRemove = new HashSet<Pickup>();
			foreach ( Pickup pickup in Pickups )
			{
				bool intersects = pickup.BoundingBox.Intersects( boundingBox );
				if ( intersects )
				{
					PickupsToRemove.Add( pickup );
				}
			}
			foreach ( Pickup pickup in PickupsToRemove )
			{
				Pickups.Remove( pickup );
			}

			return false;
		}
	}
}
