using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Security;
using log4net;

namespace Entropa.WcfUtils {

	/// <summary>
	/// This emits a fault-safe proxy for a given contract interface.
	/// </summary>
	public sealed class FaultSafeProxyEmitter<TInterface> : FaultSafeEmitterBase<TInterface> where TInterface : class {

		/// <summary>
		/// The channel field name.
		/// </summary>
		private const string			CHANNEL_FIELD			= "_channel";
		/// <summary>
		/// The channel factory field name.
		/// </summary>
		private const string			FACTORY_FIELD			= "_factory";
		/// <summary>
		/// The exception message to be thrown if the channel factory object has no "Credentials" object (though it always should).
		/// </summary>
		private const string			NO_CREDENTIALS_MESSAGE	= "factory has no credentials";
		/// <summary>
		/// The logger.
		/// </summary>
		private readonly static ILog	_log					= LogManager.GetLogger( typeof( FaultSafeProxyEmitter<TInterface> ) );
		/// <summary>
		/// The Abort method.
		/// </summary>
		private MethodBuilder			_abortMethod;
		/// <summary>
		/// The channel field.
		/// </summary>
		private FieldBuilder			_channelField;
		/// <summary>
		/// The Close method.
		/// </summary>
		private MethodBuilder			_closeMethod;
		/// <summary>
		/// The channel factory field.
		/// </summary>
		private FieldBuilder			_factoryField;
		/// <summary>
		/// The GetChannel method.
		/// </summary>
		private MethodBuilder			_getChannelMethod;

		/// <summary>
		/// The construtor.
		/// </summary>
		private FaultSafeProxyEmitter() {
			// The private constructor prevents calling this class via anything but the Create method.
		}

		/// <summary>
		/// <see cref="FaultSafeEmitterBase{TInterface}.AssemblyName"/>
		/// </summary>
		protected override string AssemblyName {
			get { return "FaultSafeProxyAssembly"; }
		}

		/// <summary>
		/// This builds the "Abort" method which aborts the channel.
		/// </summary>
		/// <remarks><![CDATA[
		///   .method private hidebysig instance void 
		///           Abort() cil managed
		///   {
		///     // Code size       46 (0x2e)
		///     .maxstack  2
		///     .locals init ([0] class [System.ServiceModel]System.ServiceModel.IServiceChannel serviceChannel,
		///              [1] bool CS$4$0000)
		///     IL_0000:  nop
		///     IL_0001:  ldnull
		///     IL_0002:  ldarg.0
		///     IL_0003:  ldfld      class [ClientServer]ClientServer.IComplexMathContract Client.FaultSafeProxy::_channel
		///     IL_0008:  ceq
		///     IL_000a:  ldc.i4.0
		///     IL_000b:  ceq
		///     IL_000d:  stloc.1
		///     IL_000e:  ldloc.1
		///     IL_000f:  brtrue.s   IL_0013
		/// 
		///     IL_0011:  br.s       IL_002d
		/// 
		///     IL_0013:  ldarg.0
		///     IL_0014:  ldfld      class [ClientServer]ClientServer.IComplexMathContract Client.FaultSafeProxy::_channel
		///     IL_0019:  castclass  [System.ServiceModel]System.ServiceModel.IServiceChannel
		///     IL_001e:  stloc.0
		///     IL_001f:  ldloc.0
		///     IL_0020:  callvirt   instance void [System.ServiceModel]System.ServiceModel.ICommunicationObject::Abort()
		///     IL_0025:  nop
		///     IL_0026:  ldarg.0
		///     IL_0027:  ldnull
		///     IL_0028:  stfld      class [ClientServer]ClientServer.IComplexMathContract Client.FaultSafeProxy::_channel
		///     IL_002d:  ret
		///   } // end of method FaultSafeProxy::Abort
		/// 
		/// ]]></remarks>
		/// <param name="typeBuilder"></param>
		/// <returns></returns>
		private MethodBuilder BuildAbortMethod( TypeBuilder typeBuilder ) {
			MethodBuilder methodBuilder = typeBuilder.DefineMethod(
				"Abort",
				MethodAttributes.Private | MethodAttributes.HideBySig,
				CallingConventions.Standard,
				null,
				new Type[0]
				);
			ILGenerator il = methodBuilder.GetILGenerator();
			// Declare local variables
			il.DeclareLocal( typeof( IServiceChannel ) );
			il.DeclareLocal( typeof( bool ) );

			// Define some labels ahead of time
			Label label0013 = il.DefineLabel();
			Label label002D = il.DefineLabel();

			// Get some methods ahead of time
			MethodInfo communicationsObjectAbortMethodInfo = typeof( ICommunicationObject ).GetMethod( "Abort", new Type[0] );
			if ( null == communicationsObjectAbortMethodInfo )
				throw new InvalidOperationException( String.Format( "Could not find ICommunicationObject.Abort() method info" ) );

			//     IL_0000:  nop
			//     IL_0001:  ldnull
			//     IL_0002:  ldarg.0
			//     IL_0003:  ldfld      class [ClientServer]ClientServer.IComplexMathContract Client.FaultSafeProxy::_channel
			//     IL_0008:  ceq
			//     IL_000a:  ldc.i4.0
			//     IL_000b:  ceq
			//     IL_000d:  stloc.1
			//     IL_000e:  ldloc.1
			//     IL_000f:  brtrue.s   IL_0013

			il.Emit( OpCodes.Nop );
			il.Emit( OpCodes.Ldnull );
			il.Emit( OpCodes.Ldarg_0 );
			il.Emit( OpCodes.Ldfld, this._channelField );
			il.Emit( OpCodes.Ceq );
			il.Emit( OpCodes.Ldc_I4_0 );
			il.Emit( OpCodes.Ceq );
			il.Emit( OpCodes.Stloc_1 );
			il.Emit( OpCodes.Ldloc_1 );
			il.Emit( OpCodes.Brtrue_S, label0013 );
			// 
			//     IL_0011:  br.s       IL_002d
			il.Emit( OpCodes.Br_S, label002D );

			//     IL_0013:  ldarg.0
			//     IL_0014:  ldfld      class [ClientServer]ClientServer.IComplexMathContract Client.FaultSafeProxy::_channel
			//     IL_0019:  castclass  [System.ServiceModel]System.ServiceModel.IServiceChannel
			//     IL_001e:  stloc.0
			//     IL_001f:  ldloc.0
			//     IL_0020:  callvirt   instance void [System.ServiceModel]System.ServiceModel.ICommunicationObject::Abort()
			//     IL_0025:  nop
			//     IL_0026:  ldarg.0
			//     IL_0027:  ldnull
			//     IL_0028:  stfld      class [ClientServer]ClientServer.IComplexMathContract Client.FaultSafeProxy::_channel
			//     IL_002d:  ret
			il.MarkLabel( label0013 );
			il.Emit( OpCodes.Ldarg_0 );
			il.Emit( OpCodes.Ldfld, this._channelField );
			il.Emit( OpCodes.Castclass, typeof( IServiceChannel ) );
			il.Emit( OpCodes.Stloc_0 );
			il.Emit( OpCodes.Ldloc_0 );
			il.Emit( OpCodes.Callvirt, communicationsObjectAbortMethodInfo );
			il.Emit( OpCodes.Nop );
			il.Emit( OpCodes.Ldarg_0 );
			il.Emit( OpCodes.Ldnull );
			il.Emit( OpCodes.Stfld, this._channelField );
			il.MarkLabel( label002D );
			il.Emit( OpCodes.Ret );

			return methodBuilder;
		}

