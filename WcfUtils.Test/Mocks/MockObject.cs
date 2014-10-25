/**
 *
 *   Copyright (c) 2014 Entropa Software Ltd.  All Rights Reserved.    
 *
 */
using System;
using System.Runtime.Serialization;
using log4net;

namespace Entropa.WcfUtils.Test.Mocks {

	/// <summary>
	/// A mock object, used for sending complex types over a contract.
	/// </summary>
	[DataContract]
	public class MockObject {

		/// <summary>
		/// A value.
		/// </summary>
		[DataMember]
		public string A { get; set; }

		/// <summary>
		/// Another value.
		/// </summary>
		[DataMember]
		public string B { get; set; }
	}
}
