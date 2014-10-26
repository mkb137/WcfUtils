using System;
using System.Net;
using System.Reflection;
using System.Reflection.Emit;
using System.Security;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Security;
using log4net;

namespace Entropa.WcfUtils {

	/// <summary>
	///     This emits a fault-safe proxy for a given service reference-generated client.
	/// </summary>
	/// <typeparam name="TInterface">The service contract interface type.</typeparam>
	/// <typeparam name="TClient">The type of the auto-generated client.</typeparam>
	public sealed class FaultSafeServiceReferenceClientEmitter<TInterface, TClient> : FaultSafeEmitterBase<TInterface>
		where TInterface : class
		where TClient : ClientBase<TInterface> {

	
		/// <summary>
		/// The logger.
		/// </summary>
		// ReSharper disable once StaticFieldInGenericType
		private readonly static ILog	_log					= LogManager.GetLogger( "Entropa.WcfUtils.FaultSafeServiceReferenceClientEmitter" );
		
		/// <summary>
		///     The construtor.
		/// </summary>
		private FaultSafeServiceReferenceClientEmitter() {
			// The private constructor prevents calling this class via anything but the Create method.
		}

		#region Create-related Methods

		/// <summary>
		///     Creates a proxy to the service client of type <typeparamref name="TClient" />.
		/// </summary>
		/// <returns></returns>
		public static TInterface Create() {
			FaultSafeServiceReferenceClientEmitter<TInterface, TClient> emitter = new FaultSafeServiceReferenceClientEmitter<TInterface, TClient>();
			return emitter.CreateProxy();
		}

		/// <summary>
		///     The constructor with endpoint configured.
		/// </summary>
		/// <param name="endpointConfigurationName"></param>
		public static TInterface Create( string endpointConfigurationName ) {
			FaultSafeServiceReferenceClientEmitter<TInterface, TClient> emitter = new FaultSafeServiceReferenceClientEmitter<TInterface, TClient>();
			return emitter.CreateProxy( endpointConfigurationName );
		}

		/// <summary>
		///     The constructor with endpoint and remove address configured.
		/// </summary>
		/// <param name="endpointConfigurationName"></param>
		/// <param name="remoteAddress"></param>
		public static TInterface Create( string endpointConfigurationName, EndpointAddress remoteAddress ) {
			FaultSafeServiceReferenceClientEmitter<TInterface, TClient> emitter = new FaultSafeServiceReferenceClientEmitter<TInterface, TClient>();
			return emitter.CreateProxy( endpointConfigurationName, remoteAddress );
		}

		/// <summary>
		///     The constructor with binding remote address configured.
		/// </summary>
		/// <param name="binding"></param>
		/// <param name="remoteAddress"></param>
		public static TInterface Create( Binding binding, EndpointAddress remoteAddress ) {
			FaultSafeServiceReferenceClientEmitter<TInterface, TClient> emitter = new FaultSafeServiceReferenceClientEmitter<TInterface, TClient>();
			return emitter.CreateProxy( binding, remoteAddress );
		}

		/// <summary>
		///     The constructor.
		/// </summary>
		/// <param name="userName"></param>
		/// <param name="password"></param>
		public static TInterface Create( string userName, string password ) {
			FaultSafeServiceReferenceClientEmitter<TInterface, TClient> emitter = new FaultSafeServiceReferenceClientEmitter<TInterface, TClient>();
			return emitter.CreateProxy( userName, password );
		}

		/// <summary>
		///     The constructor with endpoint configured.
		/// </summary>
		/// <param name="endpointConfigurationName"></param>
		/// <param name="userName"></param>
		/// <param name="password"></param>
		public static TInterface Create( string endpointConfigurationName, string userName, string password ) {
			FaultSafeServiceReferenceClientEmitter<TInterface, TClient> emitter = new FaultSafeServiceReferenceClientEmitter<TInterface, TClient>();
			return emitter.CreateProxy( endpointConfigurationName, userName, password );
		}

		/// <summary>
		///     The constructor with endpoint and remove address configured.
		/// </summary>
		/// <param name="endpointConfigurationName"></param>
		/// <param name="remoteAddress"></param>
		/// <param name="userName"></param>
		/// <param name="password"></param>
		public static TInterface Create( string endpointConfigurationName, EndpointAddress remoteAddress, string userName, string password ) {
			FaultSafeServiceReferenceClientEmitter<TInterface, TClient> emitter = new FaultSafeServiceReferenceClientEmitter<TInterface, TClient>();
			return emitter.CreateProxy( endpointConfigurationName, remoteAddress, userName, password );
		}

		/// <summary>
		///     The constructor with binding remote address configured.
		/// </summary>
		/// <param name="binding"></param>
		/// <param name="remoteAddress"></param>
		/// <param name="userName"></param>
		/// <param name="password"></param>
		public static TInterface Create( Binding binding, EndpointAddress remoteAddress, string userName, string password ) {
			FaultSafeServiceReferenceClientEmitter<TInterface, TClient> emitter = new FaultSafeServiceReferenceClientEmitter<TInterface, TClient>();
			return emitter.CreateProxy( remoteAddress, userName, password );
		}

		/// <summary>
		///     Creates a proxy using the given parameters in the client constructor.
		/// </summary>
		/// <param name="endpointConfigurationName"></param>
		/// <returns></returns>
		private TInterface CreateProxy( string endpointConfigurationName ) {
			// Create the type
			Type generatedType = this.CreateProxyType();
			// Create an instance of the type using our given constructor parameters
			object instance = Activator.CreateInstance( generatedType, endpointConfigurationName );
			// Return the created instance, cast to the interface type for convenience.
			return instance as TInterface;
		}

		/// <summary>
		///     Creates a proxy using the given parameters in the client constructor.
		/// </summary>
		/// <param name="endpointConfigurationName"></param>
		/// <param name="remoteAddress"></param>
		/// <returns></returns>
		private TInterface CreateProxy( string endpointConfigurationName, EndpointAddress remoteAddress ) {
			// Create the type
			Type generatedType = this.CreateProxyType();
			// Create an instance of the type using our given constructor parameters
			object instance = Activator.CreateInstance( generatedType, endpointConfigurationName, remoteAddress );
			// Return the created instance, cast to the interface type for convenience.
			return instance as TInterface;
		}

		/// <summary>
		///     Creates a proxy using the given parameters in the client constructor.
		/// </summary>
		/// <param name="binding"></param>
		/// <param name="remoteAddress"></param>
		/// <returns></returns>
		private TInterface CreateProxy( Binding binding, EndpointAddress remoteAddress ) {
			// Create the type
			Type generatedType = this.CreateProxyType();
			// Create an instance of the type using our given constructor parameters
			object instance = Activator.CreateInstance( generatedType, binding, remoteAddress );
			// Return the created instance, cast to the interface type for convenience.
			return instance as TInterface;
		}

		/// <summary>
		///     Creates a proxy using the given parameters in the client constructor.
		/// </summary>
		/// <param name="userName"></param>
		/// <param name="password"></param>
		/// <returns></returns>
		private TInterface CreateProxy( string userName, string password ) {
			// Create the type
			Type generatedType = this.CreateProxyType();
			// Create an instance of the type using our given constructor parameters
			object instance = Activator.CreateInstance( generatedType, userName, password );
			// Return the created instance, cast to the interface type for convenience.
			return instance as TInterface;
		}

		/// <summary>
		///     Creates a proxy using the given parameters in the client constructor.
		/// </summary>
		/// <param name="endpointConfigurationName"></param>
		/// <param name="userName"></param>
		/// <param name="password"></param>
		/// <returns></returns>
		private TInterface CreateProxy( string endpointConfigurationName, string userName, string password ) {
			// Create the type
			Type generatedType = this.CreateProxyType();
			// Create an instance of the type using our given constructor parameters
			object instance = Activator.CreateInstance( generatedType, endpointConfigurationName, userName, password );
			// Return the created instance, cast to the interface type for convenience.
			return instance as TInterface;
		}

		/// <summary>
		///     Creates a proxy using the given parameters in the client constructor.
		/// </summary>
		/// <param name="endpointConfigurationName"></param>
		/// <param name="remoteAddress"></param>
		/// <param name="userName"></param>
		/// <param name="password"></param>
		/// <returns></returns>
		private TInterface CreateProxy( string endpointConfigurationName, EndpointAddress remoteAddress, string userName, string password ) {
			// Create the type
			Type generatedType = this.CreateProxyType();
			// Create an instance of the type using our given constructor parameters
			object instance = Activator.CreateInstance( generatedType, endpointConfigurationName, remoteAddress, userName, password );
			// Return the created instance, cast to the interface type for convenience.
			return instance as TInterface;
		}

		/// <summary>
		///     Creates the proxy.
		/// </summary>
		/// <param name="remoteAddress"></param>
		/// <param name="userName"></param>
		/// <param name="password"></param>
		/// <returns></returns>
		private TInterface CreateProxy( EndpointAddress remoteAddress, string userName, string password ) {
			// Create the type
			Type generatedType = this.CreateProxyType();
			// Create an instance of the type using our given constructor parameters
			object instance = Activator.CreateInstance( generatedType, remoteAddress, userName, password );
			// Return the created instance, cast to the interface type for convenience.
			return instance as TInterface;
		}

		/// <summary>
		///     Creates the proxy.
		/// </summary>
		/// <returns></returns>
		private TInterface CreateProxy() {
			// Create the type
			Type generatedType = this.CreateProxyType();
			// Create an instance of the type using our given constructor parameters
			object instance = Activator.CreateInstance( generatedType );
			// Return the created instance, cast to the interface type for convenience.
			return instance as TInterface;
		}

		#endregion

		#region Proxy Generation Methods

		/// <summary>
		/// The binding field, a Binding.
		/// </summary>
		private FieldBuilder	_bindingField;
		/// <summary>
		/// The client field of our <typeparamref name="TClient"/> type.
		/// </summary>
		private FieldBuilder	 _clientField;
		/// <summary>
		/// The endpoint configuration name field, a string.
		/// </summary>
		private FieldBuilder	_endpointConfigurationNameField;
		/// <summary>
		/// The password field, a SecureString.
		/// </summary>
		private FieldBuilder	_passwordField;
		/// <summary>
		/// The remote address field, an EndpointAddress.
		/// </summary>
		private FieldBuilder	 _remoteAddressField;
		/// <summary>
		/// The username field, a string.
		/// </summary>
		private FieldBuilder	_userNameField;
		/// <summary>
		/// The Abort method.
		/// </summary>
		private  MethodBuilder	_abortMethod;
		/// <summary>
		/// The Close method.
		/// </summary>
		private  MethodBuilder	_closeMethod;
		/// <summary>
		/// The CreateClient method.
		/// </summary>
		private  MethodBuilder	_createClientMethod;
		/// <summary>
		/// The GetClient method.
		/// </summary>
		private  MethodBuilder	_getClientMethod;

		/// <summary>
		/// <see cref="FaultSafeEmitterBase{TInterface}.AssemblyName"/>
		/// </summary>
		protected override string AssemblyName {
			get { return "FaultSafeServiceReferenceClientAssembly"; }
		}

		/// <summary>
		/// Builds the ( string userName, string password ) constructor.
		/// </summary>
		/// <param name="typeBuilder"></param>
		private void BuildConstructor_Auth( TypeBuilder typeBuilder ) {
			ILGenerator il = GetConstructorGenerator( typeBuilder, typeof( String ), typeof( String ) );
			DeclareLocals( il, typeof( char ), typeof( string ), typeof( int ), typeof( bool ) );
			CallBaseConstructor( il );
			InitializeReadonlyField( il, this._bindingField );
			InitializeReadonlyField( il, this._endpointConfigurationNameField );
			InitializeReadonlyField( il, this._remoteAddressField );
			InitializeReadonlyField( il, this._userNameField, OpCodes.Ldarg_1 );
			this.InitializeReadonlySecureStringField( il, this._passwordField, OpCodes.Ldarg_2 );
			EndConstructor( il );
		}

		/// <summary>
		/// Builds the ( Binding binding, EndpointAddress remoteAddress ) constructor.
		/// </summary>
		/// <param name="typeBuilder"></param>
		private void BuildConstructor_BindingRemoteAddress( TypeBuilder typeBuilder ) {
			ILGenerator il = GetConstructorGenerator( typeBuilder, typeof( Binding ), typeof( EndpointAddress ) );
			CallBaseConstructor( il );
			InitializeReadonlyField( il, this._bindingField, OpCodes.Ldarg_1 );
			InitializeReadonlyField( il, this._endpointConfigurationNameField );
			InitializeReadonlyField( il, this._remoteAddressField, OpCodes.Ldarg_2 );
			InitializeReadonlyField( il, this._userNameField );
			InitializeReadonlyField( il, this._passwordField );
			EndConstructor( il );
		}

		/// <summary>
		/// Builds the ( Binding binding, EndpointAddress remoteAddress, string userName, string password ) constructor.
		/// </summary>
		/// <param name="typeBuilder"></param>
		private void BuildConstructor_BindingRemoteAddress_Auth( TypeBuilder typeBuilder ) {
			ILGenerator il = GetConstructorGenerator( typeBuilder, typeof( Binding ), typeof( EndpointAddress ), typeof( String ), typeof( String ) );
			DeclareLocals( il, typeof( char ), typeof( string ), typeof( int ), typeof( bool ) );
			CallBaseConstructor( il );
			InitializeReadonlyField( il, this._bindingField, OpCodes.Ldarg_1 );
			InitializeReadonlyField( il, this._endpointConfigurationNameField );
			InitializeReadonlyField( il, this._remoteAddressField, OpCodes.Ldarg_2 );
			InitializeReadonlyField( il, this._userNameField, OpCodes.Ldarg_3 );
			this.InitializeReadonlySecureStringField( il, this._passwordField, OpCodes.Ldarg_S, "password" );
			EndConstructor( il );
		}

		/// <summary>
		/// Builds the default () constructor.
		/// </summary>
		/// <remarks><![CDATA[
		/// .method public hidebysig specialname rtspecialname 
		///       instance void  .ctor() cil managed
		/// {
		///   // Code size       45 (0x2d)
		///   .maxstack  8
		///   IL_0000:  ldarg.0
		///   IL_0001:  call       instance void [mscorlib]System.Object::.ctor()
		///   IL_0006:  nop
		///   IL_0007:  nop
		///   IL_0008:  ldarg.0
		///   IL_0009:  ldnull
		///   IL_000a:  stfld      class [System.ServiceModel]System.ServiceModel.Channels.Binding class Entropa.WcfUtils.Test.Prototypes.PrototypeFaultSafeServiceReferenceClient`1<!TClient>::_binding
		///   IL_000f:  ldarg.0
		///   IL_0010:  ldnull
		///   IL_0011:  stfld      string class Entropa.WcfUtils.Test.Prototypes.PrototypeFaultSafeServiceReferenceClient`1<!TClient>::_endpointConfigurationName
		///   IL_0016:  ldarg.0
		///   IL_0017:  ldnull
		///   IL_0018:  stfld      class [System.ServiceModel]System.ServiceModel.EndpointAddress class Entropa.WcfUtils.Test.Prototypes.PrototypeFaultSafeServiceReferenceClient`1<!TClient>::_remoteAddress
		///   IL_001d:  ldarg.0
		///   IL_001e:  ldnull
		///   IL_001f:  stfld      string class Entropa.WcfUtils.Test.Prototypes.PrototypeFaultSafeServiceReferenceClient`1<!TClient>::_userName
		///   IL_0024:  ldarg.0
		///   IL_0025:  ldnull
		///   IL_0026:  stfld      class [mscorlib]System.Security.SecureString class Entropa.WcfUtils.Test.Prototypes.PrototypeFaultSafeServiceReferenceClient`1<!TClient>::_password
		///   IL_002b:  nop
		///   IL_002c:  ret
		/// } // end of method PrototypeFaultSafeServiceReferenceClient`1::.ctor
		/// 
		/// ]]></remarks>
		/// <param name="typeBuilder"></param>
		private void BuildConstructor_Default( TypeBuilder typeBuilder ) {
			ILGenerator il = GetConstructorGenerator( typeBuilder );
			CallBaseConstructor( il );
			InitializeReadonlyField( il, this._bindingField );
			InitializeReadonlyField( il, this._endpointConfigurationNameField );
			InitializeReadonlyField( il, this._remoteAddressField );
			InitializeReadonlyField( il, this._userNameField );
			InitializeReadonlyField( il, this._passwordField );
			EndConstructor( il );
		}

		/// <summary>
		/// Builds the ( string endpointConfigurationName ) constructor.
		/// </summary>
		/// <param name="typeBuilder"></param>
		private void BuildConstructor_Endpoint( TypeBuilder typeBuilder ) {
			ILGenerator il = GetConstructorGenerator( typeBuilder, typeof( String ) );
			CallBaseConstructor( il );
			InitializeReadonlyField( il, this._bindingField );
			InitializeReadonlyField( il, this._endpointConfigurationNameField, OpCodes.Ldarg_1 );
			InitializeReadonlyField( il, this._remoteAddressField );
			InitializeReadonlyField( il, this._userNameField );
			InitializeReadonlyField( il, this._passwordField );
			EndConstructor( il );
		}

		/// <summary>
		/// Builds the ( string endpointConfigurationName, EndpointAddress remoteAddress ) constructor.
		/// </summary>
		/// <param name="typeBuilder"></param>
		private void BuildConstructor_EndpointRemoteAddress( TypeBuilder typeBuilder ) {
			ILGenerator il = GetConstructorGenerator( typeBuilder, typeof( String ), typeof( EndpointAddress ) );
			CallBaseConstructor( il );
			InitializeReadonlyField( il, this._bindingField );
			InitializeReadonlyField( il, this._endpointConfigurationNameField, OpCodes.Ldarg_1 );
			InitializeReadonlyField( il, this._remoteAddressField, OpCodes.Ldarg_2 );
			InitializeReadonlyField( il, this._userNameField );
			InitializeReadonlyField( il, this._passwordField );
			EndConstructor( il );
		}

		/// <summary>
		/// Builds the ( string endpointConfigurationName, EndpointAddress remoteAddress, string userName, string password )  constructor.
		/// </summary>
		/// <param name="typeBuilder"></param>
		private void BuildConstructor_EndpointRemoteAddress_Auth( TypeBuilder typeBuilder ) {
			ILGenerator il = GetConstructorGenerator( typeBuilder, typeof( String ), typeof( EndpointAddress ), typeof( String ), typeof( String ) );
			DeclareLocals( il, typeof( char ), typeof( string ), typeof( int ), typeof( bool ) );
			CallBaseConstructor( il );
			InitializeReadonlyField( il, this._bindingField );
			InitializeReadonlyField( il, this._endpointConfigurationNameField, OpCodes.Ldarg_1 );
			InitializeReadonlyField( il, this._remoteAddressField, OpCodes.Ldarg_2 );
			InitializeReadonlyField( il, this._userNameField, OpCodes.Ldarg_3 );
			this.InitializeReadonlySecureStringField( il, this._passwordField, OpCodes.Ldarg_S, "password" );
			EndConstructor( il );
		}

		/// <summary>
		/// Builds the ( string endpointConfigurationName, string userName, string password ) constructor.
		/// </summary>
		/// <param name="typeBuilder"></param>
		private void BuildConstructor_Endpoint_Auth( TypeBuilder typeBuilder ) {
			ILGenerator il = GetConstructorGenerator( typeBuilder, typeof( String ), typeof( String ), typeof( String ) );
			DeclareLocals( il, typeof( char ), typeof( string ), typeof( int ), typeof( bool ) );
			CallBaseConstructor( il );
			InitializeReadonlyField( il, this._bindingField );
			InitializeReadonlyField( il, this._endpointConfigurationNameField, OpCodes.Ldarg_1 );
			InitializeReadonlyField( il, this._remoteAddressField );
			InitializeReadonlyField( il, this._userNameField, OpCodes.Ldarg_2 );
			this.InitializeReadonlySecureStringField( il, this._passwordField, OpCodes.Ldarg_3 );
			EndConstructor( il );
		}

		/// <summary>
		/// Builds the constructors on the type.
		/// </summary>
		/// <param name="typeBuilder"></param>
		private void BuildConstructors( TypeBuilder typeBuilder ) {
			_log.DebugFormat( "BuildConstructors" );
			this.BuildConstructor_Default( typeBuilder );
			this.BuildConstructor_Endpoint( typeBuilder );
			this.BuildConstructor_EndpointRemoteAddress( typeBuilder );
			this.BuildConstructor_BindingRemoteAddress( typeBuilder );
			this.BuildConstructor_Auth( typeBuilder );
			this.BuildConstructor_Endpoint_Auth( typeBuilder );
			this.BuildConstructor_EndpointRemoteAddress_Auth( typeBuilder );
			this.BuildConstructor_BindingRemoteAddress_Auth( typeBuilder );
		}

		/// <summary>
		/// Builds the fields on the type.
		/// </summary>
		/// <param name="typeBuilder"></param>
		private void BuildFields( TypeBuilder typeBuilder ) {
			this._clientField = typeBuilder.DefineField(
				"_client",
				typeof( TClient ),
				FieldAttributes.Private
				);
			this._bindingField = typeBuilder.DefineField(
				"_binding",
				typeof( Binding ),
				FieldAttributes.Private | FieldAttributes.InitOnly
				);
			this._endpointConfigurationNameField = typeBuilder.DefineField(
				"_endpointConfigurationName",
				typeof( string ),
				FieldAttributes.Private | FieldAttributes.InitOnly
				);
			this._passwordField = typeBuilder.DefineField(
				"_password",
				typeof( SecureString ),
				FieldAttributes.Private | FieldAttributes.InitOnly
				);
			this._remoteAddressField = typeBuilder.DefineField(
				"_remoteAddress",
				typeof( EndpointAddress ),
				FieldAttributes.Private | FieldAttributes.InitOnly
				);
			this._userNameField = typeBuilder.DefineField(
				"_userName",
				typeof( string ),
				FieldAttributes.Private | FieldAttributes.InitOnly
				);			
		}

		/// <summary>
		/// This builds our proxy type.
		/// </summary>
		/// <param name="typeBuilder"></param>
		private void BuildType( TypeBuilder typeBuilder ) {
			_log.DebugFormat( "BuildType" );
			this.BuildFields( typeBuilder );
			this.BuildConstructors( typeBuilder );
			this.BuildClientControlMethods( typeBuilder );
			// TODO
		}

		/// <summary>
		/// Builds the client control methods Abort, Close, CreateClient(), and GetClient()
		/// </summary>
		/// <param name="typeBuilder"></param>
		private void BuildClientControlMethods( TypeBuilder typeBuilder ) {
			this.BuildAbortMethod( typeBuilder );
			this.BuildCloseMethod( typeBuilder );
			this.BuildCreateClientMethod( typeBuilder );
			this.BuildGetClientMethod( typeBuilder );
		}

		/// <summary>
		/// Builds the "Abort" method.
		/// </summary>
		/// <remarks><![CDATA[
		///.method private hidebysig instance void  Abort() cil managed
		///{
		///  // Code size       39 (0x27)
		///  .maxstack  2
		///  .locals init (bool V_0)
		///  IL_0000:  nop
		///  IL_0001:  ldnull
		///  IL_0002:  ldarg.0
		///  IL_0003:  ldfld      class Entropa.WcfUtils.Test.MockServiceReference.MockContractClient Entropa.WcfUtils.Test.Prototypes.PrototypeFaultSafeServiceReferenceClient::_client
		///  IL_0008:  ceq
		///  IL_000a:  ldc.i4.0
		///  IL_000b:  ceq
		///  IL_000d:  stloc.0
		///  IL_000e:  ldloc.0
		///  IL_000f:  brtrue.s   IL_0013
		///  IL_0011:  br.s       IL_0026
		///  IL_0013:  ldarg.0
		///  IL_0014:  ldfld      class Entropa.WcfUtils.Test.MockServiceReference.MockContractClient Entropa.WcfUtils.Test.Prototypes.PrototypeFaultSafeServiceReferenceClient::_client
		///  IL_0019:  callvirt   instance void class [System.ServiceModel]System.ServiceModel.ClientBase`1<class Entropa.WcfUtils.Test.MockServiceReference.IMockContract>::Abort()
		///  IL_001e:  nop
		///  IL_001f:  ldarg.0
		///  IL_0020:  ldnull
		///  IL_0021:  stfld      class Entropa.WcfUtils.Test.MockServiceReference.MockContractClient Entropa.WcfUtils.Test.Prototypes.PrototypeFaultSafeServiceReferenceClient::_client
		///  IL_0026:  ret
		///} // end of method PrototypeFaultSafeServiceReferenceClient::Abort
		///
		/// ]]></remarks>
		/// <param name="typeBuilder"></param>
		private void BuildAbortMethod( TypeBuilder typeBuilder ) {
			this._abortMethod = typeBuilder.DefineMethod(
				"Abort",
				MethodAttributes.Private | MethodAttributes.HideBySig,
				CallingConventions.Standard,
				null,
				new Type[0]
				);
			ILGenerator il = this._abortMethod.GetILGenerator();

			MethodInfo clientAbortMethodInfo = typeof( TClient ).GetMethod( "Abort" );
			if ( null == clientAbortMethodInfo ) throw new InvalidOperationException( String.Format( "Type '{0}' has no Abort method.", typeof( TClient ) ) );

			// Define some labels ahead of time
			Label label0013 = il.DefineLabel();
			Label label0026 = il.DefineLabel();

			//  .locals init (bool V_0)
			il.DeclareLocal( typeof( bool ) );

			//  IL_0000:  nop
			//  IL_0001:  ldnull
			//  IL_0002:  ldarg.0
			//  IL_0003:  ldfld      class Entropa.WcfUtils.Test.MockServiceReference.MockContractClient Entropa.WcfUtils.Test.Prototypes.PrototypeFaultSafeServiceReferenceClient::_client
			//  IL_0008:  ceq
			il.Emit( OpCodes.Nop );
			il.Emit( OpCodes.Ldnull );
			il.Emit( OpCodes.Ldarg_0 );
			il.Emit( OpCodes.Ldfld, this._clientField );
			il.Emit( OpCodes.Ceq );

			//  IL_000a:  ldc.i4.0
			//  IL_000b:  ceq
			//  IL_000d:  stloc.0
			il.Emit( OpCodes.Ldc_I4_0 );
			il.Emit( OpCodes.Ceq );
			il.Emit( OpCodes.Stloc_0 );

			//  IL_000e:  ldloc.0
			//  IL_000f:  brtrue.s   IL_0013
			//  IL_0011:  br.s       IL_0026
			il.Emit( OpCodes.Ldloc_0 );
			il.Emit( OpCodes.Brtrue_S, label0013 );
			il.Emit( OpCodes.Br_S, label0026 );

			//  IL_0013:  ldarg.0
			//  IL_0014:  ldfld      class Entropa.WcfUtils.Test.MockServiceReference.MockContractClient Entropa.WcfUtils.Test.Prototypes.PrototypeFaultSafeServiceReferenceClient::_client
			//  IL_0019:  callvirt   instance void class [System.ServiceModel]System.ServiceModel.ClientBase`1<class Entropa.WcfUtils.Test.MockServiceReference.IMockContract>::Abort()
			il.MarkLabel( label0013 );
			il.Emit( OpCodes.Ldarg_0 );
			il.Emit( OpCodes.Ldflda, this._clientField );
			il.Emit( OpCodes.Callvirt, clientAbortMethodInfo );

			//  IL_001e:  nop
			//  IL_001f:  ldarg.0
			//  IL_0020:  ldnull
			//  IL_0021:  stfld      class Entropa.WcfUtils.Test.MockServiceReference.MockContractClient Entropa.WcfUtils.Test.Prototypes.PrototypeFaultSafeServiceReferenceClient::_client
			il.Emit( OpCodes.Nop );
			il.Emit( OpCodes.Ldarg_0 );
			il.Emit( OpCodes.Ldnull );
			il.Emit( OpCodes.Stfld, this._clientField );

			//  IL_0026:  ret
			il.MarkLabel( label0026 );
			il.Emit( OpCodes.Ret );
		}


		/// <summary>
		/// Builds the "Close" method.
		/// </summary>
		/// <remarks><![CDATA[
		///.method private hidebysig instance void  Close() cil managed
		///{
		///  // Code size       39 (0x27)
		///  .maxstack  2
		///  .locals init (bool V_0)
		///  IL_0000:  nop
		///  IL_0001:  ldnull
		///  IL_0002:  ldarg.0
		///  IL_0003:  ldfld      class Entropa.WcfUtils.Test.MockServiceReference.MockContractClient Entropa.WcfUtils.Test.Prototypes.PrototypeFaultSafeServiceReferenceClient::_client
		///  IL_0008:  ceq
		///  IL_000a:  ldc.i4.0
		///  IL_000b:  ceq
		///  IL_000d:  stloc.0
		///  IL_000e:  ldloc.0
		///  IL_000f:  brtrue.s   IL_0013
		///  IL_0011:  br.s       IL_0026
		///  IL_0013:  ldarg.0
		///  IL_0014:  ldfld      class Entropa.WcfUtils.Test.MockServiceReference.MockContractClient Entropa.WcfUtils.Test.Prototypes.PrototypeFaultSafeServiceReferenceClient::_client
		///  IL_0019:  callvirt   instance void class [System.ServiceModel]System.ServiceModel.ClientBase`1<class Entropa.WcfUtils.Test.MockServiceReference.IMockContract>::Close()
		///  IL_001e:  nop
		///  IL_001f:  ldarg.0
		///  IL_0020:  ldnull
		///  IL_0021:  stfld      class Entropa.WcfUtils.Test.MockServiceReference.MockContractClient Entropa.WcfUtils.Test.Prototypes.PrototypeFaultSafeServiceReferenceClient::_client
		///  IL_0026:  ret
		///} // end of method PrototypeFaultSafeServiceReferenceClient::Close
		/// 
		/// ]]></remarks>
		/// <param name="typeBuilder"></param>
		private void BuildCloseMethod( TypeBuilder typeBuilder ) {
			this._closeMethod = typeBuilder.DefineMethod(
				"Close",
				MethodAttributes.Private | MethodAttributes.HideBySig,
				CallingConventions.Standard,
				null,
				new Type[0]
				);
			ILGenerator il = this._closeMethod.GetILGenerator();

			MethodInfo clientCloseMethodInfo = typeof( TClient ).GetMethod( "Close" );
			if ( null == clientCloseMethodInfo ) throw new InvalidOperationException( String.Format( "Type '{0}' has no Close method.", typeof( TClient ) ) );

			// Define some labels ahead of time
			Label label0013 = il.DefineLabel();
			Label label0026 = il.DefineLabel();

			//  .locals init (bool V_0)
			il.DeclareLocal( typeof( bool ) );

			//  IL_0000:  nop
			//  IL_0001:  ldnull
			//  IL_0002:  ldarg.0
			//  IL_0003:  ldfld      class Entropa.WcfUtils.Test.MockServiceReference.MockContractClient Entropa.WcfUtils.Test.Prototypes.PrototypeFaultSafeServiceReferenceClient::_client
			//  IL_0008:  ceq
			il.Emit( OpCodes.Nop );
			il.Emit( OpCodes.Ldnull );
			il.Emit( OpCodes.Ldarg_0 );
			il.Emit( OpCodes.Ldfld, this._clientField );
			il.Emit( OpCodes.Ceq );

			//  IL_000a:  ldc.i4.0
			//  IL_000b:  ceq
			//  IL_000d:  stloc.0
			il.Emit( OpCodes.Ldc_I4_0 );
			il.Emit( OpCodes.Ceq );
			il.Emit( OpCodes.Stloc_0 );

			//  IL_000e:  ldloc.0
			//  IL_000f:  brtrue.s   IL_0013
			//  IL_0011:  br.s       IL_0026
			il.Emit( OpCodes.Ldloc_0 );
			il.Emit( OpCodes.Brtrue_S, label0013 );
			il.Emit( OpCodes.Br_S, label0026 );

			//  IL_0013:  ldarg.0
			//  IL_0014:  ldfld      class Entropa.WcfUtils.Test.MockServiceReference.MockContractClient Entropa.WcfUtils.Test.Prototypes.PrototypeFaultSafeServiceReferenceClient::_client
			//  IL_0019:  callvirt   instance void class [System.ServiceModel]System.ServiceModel.ClientBase`1<class Entropa.WcfUtils.Test.MockServiceReference.IMockContract>::Close()
			il.MarkLabel( label0013 );
			il.Emit( OpCodes.Ldarg_0 );
			il.Emit( OpCodes.Ldflda, this._clientField );
			il.Emit( OpCodes.Callvirt, clientCloseMethodInfo );

			//  IL_001e:  nop
			//  IL_001f:  ldarg.0
			//  IL_0020:  ldnull
			//  IL_0021:  stfld      class Entropa.WcfUtils.Test.MockServiceReference.MockContractClient Entropa.WcfUtils.Test.Prototypes.PrototypeFaultSafeServiceReferenceClient::_client
			il.Emit( OpCodes.Nop );
			il.Emit( OpCodes.Ldarg_0 );
			il.Emit( OpCodes.Ldnull );
			il.Emit( OpCodes.Stfld, this._clientField );

			//  IL_0026:  ret
			il.MarkLabel( label0026 );
			il.Emit( OpCodes.Ret );
		}

		/// <summary>
		/// Builds the "CreateClient" method.
		/// </summary>
		/// <remarks><![CDATA[
		///.method private hidebysig instance class Entropa.WcfUtils.Test.MockServiceReference.MockContractClient 
		///        CreateClient() cil managed
		///{
		///  // Code size       349 (0x15d)
		///  .maxstack  4
		///  .locals init (class [mscorlib]System.Type V_0,
		///           object V_1,
		///           class Entropa.WcfUtils.Test.MockServiceReference.MockContractClient V_2,
		///           class Entropa.WcfUtils.Test.MockServiceReference.MockContractClient V_3,
		///           bool V_4,
		///           object[] V_5)
		///  IL_0000:  nop
		///  IL_0001:  ldtoken    Entropa.WcfUtils.Test.MockServiceReference.MockContractClient
		///  IL_0006:  call       class [mscorlib]System.Type [mscorlib]System.Type::GetTypeFromHandle(valuetype [mscorlib]System.RuntimeTypeHandle)
		///  IL_000b:  stloc.0
		///  IL_000c:  ldarg.0
		///  IL_000d:  ldfld      class [System.ServiceModel]System.ServiceModel.Channels.Binding Entropa.WcfUtils.Test.Prototypes.PrototypeFaultSafeServiceReferenceClient::_binding
		///  IL_0012:  brfalse.s  IL_0025
		///  IL_0014:  ldnull
		///  IL_0015:  ldarg.0
		///  IL_0016:  ldfld      class [System.ServiceModel]System.ServiceModel.EndpointAddress Entropa.WcfUtils.Test.Prototypes.PrototypeFaultSafeServiceReferenceClient::_remoteAddress
		///  IL_001b:  call       bool [System.ServiceModel]System.ServiceModel.EndpointAddress::op_Inequality(class [System.ServiceModel]System.ServiceModel.EndpointAddress,
		///                                                                                                    class [System.ServiceModel]System.ServiceModel.EndpointAddress)
		///  IL_0020:  ldc.i4.0
		///  IL_0021:  ceq
		///  IL_0023:  br.s       IL_0026
		///  IL_0025:  ldc.i4.1
		///  IL_0026:  nop
		///  IL_0027:  stloc.s    V_4
		///  IL_0029:  ldloc.s    V_4
		///  IL_002b:  brtrue.s   IL_0059
		///  IL_002d:  nop
		///  IL_002e:  ldloc.0
		///  IL_002f:  ldc.i4.2
		///  IL_0030:  newarr     [mscorlib]System.Object
		///  IL_0035:  stloc.s    V_5
		///  IL_0037:  ldloc.s    V_5
		///  IL_0039:  ldc.i4.0
		///  IL_003a:  ldarg.0
		///  IL_003b:  ldfld      class [System.ServiceModel]System.ServiceModel.Channels.Binding Entropa.WcfUtils.Test.Prototypes.PrototypeFaultSafeServiceReferenceClient::_binding
		///  IL_0040:  stelem.ref
		///  IL_0041:  ldloc.s    V_5
		///  IL_0043:  ldc.i4.1
		///  IL_0044:  ldarg.0
		///  IL_0045:  ldfld      class [System.ServiceModel]System.ServiceModel.EndpointAddress Entropa.WcfUtils.Test.Prototypes.PrototypeFaultSafeServiceReferenceClient::_remoteAddress
		///  IL_004a:  stelem.ref
		///  IL_004b:  ldloc.s    V_5
		///  IL_004d:  call       object [mscorlib]System.Activator::CreateInstance(class [mscorlib]System.Type,
		///                                                                         object[])
		///  IL_0052:  stloc.1
		///  IL_0053:  nop
		///  IL_0054:  br         IL_00da
		///  IL_0059:  ldarg.0
		///  IL_005a:  ldfld      string Entropa.WcfUtils.Test.Prototypes.PrototypeFaultSafeServiceReferenceClient::_endpointConfigurationName
		///  IL_005f:  brfalse.s  IL_0072
		///  IL_0061:  ldnull
		///  IL_0062:  ldarg.0
		///  IL_0063:  ldfld      class [System.ServiceModel]System.ServiceModel.EndpointAddress Entropa.WcfUtils.Test.Prototypes.PrototypeFaultSafeServiceReferenceClient::_remoteAddress
		///  IL_0068:  call       bool [System.ServiceModel]System.ServiceModel.EndpointAddress::op_Inequality(class [System.ServiceModel]System.ServiceModel.EndpointAddress,
		///                                                                                                    class [System.ServiceModel]System.ServiceModel.EndpointAddress)
		///  IL_006d:  ldc.i4.0
		///  IL_006e:  ceq
		///  IL_0070:  br.s       IL_0073
		///  IL_0072:  ldc.i4.1
		///  IL_0073:  nop
		///  IL_0074:  stloc.s    V_4
		///  IL_0076:  ldloc.s    V_4
		///  IL_0078:  brtrue.s   IL_00a3
		///  IL_007a:  nop
		///  IL_007b:  ldloc.0
		///  IL_007c:  ldc.i4.2
		///  IL_007d:  newarr     [mscorlib]System.Object
		///  IL_0082:  stloc.s    V_5
		///  IL_0084:  ldloc.s    V_5
		///  IL_0086:  ldc.i4.0
		///  IL_0087:  ldarg.0
		///  IL_0088:  ldfld      string Entropa.WcfUtils.Test.Prototypes.PrototypeFaultSafeServiceReferenceClient::_endpointConfigurationName
		///  IL_008d:  stelem.ref
		///  IL_008e:  ldloc.s    V_5
		///  IL_0090:  ldc.i4.1
		///  IL_0091:  ldarg.0
		///  IL_0092:  ldfld      class [System.ServiceModel]System.ServiceModel.EndpointAddress Entropa.WcfUtils.Test.Prototypes.PrototypeFaultSafeServiceReferenceClient::_remoteAddress
		///  IL_0097:  stelem.ref
		///  IL_0098:  ldloc.s    V_5
		///  IL_009a:  call       object [mscorlib]System.Activator::CreateInstance(class [mscorlib]System.Type,
		///                                                                         object[])
		///  IL_009f:  stloc.1
		///  IL_00a0:  nop
		///  IL_00a1:  br.s       IL_00da
		///  IL_00a3:  ldnull
		///  IL_00a4:  ldarg.0
		///  IL_00a5:  ldfld      string Entropa.WcfUtils.Test.Prototypes.PrototypeFaultSafeServiceReferenceClient::_endpointConfigurationName
		///  IL_00aa:  ceq
		///  IL_00ac:  stloc.s    V_4
		///  IL_00ae:  ldloc.s    V_4
		///  IL_00b0:  brtrue.s   IL_00d1
		///  IL_00b2:  nop
		///  IL_00b3:  ldloc.0
		///  IL_00b4:  ldc.i4.1
		///  IL_00b5:  newarr     [mscorlib]System.Object
		///  IL_00ba:  stloc.s    V_5
		///  IL_00bc:  ldloc.s    V_5
		///  IL_00be:  ldc.i4.0
		///  IL_00bf:  ldarg.0
		///  IL_00c0:  ldfld      string Entropa.WcfUtils.Test.Prototypes.PrototypeFaultSafeServiceReferenceClient::_endpointConfigurationName
		///  IL_00c5:  stelem.ref
		///  IL_00c6:  ldloc.s    V_5
		///  IL_00c8:  call       object [mscorlib]System.Activator::CreateInstance(class [mscorlib]System.Type,
		///                                                                         object[])
		///  IL_00cd:  stloc.1
		///  IL_00ce:  nop
		///  IL_00cf:  br.s       IL_00da
		///  IL_00d1:  nop
		///  IL_00d2:  ldloc.0
		///  IL_00d3:  call       object [mscorlib]System.Activator::CreateInstance(class [mscorlib]System.Type)
		///  IL_00d8:  stloc.1
		///  IL_00d9:  nop
		///  IL_00da:  ldloc.1
		///  IL_00db:  isinst     Entropa.WcfUtils.Test.MockServiceReference.MockContractClient
		///  IL_00e0:  stloc.2
		///  IL_00e1:  ldnull
		///  IL_00e2:  ldloc.2
		///  IL_00e3:  ceq
		///  IL_00e5:  ldc.i4.0
		///  IL_00e6:  ceq
		///  IL_00e8:  stloc.s    V_4
		///  IL_00ea:  ldloc.s    V_4
		///  IL_00ec:  brtrue.s   IL_00f9
		///  IL_00ee:  ldstr      "Could not create instance of MockContractClient"
		///  IL_00f3:  newobj     instance void [mscorlib]System.InvalidOperationException::.ctor(string)
		///  IL_00f8:  throw
		///  IL_00f9:  ldarg.0
		///  IL_00fa:  ldfld      string Entropa.WcfUtils.Test.Prototypes.PrototypeFaultSafeServiceReferenceClient::_userName
		///  IL_00ff:  brfalse.s  IL_010c
		///  IL_0101:  ldnull
		///  IL_0102:  ldarg.0
		///  IL_0103:  ldfld      class [mscorlib]System.Security.SecureString Entropa.WcfUtils.Test.Prototypes.PrototypeFaultSafeServiceReferenceClient::_password
		///  IL_0108:  ceq
		///  IL_010a:  br.s       IL_010d
		///  IL_010c:  ldc.i4.1
		///  IL_010d:  nop
		///  IL_010e:  stloc.s    V_4
		///  IL_0110:  ldloc.s    V_4
		///  IL_0112:  brtrue.s   IL_0157
		///  IL_0114:  nop
		///  IL_0115:  ldnull
		///  IL_0116:  ldloc.2
		///  IL_0117:  callvirt   instance class [System.ServiceModel]System.ServiceModel.Description.ClientCredentials class [System.ServiceModel]System.ServiceModel.ClientBase`1<class Entropa.WcfUtils.Test.MockServiceReference.IMockContract>::get_ClientCredentials()
		///  IL_011c:  ceq
		///  IL_011e:  stloc.s    V_4
		///  IL_0120:  ldloc.s    V_4
		///  IL_0122:  brtrue.s   IL_014a
		///  IL_0124:  nop
		///  IL_0125:  ldloc.2
		///  IL_0126:  callvirt   instance class [System.ServiceModel]System.ServiceModel.Description.ClientCredentials class [System.ServiceModel]System.ServiceModel.ClientBase`1<class Entropa.WcfUtils.Test.MockServiceReference.IMockContract>::get_ClientCredentials()
		///  IL_012b:  callvirt   instance class [System.ServiceModel]System.ServiceModel.Security.WindowsClientCredential [System.ServiceModel]System.ServiceModel.Description.ClientCredentials::get_Windows()
		///  IL_0130:  ldarg.0
		///  IL_0131:  ldfld      string Entropa.WcfUtils.Test.Prototypes.PrototypeFaultSafeServiceReferenceClient::_userName
		///  IL_0136:  ldarg.0
		///  IL_0137:  ldfld      class [mscorlib]System.Security.SecureString Entropa.WcfUtils.Test.Prototypes.PrototypeFaultSafeServiceReferenceClient::_password
		///  IL_013c:  newobj     instance void [System]System.Net.NetworkCredential::.ctor(string,
		///                                                                                 class [mscorlib]System.Security.SecureString)
		///  IL_0141:  callvirt   instance void [System.ServiceModel]System.ServiceModel.Security.WindowsClientCredential::set_ClientCredential(class [System]System.Net.NetworkCredential)
		///  IL_0146:  nop
		///  IL_0147:  nop
		///  IL_0148:  br.s       IL_0156
		///  IL_014a:  nop
		///  IL_014b:  ldstr      "MockContractClient client has no ClientCredentials"
		///  IL_0150:  newobj     instance void [mscorlib]System.InvalidOperationException::.ctor(string)
		///  IL_0155:  throw
		///  IL_0156:  nop
		///  IL_0157:  ldloc.2
		///  IL_0158:  stloc.3
		///  IL_0159:  br.s       IL_015b
		///  IL_015b:  ldloc.3
		///  IL_015c:  ret
		///} // end of method PrototypeFaultSafeServiceReferenceClient::CreateClient
		/// ]]></remarks>
		/// <param name="typeBuilder"></param>
		private void BuildCreateClientMethod( TypeBuilder typeBuilder ) {
			this._createClientMethod = typeBuilder.DefineMethod(
				"CreateClient",
				MethodAttributes.Private | MethodAttributes.HideBySig,
				CallingConventions.Standard,
				typeof( TInterface ),
				new Type[0]
				);
			ILGenerator il = this._createClientMethod.GetILGenerator();

			// Get some methods
			MethodInfo endpointAddressInequalityMethodInfo = typeof( EndpointAddress ).GetMethod( "op_Inequality", new []{ typeof( EndpointAddress ), typeof( EndpointAddress ) } );
			if ( null == endpointAddressInequalityMethodInfo ) throw new InvalidOperationException( String.Format( "Could not find method 'op_Inequality' on type EndpointAddress" ) );

			MethodInfo getTypeFromHandleMethodInfo = typeof( Type ).GetMethod( "GetTypeFromHandle", new []{ typeof( RuntimeTypeHandle ) } ) ;
			if ( null == getTypeFromHandleMethodInfo ) throw new InvalidOperationException( String.Format( "Could not find method 'GetTypeFromHandle' on type Type" ) );

			MethodInfo createInstancMethodInfo = typeof( Activator ).GetMethod( "CreateInstance", new []{ typeof( Type ), typeof( object[] ) } ) ;
			if ( null == createInstancMethodInfo ) throw new InvalidOperationException( String.Format( "Could not find method 'CreateInstance' on type Activator" ) );

			ConstructorInfo invalidOperationExceptionConstructorInfo = typeof( InvalidOperationException ).GetConstructor( new [] { typeof( String ) } );
			if ( null == invalidOperationExceptionConstructorInfo ) throw new InvalidOperationException( "Could not find (string) constructor for InvalidOperationException" );

			MethodInfo getClientCredentialsMethodInfo = typeof( TClient ).GetMethod( "get_ClientCredentials" );
			if ( null == getClientCredentialsMethodInfo ) throw new InvalidOperationException( String.Format( "Could not find method 'get_ClientCredentials' on type {0}", typeof( TClient ) ) );

			MethodInfo getWindowsMethodInfo = typeof( ClientCredentials ).GetMethod( "get_Windows" );
			if ( null == getWindowsMethodInfo ) throw new InvalidOperationException( String.Format( "Could not find method 'get_Windows' on type ClientCredentials" ) );

			MethodInfo setClientCredentialsMethodInfo = typeof( WindowsClientCredential ).GetMethod( "set_ClientCredential", new []{ typeof( NetworkCredential ) } );
			if ( null == setClientCredentialsMethodInfo ) throw new InvalidOperationException( String.Format( "Could not find method 'set_ClientCredential' on type WindowsClientCredential" ) );

			ConstructorInfo networkCredentialConstructorInfo = typeof( NetworkCredential ).GetConstructor( new []{ typeof( string ), typeof( SecureString ) } );
			if ( null == networkCredentialConstructorInfo ) throw new InvalidOperationException( "Could not find (string, SecureString) constructor for NetworkCredential" );

			// Define some labels ahead of time
			Label label0025 = il.DefineLabel();
			Label label0026 = il.DefineLabel();
			Label label0059 = il.DefineLabel();
			Label label00Da = il.DefineLabel();
			Label label0072 = il.DefineLabel();
			Label label0073 = il.DefineLabel();
			Label label00A3 = il.DefineLabel();
			Label label00D1 = il.DefineLabel();
			Label label00F9 = il.DefineLabel();
			Label label010C = il.DefineLabel();
			Label label010D = il.DefineLabel();
			Label label0157 = il.DefineLabel();
			Label label014A = il.DefineLabel();
			Label label0156 = il.DefineLabel();
			Label label015B = il.DefineLabel();

			// Set some addresses so we set them correctly
			const byte v4 = 4;
			const byte v5 = 5;

			//  .locals init (class [mscorlib]System.Type V_0,
			//           object V_1,
			//           class Entropa.WcfUtils.Test.MockServiceReference.MockContractClient V_2,
			//           class Entropa.WcfUtils.Test.MockServiceReference.MockContractClient V_3,
			//           bool V_4,
			//           object[] V_5)
			il.DeclareLocal( typeof( Type ) );		
			il.DeclareLocal( typeof( object ) );		
			il.DeclareLocal( typeof( TClient ) );
			il.DeclareLocal( typeof( TClient ) );		
			il.DeclareLocal( typeof( bool ) );		
			il.DeclareLocal( typeof( object[] ) );		
		
			//  IL_0000:  nop
			//  IL_0001:  ldtoken    Entropa.WcfUtils.Test.MockServiceReference.MockContractClient
			//  IL_0006:  call       class [mscorlib]System.Type [mscorlib]System.Type::GetTypeFromHandle(valuetype [mscorlib]System.RuntimeTypeHandle)
			//  IL_000b:  stloc.0
			il.Emit( OpCodes.Nop );
			il.Emit( OpCodes.Ldtoken, typeof( TClient ) ); // TODO .FullName?
			il.Emit( OpCodes.Call, getTypeFromHandleMethodInfo );
			il.Emit( OpCodes.Stloc_0 );

			//  IL_000c:  ldarg.0
			//  IL_000d:  ldfld      class [System.ServiceModel]System.ServiceModel.Channels.Binding Entropa.WcfUtils.Test.Prototypes.PrototypeFaultSafeServiceReferenceClient::_binding
			//  IL_0012:  brfalse.s  IL_0025
			//  IL_0014:  ldnull
			//  IL_0015:  ldarg.0
			//  IL_0016:  ldfld      class [System.ServiceModel]System.ServiceModel.EndpointAddress Entropa.WcfUtils.Test.Prototypes.PrototypeFaultSafeServiceReferenceClient::_remoteAddress
			//  IL_001b:  call       bool [System.ServiceModel]System.ServiceModel.EndpointAddress::op_Inequality(class [System.ServiceModel]System.ServiceModel.EndpointAddress,
			//                                                                                                    class [System.ServiceModel]System.ServiceModel.EndpointAddress)
			//  IL_0020:  ldc.i4.0
			//  IL_0021:  ceq
			//  IL_0023:  br.s       IL_0026
			//  IL_0025:  ldc.i4.1
			//  IL_0026:  nop
			//  IL_0027:  stloc.s    V_4
			//  IL_0029:  ldloc.s    V_4
			//  IL_002b:  brtrue.s   IL_0059
			//  IL_002d:  nop
			//  IL_002e:  ldloc.0
			//  IL_002f:  ldc.i4.2
			//  IL_0030:  newarr     [mscorlib]System.Object
			//  IL_0035:  stloc.s    V_5
			//  IL_0037:  ldloc.s    V_5
			//  IL_0039:  ldc.i4.0
			//  IL_003a:  ldarg.0
			//  IL_003b:  ldfld      class [System.ServiceModel]System.ServiceModel.Channels.Binding Entropa.WcfUtils.Test.Prototypes.PrototypeFaultSafeServiceReferenceClient::_binding
			//  IL_0040:  stelem.ref
			//  IL_0041:  ldloc.s    V_5
			//  IL_0043:  ldc.i4.1
			//  IL_0044:  ldarg.0
			//  IL_0045:  ldfld      class [System.ServiceModel]System.ServiceModel.EndpointAddress Entropa.WcfUtils.Test.Prototypes.PrototypeFaultSafeServiceReferenceClient::_remoteAddress
			//  IL_004a:  stelem.ref
			//  IL_004b:  ldloc.s    V_5
			//  IL_004d:  call       object [mscorlib]System.Activator::CreateInstance(class [mscorlib]System.Type,
			//                                                                         object[])
			//  IL_0052:  stloc.1
			//  IL_0053:  nop
			//  IL_0054:  br         IL_00da
			il.Emit( OpCodes.Ldarg_0 );
			il.Emit( OpCodes.Ldfld, this._bindingField );
			il.Emit( OpCodes.Brfalse_S, label0025 );
			il.Emit( OpCodes.Ldnull );
			il.Emit( OpCodes.Ldarg_0 );
			il.Emit( OpCodes.Ldfld, this._remoteAddressField );
			il.Emit( OpCodes.Call, endpointAddressInequalityMethodInfo );
			il.Emit( OpCodes.Ldc_I4_0 );
			il.Emit( OpCodes.Ceq );
			il.Emit( OpCodes.Br_S, label0026 );
			il.MarkLabel( label0025 );
			il.Emit( OpCodes.Ldc_I4_1 );
			il.MarkLabel( label0026 );
			il.Emit( OpCodes.Nop );
			il.Emit( OpCodes.Stloc_S, v4 );
			il.Emit( OpCodes.Ldloc_S, v4 );
			il.Emit( OpCodes.Brtrue_S, label0059 );
			il.Emit( OpCodes.Nop );
			il.Emit( OpCodes.Ldloc_0 );
			il.Emit( OpCodes.Ldc_I4_2 );
			il.Emit( OpCodes.Newarr, typeof( object ) );
			il.Emit( OpCodes.Stloc_S, v5 );
			il.Emit( OpCodes.Ldloc_S, v5 );
			il.Emit( OpCodes.Ldc_I4_0 );
			il.Emit( OpCodes.Ldarg_0 );
			il.Emit( OpCodes.Ldfld, this._bindingField );
			il.Emit( OpCodes.Stelem_Ref );
			il.Emit( OpCodes.Ldloc_S, v5 );
			il.Emit( OpCodes.Ldc_I4_1 );
			il.Emit( OpCodes.Ldarg_0 );
			il.Emit( OpCodes.Ldfld, this._remoteAddressField );
			il.Emit( OpCodes.Stelem_Ref );
			il.Emit( OpCodes.Ldloc_S, v5 );
			il.Emit( OpCodes.Call, createInstancMethodInfo );
			il.Emit( OpCodes.Stloc_1 );
			il.Emit( OpCodes.Nop );
			il.Emit( OpCodes.Br, label00Da );

			//  IL_0059:  ldarg.0
			//  IL_005a:  ldfld      string Entropa.WcfUtils.Test.Prototypes.PrototypeFaultSafeServiceReferenceClient::_endpointConfigurationName
			//  IL_005f:  brfalse.s  IL_0072
			//  IL_0061:  ldnull
			//  IL_0062:  ldarg.0
			//  IL_0063:  ldfld      class [System.ServiceModel]System.ServiceModel.EndpointAddress Entropa.WcfUtils.Test.Prototypes.PrototypeFaultSafeServiceReferenceClient::_remoteAddress
			//  IL_0068:  call       bool [System.ServiceModel]System.ServiceModel.EndpointAddress::op_Inequality(class [System.ServiceModel]System.ServiceModel.EndpointAddress,
			//                                                                                                    class [System.ServiceModel]System.ServiceModel.EndpointAddress)
			//  IL_006d:  ldc.i4.0
			//  IL_006e:  ceq
			//  IL_0070:  br.s       IL_0073
			//  IL_0072:  ldc.i4.1
			//  IL_0073:  nop
			//  IL_0074:  stloc.s    V_4
			//  IL_0076:  ldloc.s    V_4
			//  IL_0078:  brtrue.s   IL_00a3
			//  IL_007a:  nop
			//  IL_007b:  ldloc.0
			//  IL_007c:  ldc.i4.2
			//  IL_007d:  newarr     [mscorlib]System.Object
			//  IL_0082:  stloc.s    V_5
			//  IL_0084:  ldloc.s    V_5
			//  IL_0086:  ldc.i4.0
			//  IL_0087:  ldarg.0
			//  IL_0088:  ldfld      string Entropa.WcfUtils.Test.Prototypes.PrototypeFaultSafeServiceReferenceClient::_endpointConfigurationName
			//  IL_008d:  stelem.ref
			//  IL_008e:  ldloc.s    V_5
			//  IL_0090:  ldc.i4.1
			//  IL_0091:  ldarg.0
			//  IL_0092:  ldfld      class [System.ServiceModel]System.ServiceModel.EndpointAddress Entropa.WcfUtils.Test.Prototypes.PrototypeFaultSafeServiceReferenceClient::_remoteAddress
			//  IL_0097:  stelem.ref
			//  IL_0098:  ldloc.s    V_5
			//  IL_009a:  call       object [mscorlib]System.Activator::CreateInstance(class [mscorlib]System.Type,
			//                                                                         object[])
			//  IL_009f:  stloc.1
			//  IL_00a0:  nop
			//  IL_00a1:  br.s       IL_00da
			il.MarkLabel( label0059 );
			il.Emit( OpCodes.Ldarg_0 );
			il.Emit( OpCodes.Ldfld, this._endpointConfigurationNameField );
			il.Emit( OpCodes.Brfalse_S, label0072 );
			il.Emit( OpCodes.Ldnull );
			il.Emit( OpCodes.Ldarg_0 );
			il.Emit( OpCodes.Ldfld, this._remoteAddressField );
			il.Emit( OpCodes.Call, endpointAddressInequalityMethodInfo );
			il.Emit( OpCodes.Ldc_I4_0 );
			il.Emit( OpCodes.Ceq );
			il.Emit( OpCodes.Br_S, label0073 );
			il.MarkLabel( label0072 );
			il.Emit( OpCodes.Ldc_I4_1 );
			il.MarkLabel( label0073 );
			il.Emit( OpCodes.Nop );
			il.Emit( OpCodes.Stloc_S, v4 );
			il.Emit( OpCodes.Ldloc_S, v4 );
			il.Emit( OpCodes.Brtrue_S, label00A3 );
			il.Emit( OpCodes.Nop );
			il.Emit( OpCodes.Ldloc_0 );
			il.Emit( OpCodes.Ldc_I4_2 );
			il.Emit( OpCodes.Newarr, typeof( object ) );
			il.Emit( OpCodes.Stloc_S, v5 );
			il.Emit( OpCodes.Ldloc_S, v5 );
			il.Emit( OpCodes.Ldc_I4_0 );
			il.Emit( OpCodes.Ldarg_0 );
			il.Emit( OpCodes.Ldfld, this._endpointConfigurationNameField );
			il.Emit( OpCodes.Stelem_Ref );
			il.Emit( OpCodes.Ldloc_S, v5 );
			il.Emit( OpCodes.Ldc_I4_1 );
			il.Emit( OpCodes.Ldarg_0 );
			il.Emit( OpCodes.Ldfld, this._remoteAddressField );
			il.Emit( OpCodes.Stelem_Ref );
			il.Emit( OpCodes.Ldloc_S, v5 );
			il.Emit( OpCodes.Call, createInstancMethodInfo );
			il.Emit( OpCodes.Stloc_1 );
			il.Emit( OpCodes.Nop );
			il.Emit( OpCodes.Br_S, label00Da );
		
			//  IL_00a3:  ldnull
			//  IL_00a4:  ldarg.0
			//  IL_00a5:  ldfld      string Entropa.WcfUtils.Test.Prototypes.PrototypeFaultSafeServiceReferenceClient::_endpointConfigurationName
			//  IL_00aa:  ceq
			//  IL_00ac:  stloc.s    V_4
			//  IL_00ae:  ldloc.s    V_4
			//  IL_00b0:  brtrue.s   IL_00d1
			il.MarkLabel( label00A3 );
			il.Emit( OpCodes.Ldnull );
			il.Emit( OpCodes.Ldarg_0 );
			il.Emit( OpCodes.Ldfld, this._endpointConfigurationNameField );
			il.Emit( OpCodes.Ceq );
			il.Emit( OpCodes.Stloc_S, v4 );
			il.Emit( OpCodes.Ldloc_S, v4 );
			il.Emit( OpCodes.Brtrue_S, label00D1 );

			//  IL_00b2:  nop
			//  IL_00b3:  ldloc.0
			//  IL_00b4:  ldc.i4.1
			//  IL_00b5:  newarr     [mscorlib]System.Object
			//  IL_00ba:  stloc.s    V_5
			//  IL_00bc:  ldloc.s    V_5
			//  IL_00be:  ldc.i4.0
			//  IL_00bf:  ldarg.0
			//  IL_00c0:  ldfld      string Entropa.WcfUtils.Test.Prototypes.PrototypeFaultSafeServiceReferenceClient::_endpointConfigurationName
			//  IL_00c5:  stelem.ref
			//  IL_00c6:  ldloc.s    V_5
			//  IL_00c8:  call       object [mscorlib]System.Activator::CreateInstance(class [mscorlib]System.Type,
			//                                                                         object[])
			//  IL_00cd:  stloc.1
			//  IL_00ce:  nop
			//  IL_00cf:  br.s       IL_00da
			il.Emit( OpCodes.Nop );
			il.Emit( OpCodes.Ldloc_0 );
			il.Emit( OpCodes.Ldc_I4_1 );
			il.Emit( OpCodes.Newarr, typeof( object ) );
			il.Emit( OpCodes.Stloc_S, v5 );
			il.Emit( OpCodes.Ldloc_S, v5 );
			il.Emit( OpCodes.Ldc_I4_0 );
			il.Emit( OpCodes.Ldarg_0 );
			il.Emit( OpCodes.Ldfld, this._endpointConfigurationNameField );
			il.Emit( OpCodes.Stelem_Ref );
			il.Emit( OpCodes.Ldloc_S, v5 );
			il.Emit( OpCodes.Call, createInstancMethodInfo );
			il.Emit( OpCodes.Stloc_1 );
			il.Emit( OpCodes.Nop );
			il.Emit( OpCodes.Br_S, label00Da );
	
			//  IL_00d1:  nop
			//  IL_00d2:  ldloc.0
			//  IL_00d3:  call       object [mscorlib]System.Activator::CreateInstance(class [mscorlib]System.Type)
			//  IL_00d8:  stloc.1
			//  IL_00d9:  nop
			il.MarkLabel( label00D1 );
			il.Emit( OpCodes.Nop );
			il.Emit( OpCodes.Ldloc_0 );
			il.Emit( OpCodes.Call, createInstancMethodInfo );
			il.Emit( OpCodes.Stloc_1 );
			il.Emit( OpCodes.Nop );
			
			//  IL_00da:  ldloc.1
			//  IL_00db:  isinst     Entropa.WcfUtils.Test.MockServiceReference.MockContractClient
			//  IL_00e0:  stloc.2
			//  IL_00e1:  ldnull
			//  IL_00e2:  ldloc.2
			//  IL_00e3:  ceq
			//  IL_00e5:  ldc.i4.0
			//  IL_00e6:  ceq
			//  IL_00e8:  stloc.s    V_4
			//  IL_00ea:  ldloc.s    V_4
			//  IL_00ec:  brtrue.s   IL_00f9
			il.MarkLabel( label00Da );
			il.Emit( OpCodes.Ldloc_1 );
			il.Emit( OpCodes.Isinst, typeof( TClient ) );
			il.Emit( OpCodes.Stloc_2 );
			il.Emit( OpCodes.Ldnull );
			il.Emit( OpCodes.Ldloc_2 );
			il.Emit( OpCodes.Ceq );
			il.Emit( OpCodes.Ldc_I4_0 );
			il.Emit( OpCodes.Ceq );
			il.Emit( OpCodes.Stloc_S, v4 );
			il.Emit( OpCodes.Ldloc_S, v4 );
			il.Emit( OpCodes.Brtrue_S, label00F9 );

			//  IL_00ee:  ldstr      "Could not create instance of MockContractClient"
			//  IL_00f3:  newobj     instance void [mscorlib]System.InvalidOperationException::.ctor(string)
			//  IL_00f8:  throw
			il.Emit( OpCodes.Ldstr, String.Format( "Could not create instance of '{0}'", typeof( TClient ).Name ) );
			il.Emit( OpCodes.Newobj, invalidOperationExceptionConstructorInfo );
			il.Emit( OpCodes.Throw );
	
			//  IL_00f9:  ldarg.0
			//  IL_00fa:  ldfld      string Entropa.WcfUtils.Test.Prototypes.PrototypeFaultSafeServiceReferenceClient::_userName
			//  IL_00ff:  brfalse.s  IL_010c
			//  IL_0101:  ldnull
			//  IL_0102:  ldarg.0
			//  IL_0103:  ldfld      class [mscorlib]System.Security.SecureString Entropa.WcfUtils.Test.Prototypes.PrototypeFaultSafeServiceReferenceClient::_password
			//  IL_0108:  ceq
			//  IL_010a:  br.s       IL_010d
			il.MarkLabel( label00F9 );
			il.Emit( OpCodes.Ldarg_0 );
			il.Emit( OpCodes.Ldfld, this._userNameField );
			il.Emit( OpCodes.Brfalse_S, label010C );
			il.Emit( OpCodes.Ldnull );
			il.Emit( OpCodes.Ldarg_0 );
			il.Emit( OpCodes.Ldfld, this._passwordField );
			il.Emit( OpCodes.Ceq );
			il.Emit( OpCodes.Br_S, label010D );

			//  IL_010c:  ldc.i4.1
			//  IL_010d:  nop
			//  IL_010e:  stloc.s    V_4
			//  IL_0110:  ldloc.s    V_4
			//  IL_0112:  brtrue.s   IL_0157
			il.MarkLabel( label010C );
			il.Emit( OpCodes.Ldc_I4_1 );
			il.MarkLabel( label010D );
			il.Emit( OpCodes.Nop );
			il.Emit( OpCodes.Stloc_S, v4 );
			il.Emit( OpCodes.Ldloc_S, v4 );
			il.Emit( OpCodes.Brtrue_S, label0157 );

			//  IL_0114:  nop
			//  IL_0115:  ldnull
			//  IL_0116:  ldloc.2
			//  IL_0117:  callvirt   instance class [System.ServiceModel]System.ServiceModel.Description.ClientCredentials class [System.ServiceModel]System.ServiceModel.ClientBase`1<class Entropa.WcfUtils.Test.MockServiceReference.IMockContract>::get_ClientCredentials()
			//  IL_011c:  ceq
			//  IL_011e:  stloc.s    V_4
			//  IL_0120:  ldloc.s    V_4
			//  IL_0122:  brtrue.s   IL_014a
			il.Emit( OpCodes.Nop );
			il.Emit( OpCodes.Ldnull );
			il.Emit( OpCodes.Ldloc_2 );
			il.Emit( OpCodes.Callvirt, getClientCredentialsMethodInfo );
			il.Emit( OpCodes.Ceq );
			il.Emit( OpCodes.Stloc_S, v4 );
			il.Emit( OpCodes.Ldloc_S, v4 );
			il.Emit( OpCodes.Brtrue_S, label014A );

			//  IL_0124:  nop
			//  IL_0125:  ldloc.2
			//  IL_0126:  callvirt   instance class [System.ServiceModel]System.ServiceModel.Description.ClientCredentials class [System.ServiceModel]System.ServiceModel.ClientBase`1<class Entropa.WcfUtils.Test.MockServiceReference.IMockContract>::get_ClientCredentials()
			//  IL_012b:  callvirt   instance class [System.ServiceModel]System.ServiceModel.Security.WindowsClientCredential [System.ServiceModel]System.ServiceModel.Description.ClientCredentials::get_Windows()
			//  IL_0130:  ldarg.0
			//  IL_0131:  ldfld      string Entropa.WcfUtils.Test.Prototypes.PrototypeFaultSafeServiceReferenceClient::_userName
			//  IL_0136:  ldarg.0
			//  IL_0137:  ldfld      class [mscorlib]System.Security.SecureString Entropa.WcfUtils.Test.Prototypes.PrototypeFaultSafeServiceReferenceClient::_password
			//  IL_013c:  newobj     instance void [System]System.Net.NetworkCredential::.ctor(string,
			//                                                                                 class [mscorlib]System.Security.SecureString)
			//  IL_0141:  callvirt   instance void [System.ServiceModel]System.ServiceModel.Security.WindowsClientCredential::set_ClientCredential(class [System]System.Net.NetworkCredential)
			//  IL_0146:  nop
			il.Emit( OpCodes.Nop );
			il.Emit( OpCodes.Ldloc_2 );
			il.Emit( OpCodes.Callvirt, getClientCredentialsMethodInfo );
			il.Emit( OpCodes.Callvirt, getWindowsMethodInfo );
			il.Emit( OpCodes.Ldarg_0 );
			il.Emit( OpCodes.Ldfld, this._userNameField );
			il.Emit( OpCodes.Ldarg_0 );
			il.Emit( OpCodes.Ldfld, this._passwordField );
			il.Emit( OpCodes.Newobj, networkCredentialConstructorInfo );
			il.Emit( OpCodes.Callvirt, setClientCredentialsMethodInfo );
			il.Emit( OpCodes.Nop );
	
			//  IL_0147:  nop
			//  IL_0148:  br.s       IL_0156
			//  IL_014a:  nop
			//  IL_014b:  ldstr      "MockContractClient client has no ClientCredentials"
			//  IL_0150:  newobj     instance void [mscorlib]System.InvalidOperationException::.ctor(string)
			//  IL_0155:  throw
			il.Emit( OpCodes.Nop );
			il.Emit( OpCodes.Br_S, label0156 );
			il.MarkLabel( label014A );
			il.Emit( OpCodes.Nop );
			il.Emit( OpCodes.Ldstr, String.Format( "'{0}' client has no ClientCredentials", typeof( TClient ).Name ) );
			il.Emit( OpCodes.Newobj, invalidOperationExceptionConstructorInfo );
			il.Emit( OpCodes.Throw );

			//  IL_0156:  nop
			//  IL_0157:  ldloc.2
			//  IL_0158:  stloc.3
			//  IL_0159:  br.s       IL_015b
			//  IL_015b:  ldloc.3
			//  IL_015c:  ret
			//} // end of method PrototypeFaultSafeServiceReferenceClient::CreateClient
			il.MarkLabel( label0156 );
			il.Emit( OpCodes.Nop );
			il.MarkLabel( label0157 );
			il.Emit( OpCodes.Ldloc_2 );
			il.Emit( OpCodes.Stloc_3 );
			il.Emit( OpCodes.Br_S, label015B );
			il.MarkLabel( label015B );
			il.Emit( OpCodes.Ldloc_3 );
			il.Emit( OpCodes.Ret );
		}

		/// <summary>
		/// Builds the "GetClient" method.
		/// </summary>
		/// <remarks><![CDATA[
		/// .method public hidebysig instance class Entropa.WcfUtils.Test.MockServiceReference.IMockContract 
		///        GetClient() cil managed
		///{
		///  // Code size       42 (0x2a)
		///  .maxstack  2
		///  .locals init (class Entropa.WcfUtils.Test.MockServiceReference.IMockContract V_0,
		///           bool V_1)
		///  IL_0000:  nop
		///  IL_0001:  ldnull
		///  IL_0002:  ldarg.0
		///  IL_0003:  ldfld      class Entropa.WcfUtils.Test.MockServiceReference.MockContractClient Entropa.WcfUtils.Test.Prototypes.PrototypeFaultSafeServiceReferenceClient::_client
		///  IL_0008:  ceq
		///  IL_000a:  ldc.i4.0
		///  IL_000b:  ceq
		///  IL_000d:  stloc.1
		///  IL_000e:  ldloc.1
		///  IL_000f:  brtrue.s   IL_001f
		///  IL_0011:  nop
		///  IL_0012:  ldarg.0
		///  IL_0013:  ldarg.0
		///  IL_0014:  call       instance class Entropa.WcfUtils.Test.MockServiceReference.MockContractClient Entropa.WcfUtils.Test.Prototypes.PrototypeFaultSafeServiceReferenceClient::CreateClient()
		///  IL_0019:  stfld      class Entropa.WcfUtils.Test.MockServiceReference.MockContractClient Entropa.WcfUtils.Test.Prototypes.PrototypeFaultSafeServiceReferenceClient::_client
		///  IL_001e:  nop
		///  IL_001f:  ldarg.0
		///  IL_0020:  ldfld      class Entropa.WcfUtils.Test.MockServiceReference.MockContractClient Entropa.WcfUtils.Test.Prototypes.PrototypeFaultSafeServiceReferenceClient::_client
		///  IL_0025:  stloc.0
		///  IL_0026:  br.s       IL_0028
		///  IL_0028:  ldloc.0
		///  IL_0029:  ret
		///} // end of method PrototypeFaultSafeServiceReferenceClient::GetClient
		/// ]]></remarks>
		/// <param name="typeBuilder"></param>
		private void BuildGetClientMethod( TypeBuilder typeBuilder ) {
			this._getClientMethod = typeBuilder.DefineMethod(
				"GetClient",
				MethodAttributes.Private | MethodAttributes.HideBySig,
				CallingConventions.Standard,
				typeof( TInterface ),
				new Type[0]
				);
			ILGenerator il = this._getClientMethod.GetILGenerator();

			// Define some labels ahead of time
			Label label001F = il.DefineLabel();
			Label label0028 = il.DefineLabel();

			//  .locals init (class Entropa.WcfUtils.Test.MockServiceReference.IMockContract V_0,
			//           bool V_1)
			il.DeclareLocal( typeof( TInterface ) );
			il.DeclareLocal( typeof( bool ) );

			//  IL_0000:  nop
			//  IL_0001:  ldnull
			//  IL_0002:  ldarg.0
			//  IL_0003:  ldfld      class Entropa.WcfUtils.Test.MockServiceReference.MockContractClient Entropa.WcfUtils.Test.Prototypes.PrototypeFaultSafeServiceReferenceClient::_client
			//  IL_0008:  ceq
			il.Emit( OpCodes.Nop );
			il.Emit( OpCodes.Ldnull );
			il.Emit( OpCodes.Ldarg_0 );
			il.Emit( OpCodes.Ldfld, this._clientField );
			il.Emit( OpCodes.Ceq );

			//  IL_000a:  ldc.i4.0
			//  IL_000b:  ceq
			//  IL_000d:  stloc.1
			//  IL_000e:  ldloc.1
			//  IL_000f:  brtrue.s   IL_001f
			il.Emit( OpCodes.Ldc_I4_0 );
			il.Emit( OpCodes.Ceq );
			il.Emit( OpCodes.Stloc_1 );
			il.Emit( OpCodes.Ldloc_1 );
			il.Emit( OpCodes.Brtrue_S, label001F );

			//  IL_0011:  nop
			//  IL_0012:  ldarg.0
			//  IL_0013:  ldarg.0
			//  IL_0014:  call       instance class Entropa.WcfUtils.Test.MockServiceReference.MockContractClient Entropa.WcfUtils.Test.Prototypes.PrototypeFaultSafeServiceReferenceClient::CreateClient()
			//  IL_0019:  stfld      class Entropa.WcfUtils.Test.MockServiceReference.MockContractClient Entropa.WcfUtils.Test.Prototypes.PrototypeFaultSafeServiceReferenceClient::_client
			//  IL_001e:  nop
			il.Emit( OpCodes.Nop );
			il.Emit( OpCodes.Ldarg_0 );
			il.Emit( OpCodes.Ldarg_0 );
			il.Emit( OpCodes.Call, this._createClientMethod );
			il.Emit( OpCodes.Stfld, this._clientField );
			il.Emit( OpCodes.Nop );
			
	
			//  IL_001f:  ldarg.0
			//  IL_0020:  ldfld      class Entropa.WcfUtils.Test.MockServiceReference.MockContractClient Entropa.WcfUtils.Test.Prototypes.PrototypeFaultSafeServiceReferenceClient::_client
			//  IL_0025:  stloc.0
			//  IL_0026:  br.s       IL_0028
			//  IL_0028:  ldloc.0
			//  IL_0029:  ret
			il.MarkLabel( label001F );
			il.Emit( OpCodes.Ldarg_0 );
			il.Emit( OpCodes.Ldfld, this._clientField );
			il.Emit( OpCodes.Stloc_0 );
			il.Emit( OpCodes.Br_S, label0028 );
			il.MarkLabel( label0028 );
			il.Emit( OpCodes.Ldloc_0 );
			il.Emit( OpCodes.Ret );
		}

		/// <summary>
		/// Emits the call to the base Object constructor.
		/// </summary>
		/// <param name="il"></param>
		private static void CallBaseConstructor( ILGenerator il ) {
			ConstructorInfo objectConstructorInfo = typeof( Object ).GetConstructor( new Type[0] );
			if ( null == objectConstructorInfo ) throw new InvalidOperationException( String.Format( "Could not find Object.ctor() constructor info" ) );
			// Call the base constructor
			//     IL_0000:  ldarg.0
			//     IL_0001:  call       instance void [mscorlib]System.Object::.ctor()
			//     IL_0006:  nop
			//     IL_0007:  nop
			il.Emit( OpCodes.Ldarg_0 );
			il.Emit( OpCodes.Call, objectConstructorInfo );
			il.Emit( OpCodes.Nop );
			il.Emit( OpCodes.Nop );
		}

		/// <summary>
		/// Creates the proxy type.
		/// </summary>
		/// <returns></returns>
		private Type CreateProxyType() {
			_log.DebugFormat( "CreateProxyType - TInterface = '{0}', TClient = '{1}'", typeof( TInterface ).Name, typeof( TClient ).Name );
			// Validate our type.
			ValidateTypeParameters();
			// Create the dynamic assembly builder
			AssemblyBuilder assemblyBuilder = this.CreateAssemblyBuilder();
			// Create the dynamic module builder
			ModuleBuilder moduleBuilder = CreateModuleBuilder( assemblyBuilder );
			// Create the type builder
			TypeBuilder typeBuilder = this.CreateTypeBuilder( moduleBuilder );
			// Build our type
			this.BuildType( typeBuilder );
			// Create the type
			Type generatedType = typeBuilder.CreateType();
			_log.DebugFormat( " - created type '{0}'", generatedType );
#if DEBUG
			// Save the assembly to file
			assemblyBuilder.Save( assemblyBuilder.GetName().Name + ".dll" );
#endif
			return generatedType;
		}

		/// <summary>
		/// Emites the constructor-ending codes.
		/// </summary>
		/// <param name="il"></param>
		private static void EndConstructor( ILGenerator il ) {
			//   IL_002b:  nop
			//   IL_002c:  ret
			il.Emit( OpCodes.Nop );
			il.Emit( OpCodes.Ret );
		}

		/// <summary>
		/// Creates a constructor generator taking the given parameters as types and returns the IL generator.
		/// </summary>
		/// <param name="typeBuilder"></param>
		/// <param name="parameterTypes"></param>
		/// <returns></returns>
		private static ILGenerator GetConstructorGenerator( TypeBuilder typeBuilder, params Type[] parameterTypes ) {
			ConstructorBuilder constructorBuilder = typeBuilder.DefineConstructor(
				MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName,
				CallingConventions.Standard,
				parameterTypes
				);

			ILGenerator il = constructorBuilder.GetILGenerator();
			return il;
		}

		/// <summary>
		/// Initializes the given field to null.
		/// </summary>
		/// <param name="il"></param>
		/// <param name="field"></param>
		private static void InitializeReadonlyField( ILGenerator il, FieldBuilder field ) {
			InitializeReadonlyField( il, field, OpCodes.Ldnull );
		}

		/// <summary>
		/// Initializes the given field to null.
		/// </summary>
		/// <param name="il"></param>
		/// <param name="valueCode"></param>
		/// <param name="field"></param>
		private static void InitializeReadonlyField( ILGenerator il, FieldBuilder field, OpCode valueCode ) {
			//   IL_0008:  ldarg.0
			//   IL_0009:  ldnull
			//   IL_000a:  stfld      class [System.ServiceModel]System.ServiceModel.Channels.Binding class Entropa.WcfUtils.Test.Prototypes.PrototypeFaultSafeServiceReferenceClient`1<!TClient>::_binding
			il.Emit( OpCodes.Ldarg_0 );
			il.Emit( valueCode );
			il.Emit( OpCodes.Stfld, field );
		}

		/// <summary>
		/// Initializes a SecureString field.
		/// </summary>
		/// <remarks><![CDATA[
		///   IL_0024:  ldarg.0
		///   IL_0025:  newobj     instance void [mscorlib]System.Security.SecureString::.ctor()
		///   IL_002a:  stfld      class [mscorlib]System.Security.SecureString class Entropa.WcfUtils.Test.Prototypes.PrototypeFaultSafeServiceReferenceClient`1<!TClient>::_password
		///   IL_002f:  nop
		///   IL_0030:  ldarg.s    password
		///   IL_0032:  stloc.1
		///   IL_0033:  ldc.i4.0
		///   IL_0034:  stloc.2
		///   IL_0035:  br.s       IL_0052
		///   IL_0037:  ldloc.1
		///   IL_0038:  ldloc.2
		///   IL_0039:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
		///   IL_003e:  stloc.0
		///   IL_003f:  nop
		///   IL_0040:  ldarg.0
		///   IL_0041:  ldfld      class [mscorlib]System.Security.SecureString class Entropa.WcfUtils.Test.Prototypes.PrototypeFaultSafeServiceReferenceClient`1<!TClient>::_password
		///   IL_0046:  ldloc.0
		///   IL_0047:  callvirt   instance void [mscorlib]System.Security.SecureString::AppendChar(char)
		///   IL_004c:  nop
		///   IL_004d:  nop
		///   IL_004e:  ldloc.2
		///   IL_004f:  ldc.i4.1
		///   IL_0050:  add
		///   IL_0051:  stloc.2
		///   IL_0052:  ldloc.2
		///   IL_0053:  ldloc.1
		///   IL_0054:  callvirt   instance int32 [mscorlib]System.String::get_Length()
		///   IL_0059:  clt
		///   IL_005b:  stloc.3
		///   IL_005c:  ldloc.3
		///   IL_005d:  brtrue.s   IL_0037
		///   IL_005f:  ldarg.0
		///   IL_0060:  ldfld      class [mscorlib]System.Security.SecureString class Entropa.WcfUtils.Test.Prototypes.PrototypeFaultSafeServiceReferenceClient`1<!TClient>::_password
		///   IL_0065:  callvirt   instance void [mscorlib]System.Security.SecureString::MakeReadOnly()
		///   IL_006a:  nop
		/// 
		/// ]]></remarks>
		/// <param name="il"></param>
		/// <param name="field"></param>
		/// <param name="valueCode"></param>
		/// <param name="parameterName"></param>
		private void InitializeReadonlySecureStringField( ILGenerator il, FieldBuilder field, OpCode valueCode, string parameterName = null ) {
			ConstructorInfo secureStringConstructorInfo = typeof( SecureString ).GetConstructor( new Type[0] );
			if ( null == secureStringConstructorInfo ) throw new InvalidOperationException( String.Format( "Could not find SecureString.ctor() constructor info" ) );
			MethodInfo getCharsMethodInfo = typeof( String ).GetMethod( "get_Chars", new []{ typeof( Int32 )} );
			if ( null == getCharsMethodInfo )throw new InvalidOperationException( String.Format( "Could not find member 'get_Chars' on String" ));
			MethodInfo getLengthMethodInfo = typeof( String ).GetMethod( "get_Length", new Type[0] );
			if ( null == getLengthMethodInfo )throw new InvalidOperationException( String.Format( "Could not find member 'get_Length' on String" ));

			MethodInfo appendCharMethodInfo = typeof( SecureString ).GetMethod( "AppendChar", new []{ typeof( char )} );
			if ( null == appendCharMethodInfo )throw new InvalidOperationException( String.Format( "Could not find member 'AppendChar' on SecureString" ));
			MethodInfo makeReadOnlyMethodInfo = typeof( SecureString ).GetMethod( "MakeReadOnly", new Type[0] );
			if ( null == makeReadOnlyMethodInfo )throw new InvalidOperationException( String.Format( "Could not find member 'MakeReadOnly' on SecureString" ));

			// Define a couple of the labels we'll need
			Label label0052 = il.DefineLabel();
			Label label0037 = il.DefineLabel();

			// _password = new SecureString()
			//   IL_0024:  ldarg.0
			//   IL_0025:  newobj     instance void [mscorlib]System.Security.SecureString::.ctor()
			//   IL_002a:  stfld      class [mscorlib]System.Security.SecureString class Entropa.WcfUtils.Test.Prototypes.PrototypeFaultSafeServiceReferenceClient`1<!TClient>::_password
			il.Emit( OpCodes.Ldarg_0 );
			il.Emit( OpCodes.Newobj, secureStringConstructorInfo );
			il.Emit( OpCodes.Stfld, this._passwordField );

			//	foreach ( char c in password ) {
			//		this._password.AppendChar( c );
			//	}

			//   IL_002f:  nop
			//   IL_0030:  ldarg.s    password
			//   IL_0032:  stloc.1
			//   IL_0033:  ldc.i4.0
			//   IL_0034:  stloc.2
			//   IL_0035:  br.s       IL_0052
			//   IL_0037:  ldloc.1
			//   IL_0038:  ldloc.2
			//   IL_0039:  callvirt   instance char [mscorlib]System.String::get_Chars(int32)
			//   IL_003e:  stloc.0
			il.Emit( OpCodes.Nop );
			// If using lds instead of ldarg we need a name
			if ( null != parameterName ) {
				il.Emit( valueCode, parameterName );
			} else {
				il.Emit( valueCode );
			}
			il.Emit( OpCodes.Stloc_1 );
			il.Emit( OpCodes.Ldc_I4_0 );
			il.Emit( OpCodes.Stloc_2 );
			il.Emit( OpCodes.Br_S, label0052 );
			il.MarkLabel( label0037 );
			il.Emit( OpCodes.Ldloc_1 );
			il.Emit( OpCodes.Ldloc_2 );
			il.Emit( OpCodes.Callvirt, getCharsMethodInfo );
			il.Emit( OpCodes.Stloc_0 );

			//   IL_003f:  nop
			//   IL_0040:  ldarg.0
			//   IL_0041:  ldfld      class [mscorlib]System.Security.SecureString class Entropa.WcfUtils.Test.Prototypes.PrototypeFaultSafeServiceReferenceClient`1<!TClient>::_password
			//   IL_0046:  ldloc.0
			//   IL_0047:  callvirt   instance void [mscorlib]System.Security.SecureString::AppendChar(char)
			//   IL_004c:  nop
			il.Emit( OpCodes.Nop );
			il.Emit( OpCodes.Ldarg_0 );
			il.Emit( OpCodes.Ldfld, field );
			il.Emit( OpCodes.Ldloc_0 );
			il.Emit( OpCodes.Callvirt, appendCharMethodInfo );
			il.Emit( OpCodes.Nop );

			//   IL_004d:  nop
			//   IL_004e:  ldloc.2
			//   IL_004f:  ldc.i4.1
			//   IL_0050:  add
			//   IL_0051:  stloc.2
			//   IL_0052:  ldloc.2
			//   IL_0053:  ldloc.1
			//   IL_0054:  callvirt   instance int32 [mscorlib]System.String::get_Length()
			il.Emit( OpCodes.Nop );
			il.Emit( OpCodes.Ldloc_2 );
			il.Emit( OpCodes.Ldc_I4_1 );
			il.Emit( OpCodes.Add );
			il.Emit( OpCodes.Stloc_2 );
			il.MarkLabel( label0052 );
			il.Emit( OpCodes.Ldloc_2 );
			il.Emit( OpCodes.Ldloc_1 );
			il.Emit( OpCodes.Callvirt, getLengthMethodInfo );

			//   IL_0059:  clt
			//   IL_005b:  stloc.3
			//   IL_005c:  ldloc.3
			//   IL_005d:  brtrue.s   IL_0037
			il.Emit( OpCodes.Clt );
			il.Emit( OpCodes.Stloc_3 );
			il.Emit( OpCodes.Ldloc_3 );
			il.Emit( OpCodes.Brtrue_S, label0037 );

			//	this._password.MakeReadOnly();
			//   IL_005f:  ldarg.0
			//   IL_0060:  ldfld      class [mscorlib]System.Security.SecureString class Entropa.WcfUtils.Test.Prototypes.PrototypeFaultSafeServiceReferenceClient`1<!TClient>::_password
			//   IL_0065:  callvirt   instance void [mscorlib]System.Security.SecureString::MakeReadOnly()
			//   IL_006a:  nop
			il.Emit( OpCodes.Ldarg_0 );
			il.Emit( OpCodes.Ldfld, field );
			il.Emit( OpCodes.Callvirt, makeReadOnlyMethodInfo );
			il.Emit( OpCodes.Nop );
		}

		/// <summary>
		/// <see cref="FaultSafeEmitterBase{TInterface}.TypeName"/>
		/// </summary>
		protected override string TypeName {
			get { return "FaultSafeServiceReferenceClient"; }
		}

		#endregion
	}
}
