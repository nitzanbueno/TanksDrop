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
	abstract class PowerUp
	{
		public TankObject Owner;
		protected bool Taken;
		protected Random random;
		protected int time;
		protected TimeSpan creationTime;
		protected TimeSpan takeTime;

		public PowerUp( GameTime gameTime, int duration )
		{
			random = new Random();
			Taken = false;
			Owner = null;
			creationTime = gameTime.TotalGameTime;
			time = duration;
		}

		public PowerUp( TankObject owner, GameTime gameTime )
		{
			random = new Random();
			Taken = true;
			Owner = owner;
			creationTime = gameTime.TotalGameTime;
		}

		/// <summary>
		/// Call this whenever a tank runs over me.
		/// </summary>
		/// <param name="Taker">The tank that ran over me.</param>
		/// <param name="gameTime">The current game time, used to determine when to make me disappear.</param>
		public virtual void Take( TankObject Taker, GameTime gameTime )
		{
			Owner = Taker;
			Taken = true;
			takeTime = gameTime.TotalGameTime;
		}

		/// <summary>
		/// Use this for something I do every frame, or if I am an appearing power-up, whatever I do.
		/// </summary>
		/// <param name="gameTime">The current game time. Used to make me disappear.</param>
		/// <param name="Tanks">All players in the game, both dead and alive.</param>
		/// <param name="Projectiles">All moving projectiles on the board.</param> 
		/// <param name="Fences">All fences on the board.</param>
		/// <returns>True if I should be removed, otherwise false.</returns>
		public abstract bool Update( GameTime gameTime, TankObject[] Tanks, HashSet<ProjectileObject> Projectiles, HashSet<FenceObject> Fences );

		/*
		/// <summary>
		/// Called when a tank places an instant power-up.
		/// </summary>
		/// <param name="Tanks">All tanks in the game, both dead and alive.</param>
		/// <param name="Projectiles">All projectiles on the board.</param>
		/// <param name="Fences">All fences on the board.</param>
		public abstract void Use( TankObject[] Tanks, ref HashSet<ProjectileObject> Projectiles, ref HashSet<FenceObject> Fences );

		/// <summary>
		/// If there is something I need to do every frame just when my tank holds me, it belongs here. (Otherwise, it belongs in the 'Update' Function.)
		/// </summary>
		/// <param name="Tanks">All players in the game, both dead and alive.</param>
		/// <param name="Projectiles">All moving projectiles on the board.</param>
		/// <param name="Fences">All fences on the board.</param>
		protected abstract void DoPickup( TankObject[] Tanks, ref HashSet<ProjectileObject> Projectiles, ref HashSet<FenceObject> Fences );
		public abstract void Revert();
		*/

		public void ChangeDuration( int newDuration )
		{
			time = newDuration;
		}

		/// <summary>
		/// Should my tank go through the given fence using me as its power up?
		/// </summary>
		/// <param name="fenceObject">The Given Fence.</param>
		/// <returns>True if the tank should go through the given fence - otherwise, false.</returns>
		public virtual bool DoesGoThruFence( FenceObject fenceObject ) { return false; }

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
		/// <returns>The new velocity of the tank - if it's 0, it's locked, for instance.</returns>
		public virtual float OnMove( float moveFactor )
		{
			return moveFactor;
		}

		public virtual float OnRotate( float angleFactor )
		{
			return angleFactor;
		}

		public virtual bool Shoot( Type PendingProjectile, GameTime gameTime, HashSet<ProjectileObject> Projectiles )
		{
			return true;
		}
	}
}