		/// <summary>
		/// Builds the channel field.
		/// </summary>
		/// <remarks><![CDATA[
		///   .field private class [ClientServer]ClientServer.IComplexMathContract _channel
		/// ]]></remarks>
		/// <param name="typeBuilder"></param>
		/// <returns></returns>
		private static FieldBuilder BuildChannelField( TypeBuilder typeBuilder ) {
			return typeBuilder.DefineField(
				CHANNEL_FIELD,
				typeof( TInterface ),
				FieldAttributes.Private
				);

		}

		/// <summary>
		/// This builds the "Close" method that closes the channel.
		/// </summary>
		/// <remarks><![CDATA[
		///   .method private hidebysig instance void 
		///           CloseChannel() cil managed
		///   {
		///     // Code size       46 (0x2e)
		///     .maxstack  2
		///     .locals init ([0] class [System.ServiceModel]System.ServiceModel.IServiceChannel serviceChannel,
		///              [1] bool CS$4$0000)
		///     IL_0000:  nop
		///     IL_0001:  ldnull
		///     IL_0002:  ldarg.0
		///     IL_0003:  ldfld      class [ClientServer]ClientServer.IComplexMathContract Client.FaultSafeProxy::_channel
		///     IL_0008:  ceq
		///     IL_000a:  ldc.i4.0
		///     IL_000b:  ceq
		///     IL_000d:  stloc.1
		///     IL_000e:  ldloc.1
		///     IL_000f:  brtrue.s   IL_0013
		/// 
		///     IL_0011:  br.s       IL_002d
		/// 
		///     IL_0013:  ldarg.0
		///     IL_0014:  ldfld      class [ClientServer]ClientServer.IComplexMathContract Client.FaultSafeProxy::_channel
		///     IL_0019:  castclass  [System.ServiceModel]System.ServiceModel.IServiceChannel
		///     IL_001e:  stloc.0
		///     IL_001f:  ldloc.0
		///     IL_0020:  callvirt   instance void [System.ServiceModel]System.ServiceModel.ICommunicationObject::Close()
		///     IL_0025:  nop
		///     IL_0026:  ldarg.0
		///     IL_0027:  ldnull
		///     IL_0028:  stfld      class [ClientServer]ClientServer.IComplexMathContract Client.FaultSafeProxy::_channel
		///     IL_002d:  ret
		///   } // end of method FaultSafeProxy::CloseChannel
		/// 		/// ]]></remarks>
		/// <param name="typeBuilder"></param>
		/// <returns></returns>
		private MethodBuilder BuildCloseMethod( TypeBuilder typeBuilder ) {
			MethodBuilder methodBuilder = typeBuilder.DefineMethod(
				"Close",
				MethodAttributes.Private | MethodAttributes.HideBySig,
				CallingConventions.Standard,
				null,
				new Type[0]
				);
			ILGenerator il = methodBuilder.GetILGenerator();
			// Declare local variables
			il.DeclareLocal( typeof( IServiceChannel ) );
			il.DeclareLocal( typeof( bool ) );

			// Define some labels ahead of time
			Label label0013 = il.DefineLabel();
			Label label002D = il.DefineLabel();

			// Get some methods ahead of time
			MethodInfo communicationsObjectCloseMethodInfo = typeof( ICommunicationObject ).GetMethod( "Close", new Type[0] );
			if ( null == communicationsObjectCloseMethodInfo )
				throw new InvalidOperationException( String.Format( "Could not find ICommunicationObject.Close() method info" ) );
			//     IL_0000:  nop
			//     IL_0001:  ldnull
			//     IL_0002:  ldarg.0
			//     IL_0003:  ldfld      class [ClientServer]ClientServer.IComplexMathContract Client.FaultSafeProxy::_channel
			//     IL_0008:  ceq
			//     IL_000a:  ldc.i4.0
			//     IL_000b:  ceq
			//     IL_000d:  stloc.1
			//     IL_000e:  ldloc.1
			//     IL_000f:  brtrue.s   IL_0013

			il.Emit( OpCodes.Nop );
			il.Emit( OpCodes.Ldnull );
			il.Emit( OpCodes.Ldarg_0 );
			il.Emit( OpCodes.Ldfld, this._channelField );
			il.Emit( OpCodes.Ceq );
			il.Emit( OpCodes.Ldc_I4_0 );
			il.Emit( OpCodes.Ceq );
			il.Emit( OpCodes.Stloc_1 );
			il.Emit( OpCodes.Ldloc_1 );
			il.Emit( OpCodes.Brtrue_S, label0013 );
			// 
			//     IL_0011:  br.s       IL_002d
			il.Emit( OpCodes.Br_S, label002D );

			//     IL_0013:  ldarg.0
			//     IL_0014:  ldfld      class [ClientServer]ClientServer.IComplexMathContract Client.FaultSafeProxy::_channel
			//     IL_0019:  castclass  [System.ServiceModel]System.ServiceModel.IServiceChannel
			//     IL_001e:  stloc.0
			//     IL_001f:  ldloc.0
			//     IL_0020:  callvirt   instance void [System.ServiceModel]System.ServiceModel.ICommunicationObject::Close()
			//     IL_0025:  nop
			//     IL_0026:  ldarg.0
			//     IL_0027:  ldnull
			//     IL_0028:  stfld      class [ClientServer]ClientServer.IComplexMathContract Client.FaultSafeProxy::_channel
			//     IL_002d:  ret
			il.MarkLabel( label0013 );
			il.Emit( OpCodes.Ldarg_0 );
			il.Emit( OpCodes.Ldfld, this._channelField );
			il.Emit( OpCodes.Castclass, typeof( IServiceChannel ) );
			il.Emit( OpCodes.Stloc_0 );
			il.Emit( OpCodes.Ldloc_0 );
			il.Emit( OpCodes.Callvirt, communicationsObjectCloseMethodInfo );
			il.Emit( OpCodes.Nop );
			il.Emit( OpCodes.Ldarg_0 );
			il.Emit( OpCodes.Ldnull );
			il.Emit( OpCodes.Stfld, this._channelField );
			il.MarkLabel( label002D );
			il.Emit( OpCodes.Ret );
			return methodBuilder;
		}

