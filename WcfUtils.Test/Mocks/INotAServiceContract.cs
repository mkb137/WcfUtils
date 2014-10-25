/**
 *
 *   Copyright (c) 2014 Entropa Software Ltd.  All Rights Reserved.    
 *
 */
using System;
using log4net;

namespace Entropa.WcfUtils.Test.Mocks {

	/// <summary>
	/// This interface does not implement a service contract.
	/// </summary>
	public interface INotAServiceContract {
		/// <summary>
		/// Adds two numbers.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		int Add( int a, int b );
		/// <summary>
		/// Adds two numbers.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		double Add( double a, double b );
		/// <summary>
		/// Subtracts two numbers.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		int Subtract( int a, int b );
	}
}
