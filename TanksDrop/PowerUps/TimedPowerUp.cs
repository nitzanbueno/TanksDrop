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
	abstract class TimedPowerUp : PowerUp
	{
		public TimedPowerUp( GameTime gameTime, int duration )
			: base( gameTime, duration )
		{
		}

		/// <summary>
		/// Load my texture here.
		/// </summary>
		/// <param name="Content">Content to load texture from.</param>
		public abstract Texture2D LoadTex( ContentManager Content );

		public override bool Update( GameTime gameTime, TankObject[] Tanks, HashSet<ProjectileObject> Projectiles, HashSet<FenceObject> Fences )
		{
			TimeSpan now = gameTime.TotalGameTime;
			if ( Taken )
			{
				DoPickup( Tanks, Projectiles, Fences );
				if ( ( now - takeTime ).TotalMilliseconds > time )
				{
					return true;
				}
			}

			return false;
		}

		public abstract void DoPickup( TankObject[] Tanks, HashSet<ProjectileObject> Projectiles, HashSet<FenceObject> Fences );

		public abstract void Revert();

		public virtual void Draw( SpriteBatch spriteBatch, GameTime gameTime )
		{
		}

		public virtual bool BulletHit( ProjectileObject Proj, TankObject Requestor )
		{
			Requestor.IsInGame = false;
			return true;
		}

		public virtual bool Hit( TankObject Hitten )
		{
			return true;
		}
	}
}
