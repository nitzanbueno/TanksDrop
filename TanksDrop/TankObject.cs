using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using TanksDrop.PowerUps;
using TanksDrop.Projectiles;

namespace TanksDrop
{
	class TankObject
	{
		public Vector2 Position;
		private float angle;
		public float Rotation
		{
			get
			{
				return MathHelper.ToRadians( angle );
			}

			set
			{
				angle = MathHelper.ToDegrees( value );
			}
		}
		private float Left
		{
			get
			{
				return MathHelper.ToRadians( angle + 270 );
			}
		}
		private float Right
		{
			get
			{
				return MathHelper.ToRadians( angle + 90 );
			}
		}
		public Colors TankColor;
		public Color RGBColor
		{
			get
			{
				switch ( TankColor )
				{
					case Colors.Aqua:
						return Color.Aqua;
					case Colors.Blue:
						return Color.Blue;
					case Colors.Green:
						return Color.Green;
					case Colors.Orange:
						return Color.Orange;
					case Colors.Pink:
						return Color.Pink;
					case Colors.Purple:
						return Color.Purple;
					case Colors.Red:
						return Color.Red;
					case Colors.Yellow:
						return Color.Yellow;
					default:
						return Color.White;
				}
			}
		}
		public KeySet Keys;
		public Rectangle rect
		{
			get
			{
				return new Rectangle( ( int )Position.X, ( int )Position.Y, 64, 64 );
			}
		}
		public Vector2 Origin;
		public Vector2 OP;
		public float OR;
		private bool _game;
		public bool IsInGame
		{
			get
			{
				return _game;
			}

			set
			{
				if ( value == false && _game == true )
				{
					if ( powerUp == null || powerUp.Hit( this ) )
					{
						_game = false;
					}
				}
				else
				{
					_game = value;
				}
			}
		}
		public bool IsOfficiallyOut;
		public int Score;
		public bool AI;
		public float OS;
		public float OSP;
		private float _scale;
		public float Scale
		{
			get
			{
				return _scale;
			}
			set
			{
				_scale = value;
				Origin = new Vector2( 16, 16 ) * Scale;
			}
		}
		public float Width
		{
			get
			{
				return 32 * Scale;
			}
		}
		public float Height
		{
			get
			{
				return 32 * Scale;
			}
		}
		public int PlacedFences;
		private int ShotLimit;
		public int ShotTime;
		private int FenceLimit;
		public int FenceTime;
		public int ShotsOnBoard;
		private Type PendingProjectile;
		private FenceObject PendingFence;
		public TimedPowerUp powerUp;
		private InstantPowerUp PendingPowerUp;
		private Random random;
		public float Speed;
		private TimeSpan LastShoot;
		private float BulletSpeed;

		public TankObject( Vector2 position, float rotation, Colors color, KeySet keys, float scale, int shotlimit, int shottime, int fencelimit, int fencetime, float speed, float bulletspeed )
		{
			Position = position;
			OP = position;
			angle = rotation;
			OR = MathHelper.ToRadians( rotation );
			TankColor = color;
			Keys = keys;
			IsInGame = true;
			IsOfficiallyOut = false;
			Score = 0;
			OS = scale;
			Scale = scale;
			ShotLimit = shotlimit;
			ShotTime = shottime;
			FenceLimit = fencelimit;
			FenceTime = fencetime;
			random = new Random();
			OSP = speed;
			Speed = OSP;
			BulletSpeed = bulletspeed;
		}

		public TankObject()
		{
		}

