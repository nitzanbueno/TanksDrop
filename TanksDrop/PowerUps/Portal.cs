using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using TanksDrop.Projectiles;

namespace TanksDrop.PowerUps
{
	class Portal : AppearingPowerUp
	{
		Vector2 portal1Pos;
		Vector2 portal2Pos;
		Rectangle portal1Bounds;
		Rectangle portal2Bounds;
		Vector2 Origin
		{
			get
			{
				return new Vector2( 16, 16 ) * Scale;
			}
		}
		public override float Scale
		{
			get
			{
				return 3;
			}
		}
		Texture2D texMap;
		int Frame;
		TimeSpan LastFrameUpdate;

		public Portal( GameTime gameTime ) : base( gameTime, 10000 ) { }

		public Portal( GameTime gameTime, int duration ) : base( gameTime, duration ) { }

		public override void LoadTex( ContentManager Content )
		{
			texMap = Content.Load<Texture2D>( "Sprites\\PortalMap" );
		}

		public override void Draw( SpriteBatch spriteBatch, GameTime gameTime )
		{
			spriteBatch.Draw( texMap, portal1Pos, new Rectangle( Frame * 32, 0, 32, 32 ), Color.White, 0, Origin / Scale, Scale, SpriteEffects.None, 0 );
			spriteBatch.Draw( texMap, portal2Pos, new Rectangle( Frame * 32, 32, 32, 32 ), Color.White, 0, Origin / Scale, Scale, SpriteEffects.None, 0 );
		}

		bool didTankEnter;

		public override bool Update( GameTime gameTime, TankObject[] Tanks, HashSet<ProjectileObject> Projectiles, HashSet<FenceObject> Fences )
		{
			if ( portal1Pos == Vector2.Zero && portal2Pos == Vector2.Zero )
			{
				int width = 1000;
				int height = 1000;
				foreach ( ProjectileObject p in Projectiles )
				{
					if ( p is AProj )
					{
						width = ( ( AProj )p ).ScrWidth;
						height = ( ( AProj )p ).ScrWidth;
						break;
					}
				}
				portal1Pos = new Vector2( random.Next( width ), random.Next( height ) );
				portal2Pos = new Vector2( random.Next( width ), random.Next( height ) );
				portal1Bounds = new Rectangle( ( int )( portal1Pos.X - Origin.X ), ( int )( portal1Pos.Y - Origin.Y ), ( int )( 32 * Scale ), ( int )( 32 * Scale ) );
				portal2Bounds = new Rectangle( ( int )( portal2Pos.X - Origin.X ), ( int )( portal2Pos.Y - Origin.Y ), ( int )( 32 * Scale ), ( int )( 32 * Scale ) );
			}
			if ( ( gameTime.TotalGameTime - LastFrameUpdate ).TotalMilliseconds > 500 )
			{
				Frame += 1;
				Frame %= 4;
				LastFrameUpdate = gameTime.TotalGameTime;
			}
			bool didTankEnterNow = false;
			foreach ( TankObject Tank in Tanks )
			{
				if ( GameTools.RotRectIntersectRect( Tank.Position, Tank.Origin, Tank.Width, Tank.Height, Tank.Rotation, portal1Bounds ) )
				{
					if ( !didTankEnter )
					{
						Tank.Position = GameTools.ReturnMove( Tank.Width / 2 + 40, MathHelper.ToDegrees( Tank.Rotation ), portal2Pos );
						didTankEnter = true;
						didTankEnterNow = true;
					}
					else
					{
						Tank.Position = GameTools.ReturnMove( 40, MathHelper.ToDegrees( Tank.Rotation ), Tank.Position );
					}
				}
				else if ( GameTools.RotRectIntersectRect( Tank.Position, Tank.Origin, Tank.Width, Tank.Height, Tank.Rotation, portal2Bounds ) )
				{
					if ( !didTankEnter )
					{
						Tank.Position = GameTools.ReturnMove( Tank.Width / 2 + 40, MathHelper.ToDegrees( Tank.Rotation ), portal1Pos );
						didTankEnter = true;
						didTankEnterNow = true;
					}
					else
					{
						Tank.Position = GameTools.ReturnMove( 40, MathHelper.ToDegrees( Tank.Rotation ), Tank.Position );
					}
				}
				//else didTankEnterNow = false;
			}

			foreach ( ProjectileObject Proj in Projectiles )
			{
				Rectangle boundingBox = new Rectangle( ( int )( Proj.Position.X ), ( int )( Proj.Position.Y ), Proj.bWidth, Proj.bHeight );
				float Rotation = MathHelper.ToRadians( Proj.Angle );
				if ( portal1Bounds.Intersects( boundingBox ) )
				{
					if ( !didTankEnter )
					{
						Proj.Position = GameTools.ReturnMove( Proj.bWidth / 2 + 40, MathHelper.ToDegrees( Rotation ), portal2Pos );
						didTankEnter = true;
						didTankEnterNow = true;
					}
					else
					{
						Proj.Position = GameTools.ReturnMove( 40, MathHelper.ToDegrees( Rotation ), Proj.Position );
					}
				}
				else if ( portal2Bounds.Intersects( boundingBox ) )
				{
					if ( !didTankEnter )
					{
						Proj.Position = GameTools.ReturnMove( Proj.bWidth / 2 + 40, MathHelper.ToDegrees( Rotation ), portal1Pos );
						didTankEnter = true;
						didTankEnterNow = true;
					}
					else
					{
						Proj.Position = GameTools.ReturnMove( 40, MathHelper.ToDegrees( Rotation ), Proj.Position );
					}
				}
				//else didTankEnterNow = false;
			}

			didTankEnter = didTankEnterNow;
			return base.Update( gameTime, Tanks, Projectiles, Fences );
		}
	}
}
