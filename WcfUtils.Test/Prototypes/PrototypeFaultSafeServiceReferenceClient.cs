using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using System.Security;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;
using Entropa.WcfUtils.Test.MockServiceReference;
using log4net;

namespace Entropa.WcfUtils.Test.Prototypes {

	/// <summary>
	///     This prototypes a fault safe wrapper around a ServiceClient object.
	/// </summary>
	internal sealed class PrototypeFaultSafeServiceReferenceClient<TClient> : IMockContract, IDisposable
		where TClient : ClientBase<IMockContract>, IMockContract {

		/// <summary>
		/// The binding, if passed to the constructor.
		/// </summary>
		private readonly Binding			_binding;
		/// <summary>
		/// The endpoint configuration name, if passed to the constructor.
		/// </summary>
		private readonly string				_endpointConfigurationName;
		/// <summary>
		/// The password, if passed to the constructor.
		/// </summary>
		private readonly SecureString		_password;
		/// <summary>
		/// The remote address, if passed to the constructor.
		/// </summary>
		private readonly EndpointAddress	_remoteAddress;
		/// <summary>
		/// The user name, if passed to the constructor.
		/// </summary>
		private readonly string				_userName;
		/// <summary>
		/// The service reference client.
		/// </summary>
		private TClient						_client;

		/// <summary>
		///     The constructor.
		/// </summary>
		public PrototypeFaultSafeServiceReferenceClient() {
			this._binding					= null;
			this._endpointConfigurationName	= null;
			this._remoteAddress				= null;
			this._userName					= null;
			this._password					= null;
		}

		/// <summary>
		///     The constructor with endpoint configured.
		/// </summary>
		/// <param name="endpointConfigurationName"></param>
		public PrototypeFaultSafeServiceReferenceClient( string endpointConfigurationName ) {
			this._binding					= null;
			this._endpointConfigurationName	= endpointConfigurationName;
			this._remoteAddress				= null;
			this._userName					= null;
			this._password					= null;
		}

		/// <summary>
		///     The constructor with endpoint and remove address configured.
		/// </summary>
		/// <param name="endpointConfigurationName"></param>
		/// <param name="remoteAddress"></param>
		public PrototypeFaultSafeServiceReferenceClient( string endpointConfigurationName, EndpointAddress remoteAddress ) {
			this._binding					= null;
			this._endpointConfigurationName	= endpointConfigurationName;
			this._remoteAddress				= remoteAddress;
			this._userName					= null;
			this._password					= null;
		}

		/// <summary>
		///     The constructor with binding remote address configured.
		/// </summary>
		/// <param name="binding"></param>
		/// <param name="remoteAddress"></param>
		public PrototypeFaultSafeServiceReferenceClient( Binding binding, EndpointAddress remoteAddress ) {
			this._binding					= binding;
			this._endpointConfigurationName	= null;
			this._remoteAddress				= remoteAddress;
			this._userName					= null;
			this._password					= null;
		}

		/// <summary>
		///     The constructor.
		/// </summary>
		/// <param name="userName"></param>
		/// <param name="password"></param>
		public PrototypeFaultSafeServiceReferenceClient( string userName, string password ) {
			this._binding					= null;
			this._endpointConfigurationName	= null;
			this._remoteAddress				= null;
			this._userName					= userName;
			this._password					= new SecureString();
			foreach ( char c in password ) {
				this._password.AppendChar( c );
			}
			this._password.MakeReadOnly();
		}

		/// <summary>
		///     The constructor with endpoint configured.
		/// </summary>
		/// <param name="endpointConfigurationName"></param>
		/// <param name="userName"></param>
		/// <param name="password"></param>
		public PrototypeFaultSafeServiceReferenceClient( string endpointConfigurationName, string userName, string password ) {
			this._binding					= null;
			this._endpointConfigurationName	= endpointConfigurationName;
			this._remoteAddress				= null;
			this._userName					= userName;
			this._password					= new SecureString();
			foreach ( char c in password ) {
				this._password.AppendChar( c );
			}
			this._password.MakeReadOnly();
		}

		/// <summary>
		///     The constructor with endpoint and remove address configured.
		/// </summary>
		/// <param name="endpointConfigurationName"></param>
		/// <param name="remoteAddress"></param>
		/// <param name="userName"></param>
		/// <param name="password"></param>
		public PrototypeFaultSafeServiceReferenceClient( string endpointConfigurationName, EndpointAddress remoteAddress, string userName, string password ) {
			this._binding					= null;
			this._endpointConfigurationName	= endpointConfigurationName;
			this._remoteAddress				= remoteAddress;
			this._userName					= userName;
			this._password					= new SecureString();
			foreach ( char c in password ) {
				this._password.AppendChar( c );
			}
			this._password.MakeReadOnly();
		}

		/// <summary>
		///     The constructor with binding remote address configured.
		/// </summary>
		/// <param name="binding"></param>
		/// <param name="remoteAddress"></param>
		/// <param name="userName"></param>
		/// <param name="password"></param>
		public PrototypeFaultSafeServiceReferenceClient( Binding binding, EndpointAddress remoteAddress, string userName, string password ) {
			this._binding					= binding;
			this._endpointConfigurationName	= null;
			this._remoteAddress				= remoteAddress;
			this._userName					= userName;
			this._password					= new SecureString();
			foreach ( char c in password ) {
				this._password.AppendChar( c );
			}
			this._password.MakeReadOnly();
		}

		#region Client Management

		/// <summary>
		/// Creates the client object.
		/// </summary>
		/// <returns></returns>
		private TClient CreateClient() {
			TClient client;
			// If we were given the binding and remote address...
			if ( ( null != this._binding ) && ( null != this._remoteAddress ) ) {
				// Find and use that constructor
				ConstructorInfo constructorInfo = typeof( TClient ).GetConstructor( new []{ typeof( Binding ), typeof( EndpointAddress ) } );
				if ( null != constructorInfo ) {
					client = (TClient)constructorInfo.Invoke( new object[]{ this._binding, this._remoteAddress } );
				} else {
					throw new Exception( String.Format( "Could not find constructor of the format {0}( Binding, EndpointAddress )", typeof( TClient ).Name ) );
				}
			} 
			// If we were given the endpoint and remote address...
			else if ( ( null != this._endpointConfigurationName ) && ( null != this._remoteAddress ) ) {
				// Find and use that constructor
				ConstructorInfo constructorInfo = typeof( TClient ).GetConstructor( new []{ typeof( string ), typeof( EndpointAddress ) } );
				if ( null != constructorInfo ) {
					client = (TClient)constructorInfo.Invoke( new object[]{ this._endpointConfigurationName, this._remoteAddress } );
				} else {
					throw new Exception( String.Format( "Could not find constructor of the format {0}( string, EndpointAddress )", typeof( TClient ).Name ) );
				}
			} 
			// If we were given the endpoint alone...
			else if ( null != this._endpointConfigurationName )  {
				// Find and use that constructor
				ConstructorInfo constructorInfo = typeof( TClient ).GetConstructor( new []{ typeof( string ) } );
				if ( null != constructorInfo ) {
					client = (TClient)constructorInfo.Invoke( new object[]{ this._endpointConfigurationName } );
				} else {
					throw new Exception( String.Format( "Could not find constructor of the format {0}( string )", typeof( TClient ).Name ) );
				}
			} 
			// If we were given nothing...
			else {
				// Find and use that constructor
				ConstructorInfo constructorInfo = typeof( TClient ).GetConstructor( new Type[0] );
				if ( null != constructorInfo ) {
					client = (TClient)constructorInfo.Invoke( new object[0] );
				} else {
					throw new Exception( String.Format( "Could not find constructor of the format {0}()", typeof( TClient ).Name ) );
				}				
			}
			// If we were given username and password...
			if ( ( null != this._userName ) && ( null != this._password ) ) {
				if ( null != client.ClientCredentials ) {
					client.ClientCredentials.Windows.ClientCredential = new NetworkCredential( this._userName, this._password );
				} else {
					throw new Exception( String.Format( "{0} client has no ClientCredentials", typeof( TClient ).Name ) );
				}
			}
			return client;
		}

		/// <summary>
		/// Returns our client object.
		/// </summary>
		public IMockContract GetClient() {
			// If the client is null (because we've never created it or it faulted and we removed it)....
			if ( null == this._client ) {
				this._client = CreateClient();
			}
			return this._client;
		}

		#endregion

		#region Client Management

		/// <summary>
		/// Aborts the service client in the case of exception.
		/// </summary>
		public void Abort() {
			if ( null == this._client ) return;
			this._client.Abort();
			this._client = null;
		}

		/// <summary>
		/// Closes the client.
		/// </summary>
		public void Close() {
			if ( null == this._client ) return;
			this._client.Close();
			this._client = null;
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

		#region TContract
		

		public int AddInt( int a, int b ) {
			try {
				return this.GetClient().AddInt( a, b );
			} catch ( Exception ) {
				this.Abort();
				throw;
			}
		}

		public Task<int> AddIntAsync( int a, int b ) {
			try {
				return this.GetClient().AddIntAsync( a, b );
			} catch ( Exception ) {
				this.Abort();
				throw;
			}
		}

		public double AddDouble( double a, double b ) {
			try {
				return this.GetClient().AddDouble( a, b );
			} catch ( Exception ) {
				this.Abort();
				throw;
			}
		}

		public Task<double> AddDoubleAsync( double a, double b ) {
			try {
				return this.GetClient().AddDoubleAsync( a, b );
			} catch ( Exception ) {
				this.Abort();
				throw;
			}
		}

		public int Subtract( int a, int b ) {
			try {
				return this.GetClient().Subtract( a, b );
			} catch ( Exception ) {
				this.Abort();
				throw;
			}
		}

		public Task<int> SubtractAsync( int a, int b ) {
			try {
				return this.GetClient().SubtractAsync( a, b );
			} catch ( Exception ) {
				this.Abort();
				throw;
			}
		}

		public int Add3( int a, int b, int c ) {
			try {
				return this.GetClient().Add3( a, b, c );
			} catch ( Exception ) {
				this.Abort();
				throw;
			}
		}

		public Task<int> Add3Async( int a, int b, int c ) {
			try {
				return this.GetClient().Add3Async( a, b, c );
			} catch ( Exception ) {
				this.Abort();
				throw;
			}
		}

		public int Add4( int a, int b, int c, int d ) {
			try {
				return this.GetClient().Add4( a, b, c, d );
			} catch ( Exception ) {
				this.Abort();
				throw;
			}
		}

		public Task<int> Add4Async( int a, int b, int c, int d ) {
			try {
				return this.GetClient().Add4Async( a, b, c, d );
			} catch ( Exception ) {
				this.Abort();
				throw;
			}
		}

		public int Add5( int a, int b, int c, int d, int e ) {
			try {
				return this.GetClient().Add5( a, b, c, d, e );
			} catch ( Exception ) {
				this.Abort();
				throw;
			}
		}

		public Task<int> Add5Async( int a, int b, int c, int d, int e ) {
			try {
				return this.GetClient().Add5Async( a, b, c, d, e );
			} catch ( Exception ) {
				this.Abort();
				throw;
			}
		}

		public int Add6( int a, int b, int c, int d, int e, int f ) {
			try {
				return this.GetClient().Add6( a, b, c, d, e, f );
			} catch ( Exception ) {
				this.Abort();
				throw;
			}
		}

		public Task<int> Add6Async( int a, int b, int c, int d, int e, int f ) {
			try {
				return this.GetClient().Add6Async( a, b, c, d, e, f );
			} catch ( Exception ) {
				this.Abort();
				throw;
			}
		}

		public int Add7( int a, int b, int c, int d, int e, int f, int g ) {
			try {
				return this.GetClient().Add7( a, b, c, d, e, f, g );
			} catch ( Exception ) {
				this.Abort();
				throw;
			}
		}

		public Task<int> Add7Async( int a, int b, int c, int d, int e, int f, int g ) {
			try {
				return this.GetClient().Add7Async( a, b, c, d, e, f, g );
			} catch ( Exception ) {
				this.Abort();
				throw;
			}
		}

		public int Add8( int a, int b, int c, int d, int e, int f, int g, int h ) {
			try {
				return this.GetClient().Add8( a, b, c, d, e, f, g, h );
			} catch ( Exception ) {
				this.Abort();
				throw;
			}
		}

		public Task<int> Add8Async( int a, int b, int c, int d, int e, int f, int g, int h ) {
			try {
				return this.GetClient().Add8Async( a, b, c, d, e, f, g, h );
			} catch ( Exception ) {
				this.Abort();
				throw;
			}
		}

		public int Divide( int a, int b ) {
			try {
				return this.GetClient().Divide( a, b );
			} catch ( Exception ) {
				this.Abort();
				throw;
			}
		}

		public Task<int> DivideAsync( int a, int b ) {
			try {
				return this.GetClient().DivideAsync( a, b );
			} catch ( Exception ) {
				this.Abort();
				throw;
			}
		}

		public List<MockObject> GetCollection( int noElements ) {
			try {
				return this.GetClient().GetCollection( noElements );
			} catch ( Exception ) {
				this.Abort();
				throw;
			}
		}

		public Task<List<MockObject>> GetCollectionAsync( int noElements ) {
			try {
				return this.GetClient().GetCollectionAsync( noElements );
			} catch ( Exception ) {
				this.Abort();
				throw;
			}
		}

		public string MultipleParameterTypes( string a, object b, DateTime c, TimeSpan d, double? e, int? f ) {
			try {
				return this.GetClient().MultipleParameterTypes( a, b, c, d, e, f );
			} catch ( Exception ) {
				this.Abort();
				throw;
			}
		}

		public Task<string> MultipleParameterTypesAsync( string a, object b, DateTime c, TimeSpan d, double? e, int? f ) {
			try {
				return this.GetClient().MultipleParameterTypesAsync( a, b, c, d, e, f );
			} catch ( Exception ) {
				this.Abort();
				throw;
			}
		}

		public int Multiply( int a, int b ) {
			try {
				return this.GetClient().Multiply( a, b );
			} catch ( Exception ) {
				this.Abort();
				throw;
			}
		}

		public Task<int> MultiplyAsync( int a, int b ) {
			try {
				return this.GetClient().MultiplyAsync( a, b );
			} catch ( Exception ) {
				this.Abort();
				throw;
			}
		}

		public string NoParameter() {
			try {
				return this.GetClient().NoParameter();
			} catch ( Exception ) {
				this.Abort();
				throw;
			}
		}

		public Task<string> NoParameterAsync() {
			try {
				return this.GetClient().NoParameterAsync();
			} catch ( Exception ) {
				this.Abort();
				throw;
			}
		}

		public void NoParameterNoReturn() {
			try {
				this.GetClient().NoParameterNoReturn();
			} catch ( Exception ) {
				this.Abort();
				throw;
			}
		}

		public Task NoParameterNoReturnAsync() {
			try {
				return this.GetClient().NoParameterNoReturnAsync();
			} catch ( Exception ) {
				this.Abort();
				throw;
			}
		}

		public void NoReturn( string a ) {
			try {
				this.GetClient().NoReturn( a );
			} catch ( Exception ) {
				this.Abort();
				throw;
			}
		}

		public Task NoReturnAsync( string a ) {
			try {
				return this.GetClient().NoReturnAsync( a );
			} catch ( Exception ) {
				this.Abort();
				throw;
			}
		}

		public void ThrowException() {
			try {
				this.GetClient().ThrowException();
			} catch ( Exception ) {
				this.Abort();
				throw;
			}
		}

		public Task ThrowExceptionAsync() {
			try {
				return this.GetClient().ThrowExceptionAsync( );
			} catch ( Exception ) {
				this.Abort();
				throw;
			}
		}


		#endregion
	}

}