		public void Update( GameTime gameTime, KeyboardState key, KeyboardState oldkey, int width, int height, TankObject[] Tanks, HashSet<ProjectileObject> Projectiles, HashSet<FenceObject> Fences, HashSet<Pickup> Pickups, GraphicsDevice gd )
		{
			if ( key.IsKeyDown( Keys.KeyPlace ) && oldkey.IsKeyUp( Keys.KeyPlace ) && ( PlacedFences < FenceLimit || FenceLimit < 0 ) )
			{
				if ( PendingPowerUp == null && IsInGame )
				{
					PlacedFences++;
					float dist = Vector2.Distance( Vector2.Zero, Origin );
					Fences.Add( new BasicFence( GameTools.ReturnMove( dist / 4, angle, GameTools.ReturnMove( dist, angle + 45, Position ) ), GameTools.ReturnMove( dist / 4, angle, GameTools.ReturnMove( dist, angle - 45, Position ) ), 8, this, gameTime, gd ) );
				}
				else if ( PendingPowerUp != null )
				{
					PendingPowerUp.Use( Tanks, Projectiles, Fences, Pickups, gameTime );
					PendingPowerUp = null;
				}
			}
			if ( !IsInGame ) return;

			if ( AI )
			{
				TankObject ClosestTank = null;
				float dist = float.PositiveInfinity;
				foreach ( TankObject Tank in Tanks )
				{
					float d = Vector2.Distance( Tank.Position, Position );
					if ( d < dist && Tank.IsInGame && Tank != this )
					{
						dist = d;
						ClosestTank = Tank;
					}
				}
				if ( ClosestTank != null )
				{
					float ang = ( ( ( float )Math.Atan2( Position.Y - ClosestTank.Position.Y, Position.X - ClosestTank.Position.X ) * 180 / ( float )Math.PI ) + 180 ) % 360;
					/*while ( angle < 0 )
					{
						angle += 360;
					}
					angle %= 480;

					/*
					 * If (absolute of [heading - converted angle] <= 20) then turn to converted angle;
						Otherwise If (heading - converted angle < -20) then turn -20 degrees;
						Otherwise If (heading - converted angel > 20) then turn 20 degrees;
					 *
					float factor = 10;
					if ( Math.Abs( angle - ang ) <= factor ) angle = ang;
					else if ( angle - ang < -factor ) angle -= factor;
					else if ( angle - ang > factor ) angle += factor;
					*/
					angle = ang;

					if ( ( gameTime.TotalGameTime - LastShoot ).TotalMilliseconds > 2000 )
					{
						Shoot( gameTime, width, height, Projectiles );
						LastShoot = gameTime.TotalGameTime;
					}
					FilterMove( 5F, Fences, Pickups, gameTime );
					return;
				}
			}

			float moveFactor = 0;

			if ( key.IsKeyDown( Keys.KeyUp ) )
			{
				moveFactor += Speed;
			}
			if ( key.IsKeyDown( Keys.KeyDown ) )
			{
				moveFactor -= Speed;
			}
			if ( moveFactor != 0 )
			{
				FilterMove( moveFactor, Fences, Pickups, gameTime );
			}

			float angleFactor = 0;

			if ( key.IsKeyDown( Keys.KeyLeft ) )
			{
				angleFactor -= Speed;
			}
			if ( key.IsKeyDown( Keys.KeyRight ) )
			{
				angleFactor += Speed;
			}

			angle %= 360;
			while ( angle < 0 )
			{
				angle += 360;
			}

			FilterRotate( Fences, angleFactor );

			if ( Position.X > width + Origin.X )
				Position.X = -Origin.X;
			if ( Position.X < -Origin.X )
				Position.X = width + Origin.X;
			if ( Position.Y > height + Origin.Y )
				Position.Y = -Origin.Y;
			if ( Position.Y < -Origin.Y )
				Position.Y = height + Origin.Y;

			foreach ( Pickup pickup in Pickups )
			{
				if ( GameTools.RotRectIntersectRect( Position, Origin, Width, Height, Rotation, pickup.BoundingBox ) )
				{
					pickup.Take( this, gameTime );
				}
			}

			if ( key.IsKeyDown( Keys.KeyShoot ) && oldkey.IsKeyUp( Keys.KeyShoot ) && ( ShotsOnBoard < ShotLimit || ShotLimit <= 0 ) && IsInGame )
			{
				Shoot( gameTime, width, height, Projectiles );
			}

			if ( powerUp != null )
			{
				if ( powerUp.Update( gameTime, Tanks, Projectiles, Fences ) )
				{
					powerUp.Revert();
					powerUp = null;
				}
			}
		}

		private void FilterRotate( HashSet<FenceObject> Fences, float factor )
		{
			float angleFactor = factor;
			Vector2 ul = Position - Origin;
			Vector2 ur = ul + new Vector2( Width, 0 );
			Vector2 lr = ul + new Vector2( 0, Height );
			Vector2 ll = ul + new Vector2( Width, Height );
			ul = GameTools.Transform( ul, Position, Rotation );
			ur = GameTools.Transform( ur, Position, Rotation );
			ll = GameTools.Transform( ll, Position, Rotation );
			lr = GameTools.Transform( lr, Position, Rotation );
			foreach ( FenceObject Fence in Fences )
			{
				if ( ( GameTools.LineIntersectsLine( lr, ur, Fence.Point1, Fence.Point2 ) ||
					GameTools.LineIntersectsLine( lr, ll, Fence.Point1, Fence.Point2 ) ||
					GameTools.LineIntersectsLine( ll, ul, Fence.Point1, Fence.Point2 ) ||
					GameTools.LineIntersectsLine( ur, ul, Fence.Point1, Fence.Point2 ) ) )
				{
					angleFactor = Fence.OnTankCollision( this, angleFactor );
					break;
				}
			}

			// then if there wasn't,
			if ( angleFactor == 0 ) return;

			// and there is a power-up
			if ( powerUp != null )
			{
				// check if it allows you rotation.
				angleFactor = powerUp.OnRotate( angleFactor );
			}

			// If it does,
			if ( angleFactor == 0 ) return;

			// Fucking Move.
			angle += angleFactor;
		}

