using System;
using System.Collections.Generic;
using System.IO;
using System.ServiceModel;
using Entropa.WcfUtils.MockService;
using Entropa.WcfUtils.Test.MockServiceReference;
using Entropa.WcfUtils.Test.Prototypes;
using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IMockContract = Entropa.WcfUtils.Test.MockServiceReference.IMockContract;
using MockObject = Entropa.WcfUtils.Test.MockServiceReference.MockObject;

namespace Entropa.WcfUtils.Test {

	/// <summary>
	/// This tests the <see cref="PrototypeFaultSafeServiceReferenceClient{TInterface,TClient}"/> prototype.
	/// </summary>
	[TestClass]
	public class PrototypeFaultSafeServiceReferenceClientTest {

		/// <summary>
		/// The logger.
		/// </summary>
		private readonly static ILog	_log	= LogManager.GetLogger( typeof( PrototypeFaultSafeServiceReferenceClientTest ) );

		[TestInitialize]
		public void Setup() {
			log4net.Config.XmlConfigurator.Configure( new FileInfo( "log4net.config" ) );
		}

		[TestMethod]
		[DeploymentItem( "log4net.config" )]
		public void TestPrototype() {
			// Create the host
			_log.DebugFormat( " - creating host" );
			ServiceHost host = new ServiceHost( typeof( MockServiceImpl ) );
			_log.DebugFormat( " - opening host" );
			host.Open();
			_log.DebugFormat( " - host open" );

			IDisposable disposableClient = null;
			try {
				// Create the client
				PrototypeFaultSafeServiceReferenceClient<MockContractClient> client = new PrototypeFaultSafeServiceReferenceClient<MockContractClient>() ;
				disposableClient = client;

				_log.DebugFormat( " - calling service methods" );
				Assert.AreEqual( 3,		client.AddInt( 1, 2 ),				"AddInt"	);
				Assert.AreEqual( 6,		client.Add3( 1, 2, 3 ),				"Add3"		);
				Assert.AreEqual( 10,	client.Add4( 1, 2, 3, 4 ),			"Add4"		);
				Assert.AreEqual( 15,	client.Add5( 1, 2, 3, 4, 5 ),		"Add5"		);
				Assert.AreEqual( 21,	client.Add6( 1, 2, 3, 4, 5, 6 ),	"Add6"		);
				Assert.AreEqual( 3.3,	client.AddDouble( 1.1, 2.2 ), 1e-5,	"AddDouble" );
				_log.DebugFormat( " - service methods passed???" );

				List<MockObject> collection = client.GetCollection( 3 );
				Assert.IsNotNull( collection, "GetCollection" );
				Assert.AreEqual( 3, collection.Count, "Collection count" );
				Assert.AreEqual( "A0", collection[0].A );
				Assert.AreEqual( "A1", collection[1].A );
				Assert.AreEqual( "A2", collection[2].A );

				// Ensure it's fault safe - call a method that will throw an exception
				try {
					client.ThrowException();
					Assert.Fail( "Exception should have been thrown");
				} catch ( FaultException fe ) {
					// Do nothing
					_log.DebugFormat( " - got exception '{0}'", fe.Message );
				}

				// Continue to use the client
				Assert.AreEqual( 1,	client.Subtract( 4, 3 ),	"Subtract"	);
				

			} finally {
				_log.DebugFormat( " - disposing channel" );
				if ( null != disposableClient ) {
					disposableClient.Dispose();
				}

				_log.DebugFormat( " - closing host" );
				host.Close( TimeSpan.FromSeconds( 10 ) );
				_log.DebugFormat( " - host closed" );
				
			}
		}
	}
}