		/// <summary>
		/// Builds the authenticated (endpoint, username, password) constructor.
		/// </summary>
		/// <remarks><![CDATA[
		///   .method public hidebysig specialname rtspecialname 
		///           instance void  .ctor(string endpoint,
		///                                string userName,
		///                                string password) cil managed
		///   {
		///     // Code size       128 (0x80)
		///     .maxstack  2
		///     .locals init ([0] bool CS$4$0000)
		///     IL_0000:  ldarg.0
		///     IL_0001:  call       instance void [mscorlib]System.Object::.ctor()
		///     IL_0006:  nop
		///     IL_0007:  nop
		///     IL_0008:  ldarg.0
		///     IL_0009:  ldarg.1
		///     IL_000a:  newobj     instance void class [System.ServiceModel]System.ServiceModel.ChannelFactory`1<class [ClientServer]ClientServer.IComplexMathContract>::.ctor(string)
		///     IL_000f:  stfld      class [System.ServiceModel]System.ServiceModel.ChannelFactory`1<class [ClientServer]ClientServer.IComplexMathContract> Client.FaultSafeProxy::_factory
		///     IL_0014:  ldarg.2
		///     IL_0015:  call       bool [mscorlib]System.String::IsNullOrEmpty(string)
		///     IL_001a:  brtrue.s   IL_0027
		/// 
		///     IL_001c:  ldarg.3
		///     IL_001d:  call       bool [mscorlib]System.String::IsNullOrEmpty(string)
		///     IL_0022:  ldc.i4.0
		///     IL_0023:  ceq
		///     IL_0025:  br.s       IL_0028
		/// 
		///     IL_0027:  ldc.i4.0
		///     IL_0028:  nop
		///     IL_0029:  stloc.0
		///     IL_002a:  ldloc.0
		///     IL_002b:  brtrue.s   IL_0030
		/// 
		///     IL_002d:  nop
		///     IL_002e:  br.s       IL_007f
		/// 
		///     IL_0030:  ldnull
		///     IL_0031:  ldarg.0
		///     IL_0032:  ldfld      class [System.ServiceModel]System.ServiceModel.ChannelFactory`1<class [ClientServer]ClientServer.IComplexMathContract> Client.FaultSafeProxy::_factory
		///     IL_0037:  callvirt   instance class [System.ServiceModel]System.ServiceModel.Description.ClientCredentials [System.ServiceModel]System.ServiceModel.ChannelFactory::get_Credentials()
		///     IL_003c:  ceq
		///     IL_003e:  ldc.i4.0
		///     IL_003f:  ceq
		///     IL_0041:  stloc.0
		///     IL_0042:  ldloc.0
		///     IL_0043:  brtrue.s   IL_0050
		/// 
		///     IL_0045:  ldstr      "factory has no credentials"
		///     IL_004a:  newobj     instance void [mscorlib]System.InvalidOperationException::.ctor(string)
		///     IL_004f:  throw
		/// 
		///     IL_0050:  ldarg.0
		///     IL_0051:  ldfld      class [System.ServiceModel]System.ServiceModel.ChannelFactory`1<class [ClientServer]ClientServer.IComplexMathContract> Client.FaultSafeProxy::_factory
		///     IL_0056:  callvirt   instance class [System.ServiceModel]System.ServiceModel.Description.ClientCredentials [System.ServiceModel]System.ServiceModel.ChannelFactory::get_Credentials()
		///     IL_005b:  callvirt   instance class [System.ServiceModel]System.ServiceModel.Security.UserNamePasswordClientCredential [System.ServiceModel]System.ServiceModel.Description.ClientCredentials::get_UserName()
		///     IL_0060:  ldarg.2
		///     IL_0061:  callvirt   instance void [System.ServiceModel]System.ServiceModel.Security.UserNamePasswordClientCredential::set_UserName(string)
		///     IL_0066:  nop
		///     IL_0067:  ldarg.0
		///     IL_0068:  ldfld      class [System.ServiceModel]System.ServiceModel.ChannelFactory`1<class [ClientServer]ClientServer.IComplexMathContract> Client.FaultSafeProxy::_factory
		///     IL_006d:  callvirt   instance class [System.ServiceModel]System.ServiceModel.Description.ClientCredentials [System.ServiceModel]System.ServiceModel.ChannelFactory::get_Credentials()
		///     IL_0072:  callvirt   instance class [System.ServiceModel]System.ServiceModel.Security.UserNamePasswordClientCredential [System.ServiceModel]System.ServiceModel.Description.ClientCredentials::get_UserName()
		///     IL_0077:  ldarg.3
		///     IL_0078:  callvirt   instance void [System.ServiceModel]System.ServiceModel.Security.UserNamePasswordClientCredential::set_Password(string)
		///     IL_007d:  nop
		///     IL_007e:  nop
		///     IL_007f:  ret
		///   } // end of method FaultSafeProxy::.ctor
		/// 
		/// ]]></remarks>
		/// <param name="typeBuilder"></param>
		private ConstructorBuilder BuildConstructorAuthenticated( TypeBuilder typeBuilder ) {
			ConstructorBuilder constructorBuilder = typeBuilder.DefineConstructor(
				MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName,
				CallingConventions.Standard,
				new[] { typeof( String ), typeof( String ), typeof( String ) }
				);
			ILGenerator il = constructorBuilder.GetILGenerator();
			// Define the labels we'll need ahead of time
			Label label0027 = il.DefineLabel();
			Label label0028 = il.DefineLabel();
			Label label0030 = il.DefineLabel();
			Label label0050 = il.DefineLabel();
			Label label007F = il.DefineLabel();

			// Get the methods we'll need ahead of time
			ConstructorInfo invalidOperationExceptionConstructorInfo = typeof( InvalidOperationException ).GetConstructor( new[] { typeof( String ) } );
			if ( null == invalidOperationExceptionConstructorInfo )
				throw new InvalidOperationException( String.Format( "Could not find InvalidOperationException.ctor( string ) constructor info" ) );

			MethodInfo stringIsNullOrEmptyMethodInfo = typeof( String ).GetMethod( "IsNullOrEmpty", new[] { typeof( String ) } );
			if ( null == stringIsNullOrEmptyMethodInfo )
				throw new InvalidOperationException( String.Format( "Could not find String.IsNullOrEmpty( string ) method info" ) );

			ConstructorInfo objectConstructorInfo = typeof( Object ).GetConstructor( new Type[0] );
			if ( null == objectConstructorInfo )
				throw new InvalidOperationException( String.Format( "Could not find Object.ctor() constructor info" ) );

			ConstructorInfo factoryConstructorInfo = typeof( ChannelFactory<TInterface> ).GetConstructor( new[] { typeof( String ) } );
			if ( null == factoryConstructorInfo )
				throw new InvalidOperationException( String.Format( "Could not find ChannelFactory<T>.ctor( string ) constructor info" ) );

			MethodInfo factoryCredentialsGetterMethodInfo = typeof( ChannelFactory<TInterface> ).GetProperty( "Credentials", typeof( ClientCredentials ) ).GetGetMethod();
			if ( null == factoryCredentialsGetterMethodInfo )
				throw new InvalidOperationException( String.Format( "Could not find ChannelFactory<T>.get_Credentials method info" ) );

			MethodInfo clientCredientialsUserNameGetterMethodInfo = typeof( ClientCredentials ).GetProperty( "UserName", typeof( UserNamePasswordClientCredential ) ).GetGetMethod();
			if ( null == clientCredientialsUserNameGetterMethodInfo )
				throw new InvalidOperationException( String.Format( "Could not find ClientCredentials.get_UserName method info" ) );

			MethodInfo credentialsUserNameSetterMethodInfo = typeof( UserNamePasswordClientCredential ).GetProperty( "UserName", typeof( String ) ).GetSetMethod();
			if ( null == credentialsUserNameSetterMethodInfo )
				throw new InvalidOperationException( String.Format( "Could not find UserNamePasswordClientCredential.set_UserName method info" ) );

			MethodInfo credentialsPasswordSetterMethodInfo = typeof( UserNamePasswordClientCredential ).GetProperty( "Password", typeof( String ) ).GetSetMethod();
			if ( null == credentialsPasswordSetterMethodInfo )
				throw new InvalidOperationException( String.Format( "Could not find UserNamePasswordClientCredential.set_Password method info" ) );

			//     .locals init ([0] bool CS$4$0000)
			il.DeclareLocal( typeof( bool ) );

			//     IL_0000:  ldarg.0
			//     IL_0001:  call       instance void [mscorlib]System.Object::.ctor()
			//     IL_0006:  nop
			//     IL_0007:  nop
			il.Emit( OpCodes.Ldarg_0 );
			il.Emit( OpCodes.Call, objectConstructorInfo );
			il.Emit( OpCodes.Nop );
			il.Emit( OpCodes.Nop );

			//     IL_0008:  ldarg.0
			//     IL_0009:  ldarg.1
			//     IL_000a:  newobj     instance void class [System.ServiceModel]System.ServiceModel.ChannelFactory`1<class [ClientServer]ClientServer.IComplexMathContract>::.ctor(string)
			//     IL_000f:  stfld      class [System.ServiceModel]System.ServiceModel.ChannelFactory`1<class [ClientServer]ClientServer.IComplexMathContract> Client.FaultSafeProxy::_factory
			il.Emit( OpCodes.Ldarg_0 );
			il.Emit( OpCodes.Ldarg_1 );
			il.Emit( OpCodes.Newobj, factoryConstructorInfo );
			il.Emit( OpCodes.Stfld, this._factoryField );

			//     IL_0014:  ldarg.2
			//     IL_0015:  call       bool [mscorlib]System.String::IsNullOrEmpty(string)
			//     IL_001a:  brtrue.s   IL_0027
			il.Emit( OpCodes.Ldarg_2 );
			il.Emit( OpCodes.Call, stringIsNullOrEmptyMethodInfo );
			il.Emit( OpCodes.Brtrue_S, label0027 );

			//     IL_001c:  ldarg.3
			//     IL_001d:  call       bool [mscorlib]System.String::IsNullOrEmpty(string)
			//     IL_0022:  ldc.i4.0
			//     IL_0023:  ceq
			//     IL_0025:  br.s       IL_0028
			il.Emit( OpCodes.Ldarg_3 );
			il.Emit( OpCodes.Call, stringIsNullOrEmptyMethodInfo );
			il.Emit( OpCodes.Ldc_I4_0 );
			il.Emit( OpCodes.Ceq );
			il.Emit( OpCodes.Br_S, label0028 );
			// 
			//     IL_0027:  ldc.i4.0
			//     IL_0028:  nop
			//     IL_0029:  stloc.0
			//     IL_002a:  ldloc.0
			//     IL_002b:  brtrue.s   IL_0030
			//     IL_002d:  nop
			//     IL_002e:  br.s       IL_007f
			il.MarkLabel( label0027 );
			il.Emit( OpCodes.Ldc_I4_0 );
			il.MarkLabel( label0028 );
			il.Emit( OpCodes.Nop );
			il.Emit( OpCodes.Stloc_0 );
			il.Emit( OpCodes.Ldloc_0 );
			il.Emit( OpCodes.Brtrue_S, label0030 );
			il.Emit( OpCodes.Nop );
			il.Emit( OpCodes.Br_S, label007F );

			//     IL_0030:  ldnull
			//     IL_0031:  ldarg.0
			//     IL_0032:  ldfld      class [System.ServiceModel]System.ServiceModel.ChannelFactory`1<class [ClientServer]ClientServer.IComplexMathContract> Client.FaultSafeProxy::_factory
			//     IL_0037:  callvirt   instance class [System.ServiceModel]System.ServiceModel.Description.ClientCredentials [System.ServiceModel]System.ServiceModel.ChannelFactory::get_Credentials()
			//     IL_003c:  ceq
			//     IL_003e:  ldc.i4.0
			//     IL_003f:  ceq
			//     IL_0041:  stloc.0
			//     IL_0042:  ldloc.0
			//     IL_0043:  brtrue.s   IL_0050
			il.MarkLabel( label0030 );
			il.Emit( OpCodes.Ldnull );
			il.Emit( OpCodes.Ldarg_0 );
			il.Emit( OpCodes.Ldfld, this._factoryField );
			il.Emit( OpCodes.Callvirt, factoryCredentialsGetterMethodInfo );
			il.Emit( OpCodes.Ceq );
			il.Emit( OpCodes.Ldc_I4_0 );
			il.Emit( OpCodes.Ceq );
			il.Emit( OpCodes.Stloc_0 );
			il.Emit( OpCodes.Ldloc_0 );
			il.Emit( OpCodes.Brtrue_S, label0050 );

			// 
			//     IL_0045:  ldstr      "factory has no credentials"
			//     IL_004a:  newobj     instance void [mscorlib]System.InvalidOperationException::.ctor(string)
			//     IL_004f:  throw
			il.Emit( OpCodes.Ldstr, NO_CREDENTIALS_MESSAGE );
			il.Emit( OpCodes.Newobj, invalidOperationExceptionConstructorInfo );
			il.Emit( OpCodes.Throw );


			// 
			//     IL_0050:  ldarg.0
			//     IL_0051:  ldfld      class [System.ServiceModel]System.ServiceModel.ChannelFactory`1<class [ClientServer]ClientServer.IComplexMathContract> Client.FaultSafeProxy::_factory
			//     IL_0056:  callvirt   instance class [System.ServiceModel]System.ServiceModel.Description.ClientCredentials [System.ServiceModel]System.ServiceModel.ChannelFactory::get_Credentials()
			//     IL_005b:  callvirt   instance class [System.ServiceModel]System.ServiceModel.Security.UserNamePasswordClientCredential [System.ServiceModel]System.ServiceModel.Description.ClientCredentials::get_UserName()
			//     IL_0060:  ldarg.2
			//     IL_0061:  callvirt   instance void [System.ServiceModel]System.ServiceModel.Security.UserNamePasswordClientCredential::set_UserName(string)
			//     IL_0066:  nop
			//     IL_0067:  ldarg.0
			//     IL_0068:  ldfld      class [System.ServiceModel]System.ServiceModel.ChannelFactory`1<class [ClientServer]ClientServer.IComplexMathContract> Client.FaultSafeProxy::_factory
			//     IL_006d:  callvirt   instance class [System.ServiceModel]System.ServiceModel.Description.ClientCredentials [System.ServiceModel]System.ServiceModel.ChannelFactory::get_Credentials()
			//     IL_0072:  callvirt   instance class [System.ServiceModel]System.ServiceModel.Security.UserNamePasswordClientCredential [System.ServiceModel]System.ServiceModel.Description.ClientCredentials::get_UserName()
			//     IL_0077:  ldarg.3
			//     IL_0078:  callvirt   instance void [System.ServiceModel]System.ServiceModel.Security.UserNamePasswordClientCredential::set_Password(string)
			il.MarkLabel( label0050 );
			il.Emit( OpCodes.Ldarg_0 );
			il.Emit( OpCodes.Ldfld, this._factoryField );
			il.Emit( OpCodes.Callvirt, factoryCredentialsGetterMethodInfo );
			il.Emit( OpCodes.Callvirt, typeof( ClientCredentials ).GetProperty( "UserName", typeof( UserNamePasswordClientCredential ) ).GetGetMethod() );
			il.Emit( OpCodes.Ldarg_2 );
			il.Emit( OpCodes.Callvirt, typeof( UserNamePasswordClientCredential ).GetProperty( "UserName", typeof( String ) ).GetSetMethod() );
			il.Emit( OpCodes.Nop );
			il.Emit( OpCodes.Ldarg_0 );
			il.Emit( OpCodes.Ldfld, this._factoryField );
			il.Emit( OpCodes.Callvirt, typeof( ChannelFactory<TInterface> ).GetProperty( "Credentials", typeof( ClientCredentials ) ).GetGetMethod() );
			il.Emit( OpCodes.Callvirt, typeof( ClientCredentials ).GetProperty( "UserName", typeof( UserNamePasswordClientCredential ) ).GetGetMethod() );
			il.Emit( OpCodes.Ldarg_3 );
			il.Emit( OpCodes.Callvirt, typeof( UserNamePasswordClientCredential ).GetProperty( "Password", typeof( String ) ).GetSetMethod() );

			//     IL_007d:  nop
			//     IL_007e:  nop
			//     IL_007f:  ret
			il.Emit( OpCodes.Nop );
			il.Emit( OpCodes.Nop );
			il.MarkLabel( label007F );
			il.Emit( OpCodes.Ret );
			return constructorBuilder;
		}