		public void Shoot( GameTime gameTime, int width, int height, HashSet<ProjectileObject> Projectiles )
		{
			if ( powerUp == null || powerUp.Shoot( PendingProjectile, gameTime, Projectiles ) )
			{
				ShotsOnBoard++;
				if ( PendingProjectile == null )
				{
					PendingProjectile = typeof( BasicBullet );
				}
				ProjectileObject p = ( ProjectileObject )Activator.CreateInstance( PendingProjectile, GameTools.ReturnMove( Width / 2 + 20, angle, Position ), angle, gameTime, width, height, BulletSpeed, this );
				p.Position = GameTools.ReturnMove( Width / 2 + 20 * p.Scale, angle, Position );
				Projectiles.Add( p );
				PendingProjectile = typeof( BasicBullet );
			}
		}

		private void FilterMove( float factor, HashSet<FenceObject> Fences, HashSet<Pickup> Pickups, GameTime gameTime )
		{
			// First we stop any collision with fences,
			float moveFactor = factor;
			Vector2 position = GameTools.ReturnMove( moveFactor, angle, Position );
			Vector2 ul = position - Origin;
			Vector2 ur = ul + new Vector2( Width, 0 );
			Vector2 lr = ul + new Vector2( 0, Height );
			Vector2 ll = ul + new Vector2( Width, Height );
			ul = GameTools.Transform( ul, position, Rotation );
			ur = GameTools.Transform( ur, position, Rotation );
			ll = GameTools.Transform( ll, position, Rotation );
			lr = GameTools.Transform( lr, position, Rotation );
			foreach ( FenceObject Fence in Fences )
			{
				if ( ( GameTools.LineIntersectsLine( lr, ur, Fence.Point1, Fence.Point2 ) ||
					GameTools.LineIntersectsLine( lr, ll, Fence.Point1, Fence.Point2 ) ||
					GameTools.LineIntersectsLine( ll, ul, Fence.Point1, Fence.Point2 ) ||
					GameTools.LineIntersectsLine( ur, ul, Fence.Point1, Fence.Point2 ) ) )
				{
					moveFactor = Fence.OnTankCollision( this, moveFactor );
					break;
				}
			}

			// then if there wasn't,
			if ( moveFactor == 0 ) return;

			// and there is a power-up
			if ( powerUp != null )
			{
				// check if it allows you movement.
				moveFactor = powerUp.OnMove( moveFactor );
			}

			// If it does,
			if ( moveFactor == 0 ) return;

			// Fucking Move.
			Move( moveFactor );
		}

		private void Move( float factor )
		{
			Position = GameTools.ReturnMove( factor, angle, Position );
		}

		public bool AppendProjectile( Type Projectile )
		{
			if ( PendingProjectile == typeof( BasicBullet ) || PendingProjectile == null )
			{
				PendingProjectile = Projectile;
				return true;
			}
			else
			{
				return false;
			}
		}

		public void Reset()
		{
			Position = OP;
			Rotation = OR;
			Speed = OSP;
			IsInGame = true;
			IsOfficiallyOut = false;
			ShotsOnBoard = 0;
			PlacedFences = 0;
			RemovePowerUps();
			Scale = OS;
		}

		public bool AppendPowerUp( PowerUp powerUp, GameTime gameTime )
		{
			if ( powerUp is AppearingPowerUp )
				return false;
			else if ( powerUp is InstantPowerUp )
			{
				if ( PendingPowerUp == null )
				{
					PendingPowerUp = ( InstantPowerUp )powerUp;
					PendingPowerUp.Take( this, gameTime );
					return true;
				}
				else return false;
			}
			else if ( powerUp is TimedPowerUp )
			{
				if ( this.powerUp == null )
				{
					this.powerUp = ( TimedPowerUp )powerUp;
					this.powerUp.Take( this, gameTime );
					return true;
				}
				else return false;
			}
			else
			{
				throw new Exception( "Liek if this gets thrown, i'm a sheep" );
			}
		}

		public bool BulletHit( ProjectileObject Proj )
		{
			//return false;
			if ( powerUp != null )
			{
				return powerUp.BulletHit( Proj, this );
			}
			else
			{
				IsInGame = false;
				return true;
			}
		}

		public void Kill()
		{
			_game = false;
		}

		public void RemovePowerUps()
		{
			PendingPowerUp = null;
			PendingProjectile = null;
			PendingFence = null;
			if ( powerUp != null )
			{
				powerUp.Revert();
				powerUp = null;
			}
		}
	}
}
