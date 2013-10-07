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
	/// A timed power-up that causes its taker to be able to pass through fences and bullets.
	/// </summary>
	class Ghost : TimedPowerUp
	{
		public Ghost( GameTime gameTime )
			: base( gameTime, 3000 )
		{
		}

		public override Texture2D LoadTex( ContentManager Content )
		{
			return Content.Load<Texture2D>( "Sprites\\Ghost" );
		}

		public override bool Hit( TankObject Hitten )
		{
			return Hitten != Owner;
		}

		public override bool BulletHit( ProjectileObject Proj, TankObject Requestor )
		{
			return Requestor != Owner;
		}

		public override bool DoesGoThruFence( FenceObject fenceObject )
		{
			return true;
		}

		public override void DoPickup( TankObject[] Tanks, HashSet<ProjectileObject> Projectiles, HashSet<FenceObject> Fences )
		{
		}

		public override void Revert()
		{
		}
	}
}
