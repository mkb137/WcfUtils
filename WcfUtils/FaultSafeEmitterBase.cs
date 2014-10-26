using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.ServiceModel;
using log4net;

namespace Entropa.WcfUtils {

	/// <summary>
	///     The base class for our fault-safe emitters.
	/// </summary>
	/// <typeparam name="TInterface"></typeparam>
	public abstract class FaultSafeEmitterBase<TInterface> where TInterface : class {

		/// <summary>
		///     The constructor.
		/// </summary>
		protected internal FaultSafeEmitterBase() {
		}

		/// <summary>
		/// Adds service contract types to the list.
		/// </summary>
		/// <param name="types"></param>
		/// <param name="type"></param>
		private static void AddServiceContractTypes( List<Type> types, Type type ) {
			// Get the interfaces implemented by the type
			Type[] baseTypes = type.GetInterfaces();
			foreach ( Type baseType in baseTypes ) {
				// Add its service contracts
				AddServiceContractTypes( types, baseType );
			}
			// If the type is a service contract...
			if ( HasServiceContractAttribute( type ) ) {
				// Add it to the list.
				types.Add( type );
			}
		}

		/// <summary>
		///     The generated assembly name.
		/// </summary>
		protected abstract string AssemblyName { get; }

		/// <summary>
		///     Creates the assembly builder.
		/// </summary>
		/// <returns></returns>
		protected AssemblyBuilder CreateAssemblyBuilder() {
			// Create our dynamic assembly name and version
			AssemblyName assemblyName = new AssemblyName( this.GenerateAssemblyName() );
			assemblyName.Version = new Version( 1, 0, 0, 0 );
			// Create the assembly builder, specifying whether we'll want to only run the assembly or run and save it
			AssemblyBuilder assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(
				assemblyName,
				this.EnableSave ? AssemblyBuilderAccess.RunAndSave :AssemblyBuilderAccess.Run
				);
			return assemblyBuilder;
		}

		/// <summary>
		/// Creates the module builder.
		/// </summary>
		/// <param name="assemblyBuilder"></param>
		/// <returns></returns>
		protected static ModuleBuilder CreateModuleBuilder( AssemblyBuilder assemblyBuilder ) {
			return assemblyBuilder.DefineDynamicModule(
				assemblyBuilder.GetName().Name, 
				assemblyBuilder.GetName().Name + ".mod"  // Note: A module file name is necessary in order to save
				);
		}

		/// <summary>
		/// Declares locals.
		/// </summary>
		/// <param name="il"></param>
		/// <param name="types"></param>
		protected static void DeclareLocals( ILGenerator il, params Type[] types ) {
			foreach ( Type type in types ) {
				il.DeclareLocal( type );
			}
		}

		/// <summary>
		///     Whether or not we save the assembly we've created.
		/// </summary>
		protected virtual bool EnableSave {
			get { return true; }
		}

		/// <summary>
		/// Returns the service contract types on the given type.
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		protected static IEnumerable<Type> GetServiceContractTypes( Type type ) {
			// Create a list of types
			List<Type> types = new List<Type>();
			// Get the service contract interfaces on the type
			AddServiceContractTypes( types, type );
			// Return the types on the array
			return types.ToArray();
		}
		
		/// <summary>
		/// Generates a semi-random assembly name.
		/// </summary>
		/// <returns></returns>
		protected string GenerateAssemblyName() {
			return this.AssemblyName + "_" + Guid.NewGuid();
		}
		
		/// <summary>
		/// Generates a semi-random type name.
		/// </summary>
		/// <returns></returns>
		protected string GenerateTypeName() {
			return this.TypeName + "_" + typeof ( TInterface ).Name;// + "_" + Guid.NewGuid();
		}

		/// <summary>
		/// Creates the type builder.
		/// </summary>
		/// <param name="moduleBuilder"></param>
		/// <returns></returns>
		protected TypeBuilder CreateTypeBuilder( ModuleBuilder moduleBuilder ) {
			// Define our custom type, which implements the given interface and IDisposable
			TypeBuilder typeBuilder = moduleBuilder.DefineType(
				GenerateTypeName(), 
				TypeAttributes.Class | TypeAttributes.Sealed | TypeAttributes.BeforeFieldInit,
				typeof ( Object ),
				this.GetImplementedInterfaces()
				);
			return typeBuilder;
		}

		/// <summary>
		/// Returns the list of interfaces that we implement.
		/// </summary>
		/// <returns></returns>
		protected Type[] GetImplementedInterfaces() {
			// Create a list of types we'll implement
			List<Type> types = new List<Type>(); 
#if TEMP // TODO
			// Add the service contract interfaces
			types.AddRange( GetServiceContractTypes( typeof( TInterface ) ) );
#endif
			// Add IDisposable
			types.Add( typeof( IDisposable ) );
			// Return the types as an array
			return types.ToArray();
		}

		/// <summary>
		/// Returns true if has an attribute of the given type.
		/// </summary>
		/// <param name="type">The class or interface type.</param>
		/// <param name="attributeType">The custom attribute type.</param>
		/// <param name="inherit">Whether to include base classes.</param>
		/// <returns>Whether or not the type has the given attribute.</returns>
		protected static bool HasCustomAttribute( Type type, Type attributeType, bool inherit = true ) {
			if ( null == type ) throw new ArgumentNullException( "type" );
			if ( null == attributeType ) throw new ArgumentNullException( "attributeType" );
			object[] attributes = type.GetCustomAttributes( attributeType, inherit );
			return ( attributes.Length > 0 );
		}


		/// <summary>
		/// Returns whether or not the interface type implements the service contract attribute.
		/// </summary>
		/// <param name="interfaceType"></param>
		/// <returns></returns>
		protected static bool HasServiceContractAttribute( Type interfaceType ) {
			return HasCustomAttribute( interfaceType, typeof( ServiceContractAttribute ), false );
		}

		/// <summary>
		///     The type name prefix.  A randomly generated suffix will be added.
		/// </summary>
		protected abstract string TypeName { get; }

		/// <summary>
		/// Validates the type we've been given.
		/// </summary>
		protected static void ValidateTypeParameters() {
			// Ensure T is an interface.
			Type type = typeof ( TInterface );
			if ( !type.IsInterface ) {
				throw new ArgumentOutOfRangeException( string.Format( "Type {0} must be an interface.", type.Name ) );
			}
			// Ensure the type is a service contract
			if ( !HasServiceContractAttribute( type ) ) {
				throw new ArgumentOutOfRangeException( string.Format( "Type {0} must have the [ServiceContract] attribute.", type.Name ) );
			}
		}

	}

}
