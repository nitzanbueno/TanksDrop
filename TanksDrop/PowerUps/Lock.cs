﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using TanksDrop.Projectiles;

namespace TanksDrop.PowerUps
{
	/// <summary>
	/// A timed power-up that gets put onto a tank that is not its taker, destroying its current power-up if there is, and prevents it from moving for 3 seconds.
	/// </summary>
	class Lock : TimedPowerUp
	{
		public override float Scale
		{
			get
			{
				return 1;
			}
		}

		TankObject OrigOwner;

		public Lock( TimeSpan gameTime )
			: base( gameTime, 3000 )
		{
		}

		public override void Take( TankObject Taker, TimeSpan gameTime )
		{
			OrigOwner = Taker;
			Taken = true;
			takeTime = gameTime;
		}

		public override bool Update( TimeSpan gameTime, TankObject[] Tanks, HashSet<ProjectileObject> Projectiles, HashSet<FenceObject> Fences )
		{
			if ( Taken && Owner == null )
			{
				HashSet<TankObject> tanks = new HashSet<TankObject>( Tanks );
				tanks.Remove( OrigOwner );
				TankObject newOwner = ( tanks.ToArray<TankObject>() )[ random.Next( tanks.Count ) ];
				newOwner.powerUp = this;
				Owner = newOwner;
				OrigOwner.powerUp = null;
				OrigOwner = null;
				takeTime = gameTime;
			}
			return base.Update( gameTime, Tanks, Projectiles, Fences );
		}

		public override Texture2D LoadTex( ContentManager Content )
		{
			return Content.Load<Texture2D>( "Sprites\\Lock" );
		}

		public override void DoPickup( TankObject[] Tanks, HashSet<ProjectileObject> Projectiles, HashSet<FenceObject> Fences )
		{
			// Do Nothing
		}

		public override bool Shoot( Type PendingProjectile, TimeSpan gameTime, HashSet<ProjectileObject> Projectiles )
		{
			return false;
		}

		public override float OnMove( float moveFactor )
		{
			return 0;
		}

		public override float OnRotate( float angleFactor )
		{
			return 0;
		}

		public override void Revert()
		{
		}
	}
}
