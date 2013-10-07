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
	/// <summary>
	/// A type of power-up that cannot be taken - instead, it just causes an effect while it is on the board.
	/// </summary>
	abstract class AppearingPowerUp : PowerUp
	{
		public AppearingPowerUp( TimeSpan gameTime, int duration )
			: base( gameTime, duration )
		{
		}

		/// <summary>
		/// Loads the power-up's texture.
		/// </summary>
		/// <param name="Content">Content to load texture from.</param>
		public abstract void LoadTex( ContentManager Content );

		/// <summary>
		/// Updates the game with the power-up's effect.
		/// </summary>
		/// <param name="gameTime">The current game time.</param>
		/// <param name="Tanks">The playing tanks.</param>
		/// <param name="Projectiles">The projectiles on-board.</param>
		/// <param name="Fences">The fences on-board.</param>
		/// <returns>Whether the power-up should disappear or not.</returns>
		public override bool Update( TimeSpan gameTime, TankObject[] Tanks, HashSet<ProjectileObject> Projectiles, HashSet<FenceObject> Fences )
		{
			TimeSpan now = gameTime;
			if ( ( now - creationTime ).TotalMilliseconds > time )
			{
				return true;
			}
			return false;
		}

		/// <summary>
		/// Draws the power-up.
		/// </summary>
		/// <param name="spriteBatch">The spriteBatch to draw on.</param>
		/// <param name="gameTime">The current game time.</param>
		public abstract void Draw( SpriteBatch spriteBatch, TimeSpan gameTime );

		/// <summary>
		/// Updates the game's projectiles only.
		/// </summary>
		/// <param name="gameTime">The current game time.</param>
		/// <param name="Projectiles">The projectiles on-board.</param>
		/// <returns>Whether to continue updating the projectiles or not.</returns>
		public virtual bool UpdateProjectiles( TimeSpan gameTime, HashSet<ProjectileObject> Projectiles )
		{
			return true;
		}

		/// <summary>
		/// Ends the power-up's effects.
		/// </summary>
		public virtual void Stop()
		{
		}
	}

}
