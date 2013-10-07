using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TanksDrop.Projectiles;

namespace TanksDrop
{
	/// <summary>
	/// A class that contains many essential game functions.
	/// </summary>
	class GameTools
	{
		/// <summary>
		/// Transforms a point by origin and rotation.
		/// </summary>
		/// <param name="point">The point to transform.</param>
		/// <param name="origin">The origin of the point.</param>
		/// <param name="rotation">The rotation in radians.</param>
		/// <returns>The transformed point.</returns>
		public static Vector2 Transform( Vector2 point, Vector2 origin, float rotation )
		{
			return Vector2.Transform( point - origin, Matrix.CreateRotationZ( rotation ) ) + origin;
		}

		/// <summary>
		/// Used to tell whether a segment intersects a rectangle.
		/// </summary>
		/// <param name="p1">The first end of the segment.</param>
		/// <param name="p2">The second end of the segment.</param>
		/// <param name="r">The rectangle to check.</param>
		/// <returns>true if does intersect - otherwise false.</returns>
		public static bool LineIntersectsRect( Vector2 p1, Vector2 p2, Rectangle r )
		{
			return LineIntersectsLine( p1, p2, new Vector2( r.X, r.Y ), new Vector2( r.X + r.Width, r.Y ) ) ||
				   LineIntersectsLine( p1, p2, new Vector2( r.X + r.Width, r.Y ), new Vector2( r.X + r.Width, r.Y + r.Height ) ) ||
				   LineIntersectsLine( p1, p2, new Vector2( r.X + r.Width, r.Y + r.Height ), new Vector2( r.X, r.Y + r.Height ) ) ||
				   LineIntersectsLine( p1, p2, new Vector2( r.X, r.Y + r.Height ), new Vector2( r.X, r.Y ) ) ||
				   ( r.Contains( ( int )p1.X, ( int )p1.Y ) && r.Contains( ( int )p2.X, ( int )p2.Y ) );
		}

		/// <summary>
		/// Used to tell whether a segment intersects anoher segment.
		/// </summary>
		/// <param name="l1p1">The first end of the first segment.</param>
		/// <param name="l1p2">The second end of the first segment.</param>
		/// /// <param name="l1p1">The first end of the second segment.</param>
		/// <param name="l1p2">The second end of the second segment.</param>
		/// <returns>true if does intersect - otherwise false.</returns>
		public static bool LineIntersectsLine( Vector2 l1p1, Vector2 l1p2, Vector2 l2p1, Vector2 l2p2 )
		{
			float q = ( l1p1.Y - l2p1.Y ) * ( l2p2.X - l2p1.X ) - ( l1p1.X - l2p1.X ) * ( l2p2.Y - l2p1.Y );
			float d = ( l1p2.X - l1p1.X ) * ( l2p2.Y - l2p1.Y ) - ( l1p2.Y - l1p1.Y ) * ( l2p2.X - l2p1.X );

			if ( d == 0 )
			{
				return false;
			}

			float r = q / d;

			q = ( l1p1.Y - l2p1.Y ) * ( l1p2.X - l1p1.X ) - ( l1p1.X - l2p1.X ) * ( l1p2.Y - l1p1.Y );
			float s = q / d;

			if ( r < 0 || r > 1 || s < 0 || s > 1 )
			{
				return false;
			}

			return true;
		}

		/// <summary>
		/// Moves the given point the given distance at the given angle.
		/// </summary>
		/// <param name="factor">The distance to move the point.</param>
		/// <param name="angle">The angle of the movement in degrees.</param>
		/// <param name="Position">The point to move.</param>
		/// <returns>The moved point.</returns>
		public static Vector2 ReturnMove( float factor, float angle, Vector2 Position )
		{
			return Position + ( new Vector2( ( float )Math.Cos( MathHelper.ToRadians( angle ) ), ( float )Math.Sin( MathHelper.ToRadians( angle ) ) ) * factor );
		}

		/// <summary>
		/// Moves the given point the given distance at the given angle.
		/// </summary>
		/// <param name="factor">The distance to move the point.</param>
		/// <param name="angle">The angle of the movement in radians.</param>
		/// <param name="Position">The point to move.</param>
		/// <returns>The moved point.</returns>
		public static Vector2 ReturnMoveInRadians( float factor, float angle, Vector2 Position )
		{
			return Position + ( new Vector2( ( float )Math.Cos( angle ), ( float )Math.Sin( angle ) ) * factor );
		}
		
