using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TanksDrop.Projectiles;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TanksDrop.SuddenDeaths
{
	/// <summary>
	/// A sudden death that has everyone shrink to death.
	/// </summary>
	class ShrinkyDeath : SuddenDeath
	{
		public override bool Draw( SpriteBatch spriteBatch, TankObject[] Tanks, HashSet<ProjectileObject> Projectiles, HashSet<FenceObject> Fences, GameTime gameTime )
		{
			return true;
		}

		public override bool Update( TankObject[] Tanks, HashSet<ProjectileObject> Projectiles, HashSet<FenceObject> Fences, HashSet<Pickup> Pickups, GameTime gameTime )
		{
			foreach ( TankObject Tank in Tanks )
			{
				Tank.Scale -= 0.1F;
				if ( Tank.Scale <= 0.1F )
				{
					Tank.Kill();
				}
			}
			return false;
		}
	}
}
