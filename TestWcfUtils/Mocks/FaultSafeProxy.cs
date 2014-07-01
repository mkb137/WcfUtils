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
	/// A fault safe proxy.
	/// 
	/// This serves as a prototype for the code produced from the FaultSafeProxyEmitter.  
	/// To see how a given block of code should be emittied, add it to this prototype and use the IL tool to reverse engineer the CIL.
	/// </summary>
	public class FaultSafeProxy : IMockContract, IDisposable {

		/// <summary>
		/// The channel factory.
		/// </summary>
		private readonly ChannelFactory<IMockContract>	_factory;
		/// <summary>
		/// The channel.
		/// </summary>
		private IMockContract							_channel;

		/// <summary>
		/// The constructor.
		/// </summary>
		/// <param name="endpoint"></param>
		public FaultSafeProxy( string endpoint ) : this ( endpoint, null, null ) {
		}

		/// <summary>
		/// The constructor.
		/// </summary>
		/// <param name="endpoint"></param>
		/// <param name="userName"></param>
		/// <param name="password"></param>
		public FaultSafeProxy( string endpoint, string userName, string password ) {
			this._factory = new ChannelFactory<IMockContract>( endpoint );
			if ( String.IsNullOrEmpty( userName ) || String.IsNullOrEmpty( password ) ) {
				return;
			}
			if ( null == this._factory.Credentials ) throw new InvalidOperationException( "factory has no credentials" );
			this._factory.Credentials.UserName.UserName = userName;
			this._factory.Credentials.UserName.Password = password;
		}

		#region Channel Management 

		/// <summary>
		/// Aborts the service channel in the case of exception.
		/// </summary>
		private void Abort() {
			if ( null == this._channel ) return;
			// ReSharper disable once SuspiciousTypeConversion.Global
			IServiceChannel serviceChannel = (IServiceChannel)this._channel;
			serviceChannel.Abort();
			this._channel = null;
		}

		/// <summary>
		/// Closes the channel.
		/// </summary>
		private void Close() {
			if ( null == this._channel ) return;
			// ReSharper disable once SuspiciousTypeConversion.Global
			IServiceChannel serviceChannel = (IServiceChannel)this._channel;
			serviceChannel.Close();
			this._channel = null;
		}

		/// <summary>
		/// The channel.
		/// </summary>
		private IMockContract GetChannel() {
			if ( null == this._channel ) {
				this._channel = this._factory.CreateChannel();
			}
			return this._channel;
		}

		#endregion

		#region IDisposable

		/// <summary>
		/// <see cref="IDisposable.Dispose"/>
		/// </summary>
		public void Dispose() {
			this.Close();
		}

		#endregion

		#region IMockContract

		/// <summary>
		/// <see cref="IMockContract.Add(int,int)"/>
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public int Add( int a, int b ) {
			try {
				return this.GetChannel().Add( a, b );
			} catch ( Exception ) {
				this.Abort();
				throw;
			}
		}

		/// <summary>
		/// <see cref="IMockContract.Add(double,double)"/>
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public double Add( double a, double b ) {
			try {
				return this.GetChannel().Add( a, b );
			} catch ( Exception ) {
				this.Abort();
				throw;
			}
		}

		/// <summary>
		/// <see cref="IMockContract.Add(int,int,int)"/>
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <param name="c"></param>
		/// <returns></returns>
		public int Add( int a, int b, int c ) {
			try {
				return this.GetChannel().Add( a, b, c );
			} catch ( Exception ) {
				this.Abort();
				throw;
			}
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
			try {
				return this.GetChannel().Add( a, b, c, d );
			} catch ( Exception ) {
				this.Abort();
				throw;
			}
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
			try {
				return this.GetChannel().Add( a, b, c, d, e );
			} catch ( Exception ) {
				this.Abort();
				throw;
			}
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
			try {
				return this.GetChannel().Add( a, b, c, d, e, f );
			} catch ( Exception ) {
				this.Abort();
				throw;
			}
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
			try {
				return this.GetChannel().Add( a, b, c, d, e, f, g );
			} catch ( Exception ) {
				this.Abort();
				throw;
			}
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
			try {
				return this.GetChannel().Add( a, b, c, d, e, f, g, h );
			} catch ( Exception ) {
				this.Abort();
				throw;
			}
		}

		/// <summary>
		/// <see cref="IMockContract.Divide"/>
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public int Divide( int a, int b ) {
			try {
				return this.GetChannel().Divide( a, b );
			} catch ( Exception ) {
				this.Abort();
				throw;
			}
		}

		/// <summary>
		/// <see cref="IMockContract.GetCollection"/>
		/// </summary>
		/// <param name="noElements"></param>
		/// <returns></returns>
		public MockCollection GetCollection( int noElements ) {
			try {
				return this.GetChannel().GetCollection( noElements );
			} catch ( Exception ) {
				this.Abort();
				throw;
			}
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
		public string MultipleParameterTypes( string a, object b, DateTime c, TimeSpan d, double? e, int? f ) {
			try {
				return this.GetChannel().MultipleParameterTypes( a, b, c, d, e, f );
			} catch ( Exception ) {
				this.Abort();
				throw;
			}
		}

		/// <summary>
		/// <see cref="IMockContract.Multiply"/>
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public int Multiply( int a, int b ) {
			try {
				return this.GetChannel().Multiply( a, b );
			} catch ( Exception ) {
				this.Abort();
				throw;
			}
		}

		/// <summary>
		/// <see cref="IMockContract.NoParameter"/>
		/// </summary>
		/// <returns></returns>
		public string NoParameter() {
			try {
				return this.GetChannel().NoParameter();
			} catch ( Exception ) {
				this.Abort();
				throw;
			}
		}

		/// <summary>
		/// <see cref="IMockContract.NoParameterNoReturn"/>
		/// </summary>
		public void NoParameterNoReturn() {
			try {
				this.GetChannel().NoParameterNoReturn();
			} catch ( Exception ) {
				this.Abort();
				throw;
			}
		}

		/// <summary>
		/// <see cref="IMockContract.NoReturn"/>
		/// </summary>
		/// <param name="a"></param>
		public void NoReturn( string a ) {
			try {
				this.GetChannel().NoReturn( a );
			} catch ( Exception ) {
				this.Abort();
				throw;
			}
		}

		/// <summary>
		/// <see cref="IMockContract.Subtract"/>
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public int Subtract( int a, int b ) {
			try {
				return this.GetChannel().Subtract( a, b );
			} catch ( Exception ) {
				this.Abort();
				throw;
			}
		}

		/// <summary>
		/// <see cref="IMockContract.ThrowException"/>
		/// </summary>
		public void ThrowException() {
			try {
				this.GetChannel().ThrowException();
			} catch ( Exception ) {
				this.Abort();
				throw;
			}
		}

		#endregion

	}
}