		/// <summary>
		/// Builds the unauthenticated (endpoint only) constructor.
		/// </summary>
		/// <remarks><![CDATA[
		///  .method public hidebysig specialname rtspecialname 
		///          instance void  .ctor(string endpoint) cil managed
		///  {
		///    // Code size       13 (0xd)
		///    .maxstack  8
		///   IL_0000:  ldarg.0
		///   IL_0001:  ldarg.1
		///   IL_0002:  ldnull
		///   IL_0003:  ldnull
		///   IL_0004:  call       instance void Client.FaultSafeProxy::.ctor(string,
		///                                                                   string,
		///                                                                   string)
		///   IL_0009:  nop
		///    IL_000a:  nop
		///   IL_000b:  nop
		///   IL_000c:  ret
		/// } // end of method FaultSafeProxy::.ctor
		/// ]]></remarks>
		/// <param name="typeBuilder"></param>
		/// <param name="defaultConstructorBuilder"></param>
		private static void BuildConstructorUnauthenticated( TypeBuilder typeBuilder, ConstructorBuilder defaultConstructorBuilder ) {
			ConstructorBuilder constructorBuilder = typeBuilder.DefineConstructor(
				MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName,
				CallingConventions.Standard,
				new[] { typeof( String ) }
				);
			ILGenerator il = constructorBuilder.GetILGenerator();
			il.Emit( OpCodes.Ldarg_0 );
			il.Emit( OpCodes.Ldarg_1 );
			il.Emit( OpCodes.Ldnull );
			il.Emit( OpCodes.Ldnull );
			il.Emit( OpCodes.Call, defaultConstructorBuilder );
			il.Emit( OpCodes.Nop );
			il.Emit( OpCodes.Nop );
			il.Emit( OpCodes.Nop );
			il.Emit( OpCodes.Ret );
		}

