using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using TanksDrop.Projectiles;

namespace TanksDrop.PowerUps
{
	/// <summary>
	/// An instant power-up that when used turns all projectiles 180 degrees.
	/// </summary>
	class Deflector : InstantPowerUp
	{
		public Deflector( TimeSpan gameTime )
			: base( gameTime )
		{
			time = 1;
		}

		public override void Use( TankObject[] Tanks, HashSet<ProjectileObject> Projectiles, HashSet<FenceObject> Fences, HashSet<Pickup> Pickups, TimeSpan gameTime )
		{
			foreach ( ProjectileObject Proj in Projectiles )
			{
				Proj.Turn( 180 );
			}
		}

		public override Texture2D LoadTex( ContentManager Content )
		{
			return Content.Load<Texture2D>( "Sprites\\Deflector" );
		}
	}
}
