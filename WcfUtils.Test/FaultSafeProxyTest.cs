/**
 *
 *   Copyright (c) 2014 Entropa Software Ltd.  All Rights Reserved.    
 *
 */
using System;
using System.IO;
using System.ServiceModel;
using Entropa.WcfUtils.Test.Mocks;
using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Entropa.WcfUtils.Test {

	/// <summary>
	/// This tests the <see cref="FaultSafeProxy"/> prototype.
	/// </summary>
	[TestClass]
	public class FaultSafeProxyTest {

		/// <summary>
		/// The logger.
		/// </summary>
		private readonly static ILog	_log	= LogManager.GetLogger( typeof( FaultSafeProxyTest ) );

		[TestInitialize]
		public void Setup() {
			log4net.Config.XmlConfigurator.Configure( new FileInfo( "log4net.config" ) );
		}

		[TestMethod]
		[DeploymentItem( "log4net.config" )]
		public void TestPrototype() {
			_log.DebugFormat( "TestPrototype" );
			// Create the host
			_log.DebugFormat( " - creating host" );
			ServiceHost host = new ServiceHost( typeof( MockService ) );
			_log.DebugFormat( " - opening host" );
			host.Open();
			_log.DebugFormat( " - host open" );

			_log.DebugFormat( " - creating channel" );
			IMockContract channel = new FaultSafeProxy( "MockEndpoint" );
			Assert.IsNotNull( channel, "Channel created" );
			IDisposable disposableChannel = channel as IDisposable;
			Assert.IsNotNull( disposableChannel, "Channel is disposable" );


			try {

				_log.DebugFormat( " - calling service methods" );
				Assert.AreEqual( 3,		channel.Add( 1, 2 ),				"AddInt"	);
				Assert.AreEqual( 6,		channel.Add( 1, 2, 3 ),				"Add3"		);
				Assert.AreEqual( 10,	channel.Add( 1, 2, 3, 4 ),			"Add4"		);
				Assert.AreEqual( 15,	channel.Add( 1, 2, 3, 4, 5 ),		"Add5"		);
				Assert.AreEqual( 21,	channel.Add( 1, 2, 3, 4, 5, 6 ),	"Add6"		);
				Assert.AreEqual( 3.3,	channel.Add( 1.1, 2.2 ), 1e-5,		"AddDouble" );
				_log.DebugFormat( " - service methods passed???" );

				MockCollection collection = channel.GetCollection( 3 );
				Assert.IsNotNull( collection, "GetCollection" );
				Assert.AreEqual( 3, collection.Count, "Collection count" );
				Assert.AreEqual( "A0", collection[0].A );
				Assert.AreEqual( "A1", collection[1].A );
				Assert.AreEqual( "A2", collection[2].A );

				// Ensure it's fault safe - call a method that will throw an exception
				try {
					channel.ThrowException();
					Assert.Fail( "Exception should have been thrown");
				} catch ( FaultException fe ) {
					// Do nothing
					_log.DebugFormat( " - got exception '{0}'", fe.Message );
				}

				// Continue to use the channel
				Assert.AreEqual( 1,	channel.Subtract( 4, 3 ),	"Subtract"	);

			} finally {
				_log.DebugFormat( " - disposing channel" );
				disposableChannel.Dispose();

				_log.DebugFormat( " - closing host" );
				host.Close( TimeSpan.FromSeconds( 10 ) );
				_log.DebugFormat( " - host closed" );
				
			}
		}
	}
}
