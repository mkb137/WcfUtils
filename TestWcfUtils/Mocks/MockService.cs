/**
 *
 *   Copyright (c) 2014 Entropa Software Ltd.  All Rights Reserved.    
 *
 */
using System;
using System.ServiceModel;
using log4net;

namespace TestWcfUtils.Mocks {
	/// <summary>
	/// A service implementing <see cref="IMockContract"/>
	/// </summary>
	[ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
	public class MockService : IMockContract {

		/// <summary>
		/// The logger.
		/// </summary>
		// ReSharper disable once UnusedMember.Local
		private readonly static ILog	_log	= LogManager.GetLogger( typeof( MockService ) );

		/// <summary>
		/// <see cref="IMockContract.Add(int,int)"/>
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public int Add( int a, int b ) {
			return a + b;
		}

		/// <summary>
		/// <see cref="IMockContract.Add(double,double)"/>
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public double Add( double a, double b ) {
			return a + b;
		}

		/// <summary>
		/// <see cref="IMockContract.Add(int,int,int)"/>
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <param name="c"></param>
		/// <returns></returns>
		public int Add( int a, int b, int c ) {
			return a + b + c;
		}

		/// <summary>
		/// <see cref="IMockContract.Add(int,int,int,int)"/>
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <param name="c"></param>
		/// <param name="d"></param>
		/// <returns></returns>
		public int Add( int a, int b, int c, int d ) {
			return a + b + c + d;
		}

		/// <summary>
		/// <see cref="IMockContract.Add(int,int,int,int,int)"/>
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <param name="c"></param>
		/// <param name="d"></param>
		/// <param name="e"></param>
		/// <returns></returns>
		public int Add( int a, int b, int c, int d, int e ) {
			return a + b + c + d + e;
		}

		/// <summary>
		/// <see cref="IMockContract.Add(int,int,int,int,int,int)"/>
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <param name="c"></param>
		/// <param name="d"></param>
		/// <param name="e"></param>
		/// <param name="f"></param>
		/// <returns></returns>
		public int Add( int a, int b, int c, int d, int e, int f ) {
			return a + b + c + d + e + f;
		}

		/// <summary>
		/// <see cref="IMockContract.Add(int,int,int,int,int,int,int)"/>
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <param name="c"></param>
		/// <param name="d"></param>
		/// <param name="e"></param>
		/// <param name="f"></param>
		/// <param name="g"></param>
		/// <returns></returns>
		public int Add( int a, int b, int c, int d, int e, int f, int g ) {
			return a + b + c + d + e + f + g;
		}

		/// <summary>
		/// <see cref="IMockContract.Add(int,int,int,int,int,int,int,int)"/>
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <param name="c"></param>
		/// <param name="d"></param>
		/// <param name="e"></param>
		/// <param name="f"></param>
		/// <param name="g"></param>
		/// <param name="h"></param>
		/// <returns></returns>
		public int Add( int a, int b, int c, int d, int e, int f, int g, int h ) {
			return a + b + c + d + e + f + g + h;
		}

		/// <summary>
		/// <see cref="IMockContract.Divide"/>
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public int Divide( int a, int b ) {
			if ( b == 0 ) throw new DivideByZeroException();
			return a / b;
		}

		/// <summary>
		/// <see cref="IMockContract.GetCollection"/>
		/// </summary>
		/// <param name="noElements"></param>
		/// <returns></returns>
		public MockCollection GetCollection( int noElements ) {
			MockCollection collection = new MockCollection();
			for ( int i = 0; i < noElements; i++ ) {
				collection.Add( new MockObject { A = "A" + i, B = "B" + i } );
			}
			return collection;
		}

		/// <summary>
		/// <see cref="IMockContract.MultipleParameterTypes"/>
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <param name="c"></param>
		/// <param name="d"></param>
		/// <param name="e"></param>
		/// <param name="f"></param>
		/// <returns></returns>
		public String MultipleParameterTypes( String a, object b, DateTime c, TimeSpan d, double? e, int? f ) {
			return String.Format( "a: {0}, b:{1}, c:{2:yyyy-MM-dd}, d:{3}, e:{4}, f:{5}", a, b, c, d, e, f );
		}

		/// <summary>
		/// <see cref="IMockContract.Multiply"/>
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public int Multiply( int a, int b ) {
			return a * b;
		}

		/// <summary>
		/// <see cref="IMockContract.NoParameter"/>
		/// </summary>
		/// <returns></returns>
		public string NoParameter() {
			return "AAA";
		}

		/// <summary>
		/// <see cref="IMockContract.NoParameterNoReturn"/>
		/// </summary>
		public void NoParameterNoReturn() {
		}

		/// <summary>
		/// <see cref="IMockContract.NoReturn"/>
		/// </summary>
		/// <param name="a"></param>
		public void NoReturn( string a ) {
		}

		/// <summary>
		/// <see cref="IMockContract.Subtract"/>
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public int Subtract( int a, int b ) {
			return a - b;
		}

		/// <summary>
		/// <see cref="IMockContract.ThrowException"/>
		/// </summary>
		public void ThrowException() {
			throw new InvalidOperationException( "Test" );
		}
	}
}