		/// <summary>
		/// Builds the Dispose method.
		/// </summary>
		/// <remarks><![CDATA[
		///   .method public hidebysig newslot virtual final 
		///           instance void  Dispose() cil managed
		///   {
		///     // Code size       9 (0x9)
		///     .maxstack  8
		///     IL_0000:  nop
		///     IL_0001:  ldarg.0
		///     IL_0002:  call       instance void Client.FaultSafeProxy::CloseChannel()
		///     IL_0007:  nop
		///     IL_0008:  ret
		///   } // end of method FaultSafeProxy::Dispose
		/// ]]></remarks>
		/// <param name="typeBuilder"></param>
		private void BuildDisposeMethod( TypeBuilder typeBuilder ) {

			MethodBuilder methodBuilder = typeBuilder.DefineMethod(
				"Dispose",
				MethodAttributes.Private | MethodAttributes.HideBySig | MethodAttributes.NewSlot | MethodAttributes.Virtual | MethodAttributes.Final,
				CallingConventions.Standard,
				null,
				new Type[0]
				);
			ILGenerator il = methodBuilder.GetILGenerator();
			il.Emit( OpCodes.Nop );
			il.Emit( OpCodes.Ldarg_0 );
			il.Emit( OpCodes.Call, this._closeMethod );
			il.Emit( OpCodes.Nop );
			il.Emit( OpCodes.Ret );
			// Set that this overrides IDisposable.Dispose
			typeBuilder.DefineMethodOverride( methodBuilder, typeof( IDisposable ).GetMethod( "Dispose", new Type[0] ) );
		}

