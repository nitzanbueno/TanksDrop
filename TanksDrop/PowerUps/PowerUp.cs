using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using TanksDrop.Projectiles;

namespace TanksDrop.PowerUps
{
	/// <summary>
	/// An in-game object that causes an effect when it is triggered.
	/// </summary>
	abstract class PowerUp
	{
		public TankObject Owner;
		protected bool Taken;
		protected Random random;
		protected int time;
		protected TimeSpan creationTime;
		protected TimeSpan takeTime;

		public PowerUp( TimeSpan gameTime, int duration )
		{
			random = new Random();
			Taken = false;
			Owner = null;
			creationTime = gameTime;
			time = duration;
		}

		public PowerUp( TankObject owner, TimeSpan gameTime )
		{
			random = new Random();
			Taken = true;
			Owner = owner;
			creationTime = gameTime;
		}

		/// <summary>
		/// Call this whenever a tank runs over me.
		/// </summary>
		/// <param name="Taker">The tank that ran over me.</param>
		/// <param name="gameTime">The current game time, used to determine when to make me disappear.</param>
		public virtual void Take( TankObject Taker, TimeSpan gameTime )
		{
			Owner = Taker;
			Taken = true;
			takeTime = gameTime;
		}

		/// <summary>
		/// Updates the game with the power-up's logic.
		/// </summary>
		/// <param name="gameTime">The current game time. Used to remove the power-up.</param>
		/// <param name="Tanks">All players in the game, both dead and alive.</param>
		/// <param name="Projectiles">All moving projectiles on the board.</param> 
		/// <param name="Fences">All fences on the board.</param>
		/// <returns>True if the power-up should be removed, otherwise false.</returns>
		public abstract bool Update( TimeSpan gameTime, TankObject[] Tanks, HashSet<ProjectileObject> Projectiles, HashSet<FenceObject> Fences );

		/// <summary>
		/// Changes the timed or appearing power-up's duration.
		/// </summary>
		/// <param name="newDuration">The new power-up's duration in milliseconds.</param>
		public void ChangeDuration( int newDuration )
		{
			time = newDuration;
		}

		/// <summary>
		/// Called whenever a tank collides with a fence.
		/// </summary>
		/// <param name="fenceObject">The fence the tank collided with.</param>
		/// <returns>True if the tank should go through the given fence - otherwise, false.</returns>
		public virtual bool DoesGoThruFence( FenceObject fenceObject ) { return false; }

		/// <summary>
		/// The scale to draw the power-up's pickup with.
		/// </summary>
		public virtual float Scale
		{
			get
			{
				return 2;
			}
		}

		/// <summary>
		/// Called when a tank moves.
		/// </summary>
		/// <param name="moveFactor">The velocity of the tank.</param>
		/// <returns>The new velocity of the tank - if it's 0, it doesn't move, for instance.</returns>
		public virtual float OnMove( float moveFactor )
		{
			return moveFactor;
		}

		/// <summary>
		/// Called when a tank rotates.
		/// </summary>
		/// <param name="moveFactor">The rotation angle of the tank.</param>
		/// <returns>The new angle of the tank - if it's 0, it doesn't rotate, for instance.</returns>
		public virtual float OnRotate( float angleFactor )
		{
			return angleFactor;
		}

		/// <summary>
		/// Called whenever the power-up's owner wants to shoot a projectile.
		/// </summary>
		/// <param name="PendingProjectile">The type of the wanted projectile.</param>
		/// <param name="gameTime">The current game time.</param>
		/// <param name="Projectiles">The projectiles on-board.</param>
		/// <returns>Whether the tank can shoot the bullet or not.</returns>
		public virtual bool Shoot( Type PendingProjectile, TimeSpan gameTime, HashSet<ProjectileObject> Projectiles )
		{
			return true;
		}
	}
}
