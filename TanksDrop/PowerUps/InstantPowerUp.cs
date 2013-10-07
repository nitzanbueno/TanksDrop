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
	/// <summary>
	/// A power-up that can be taken and used when needed using the 'place fence' key.
	/// </summary>
	abstract class InstantPowerUp : PowerUp
	{
		public InstantPowerUp( GameTime gameTime )
			: base( gameTime, 1 )
		{
		}

		/// <summary>
		/// Updates the game using the power-up's logic.
		/// </summary>
		/// <param name="gameTime">The current game time.</param>
		/// <param name="Tanks">The playing tanks.</param>
		/// <param name="Projectiles">The projectiles on-board.</param>
		/// <param name="Fences">The fences on-board.</param>
		/// <returns>Whether the power-up should disappear or not.</returns>
		public override bool Update( GameTime gameTime, TankObject[] Tanks, HashSet<ProjectileObject> Projectiles, HashSet<FenceObject> Fences )
		{
			if ( Taken )
			{
				return true;
			}

			return false;
		}

		/// <summary>
		/// Loads the power-up's texture.
		/// </summary>
		/// <param name="Content">Content to load texture from.</param>
		/// <returns>The texture of the power-up.</returns>
		public abstract Texture2D LoadTex( ContentManager Content );

		/// <summary>
		/// Called whenever the power-up is used.
		/// </summary>
		/// <param name="gameTime">The current game time.</param>
		/// <param name="Tanks">The playing tanks.</param>
		/// <param name="Projectiles">The projectiles on-board.</param>
		/// <param name="Fences">The fences on-board.</param>
		public abstract void Use( TankObject[] Tanks, HashSet<ProjectileObject> Projectiles, HashSet<FenceObject> Fences, HashSet<Pickup> Pickups, GameTime gameTime );
	}
}