		/// <summary>
		/// Builds the factory field.
		/// </summary>
		/// <remarks><![CDATA[
		///   .field private initonly class [System.ServiceModel]System.ServiceModel.ChannelFactory`1<class [ClientServer]ClientServer.IComplexMathContract> _factory
		/// ]]></remarks>
		/// <param name="typeBuilder"></param>
		/// <returns></returns>
		private static FieldBuilder BuildFactoryField( TypeBuilder typeBuilder ) {
			return typeBuilder.DefineField(
				FACTORY_FIELD,
				typeof( ChannelFactory<TInterface> ),
				FieldAttributes.Private
				);
		}

		/// <summary>
		/// This builds the "GetChannel" method.
		/// </summary>
		/// <remarks><![CDATA[
		///   .method public hidebysig instance class [ClientServer]ClientServer.IComplexMathContract 
		///           GetChannel() cil managed
		///   {
		///     // Code size       47 (0x2f)
		///     .maxstack  2
		///     .locals init ([0] class [ClientServer]ClientServer.IComplexMathContract CS$1$0000,
		///              [1] bool CS$4$0001)
		///     IL_0000:  nop
		///     IL_0001:  ldnull
		///     IL_0002:  ldarg.0
		///     IL_0003:  ldfld      class [ClientServer]ClientServer.IComplexMathContract Client.FaultSafeProxy::_channel
		///     IL_0008:  ceq
		///     IL_000a:  ldc.i4.0
		///     IL_000b:  ceq
		///     IL_000d:  stloc.1
		///     IL_000e:  ldloc.1
		///     IL_000f:  brtrue.s   IL_0024
		/// 
		///     IL_0011:  nop
		///     IL_0012:  ldarg.0
		///     IL_0013:  ldarg.0
		///     IL_0014:  ldfld      class [System.ServiceModel]System.ServiceModel.ChannelFactory`1<class [ClientServer]ClientServer.IComplexMathContract> Client.FaultSafeProxy::_factory
		///     IL_0019:  callvirt   instance !0 class [System.ServiceModel]System.ServiceModel.ChannelFactory`1<class [ClientServer]ClientServer.IComplexMathContract>::CreateChannel()
		///     IL_001e:  stfld      class [ClientServer]ClientServer.IComplexMathContract Client.FaultSafeProxy::_channel
		///     IL_0023:  nop
		///     IL_0024:  ldarg.0
		///     IL_0025:  ldfld      class [ClientServer]ClientServer.IComplexMathContract Client.FaultSafeProxy::_channel
		///     IL_002a:  stloc.0
		///     IL_002b:  br.s       IL_002d
		/// 
		///     IL_002d:  ldloc.0
		///     IL_002e:  ret
		///   } // end of method FaultSafeProxy::GetChannel
		/// 		/// ]]></remarks>
		/// <param name="typeBuilder"></param>
		/// <returns></returns>
		private MethodBuilder BuildGetChannelMethod( TypeBuilder typeBuilder ) {
			MethodBuilder methodBuilder = typeBuilder.DefineMethod(
				"GetChannel",
				MethodAttributes.Private | MethodAttributes.HideBySig,
				CallingConventions.Standard,
				typeof( TInterface ),
				new Type[0]
				);
			ILGenerator il = methodBuilder.GetILGenerator();
			// Declare local variables
			il.DeclareLocal( typeof( TInterface ) );
			il.DeclareLocal( typeof( bool ) );

			// Define some labels ahead of time
			Label label0024 = il.DefineLabel();
			Label label002D = il.DefineLabel();

			// Get some methods ahead of time
			MethodInfo channelFactoryCreateChannelMethodInfo = typeof( ChannelFactory<TInterface> ).GetMethod( "CreateChannel", new Type[0] );
			if ( null == channelFactoryCreateChannelMethodInfo )
				throw new InvalidOperationException( String.Format( "Could not find ChannelFactory<T>.CreateChannel() method info" ) );

			//     IL_0000:  nop
			//     IL_0001:  ldnull
			//     IL_0002:  ldarg.0
			//     IL_0003:  ldfld      class [ClientServer]ClientServer.IComplexMathContract Client.FaultSafeProxy::_channel
			//     IL_0008:  ceq
			//     IL_000a:  ldc.i4.0
			//     IL_000b:  ceq
			//     IL_000d:  stloc.1
			//     IL_000e:  ldloc.1
			//     IL_000f:  brtrue.s   IL_0024
			il.Emit( OpCodes.Nop );
			il.Emit( OpCodes.Ldnull );
			il.Emit( OpCodes.Ldarg_0 );
			il.Emit( OpCodes.Ldfld, this._channelField );
			il.Emit( OpCodes.Ceq );
			il.Emit( OpCodes.Ldc_I4_0 );
			il.Emit( OpCodes.Ceq );
			il.Emit( OpCodes.Stloc_1 );
			il.Emit( OpCodes.Ldloc_1 );
			il.Emit( OpCodes.Brtrue_S, label0024 );

			//     IL_0011:  nop
			//     IL_0012:  ldarg.0
			//     IL_0013:  ldarg.0
			//     IL_0014:  ldfld      class [System.ServiceModel]System.ServiceModel.ChannelFactory`1<class [ClientServer]ClientServer.IComplexMathContract> Client.FaultSafeProxy::_factory
			//     IL_0019:  callvirt   instance !0 class [System.ServiceModel]System.ServiceModel.ChannelFactory`1<class [ClientServer]ClientServer.IComplexMathContract>::CreateChannel()
			//     IL_001e:  stfld      class [ClientServer]ClientServer.IComplexMathContract Client.FaultSafeProxy::_channel
			//     IL_0023:  nop
			//     IL_0024:  ldarg.0
			//     IL_0025:  ldfld      class [ClientServer]ClientServer.IComplexMathContract Client.FaultSafeProxy::_channel
			//     IL_002a:  stloc.0
			//     IL_002b:  br.s       IL_002d
			il.Emit( OpCodes.Nop );
			il.Emit( OpCodes.Ldarg_0 );
			il.Emit( OpCodes.Ldarg_0 );
			il.Emit( OpCodes.Ldfld, this._factoryField );
			il.Emit( OpCodes.Callvirt, channelFactoryCreateChannelMethodInfo );
			il.Emit( OpCodes.Stfld, this._channelField );
			il.Emit( OpCodes.Nop );
			il.MarkLabel( label0024 );
			il.Emit( OpCodes.Ldarg_0 );
			il.Emit( OpCodes.Ldfld, this._channelField );
			il.Emit( OpCodes.Stloc_0 );
			il.Emit( OpCodes.Br_S, label002D );

			//     IL_002d:  ldloc.0
			//     IL_002e:  ret
			il.MarkLabel( label002D );
			il.Emit( OpCodes.Ldloc_0 );
			il.Emit( OpCodes.Ret );
			return methodBuilder;
		}

