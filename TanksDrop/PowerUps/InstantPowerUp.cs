using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using TanksDrop.Projectiles;

namespace TanksDrop.PowerUps
{
	abstract class InstantPowerUp : PowerUp
	{
		public InstantPowerUp( GameTime gameTime )
			: base( gameTime, 1 )
		{
		}

		public override bool Update( GameTime gameTime, TankObject[] Tanks, HashSet<ProjectileObject> Projectiles, HashSet<FenceObject> Fences )
		{
			TimeSpan now = gameTime.TotalGameTime;
			if ( Taken )
			{
				return true;
			}

			return false;
		}

		/// <summary>
		/// Load my texture here.
		/// </summary>
		/// <param name="Content">Content to load texture from.</param>
		public abstract Texture2D LoadTex( ContentManager Content );

		public abstract void Use( TankObject[] Tanks, HashSet<ProjectileObject> Projectiles, HashSet<FenceObject> Fences, HashSet<Pickup> Pickups, GameTime gameTime );
	}
}
