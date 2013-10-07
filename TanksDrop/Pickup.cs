using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using TanksDrop.PowerUps;
using TanksDrop.Projectiles;

namespace TanksDrop
{
	/// <summary>
	/// A game object that doesn't move and is picked up by the first tank that steps over it, 
	/// giving the tank something, either a projectile or a power-up.
	/// </summary>
	class Pickup
	{
		public Object Carrier;
		private Type CarrierType;
		public Vector2 Position;
		private Texture2D tex;
		private float scale;
		private TimeSpan originalTime;
		private int TotalTime;
		private Random random;
		private bool Taken;

		public Rectangle BoundingBox
		{
			get
			{
				try
				{
					return new Rectangle( ( int )Position.X, ( int )Position.Y, ( int )( tex.Width * scale ), ( int )( tex.Height * scale ) );
				}
				catch ( Exception ) { return new Rectangle( ( int )Position.X, ( int )Position.Y, ( int )( 32 * scale ), ( int )( 32 * scale ) ); }
			}
		}


		public Pickup( int width, int height, TimeSpan gameTime, int ExistanceTime, Object carrier, ContentManager Content )
		{
			TotalTime = ExistanceTime;
			random = new Random();
			Position = new Vector2( random.Next( width ), random.Next( height ) );
			Carrier = carrier;
			GetCarrierData( Content );
			originalTime = gameTime;
		}

		public Pickup( int width, int height, TimeSpan gameTime, int ExistanceTime, Type carrier, ContentManager Content )
		{
			TotalTime = ExistanceTime;
			random = new Random();
			Position = new Vector2( random.Next( width ), random.Next( height ) );
			GetCarrierData( carrier, gameTime, Content );
			originalTime = gameTime;
		}

		private void GetCarrierData( Type carrier, TimeSpan gameTime, ContentManager Content )
		{
			CarrierType = carrier;
			if ( carrier.IsSubclassOf( typeof( ProjectileObject ) ) )
			{
				Carrier = ( ProjectileObject )Activator.CreateInstance( carrier );
				GetCarrierData( Content );
			}
			else if ( carrier.IsSubclassOf( typeof( PowerUp ) ) )
			{
				Carrier = ( PowerUp )Activator.CreateInstance( carrier, gameTime );
				GetCarrierData( Content );

			}
			else throw new ArgumentException( "Object given couldn't be picked up." );
		}

		private void GetCarrierData( ContentManager Content )
		{
			if ( Carrier is PowerUp )
			{
				PowerUp carrierPowerUp = ( PowerUp )Carrier;
				if ( carrierPowerUp is AppearingPowerUp )
				{
					throw new ArgumentException( "Appearing Power-Ups can't be picked up!" );
				}
				else if ( carrierPowerUp is InstantPowerUp )
				{
					tex = ( ( InstantPowerUp )carrierPowerUp ).LoadTex( Content );
					scale = ( ( InstantPowerUp )carrierPowerUp ).Scale;
				}
				else if ( carrierPowerUp is TimedPowerUp )
				{
					tex = ( ( TimedPowerUp )carrierPowerUp ).LoadTex( Content );
					scale = ( ( TimedPowerUp )carrierPowerUp ).Scale;
				}	
			}
			else if ( Carrier is ProjectileObject )
			{
				ProjectileObject carrierProjectile = ( ProjectileObject )Carrier;
				tex = carrierProjectile.LoadTex( Content );
				scale = carrierProjectile.Scale;
			}
			else throw new ArgumentException( "Object given couldn't be picked up." );
		}

		public bool Update( TimeSpan gameTime )
		{
			return ( ( gameTime - originalTime ).TotalMilliseconds > TotalTime && TotalTime > 0 ) || Taken;
		}

		public void Take( TankObject taker, TimeSpan gameTime )
		{
			if ( !Taken )
			{
				if ( Carrier != null )
				{
					if ( Carrier is ProjectileObject )
					{
						ProjectileObject proj = ( ProjectileObject )Carrier;
						Taken = taker.AppendProjectile( proj.GetType() );
					}
					else if ( Carrier is PowerUp )
					{
						PowerUp powerUp = ( PowerUp )Carrier;
						Taken = taker.AppendPowerUp( powerUp, gameTime );
					}
					else throw new ArgumentException( "Object given was not a pickupable object." );
				}
				else
				{
					if ( CarrierType.IsSubclassOf( typeof( ProjectileObject ) ) )
					{
						Taken = taker.AppendProjectile( CarrierType );
					}
				}
			}
		}

		public void Draw( SpriteBatch spriteBatch )
		{
			spriteBatch.Draw( tex, Position, null, Color.White, 0F, Vector2.Zero, scale, SpriteEffects.None, 0 );
		}
	}
}