		/// <summary>
		/// Builds the type.
		/// </summary>
		/// <param name="typeBuilder"></param>
		private void BuildType( TypeBuilder typeBuilder ) {
			// Add the member variables
			this._channelField = BuildChannelField( typeBuilder );
			this._factoryField = BuildFactoryField( typeBuilder );
			// Add the endpoint/username/password constructor
			ConstructorBuilder defaultConstructorBuilder = this.BuildConstructorAuthenticated( typeBuilder );
			// Add the endpoint constructor
			BuildConstructorUnauthenticated( typeBuilder, defaultConstructorBuilder );
			// Add the custom methods
			this._abortMethod = this.BuildAbortMethod( typeBuilder );
			this._closeMethod = this.BuildCloseMethod( typeBuilder );
			this._getChannelMethod = this.BuildGetChannelMethod( typeBuilder );
			// Implement IDisposable
			this.BuildDisposeMethod( typeBuilder );
			// Implement the interface's operation contract methods
			this.ImplementServiceContracts( typeBuilder );
		}

		/// <summary>
		/// This generates a fault safe proxy for the given service contract interface.
		/// </summary>
		/// <typeparam name="TInterface"></typeparam>
		/// <param name="endpoint">The endpoint name.</param>
		/// <param name="userName"></param>
		/// <param name="password"></param>
		/// <returns></returns>
		public static TInterface Create( string endpoint, string userName, string password ) {
			FaultSafeProxyEmitter<TInterface> emitter = new FaultSafeProxyEmitter<TInterface>();
			return emitter.CreateProxy( endpoint, userName, password );
		}

		/// <summary>
		/// This generates a fault safe proxy for the given type.
		/// </summary>
		/// <typeparam name="TInterface"></typeparam>
		/// <param name="endpoint">The endpoint name.</param>
		/// <returns></returns>
		public static TInterface Create( string endpoint ) {
			return Create( endpoint, null, null );
		}

		/// <summary>
		/// Creates the proxy and returns it.
		/// </summary>
		/// <param name="endpoint"></param>
		/// <param name="userName"></param>
		/// <param name="password"></param>
		/// <returns></returns>
		private TInterface CreateProxy( string endpoint, string userName, string password ) {
			_log.DebugFormat( "Create - endpoint = '{0}', userName = '{1}'", endpoint, userName );
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
			// Create an instance of the type
			object instance = Activator.CreateInstance( generatedType, endpoint, userName, password );
			_log.DebugFormat( " - created instance '{0}'", instance );
			_log.DebugFormat( " - created IDisposable '{0}'", instance as IDisposable );
			_log.DebugFormat( " - created T '{0}'", instance as TInterface );
			// Return the created instance
			return instance as TInterface;
		}

		/// <summary>
		/// Returns an array of the types of the parameters for a given method.
		/// </summary>
		/// <param name="method"></param>
		/// <returns></returns>
		private static Type[] GetParameterTypes( MethodInfo method ) {
			ParameterInfo[] parameters = method.GetParameters();
			Type[] parameterTypes = new Type[parameters.Length];
			for ( int i = 0; i < parameters.Length; i++ ) {
				ParameterInfo parameter = parameters[i];
				parameterTypes[i] = parameter.ParameterType;
			}
			return parameterTypes;
		}

