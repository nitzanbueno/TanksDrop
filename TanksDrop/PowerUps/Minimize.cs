using System;
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
	/// A timed power-up that shrinks its owner 3x for 10 seconds, making bullets easy to dodge.
	/// </summary>
	class Minimize : TimedPowerUp
	{
		public Minimize( TimeSpan gameTime )
			: base( gameTime, 10000 )
		{
		}

		public Minimize( TimeSpan gameTime, int duration ) : base( gameTime, duration ) { }

		public override void DoPickup( TankObject[] Tanks, HashSet<ProjectileObject> Projectiles, HashSet<FenceObject> Fences )
		{
			Owner.Scale = Owner.OS / 3;
		}

		public override Texture2D LoadTex( ContentManager Content )
		{
			return Content.Load<Texture2D>( "Sprites\\Minimize" );
		}

		public override void Revert()
		{
			Owner.Scale = Owner.OS;
		}
	}
}
