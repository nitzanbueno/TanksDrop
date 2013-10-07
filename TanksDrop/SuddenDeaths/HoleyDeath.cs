using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TanksDrop.Projectiles;

namespace TanksDrop.SuddenDeaths
{
	/// <summary>
	/// A sudden death that has a black hole appear in the center, sucking and killing all tanks as well as all pickups, projectiles and fences but not appearing power-ups.
	/// </summary>
	class HoleyDeath : SuddenDeath
	{
		Texture2D BlackHoleTex;
		Vector2 Position;
		float speed;
		float scale;

		public override void Initialize( GameTime gameTime, ContentManager Content )
		{
			BlackHoleTex = Content.Load<Texture2D>( "Sprites\\BlackHole" );
			scale = 5;
			speed = 0;
			base.Initialize( gameTime, Content );
		}

		public override bool Update( TankObject[] Tanks, HashSet<ProjectileObject> Projectiles, HashSet<FenceObject> Fences, HashSet<Pickup> Pickups, GameTime gameTime )
		{
			speed += 0.1F;
			if ( Position == Vector2.Zero )
			{
				foreach ( ProjectileObject Proj in Projectiles )
				{
					if ( Proj is AProj )
					{
						Position = new Vector2( ( ( AProj )Proj ).ScrWidth / 2, ( ( AProj )Proj ).ScrHeight / 2 );
					}
				}
			}
			foreach ( TankObject Tank in Tanks )
			{
				if ( Tank.IsInGame )
				{
					if ( Vector2.Distance( Tank.Position, Position ) > 16 * scale )
					{
						float ang = ( ( ( float )Math.Atan2( Position.Y - Tank.Position.Y, Position.X - Tank.Position.X ) * 180 / ( float )Math.PI ) ) % 360;
						Tank.Position = GameTools.ReturnMove( speed, ang, Tank.Position );
					}
					else
					{
						Tank.Kill();
					}
				}
			}
			HashSet<ProjectileObject> ProjectilesToRemove = new HashSet<ProjectileObject>();
			foreach ( ProjectileObject Proj in Projectiles )
			{
				if ( Vector2.Distance( Proj.Position, Position ) > 16 * scale )
				{
					float ang = ( ( ( float )Math.Atan2( Position.Y - Proj.Position.Y, Position.X - Proj.Position.X ) * 180 / ( float )Math.PI ) ) % 360;
					Proj.Position = GameTools.ReturnMove( speed, ang , Proj.Position );
				}
				else
				{
					ProjectilesToRemove.Add( Proj );
				}
			}
			foreach ( ProjectileObject Proj in ProjectilesToRemove )
			{
				Projectiles.Remove( Proj );
			}
			HashSet<FenceObject> FencesToRemove = new HashSet<FenceObject>();
			foreach ( FenceObject Fence in Fences )
			{
				if ( Vector2.Distance( Fence.Point1, Position ) > 16 * scale && Vector2.Distance( Fence.Point2, Position ) > 16 * scale )
				{
					float ang = ( ( ( float )Math.Atan2( Position.Y - Fence.Point1.Y, Position.X - Fence.Point1.X ) * 180 / ( float )Math.PI ) ) % 360;
					Fence.Point1 = GameTools.ReturnMove( speed, ang, Fence.Point1 );
					ang = ( ( ( float )Math.Atan2( Position.Y - Fence.Point2.Y, Position.X - Fence.Point2.X ) * 180 / ( float )Math.PI ) ) % 360;
					Fence.Point2 = GameTools.ReturnMove( speed, ang, Fence.Point2 );
				}
				else
				{
					FencesToRemove.Add( Fence );
				}
			}
			foreach ( FenceObject Fence in FencesToRemove )
			{
				Fences.Remove( Fence );
			}
			HashSet<Pickup> PickupsToRemove = new HashSet<Pickup>();
			foreach ( Pickup pickup in Pickups )
			{
				if ( Vector2.Distance( pickup.Position, Position ) > 16 * scale )
				{
					float ang = ( ( ( float )Math.Atan2( Position.Y - pickup.Position.Y, Position.X - pickup.Position.X ) * 180 / ( float )Math.PI ) + 180 ) % 360;
					pickup.Position = GameTools.ReturnMove( speed, ang + 180, pickup.Position );
				}
				else
				{
					PickupsToRemove.Add( pickup );
				}
			}
			foreach ( Pickup pickup in PickupsToRemove )
			{
				Pickups.Remove( pickup );
			}
			return false;
		}

		public override bool Draw( SpriteBatch spriteBatch, TankObject[] Tanks, HashSet<ProjectileObject> Projectiles, HashSet<FenceObject> Fences, GameTime gameTime )
		{
			if ( BlackHoleTex != null && Position != Vector2.Zero )
			{
				spriteBatch.Draw( BlackHoleTex, Position, null, Color.White, 0, new Vector2( 16, 16 ), scale, SpriteEffects.None, 0 );
			}
			return true;
		}
	}
}
