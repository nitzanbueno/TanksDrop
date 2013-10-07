using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TanksDrop.Projectiles;
using Microsoft.Xna.Framework;

namespace TanksDrop.SuddenDeaths
{
	/// <summary>
	/// A sudden death that has four sets of 90 bullets each come out of the corners and get sucked into a <see cref="SuperNoveyBlackHole"/> that shoots them at a circle that cannot be avoided by normal tanks.
	/// </summary>
	class SuperNoveyDeath : SuddenDeath
	{
		bool didSuperNova;

		public override void Initialize( TimeSpan gameTime, Microsoft.Xna.Framework.Content.ContentManager Content )
		{
			didSuperNova = false;
			base.Initialize( gameTime, Content );
		}

		public override bool Update( TankObject[] Tanks, HashSet<ProjectileObject> Projectiles, HashSet<FenceObject> Fences, HashSet<Pickup> Pickups, TimeSpan gameTime )
		{/*
			if ( !didSuperNova )
			{
				//All of-
				Random random = new Random();
				HashSet<TankObject> TankSet = new HashSet<TankObject>( Tanks );
				foreach ( TankObject tank in Tanks )
				{
					if ( !tank.IsInGame )
					{
						TankSet.Remove( tank );
					}
				}
				TankObject SuperNover = ( TankSet.ToArray<TankObject>() )[ random.Next( TankSet.Count ) ];
				//-that shit, for a random tank :(

				Vector2 wh = GameTools.GetWH( Projectiles );
				for ( float angle = 0; angle < 360; angle += 0.5F )
				{
					float supernovaSpeed = 1;
					Projectiles.Add( new BasicBullet( wh / 2, angle, gameTime, ( int )wh.X, ( int )wh.Y, supernovaSpeed, new TankObject() ) ); // Either in the middle
					//Projectiles.Add( new BasicBullet( SuperNover.Position, angle, gameTime, ( int )wh.X, ( int )wh.Y, supernovaSpeed, new TankObject() ) ); // or a random tank just blows the fuck up.
				}
				didSuperNova = true;
			}*/
			base.Update( gameTime, Tanks );
			return true;
		}

		public override bool Draw( Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, TankObject[] Tanks, HashSet<ProjectileObject> Projectiles, HashSet<FenceObject> Fences, TimeSpan gameTime )
		{
			return true;
		}
	}
}
