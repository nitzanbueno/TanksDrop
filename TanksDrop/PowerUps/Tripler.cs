using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using TanksDrop.Projectiles;
using Microsoft.Xna.Framework;

namespace TanksDrop.PowerUps
{
	/// <summary>
	/// A timed power-up that makes its taker shoot three of its next projectile instead of one.
	/// The other two projectiles do not count towards the bullet limit.
	/// </summary>
	class Tripler : TimedPowerUp
	{
		int width;
		int height;

		public Tripler( TimeSpan gameTime )
			: base( gameTime, 10000 )
		{
		}

		public override Texture2D LoadTex( ContentManager Content )
		{
			return Content.Load<Texture2D>( "Sprites\\Tripler" );
		}

		public override void DoPickup( TankObject[] Tanks, HashSet<ProjectileObject> Projectiles, HashSet<FenceObject> Fences )
		{
			//
		}

		public override void Revert()
		{
			//
		}

		public override bool Shoot( Type PendingProjectile, TimeSpan gameTime, HashSet<ProjectileObject> Projectiles )
		{
			if ( width == 0 && height == 0 )
			{
				Vector2 v = GameTools.GetWH( Projectiles );
				width = ( int )v.X;
				height = ( int )v.Y;
			}
			Type proj = PendingProjectile;
			if ( PendingProjectile == null )
			{
				proj = typeof( BasicBullet );
			}
			float angle = MathHelper.ToDegrees( Owner.Rotation );
			ProjectileObject p1 = ( ProjectileObject )Activator.CreateInstance( proj, GameTools.ReturnMoveInRadians( Owner.Width / 2 + 20, Owner.Rotation, Owner.Position ), angle + 45, gameTime, width, height, 2F * Owner.Speed, Owner );
			ProjectileObject p2 = ( ProjectileObject )Activator.CreateInstance( proj, GameTools.ReturnMoveInRadians( Owner.Width / 2 + 20, Owner.Rotation, Owner.Position ), angle - 45, gameTime, width, height, 2F * Owner.Speed, Owner );
			p1.Position = GameTools.ReturnMove( Owner.Width / 2 + 20 * p1.Scale, angle, Owner.Position );
			p2.Position = GameTools.ReturnMove( Owner.Width / 2 + 20 * p2.Scale, angle, Owner.Position );
			p1.doesCount = false;
			p2.doesCount = false;
			Projectiles.Add( p1 );
			Projectiles.Add( p2 );
			return true;
		}
	}
}
