using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using TanksDrop.Projectiles;
using Microsoft.Xna.Framework.Content;

namespace TanksDrop.PowerUps
{
	/// <summary>
	/// An instant power-up that, when used, removes all tanks' power-ups, both timed and instant, all on-board projectiles and all on-board pickups.
	/// </summary>
	class Disabler : InstantPowerUp
	{
		public Disabler( GameTime gameTime )
			: base( gameTime )
		{
		}

		public override Texture2D LoadTex( ContentManager Content )
		{
			return Content.Load<Texture2D>( "Sprites\\Disabler" );
		}

		public override void Use( TankObject[] Tanks, HashSet<ProjectileObject> Projectiles, HashSet<FenceObject> Fences, HashSet<Pickup> Pickups, GameTime gameTime )
		{
			foreach ( TankObject t in Tanks )
			{
				t.RemovePowerUps();
			}
			HashSet<Pickup> pickups = new HashSet<Pickup>( Pickups );
			foreach ( Pickup pickup in pickups ) Pickups.Remove( pickup );
			HashSet<ProjectileObject> projectiles = new HashSet<ProjectileObject>( Projectiles );
			foreach ( ProjectileObject p in projectiles ) if ( !( p is AProj ) ) Projectiles.Remove( p ); 
		}
	}
}
