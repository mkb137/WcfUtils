/**
 *
 *   Copyright (c) 2014 Entropa Software Ltd.  All Rights Reserved.    
 *
 */

using System;
using System.ServiceModel;
using log4net;

namespace MockService {
	/// <summary>
	/// A service contract.
	/// </summary>
	[ServiceContract]
	public interface IMockContractBase {

		/// <summary>
		/// Adds two integers together (tests having multiple methods with the same name and different signatures).
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		[OperationContract(Name="AddInt")]
		int Add( int a, int b );

		/// <summary>
		/// Adds two doubles together (tests having multiple methods with the same name and different signatures).
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		[OperationContract(Name="AddDouble")]
		double Add( double a, double b );

		/// <summary>
		/// Subtracts two numbers.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		[OperationContract]
		int Subtract( int a, int b );
	}
}
