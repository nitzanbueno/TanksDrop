using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using TanksDrop.SuddenDeaths;
using TanksDrop.PowerUps;
using TanksDrop.Projectiles;

namespace TanksDrop
{
	class AProj : ProjectileObject
	{
		/// <summary>
		/// The width of the screen.
		/// </summary>
		public int ScrWidth;

		/// <summary>
		/// The height of the screen.
		/// </summary>
		public int ScrHeight;

		public AProj( Texture2D blank, int width, int height )
			: base()
		{
			tex = blank;
			ScrWidth = width;
			ScrHeight = height;
		}

		public override void Draw( GameTime gameTime, SpriteBatch spriteBatch )
		{
		}

		public override float Scale
		{
			get { return 0; }
		}
		public override Texture2D LoadTex( ContentManager Content )
		{
			return tex;
		}

		public override bool Update( GameTime gameTime, TankObject[] Tanks, HashSet<FenceObject> Fences, HashSet<Pickup> Pickups )
		{
			return false;
		}
	}

	/// <summary>
	/// This is the main type for your game
	/// </summary>
	public class TanksDrop : Microsoft.Xna.Framework.Game
	{
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;
		RenderTarget2D renderTarget;
		Texture2D TankMap;
		Texture2D BulletTex;
		Rectangle[][] TankSources;
		TankObject[] Tanks;
		HashSet<FenceObject> Fences;
		HashSet<ProjectileObject> Projectiles;
		HashSet<Pickup> Pickups;
		AppearingPowerUp appearingPowerUp;
		SuddenDeath[] SuddenDeaths;
		SpriteFont ScoreFont;
		Texture2D blank;
		Random random;

		// Arguments
		private int NumOfPlayers;
		private int ShotLimit;
		private int ShotTime;
		private int FenceLimit;
		private int FenceTime;
		private int TimeDelay;
		private int StartDelay;
		private int PickupTime;
		private int PickupDuration;
		private int SuddenDeathTime;
		private float BlastRadius;
		private float TankSpeed;
		private float BulletSpeed;
		private float BoostFactor;

		public int width;
		public int height;

		StreamReader reader;
		List<string> Lines;
		float TankScale;
		Type[] PickupableOptions;

		public TanksDrop()
		{
			random = new Random();
			Read();
			graphics = new GraphicsDeviceManager( this );
			width = LoadSetting( "ScreenWidth", 1000 );
			height = LoadSetting( "ScreenHeight", 1000 );
			graphics.PreferredBackBufferWidth = width;
			graphics.PreferredBackBufferHeight = height;
			Content.RootDirectory = "Content";
		}

