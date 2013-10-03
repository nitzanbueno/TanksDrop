
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
	class SpeedBoost : TimedPowerUp
	{
		private float BoostFactor;

		public SpeedBoost( GameTime gameTime )
			: base( gameTime, 5000 )
		{
		}

		public SpeedBoost( GameTime gameTime, float boostFactor )
			: base( gameTime, 5000 )
		{
			BoostFactor = boostFactor;
		}

		public void SetBoostFactor( float newBoostFactor )
		{
			BoostFactor = newBoostFactor;
		}

		public override Texture2D LoadTex( ContentManager Content )
		{
			return Content.Load<Texture2D>( "Sprites\\SpeedBoost" );
		}

		public override void DoPickup( TankObject[] Tanks, HashSet<ProjectileObject> Projectiles, HashSet<FenceObject> Fences )
		{
			Owner.Speed = Owner.OSP * BoostFactor;
		}

		public override void Revert()
		{
			Owner.Speed = Owner.OSP;
		}
	}
}
