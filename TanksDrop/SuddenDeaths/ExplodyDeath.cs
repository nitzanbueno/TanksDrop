using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TanksDrop.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TanksDrop.SuddenDeaths
{
	/// <summary>
	/// A sudden death where every living tank explodes.
	/// </summary>
	class ExplodyDeath : SuddenDeath
	{
		public override bool Draw( SpriteBatch spriteBatch, TankObject[] Tanks, HashSet<ProjectileObject> Projectiles, HashSet<FenceObject> Fences, GameTime gameTime )
		{
			return true;
		}

		public override bool Update( TankObject[] Tanks, HashSet<ProjectileObject> Projectiles, HashSet<FenceObject> Fences, HashSet<Pickup> Pickups, GameTime gameTime )
		{
			foreach ( TankObject Tank in Tanks )
			{
				if ( Tank.IsInGame )
				{
					Missile missile = new Missile( Tank.Position, Tank.Rotation, gameTime, 1000, 1000, 1F, Tank, true );
					Projectiles.Add( missile );
					Tank.Kill();
				}
			}
			base.Update( gameTime, Tanks );
			return true;
		}
	}
}
