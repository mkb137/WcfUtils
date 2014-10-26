using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Security;
using System.ServiceModel;
using System.ServiceModel.Channels;
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
		private FieldBuilder _bindingField;
		/// <summary>
		/// The client field of our <typeparamref name="TClient"/> type.
		/// </summary>
		private FieldBuilder _clientField;
		/// <summary>
		/// The endpoint configuration name field, a string.
		/// </summary>
		private FieldBuilder _endpointConfigurationNameField;
		/// <summary>
		/// The password field, a SecureString.
		/// </summary>
		private FieldBuilder _passwordField;
		/// <summary>
		/// The remote address field, an EndpointAddress.
		/// </summary>
		private FieldBuilder _remoteAddressField;
		/// <summary>
		/// The username field, a string.
		/// </summary>
		private FieldBuilder _userNameField;

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
			// TODO
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
