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
	class Deflector : InstantPowerUp
	{
		public Deflector( GameTime gameTime )
			: base( gameTime )
		{
			time = 1;
		}

		public override bool DoesGoThruFence( FenceObject fenceObject )
		{
			return false;
		}

		public override void Use( TankObject[] Tanks, HashSet<ProjectileObject> Projectiles, HashSet<FenceObject> Fences, HashSet<Pickup> Pickups, GameTime gameTime )
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
