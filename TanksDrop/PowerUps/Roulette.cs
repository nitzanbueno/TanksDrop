using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using TanksDrop.Projectiles;
using Microsoft.Xna.Framework;

namespace TanksDrop.PowerUps
{
	/// <summary>
	/// An instant power-up that when used picks a random tank, alive or dead, including the user, and either kills it if it's alive or revives it if it's dead.
	/// </summary>
	class Roulette : InstantPowerUp
	{
		public Roulette( GameTime gameTime )
			: base( gameTime )
		{

		}

		public override Texture2D LoadTex( ContentManager Content )
		{
			return Content.Load<Texture2D>( "Sprites\\Roulette" );
		}

		public override void Use( TankObject[] Tanks, HashSet<ProjectileObject> Projectiles, HashSet<FenceObject> Fences, HashSet<Pickup> Pickups, GameTime gameTime )
		{
			TankObject ChosenTank = Tanks[ random.Next( Tanks.Length ) ];
			ChosenTank.IsInGame = !ChosenTank.IsInGame;
		}
	}
}
