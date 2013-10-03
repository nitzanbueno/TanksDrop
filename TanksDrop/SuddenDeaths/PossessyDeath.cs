using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TanksDrop.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TanksDrop.SuddenDeaths
{
	class PossessyDeath : SuddenDeath
	{
		bool didShoot;

		public override bool Draw( SpriteBatch spriteBatch, TankObject[] Tanks, HashSet<ProjectileObject> Projectiles, HashSet<FenceObject> Fences, GameTime gameTime )
		{
			return true;
		}

		public override bool Update( TankObject[] Tanks, HashSet<ProjectileObject> Projectiles, HashSet<FenceObject> Fences, HashSet<Pickup> Pickups, GameTime gameTime )
		{
			int width = 0;
			int height = 0;

			foreach ( ProjectileObject Projectile in Projectiles )
			{
				if ( Projectile is AProj )
				{
					width = ( ( AProj )Projectile ).ScrWidth;
					height = ( ( AProj )Projectile ).ScrHeight;
				}
			}

			TimeSpan passedTime = ( gameTime.TotalGameTime - startTime );
			if ( passedTime.TotalMilliseconds % 200 < 10 && !didShoot )
			{
				foreach ( TankObject Tank in Tanks )
				{
					if ( Tank.IsInGame )
					{
						Tank.Shoot( gameTime, width, height, Projectiles );
					}
				}
				didShoot = true;
			}
			else didShoot = false;
			base.Update( gameTime, Tanks );
			return true;
		}
	}
}
