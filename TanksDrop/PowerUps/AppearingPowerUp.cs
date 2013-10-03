using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TanksDrop.Projectiles;
using Microsoft.Xna.Framework.Content;

namespace TanksDrop.PowerUps
{
	abstract class AppearingPowerUp : PowerUp
	{
		public AppearingPowerUp( GameTime gameTime, int duration )
			: base( gameTime, duration )
		{
		}

		/// <summary>
		/// Load my texture here.
		/// </summary>
		/// <param name="Content">Content to load texture from.</param>
		public abstract void LoadTex( ContentManager Content );

		public override bool Update( GameTime gameTime, TankObject[] Tanks, HashSet<ProjectileObject> Projectiles, HashSet<FenceObject> Fences )
		{
			TimeSpan now = gameTime.TotalGameTime;
			if ( ( now - creationTime ).TotalMilliseconds > time )
			{
				return true;
			}
			return false;
		}

		public abstract void Draw( SpriteBatch spriteBatch, GameTime gameTime );

		public virtual bool UpdateProjectiles( GameTime gameTime, HashSet<ProjectileObject> Projectiles )
		{
			return true;
		}

		public virtual void Stop()
		{
		}
	}

}
