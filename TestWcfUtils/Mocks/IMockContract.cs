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
	/// A service contract.  Extends the base contract in order to test that extended contracts are handled correctly.
	/// </summary>
	[ServiceContract]
	public interface IMockContract : IMockContractBase {

		/// <summary>
		/// Adds three integers together (tests having multiple methods with the same name and different signatures).
		/// </summary>
		/// <returns></returns>
		[OperationContract( Name = "Add3" )]
		int Add( int a, int b, int c );

		/// <summary>
		/// Adds four integers together (tests having multiple methods with the same name and different signatures).
		/// </summary>
		/// <returns></returns>
		[OperationContract( Name = "Add4" )]
		int Add( int a, int b, int c, int d );

		/// <summary>
		/// Adds five integers together  (tests having multiple methods with the same name and different signatures).
		/// </summary>
		/// <returns></returns>
		[OperationContract( Name = "Add5" )]
		int Add( int a, int b, int c, int d, int e );

		/// <summary>
		/// Adds six integers together (tests having multiple methods with the same name and different signatures).
		/// </summary>
		/// <returns></returns>
		[OperationContract( Name = "Add6" )]
		int Add( int a, int b, int c, int d, int e, int f );

		/// <summary>
		/// Adds seven integers together (tests having multiple methods with the same name and different signatures).
		/// </summary>
		/// <returns></returns>
		[OperationContract( Name = "Add7" )]
		int Add( int a, int b, int c, int d, int e, int f, int g );

		/// <summary>
		/// Adds eight integers together (tests having multiple methods with the same name and different signatures).
		/// </summary>
		/// <returns></returns>
		[OperationContract( Name = "Add8" )]
		int Add( int a, int b, int c, int d, int e, int f, int g, int h );

		/// <summary>
		/// Divides two integers.  Throws an exception if <paramref name="b"/> is zero.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		[OperationContract]
		int Divide( int a, int b );

		/// <summary>
		/// Returns a collection (tests returning a collection).
		/// </summary>
		/// <param name="noElements"></param>
		/// <returns></returns>
		[OperationContract]
		MockCollection GetCollection( int noElements );

		/// <summary>
		/// Takes multiple parameter types and puts them in a string (used to test parameters of different types).
		/// </summary>
		/// <returns></returns>
		[OperationContract]
		String MultipleParameterTypes( String a, object b, DateTime c, TimeSpan d, double? e, int? f );

		/// <summary>
		/// Multiplies two numbers.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		[OperationContract]
		int Multiply( int a, int b );

		/// <summary>
		/// A no-argument method.
		/// </summary>
		/// <returns></returns>
		[OperationContract]
		string NoParameter();

		/// <summary>
		/// A no-argument, no-return value method.
		/// </summary>
		/// <returns></returns>
		[OperationContract]
		void NoParameterNoReturn();

		/// <summary>
		/// A no-return value method.
		/// </summary>
		/// <returns></returns>
		[OperationContract]
		void NoReturn( String a );

		/// <summary>
		/// Throws an exception.  Used to test fault handling.
		/// </summary>
		[OperationContract]
		void ThrowException();

	}
}
