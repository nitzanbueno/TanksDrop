
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
	/// Boosts the taker's speed by a specified factor (default 4)
	/// </summary>
	class SpeedBoost : TimedPowerUp
	{
		private float BoostFactor;

		public SpeedBoost( TimeSpan gameTime )
			: base( gameTime, 5000 )
		{
		}

		public SpeedBoost( TimeSpan gameTime, float boostFactor )
			: base( gameTime, 5000 )
		{
			BoostFactor = boostFactor;
		}

		/// <summary>
		/// Changes the speed boost factor.
		/// </summary>
		/// <param name="newBoostFactor">The new speed boost factor.</param>
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