		/// <summary>
		/// Used to determine whether a rotated rectangle with the given origin intersects a normal rectangle.
		/// </summary>
		/// <param name="Position">The position of the rotated rectangle.</param>
		/// <param name="Origin">The origin of the rotated rectangle. Relative to the top-left point of the rectangle.</param>
		/// <param name="Width">The width of the rotated rectangle.</param>
		/// <param name="Height">The height of the rotated rectangle.</param>
		/// <param name="radAngle">The angle of the rotated rectangle in radians.</param>
		/// <param name="rect">The normal rectangle.</param>
		/// <returns>true if does intersect - otherwise false.</returns>
		public static bool RotRectIntersectRect( Vector2 Position, Vector2 Origin, float Width, float Height, float radAngle, Rectangle rect )
		{
			/*
			 * Old
			Vector2 ul = Position - Origin;
			Vector2 ur = ul + new Vector2( Width, 0 );
			Vector2 lr = ul + new Vector2( 0, Height );
			Vector2 ll = ul + new Vector2( Width, Height );
			ul = GameTools.Transform( ul, Position, radAngle );
			ur = GameTools.Transform( ur, Position, radAngle );
			ll = GameTools.Transform( ll, Position, radAngle );
			lr = GameTools.Transform( lr, Position, radAngle );
			return GameTools.LineIntersectsRect( lr, ur, rect ) ||
						GameTools.LineIntersectsRect( lr, ll, rect ) ||
						GameTools.LineIntersectsRect( ll, ul, rect ) ||
						GameTools.LineIntersectsRect( ur, ul, rect );*/
			Vector2 Location = Position - Origin;
			Rectangle Bounds = new Rectangle( ( int )Location.X, ( int )Location.Y, ( int )Width, ( int )Height );
			Vector2 ul = GameTools.Transform( new Vector2( rect.X, rect.Y ), Position, 2 * ( float )Math.PI - radAngle );
			Vector2 ur = GameTools.Transform( new Vector2( rect.X + rect.Width, rect.Y ), Position, 2 * ( float )Math.PI - radAngle );
			Vector2 ll = GameTools.Transform( new Vector2( rect.X, rect.Y + rect.Height ), Position, 2 * ( float )Math.PI - radAngle );
			Vector2 lr = GameTools.Transform( new Vector2( rect.X + rect.Width, rect.Y + rect.Height ), Position, 2 * ( float )Math.PI - radAngle );
			Rectangle ulr = GameTools.SmallRectangle( ul, 2 );
			Rectangle urr = GameTools.SmallRectangle( ur, 2 );
			Rectangle llr = GameTools.SmallRectangle( ll, 2 );
			Rectangle lrr = GameTools.SmallRectangle( lr, 2 );
			bool intersects = 
				Bounds.Intersects( ulr ) ||
				Bounds.Intersects( urr ) ||
				Bounds.Intersects( llr ) ||
				Bounds.Intersects( lrr );
			if ( intersects ) return true;
			Bounds = rect;
			ul = Position - Origin;
			ur = ul + new Vector2( Width, 0 );
			lr = ul + new Vector2( 0, Height );
			ll = ul + new Vector2( Width, Height );
			ul = GameTools.Transform( ul, Position, radAngle );
			ur = GameTools.Transform( ur, Position, radAngle );
			ll = GameTools.Transform( ll, Position, radAngle );
			lr = GameTools.Transform( lr, Position, radAngle );
			ulr = GameTools.SmallRectangle( ul, 2 );
			urr = GameTools.SmallRectangle( ur, 2 );
			llr = GameTools.SmallRectangle( ll, 2 );
			lrr = GameTools.SmallRectangle( lr, 2 );
			if ( Bounds.Intersects( ulr ) ||
				Bounds.Intersects( urr ) ||
				Bounds.Intersects( llr ) ||
				Bounds.Intersects( lrr ) ) return true;
			return false;
		}

		/// <summary>
		/// Creates a small size*size rectangle with the given point as its position.
		/// </summary>
		/// <param name="point">The point to create the small rectangle from.</param>
		/// <param name="size">The size of the small rectangle.</param>
		/// <returns>The small rectangle.</returns>
		private static Rectangle SmallRectangle( Vector2 point, int size )
		{
			return new Rectangle( ( int )point.X, ( int )point.Y, size, size );
		}

		/// <summary>
		/// Gets a random position on the board.
		/// </summary>
		/// <param name="Projectiles">The projectiles on board. Used to get the screen width and height from the AProj.</param>
		/// <returns>A random position inside the board's bounds.</returns>
		public static Vector2 GetRandomPos( HashSet<ProjectileObject> Projectiles )
		{
			Random random = new Random();
			int width = 0;
			int height = 0;
			foreach ( ProjectileObject p in Projectiles ) if ( p is AProj ) { width = ( ( AProj )p ).ScrWidth; height = ( ( AProj )p ).ScrHeight; }
			return new Vector2( random.Next( width ), random.Next( height ) );
		}

		/// <summary>
		/// Gets the width and height of the board in pixels.
		/// </summary>
		/// <param name="Projectiles">The projectiles on board. Used to get the screen width and height from the AProj.</param>
		/// <returns>A <see cref="Vector2"/> that specifies the bottom-right corner.</returns>
		public static Vector2 GetWH( HashSet<ProjectileObject> Projectiles )
		{
			int width = 0;
			int height = 0;
			foreach ( ProjectileObject p in Projectiles ) if ( p is AProj ) { width = ( ( AProj )p ).ScrWidth; height = ( ( AProj )p ).ScrHeight; }
			return new Vector2( width, height );
		}
	}
}