		/// <summary>
		/// Implements the given operation contract method.
		/// </summary>
		/// <remarks><![CDATA[
		/// Note: The following is an example.  There are changes based on whether the object takes parameters or returns a value.
		///  .method public hidebysig newslot virtual final 
		///          instance string  MultipleParameterTypes(string a,
		///                                                  object b,
		///                                                  valuetype [mscorlib]System.DateTime c,
		///                                                  valuetype [mscorlib]System.TimeSpan d,
		///                                                  valuetype [mscorlib]System.Nullable`1<float64> e,
		///                                                  valuetype [mscorlib]System.Nullable`1<int32> f) cil managed
		///  {
		///    // Code size       39 (0x27)
		///    .maxstack  7
		///    .locals init ([0] string CS$1$0000)
		///    IL_0000:  nop
		///    .try
		///    {
		///      IL_0001:  nop
		///      IL_0002:  ldarg.0
		///      IL_0003:  call       instance class TestProxyGenerator.Prototype.IMockContract TestProxyGenerator.Prototype.FaultSafeProxy::GetChannel()
		///      IL_0008:  ldarg.1
		///      IL_0009:  ldarg.2
		///      IL_000a:  ldarg.3
		///      IL_000b:  ldarg.s    d
		///      IL_000d:  ldarg.s    e
		///      IL_000f:  ldarg.s    f
		///      IL_0011:  callvirt   instance string TestProxyGenerator.Prototype.IMockContract::MultipleParameterTypes(string,
		///                                                                                                              object,
		///                                                                                                              valuetype [mscorlib]System.DateTime,
		///                                                                                                              valuetype [mscorlib]System.TimeSpan,
		///                                                                                                              valuetype [mscorlib]System.Nullable`1<float64>,
		///                                                                                                              valuetype [mscorlib]System.Nullable`1<int32>)
		///      IL_0016:  stloc.0
		///      IL_0017:  leave.s    IL_0024
		///
		///    }  // end .try
		///    catch [mscorlib]System.Exception 
		///    {
		///      IL_0019:  pop
		///      IL_001a:  nop
		///      IL_001b:  ldarg.0
		///      IL_001c:  call       instance void TestProxyGenerator.Prototype.FaultSafeProxy::Abort()
		///      IL_0021:  nop
		///      IL_0022:  rethrow
		///    }  // end handler
		///    IL_0024:  nop
		///    IL_0025:  ldloc.0
		///    IL_0026:  ret
		///  } // end of method FaultSafeProxy::MultipleParameterTypes
		/// 
		/// 
		/// 
		/// ***************************************************************
		/// This method takes no paremeters and returns no values:
		/// 
		///  .method public hidebysig newslot virtual final 
		///          instance void  NoParameterNoReturn() cil managed
		///  {
		///    // Code size       30 (0x1e)
		///    .maxstack  1
		///    IL_0000:  nop
		///    .try
		///    {
		///      IL_0001:  nop
		///      IL_0002:  ldarg.0
		///      IL_0003:  call       instance class TestProxyGenerator.Prototype.IMockContract TestProxyGenerator.Prototype.FaultSafeProxy::GetChannel()
		///      IL_0008:  callvirt   instance void TestProxyGenerator.Prototype.IMockContract::NoParameterNoReturn()
		///      IL_000d:  nop
		///      IL_000e:  nop
		///      IL_000f:  leave.s    IL_001c
		///
		///    }  // end .try
		///    catch [mscorlib]System.Exception 
		///    {
		///      IL_0011:  pop
		///      IL_0012:  nop
		///      IL_0013:  ldarg.0
		///      IL_0014:  call       instance void TestProxyGenerator.Prototype.FaultSafeProxy::Abort()
		///      IL_0019:  nop
		///      IL_001a:  rethrow
		///    }  // end handler
		///    IL_001c:  nop
		///    IL_001d:  ret
		///  } // end of method FaultSafeProxy::NoParameterNoReturn
		/// ]]></remarks>
		/// <param name="typeBuilder"></param>
		/// <param name="type"></param>
		/// <param name="method"></param>
		private void ImplementOperationContract( TypeBuilder typeBuilder, Type type, MethodInfo method ) {
			// Get the parameter types
			Type[] parameterTypes = GetParameterTypes( method );
			_log.DebugFormat( "ImplementOperationContract - type = {0}, method = {1}, return type = {2}, {3} parameters", type, method.Name, method.ReturnType, parameterTypes.Length );
			// Figure out if we have a return type
			bool hasReturnType = !String.Equals( method.ReturnType.FullName, "System.Void", StringComparison.InvariantCultureIgnoreCase );
			// Create the method
			MethodBuilder methodBuilder = typeBuilder.DefineMethod(
				method.Name,
				MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.NewSlot | MethodAttributes.Virtual | MethodAttributes.Final,
				CallingConventions.Standard,
				method.ReturnType,
				parameterTypes
				);
			// Get the IL generator
			ILGenerator il = methodBuilder.GetILGenerator();
			// Declare some labels
			Label endLabel = il.DefineLabel();
			// If there's a return type...
			if ( hasReturnType ) {
				// Declare a local variable for it
				//    .locals init ([0] string CS$1$0000)
				il.DeclareLocal( method.ReturnType );
			}
			//    IL_0000:  nop
			il.Emit( OpCodes.Nop );

			// Start the "try" and get the channel
			//    .try
			//    {
			//      IL_0001:  nop
			//      IL_0002:  ldarg.0
			//      IL_0003:  call       instance class TestProxyGenerator.Prototype.IMockContract TestProxyGenerator.Prototype.FaultSafeProxy::GetChannel()
			il.BeginExceptionBlock();
			il.Emit( OpCodes.Nop );
			il.Emit( OpCodes.Ldarg_0 );
			il.Emit( OpCodes.Call, this._getChannelMethod );

			// Load all the types we'll need.  We have a built-in 3 and more than three have to be added 
			//      IL_0008:  ldarg.1
			if ( parameterTypes.Length > 0 ) {
				il.Emit( OpCodes.Ldarg_1 );
			}
			//      IL_0009:  ldarg.2
			if ( parameterTypes.Length > 1 ) {
				il.Emit( OpCodes.Ldarg_2 );
			}
			//      IL_000a:  ldarg.3
			if ( parameterTypes.Length > 2 ) {
				il.Emit( OpCodes.Ldarg_3 );
			}
			//      IL_000b:  ldarg.s    d
			//      IL_000d:  ldarg.s    e
			//      IL_000f:  ldarg.s    f
			if ( parameterTypes.Length > 3 ) {
				for ( int i = 3; i < parameterTypes.Length; i++ ) {
					il.Emit( OpCodes.Ldarg_S, (byte)i + 1 );
				}
			}
			// Call the target method
			//      IL_0011:  callvirt   instance string TestProxyGenerator.Prototype.IMockContract::MyMethod(string)
			il.Emit( OpCodes.Callvirt, method );

			// If there's a return type...
			if ( hasReturnType ) {
				//      IL_0016:  stloc.0
				il.Emit( OpCodes.Stloc_0 );
			} else {
				//      IL_000d:  nop
				//      IL_000e:  nop
				il.Emit( OpCodes.Nop );
				il.Emit( OpCodes.Nop );
			}
			//      IL_000f:  leave.s    IL_001c
			il.Emit( OpCodes.Leave_S, endLabel );
			//    }  // end .try
			//    catch [mscorlib]System.Exception 
			//    {
			il.BeginCatchBlock( typeof( Exception ) );
			// Abort the channel and rethrow the exception
			//      IL_0011:  pop
			//      IL_0012:  nop
			//      IL_0013:  ldarg.0
			//      IL_0014:  call       instance void TestProxyGenerator.Prototype.FaultSafeProxy::Abort()
			//      IL_0019:  nop
			//      IL_001a:  rethrow
			il.Emit( OpCodes.Pop );
			il.Emit( OpCodes.Nop );
			il.Emit( OpCodes.Ldarg_0 );
			il.Emit( OpCodes.Call, this._abortMethod );
			il.Emit( OpCodes.Nop );
			il.Emit( OpCodes.Rethrow );
			//    }  // end handler
			il.EndExceptionBlock();

			//    IL_0024:  nop
			il.Emit( OpCodes.Nop );
			il.MarkLabel( endLabel );
			if ( hasReturnType ) {
				//    IL_0025:  ldloc.0
				il.Emit( OpCodes.Ldloc_0 );
			}
			//    IL_0026:  ret
			il.Emit( OpCodes.Ret );
			// The method overrides the method in the base type
			//			typeBuilder.DefineMethodOverride( methodBuilder, method );
		}

		/// <summary>
		/// Implements the operation contracts in the type.
		/// </summary>
		/// <param name="typeBuilder"></param>
		/// <param name="types"></param>
		private void ImplementOperationContracts( TypeBuilder typeBuilder, IEnumerable<Type> types ) {
			foreach ( Type type in types ) {
				this.ImplementOperationContracts( typeBuilder, type );
			}
		}

		/// <summary>
		/// Builds methods for the type's operation contracts.
		/// </summary>
		/// <param name="typeBuilder"></param>
		/// <param name="type"></param>
		private void ImplementOperationContracts( TypeBuilder typeBuilder, Type type ) {
			_log.DebugFormat( "ImplementOperationContracts - type = '{0}'", type );
			// For each method on the type...
			MethodInfo[] methods = type.GetMethods();
			foreach ( MethodInfo method in methods ) {
				// If the method is an operation contract...
				if ( null != method.GetCustomAttributes( typeof( OperationContractAttribute ) ) ) {
					_log.DebugFormat( " - got operation contract '{0}'", method.Name );
					// Implement it in the proxy
					this.ImplementOperationContract( typeBuilder, type, method );

				}
			}
		}

		/// <summary>
		/// Builds methods for each operation contract in the interface.
		/// </summary>
		/// <param name="typeBuilder"></param>
		private void ImplementServiceContracts( TypeBuilder typeBuilder ) {
			// Get all the interface types on our contract type
			IEnumerable<Type> interfaceTypes = GetServiceContractTypes( typeof( TInterface ) );
			// Implement the service contracts starting with our given type
			this.ImplementOperationContracts( typeBuilder, interfaceTypes );
		}

		/// <summary>
		/// <see cref="FaultSafeEmitterBase{TInterface}.TypeName"/>
		/// </summary>
		protected override string TypeName {
			get { return "FaultSafeProxy"; }
		}
	}
}
