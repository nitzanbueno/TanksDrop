using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using TanksDrop.Projectiles;
using Microsoft.Xna.Framework.Content;

namespace TanksDrop.SuddenDeaths
{
	/// <summary>
	/// A game technique that kills every tank on the board within a maximum of 20 seconds.
	/// Used after a set amount of time to prevent long boring matches.
	/// </summary>
	abstract class SuddenDeath
	{
		protected ContentManager Content;
		protected TimeSpan startTime;
		protected int time;

		public SuddenDeath()
		{
		}

		public virtual void Initialize( TimeSpan gameTime, ContentManager Content )
		{
			startTime = gameTime;
			time = 20000;
			this.Content = Content;
		}

		/// <summary>
		/// Update the game's physics according to sudden death logics.
		/// </summary>
		/// <param name="Tanks">The tanks in the game, alive of dead.</param>
		/// <param name="Projectiles">The projectiles on the board.</param>
		/// <param name="Fences">The fences on the board.</param>
		/// <param name="gameTime">The current game time.</param>
		/// <returns>Whether or not to keep updating.</returns>
		public abstract bool Update( TankObject[] Tanks, HashSet<ProjectileObject> Projectiles, HashSet<FenceObject> Fences, HashSet<Pickup> Pickups, TimeSpan gameTime );

		protected void Update( TimeSpan gameTime, TankObject[] Tanks )
		{
			if ( ( gameTime - startTime ).TotalMilliseconds > time )
			{
				foreach ( TankObject t in Tanks )
				{
					t.Kill();
				}
			}
		}


		/// <summary>
		/// Draws the game according to sudden death logics.
		/// </summary>
		/// <param name="spriteBatch">The SpriteBatch to draw with. Must be begun.</param>
		/// <returns>Whether or not to keep drawing.</returns>
		public abstract bool Draw( SpriteBatch spriteBatch, TankObject[] Tanks, HashSet<ProjectileObject> Projectiles, HashSet<FenceObject> Fences, TimeSpan gameTime );
	}
}
