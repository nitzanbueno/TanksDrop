using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TanksDrop.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace TanksDrop.SuddenDeaths
{
	class HomeyDeath : SuddenDeath
	{
		int width;
		int height;
		TankObject MyTank;

		public override void Initialize( GameTime gameTime, ContentManager Content )
		{
			MyTank = new TankObject( );
			base.Initialize( gameTime, Content );
		}

		bool didShoot;

		public override bool Update( TankObject[] Tanks, HashSet<ProjectileObject> Projectiles, HashSet<FenceObject> Fences, HashSet<Pickup> Pickups, GameTime gameTime )
		{
			if ( width == 0 || height == 0 )
			{
				foreach ( ProjectileObject proj in Projectiles )
				{
					if ( proj is AProj )
					{
						width = ( ( AProj )proj ).ScrWidth;
						height = ( ( AProj )proj ).ScrHeight;
					}
				}
			}

			if ( ( gameTime.TotalGameTime - startTime ).TotalMilliseconds % 2000 < 10 && !didShoot )
			{
				Projectiles.Add( new HomingBullet( new Vector2( 10, 10 ), 45, gameTime, width, height, 5, MyTank ) );
				Projectiles.Add( new HomingBullet( new Vector2( width - 10, 10 ), 135, gameTime, width, height, 5, MyTank ) );
				Projectiles.Add( new HomingBullet( new Vector2( 10, height - 10 ), 315, gameTime, width, height, 5, MyTank ) );
				Projectiles.Add( new HomingBullet( new Vector2( width - 10, height - 10 ), 225, gameTime, width, height, 5, MyTank ) );
				didShoot = true;
			}
			//else didShoot = false;

			base.Update( gameTime, Tanks );
			return true;
		}

		public override bool Draw( Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, TankObject[] Tanks, HashSet<Projectiles.ProjectileObject> Projectiles, HashSet<FenceObject> Fences, GameTime gameTime )
		{
			return true;
		}
	}
}