		private void Read()
		{
			Lines = new List<string>();
			try
			{
				reader = new StreamReader( File.OpenRead( "settings.ini" ) );
			}
			catch ( Exception )
			{
				return;
			}
			while ( !reader.EndOfStream )
			{
				try
				{
					Lines.Add( reader.ReadLine() );
				}
				catch ( Exception )
				{
					break;
				}
			}
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize()
		{
			NumOfPlayers = LoadSetting( "Players", 2 );
			FenceLimit = LoadSetting( "FenceLimit", 3 );
			ShotLimit = LoadSetting( "ShotLimit", 1 );
			ShotTime = LoadSetting( "ShotTimeLimit", 10000 );
			FenceTime = LoadSetting( "FenceTimeLimit", 10000 );
			TimeDelay = LoadSetting( "EndingDelay", 3000 );
			StartDelay = LoadSetting( "FreezeTime", 1000 );
			TankScale = LoadSetting( "TankScale", 2F );
			PickupTime = LoadSetting( "PickupTime", 5000 );
			PickupDuration = LoadSetting( "PickupDuration", 10000 );
			SuddenDeathTime = LoadSetting( "SuddenDeathAfter", 60 );
			BlastRadius = LoadSetting( "BlastRadius", 5F );
			TankSpeed = LoadSetting( "TankSpeed", 5F );
			BulletSpeed = LoadSetting( "BulletSpeed", 2 * TankSpeed );
			BoostFactor = LoadSetting( "SpeedBoostFactor", 4 );

			// TODO: Add your initialization logic here
			Tanks = new TankObject[ NumOfPlayers ];
			KeySet p1keys = LoadSetting( "Player1Keys", new KeySet( Keys.NumPad8, Keys.NumPad5, Keys.NumPad4, Keys.NumPad6, Keys.NumPad1, Keys.NumPad2 ) );
			KeySet p2keys = LoadSetting( "Player2Keys", new KeySet( Keys.W, Keys.S, Keys.A, Keys.D, Keys.Z, Keys.X ) );
			KeySet p3keys = LoadSetting( "Player3Keys", new KeySet( Keys.Up, Keys.Down, Keys.Left, Keys.Right, Keys.OemPeriod, Keys.OemQuestion ) );
			KeySet p4keys = LoadSetting( "Player4Keys", new KeySet( Keys.U, Keys.J, Keys.H, Keys.K, Keys.V, Keys.B ) );

			Colors p1Color = LoadSetting( "Player1Color", Colors.Green );
			Colors p2Color = LoadSetting( "Player2Color", Colors.Purple );
			Colors p3Color = LoadSetting( "Player3Color", Colors.Blue );
			Colors p4Color = LoadSetting( "Player4Color", Colors.Orange );

			Tanks[ 0 ] = new TankObject( new Vector2( 50, 50 ), 45, p1Color, p1keys, TankScale, ShotLimit, ShotTime, FenceLimit, FenceTime, TankSpeed, BulletSpeed );

			if ( NumOfPlayers >= 2 )
			{
				Tanks[ 1 ] = new TankObject( new Vector2( width - 50, height - 50 ), 225, p2Color, p2keys
					, TankScale, ShotLimit, ShotTime, FenceLimit, FenceTime, TankSpeed, BulletSpeed );
			}

			if ( NumOfPlayers >= 3 )
			{
				Tanks[ 2 ] = new TankObject( new Vector2( width - 50, 50 ), 135, p3Color, p3keys, TankScale, ShotLimit, ShotTime, FenceLimit, FenceTime, TankSpeed, BulletSpeed );
			}
			if ( NumOfPlayers >= 4 )
			{
				Tanks[ 3 ] = new TankObject( new Vector2( 50, height - 50 ), 315, p4Color, p4keys, TankScale, ShotLimit, ShotTime, FenceLimit, FenceTime, TankSpeed, BulletSpeed );
			}
			Projectiles = new HashSet<ProjectileObject>();
			Fences = new HashSet<FenceObject>();
			Pickups = new HashSet<Pickup>();
			SuddenDeaths = new SuddenDeath[] { 
				new ShrinkyDeath(),
				new ExplodyDeath(),
				new GlitchyDeath(),
				new PossessyDeath(),
				new HoleyDeath(),
				new HomeyDeath(),
				new SuperNoveyDeath(),
			};
			PickupableOptions = new Type[]
			{
				typeof( Deflector ),
				typeof( Minimize ),
				typeof( Portal ),
				typeof( HomingBullet ),
				typeof( Missile ),
				typeof( Switcher ),
				typeof( BlackHole ),
				typeof( SpeedBoost ),
				typeof( Accelerator ),
				typeof( Roulette ),
				typeof( Lock ),
				typeof( ExtremeBullet ),
				typeof( ExtraLife ),
				typeof( ForceField ),
				typeof( Disabler ),
				typeof( Ghost ),
				typeof( Tripler ),
				typeof( Concealer ),
			};
			IsMouseVisible = true;
			base.Initialize();
		}

		private Colors LoadSetting( string setting, Colors defaultSetting )
		{
			string color = LoadSetting( setting );
			try
			{
				return ( Colors )Enum.Parse( typeof( Colors ), color );
			}
			catch ( Exception ) { return defaultSetting; }
		}

		private KeySet LoadSetting( string setting, KeySet defaultSetting )
		{
			string keystr = LoadSetting( setting );
			if ( keystr == "" ) return defaultSetting;
			string[] keys = keystr.Replace( " ", string.Empty ).Split( ',' );
			return new KeySet(
			LoadKey( keys, 0, defaultSetting.KeyUp ),
			LoadKey( keys, 2, defaultSetting.KeyDown ),
			LoadKey( keys, 1, defaultSetting.KeyLeft ),
			LoadKey( keys, 3, defaultSetting.KeyRight ),
			LoadKey( keys, 4, defaultSetting.KeyPlace ),
			LoadKey( keys, 5, defaultSetting.KeyShoot ) );
		}

		private Keys LoadKey( string[] keys, int index, Keys defaultKey )
		{
			try
			{
				return ( Keys )Enum.Parse( typeof( Keys ), keys[ index ] );
			}
			catch ( Exception )
			{
				return defaultKey;
			}
		}

		private string LoadSetting( string setting )
		{
			foreach ( string l in Lines )
			{
				if ( l[ 0 ] != '#' )
				{
					string[] Line = l.Split( '=' );
					if ( Line[ 0 ].ToLower() == setting.ToLower() )
					{
						try
						{
							return Line[ 1 ];
						}
						catch ( Exception ) { }
					}
				}
			}
			return "";
		}

		private string LoadSetting( string setting, string defaultSetting )
		{
			foreach ( string l in Lines )
			{
				if ( l[ 0 ] != '#' )
				{
					string[] Line = l.Split( '=' );
					if ( Line[ 0 ].ToLower() == setting.ToLower() )
					{
						try
						{
							return Line[ 1 ];
						}
						catch ( Exception ) { }
					}
				}
			}
			return defaultSetting;
		}

		private int LoadSetting( string setting, int defaultSetting )
		{
			try
			{
				return Int32.Parse( LoadSetting( setting ) );
			}
			catch ( Exception )
			{
				return defaultSetting;
			}
		}

		private double LoadSetting( string setting, double defaultSetting )
		{
			try
			{
				return Double.Parse( LoadSetting( setting ) );
			}
			catch ( Exception )
			{
				return defaultSetting;
			}
		}

		private float LoadSetting( string setting, float defaultSetting )
		{
			try
			{
				return float.Parse( LoadSetting( setting ) );
			}
			catch ( Exception )
			{
				return defaultSetting;
			}
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
			// Create a new SpriteBatch, which can be used to draw textures.
			spriteBatch = new SpriteBatch( GraphicsDevice );
			renderTarget = new RenderTarget2D( GraphicsDevice, GraphicsDevice.PresentationParameters.BackBufferWidth, GraphicsDevice.PresentationParameters.BackBufferHeight, false, SurfaceFormat.Color, DepthFormat.Depth16, 0, RenderTargetUsage.PreserveContents );

			// TODO: use this.Content to load your game content here
			ScoreFont = Content.Load<SpriteFont>( "Score" );
			TankMap = Content.Load<Texture2D>( "Sprites\\TankMap" );
			BulletTex = Content.Load<Texture2D>( "Sprites\\Bullet" );
			TankSources = new Rectangle[ 8 ][];
			int tankw = TankMap.Width / 8;
			int tankh = TankMap.Height / 8;
			for ( int x = 0; x < TankSources.Length; x++ )
			{
				TankSources[ x ] = new Rectangle[ 8 ];
				for ( int y = 0; y < TankSources[ x ].Length; y++ )
				{
					TankSources[ x ][ y ] = new Rectangle( y * tankw, x * tankh, tankw, tankh );
				}
			}
			blank = new Texture2D( GraphicsDevice, 2, 2, false, SurfaceFormat.Color );
			blank.SetData( Enumerable.Repeat<Color>( Color.White, 4 ).ToArray<Color>() );
			Projectiles.Add( new AProj( blank, width, height ) );
		}

		/// <summary>
		/// UnloadContent will be called once per game and is the place to unload
		/// all content.
		/// </summary>
		protected override void UnloadContent()
		{
			// TODO: Unload any non ContentManager content here
		}

		int frame = 0;
		KeyboardState oldkey;

		TimeSpan CountDown;
		bool IsGameDone;

		bool DidPlacePickup;

		TimeSpan Round;
		SuddenDeath roundDeath;

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update( GameTime gameTime )
		{
			KeyboardState key = Keyboard.GetState();
			// Allows the game to exit
			if ( GamePad.GetState( PlayerIndex.One ).Buttons.Back == ButtonState.Pressed || key.IsKeyDown( Keys.Escape ) )
				this.Exit();
			if ( key.IsKeyDown( Keys.D3 ) )
			{
				IsFixedTimeStep = true;
				TargetElapsedTime = TimeSpan.FromMilliseconds( 100 );
			}
			else
			{
				IsFixedTimeStep = false;
			}
			bool shouldUpdate = roundDeath == null || roundDeath.Update( Tanks, Projectiles, Fences, Pickups, gameTime );
			if ( !shouldUpdate )
			{
				CheckGeneralGame( gameTime, ref key, false );
				return;
			}
			if ( gameTime.TotalGameTime.TotalMilliseconds % PickupTime < 50 )
			{
				if ( !DidPlacePickup )
				{
					PlacePickup( gameTime, false );
					DidPlacePickup = true;
				}
			}
			else
			{
				DidPlacePickup = false;
			}

			if ( key.IsKeyDown( Keys.R ) && oldkey.IsKeyUp( Keys.R ) )
			{
				Reset( false, gameTime );
			}

			TimeSpan previousTime = gameTime.TotalGameTime - gameTime.ElapsedGameTime;
			float millisecsOfElapsation = 175;
			/*if ( key.IsKeyDown( Keys.LeftShift ) )
			{
				millisecsOfElapsation /= 4;
			}*/
			if ( gameTime.TotalGameTime.Milliseconds % millisecsOfElapsation < previousTime.Milliseconds % millisecsOfElapsation )
			{
				frame += 1;
				frame %= 8;
			}
			// TODO: Add your update logic here

			if ( appearingPowerUp == null || appearingPowerUp.UpdateProjectiles( gameTime, Projectiles ) )
			{
				HashSet<ProjectileObject> CopyOfProjectiles = new HashSet<ProjectileObject>( Projectiles );

				foreach ( ProjectileObject Proj in Projectiles )
				{
					if ( Proj is Missile ) ( ( Missile )Proj ).changeBlastRadius( BlastRadius );
					if ( Proj.Update( gameTime, Tanks, Fences, Pickups ) )
					{
						CopyOfProjectiles.Remove( Proj );
						if ( Proj.doesCount )
						{
							Proj.Owner.ShotsOnBoard--;
						}
					}
				}

				Projectiles = CopyOfProjectiles;
			}

			HashSet<FenceObject> CopyOfFences = new HashSet<FenceObject>( Fences );
			foreach ( FenceObject Fence in Fences )
			{
				if ( Fence.Update( gameTime ) )
				{
					CopyOfFences.Remove( Fence );
				}
			}

			Fences = CopyOfFences;

			HashSet<Pickup> CopyOfPickups = new HashSet<Pickup>( Pickups );
			foreach ( Pickup Pickup in Pickups )
			{
				if ( Pickup.Update( gameTime ) )
				{
					CopyOfPickups.Remove( Pickup );
				}
			}

			Pickups = CopyOfPickups;

			if ( appearingPowerUp != null && appearingPowerUp.Update( gameTime, Tanks, Projectiles, Fences ) )
			{
				appearingPowerUp.Stop();
				appearingPowerUp = null;
			}

			CheckGeneralGame( gameTime, ref key, true );

			oldkey = key;
			base.Update( gameTime );
		}

		private void CheckGeneralGame( GameTime gameTime, ref KeyboardState key, bool shouldUpdateTanks )
		{
			bool IsATankOut = false;
			foreach ( TankObject tank in Tanks )
			{
				if ( !tank.IsInGame )
				{
					if ( !tank.IsOfficiallyOut )
					{
						tank.IsOfficiallyOut = true;
						IsATankOut = true;
						break;
					}
				}
				else
				{
					if ( tank.IsOfficiallyOut )
					{
						tank.IsOfficiallyOut = false;
						IsGameDone = false;
						IsATankOut = true;
						break;
					}
				}
				if ( shouldUpdateTanks )
					tank.Update( gameTime, key, oldkey, width, height, Tanks, Projectiles, Fences, Pickups, GraphicsDevice );
			}

			if ( IsATankOut )
			{
				int HowManyIn = 0;
				foreach ( TankObject Tank in Tanks )
				{
					if ( !Tank.IsOfficiallyOut && Tank.IsInGame )
					{
						HowManyIn++;
					}
				}
				if ( HowManyIn <= 1 )
				{
					IsGameDone = true;
					CountDown = gameTime.TotalGameTime;
				}
			}
			if ( IsGameDone && ( ( gameTime.TotalGameTime - CountDown ).TotalMilliseconds > TimeDelay || TimeDelay <= 0 ) )
			{
				Reset( true, gameTime );
			}
			else if ( ( gameTime.TotalGameTime - Round ).Seconds >= SuddenDeathTime && SuddenDeathTime > 0 && roundDeath == null )
			{
				BeginSuddenDeath( gameTime );
			}
		}

		private void PlacePickup( GameTime gameTime, bool notfirst )
		{
			if ( PickupableOptions.Length == 0 ) return;
			Type pickup = PickupableOptions[ random.Next( PickupableOptions.Length ) ];
			Object pickupToAdd = null;
			if ( pickup.IsSubclassOf( typeof( PowerUp ) ) )
			{
				PowerUp powerUp = ( PowerUp )Activator.CreateInstance( pickup, gameTime );
				powerUp = ProcessPowerUp( powerUp, gameTime );
				pickupToAdd = powerUp;
			}
			try
			{
				Pickups.Add( new Pickup( width, height, gameTime, PickupDuration, pickupToAdd, Content ) );
			}
			catch ( NullReferenceException )
			{
				Pickups.Add( new Pickup( width, height, gameTime, PickupDuration, pickup, Content ) );
			}
			catch ( ArgumentException )
			{
				if ( pickup.IsSubclassOf( typeof( PowerUp ) ) )
				{
					PowerUp powerUp = ( PowerUp )Activator.CreateInstance( pickup, gameTime );
					powerUp = ProcessPowerUp( powerUp, gameTime );

					if ( powerUp is AppearingPowerUp )
					{
						AppearingPowerUp newAppearingPowerUp = ( AppearingPowerUp )powerUp;
						if ( appearingPowerUp == null )
						{
							newAppearingPowerUp.LoadTex( Content );
							newAppearingPowerUp.ChangeDuration( PickupDuration );
							appearingPowerUp = newAppearingPowerUp;
						}
						else
						{
							if ( !notfirst ) PlacePickup( gameTime, true );
							else
							{
								//Give up.
							}
						}
					}
				}
			}
		}

		/// <summary>
		/// Processes the power-up - Optimizes it according to its nature.
		/// </summary>
		/// <param name="powerUp">The power-up to process</param>
		/// <param name="gameTime">The current game time</param>
		/// <returns>The processed power-up.</returns>
		private PowerUp ProcessPowerUp( PowerUp powerUp, GameTime gameTime )
		{
			PowerUp processedPowerUp = powerUp;
			if ( powerUp is SpeedBoost )
			{
				// One instance of processing is to set the speed boost's factor according to the setting file.
				processedPowerUp = new SpeedBoost( gameTime, BoostFactor );
			}
			return processedPowerUp;
		}

		private void BeginSuddenDeath( GameTime gameTime )
		{
			roundDeath = SuddenDeaths[ random.Next( SuddenDeaths.Length ) ];
			roundDeath.Initialize( gameTime, Content );
			if ( roundDeath is SuperNoveyDeath )
			{
				SuperNoveyBlackHole supernova = new SuperNoveyBlackHole( gameTime );
				supernova.LoadTex( Content );
				appearingPowerUp = supernova;
				float speed = 5;
				for ( int i = 0; i < 180; i++ )
				{
					Projectiles.Add( new BasicBullet( Vector2.Zero, 0, gameTime, width, height, speed, new TankObject() ) );
					Projectiles.Add( new BasicBullet( Vector2.UnitX * width, 0, gameTime, width, height, speed, new TankObject() ) );
					Projectiles.Add( new BasicBullet( Vector2.UnitY * height, 0, gameTime, width, height, speed, new TankObject() ) );
					Projectiles.Add( new BasicBullet( new Vector2( width, height ), 0, gameTime, width, height, speed, new TankObject() ) );
				}
			}
		}

		private void Reset( bool addScore, GameTime gameTime )
		{
			IsGameDone = false;
			Projectiles = new HashSet<ProjectileObject>();
			Projectiles.Add( new AProj( blank, width, height ) );
			Fences = new HashSet<FenceObject>();
			Pickups = new HashSet<Pickup>();
			if ( appearingPowerUp != null )
			{
				appearingPowerUp.Stop();
				appearingPowerUp = null;
			}
			foreach ( TankObject Tank in Tanks )
			{
				if ( Tank.IsInGame && addScore )
				{
					Tank.Score++;
				}
				Tank.Reset();
			}
			if ( StartDelay > 0 )
			{
				System.Threading.Thread.Sleep( StartDelay );
			}
			Round = gameTime.TotalGameTime;
			roundDeath = null;
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw( GameTime gameTime )
		{
			GraphicsDevice.SetRenderTarget( renderTarget );
			// TODO: Add your drawing code here
			spriteBatch.Begin( SpriteSortMode.BackToFront, BlendState.AlphaBlend );

			if ( roundDeath == null || roundDeath.Draw( spriteBatch, Tanks, Projectiles, Fences, gameTime ) )
			{
				GraphicsDevice.Clear( Color.CornflowerBlue );
			}
			foreach ( TankObject Tank in Tanks )
			{
				if ( Tank.IsInGame )
				{
					spriteBatch.Draw( TankMap, Tank.Position, TankSources[ ( int )Tank.TankColor ][ frame ], Color.White, Tank.Rotation, new Vector2( 16, 16 ), Tank.Scale, SpriteEffects.None, 0.5F );
					if ( Tank.powerUp != null )
					{
						Tank.powerUp.Draw( spriteBatch, gameTime );
					}
				}
				spriteBatch.DrawString( ScoreFont, Tank.Score.ToString(), Tank.OP, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 1F );
			}
			foreach ( ProjectileObject Bullet in Projectiles )
			{
				if ( Bullet.tex == null ) Bullet.LoadTex( Content );
				Bullet.Draw( gameTime, spriteBatch );
			}
			foreach ( FenceObject Fence in Fences )
			{
				Fence.Draw( spriteBatch );
			}
			foreach ( Pickup Pickup in Pickups )
			{
				Pickup.Draw( spriteBatch );
			}
			if ( appearingPowerUp != null )
				appearingPowerUp.Draw( spriteBatch, gameTime );
			spriteBatch.End();
			GraphicsDevice.SetRenderTarget( null );
			spriteBatch.Begin();
			spriteBatch.Draw( ( Texture2D )renderTarget, Vector2.Zero, Color.White );
			spriteBatch.End();

			base.Draw( gameTime );
		}
	}

	public enum Colors
	{
		Green = 0,
		Red = 1,
		Yellow = 2,
		Blue = 3,
		Purple = 4,
		Aqua = 5,
		Orange = 6,
		Pink = 7
	};
}
