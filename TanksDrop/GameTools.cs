using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TanksDrop.Projectiles;

namespace TanksDrop
{
	class GameTools
	{
		public static Vector2 Transform( Vector2 point, Vector2 origin, float rotation )
		{
			return Vector2.Transform( point - origin, Matrix.CreateRotationZ( rotation ) ) + origin;
		}

		public static bool LineIntersectsRect( Vector2 p1, Vector2 p2, Rectangle r )
		{
			return LineIntersectsLine( p1, p2, new Vector2( r.X, r.Y ), new Vector2( r.X + r.Width, r.Y ) ) ||
				   LineIntersectsLine( p1, p2, new Vector2( r.X + r.Width, r.Y ), new Vector2( r.X + r.Width, r.Y + r.Height ) ) ||
				   LineIntersectsLine( p1, p2, new Vector2( r.X + r.Width, r.Y + r.Height ), new Vector2( r.X, r.Y + r.Height ) ) ||
				   LineIntersectsLine( p1, p2, new Vector2( r.X, r.Y + r.Height ), new Vector2( r.X, r.Y ) ) ||
				   ( r.Contains( ( int )p1.X, ( int )p1.Y ) && r.Contains( ( int )p2.X, ( int )p2.Y ) );
		}

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

		public static Vector2 ReturnMove( float factor, float angle, Vector2 Position )
		{
			return Position + ( new Vector2( ( float )Math.Cos( MathHelper.ToRadians( angle ) ), ( float )Math.Sin( MathHelper.ToRadians( angle ) ) ) * factor );
		}

		public static Vector2 ReturnMoveInRadians( float factor, float angle, Vector2 Position )
		{
			return Position + ( new Vector2( ( float )Math.Cos( angle ), ( float )Math.Sin( angle ) ) * factor );
		}

		public static Vector2 RotatePoint( Vector2 thePoint, Vector2 theOrigin, float theRotation )
		{
			Vector2 aTranslatedPoint = new Vector2();
			aTranslatedPoint.X = ( float )( theOrigin.X + ( thePoint.X - theOrigin.X ) * Math.Cos( theRotation )
				- ( thePoint.Y - theOrigin.Y ) * Math.Sin( theRotation ) );
			aTranslatedPoint.Y = ( float )( theOrigin.Y + ( thePoint.Y - theOrigin.Y ) * Math.Cos( theRotation )
				+ ( thePoint.X - theOrigin.X ) * Math.Sin( theRotation ) );
			return aTranslatedPoint;
		}

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

		private static Rectangle SmallRectangle( Vector2 point, int size )
		{
			return new Rectangle( ( int )point.X, ( int )point.Y, size, size );
		}

		public static Vector2 GetRandomPos( HashSet<ProjectileObject> Projectiles )
		{
			Random random = new Random();
			int width = 0;
			int height = 0;
			foreach ( ProjectileObject p in Projectiles ) if ( p is AProj ) { width = ( ( AProj )p ).ScrWidth; height = ( ( AProj )p ).ScrHeight; }
			return new Vector2( random.Next( width ), random.Next( height ) );
		}

		public static Vector2 GetWH( HashSet<ProjectileObject> Projectiles )
		{
			int width = 0;
			int height = 0;
			foreach ( ProjectileObject p in Projectiles ) if ( p is AProj ) { width = ( ( AProj )p ).ScrWidth; height = ( ( AProj )p ).ScrHeight; }
			return new Vector2( width, height );
		}
	}
}
