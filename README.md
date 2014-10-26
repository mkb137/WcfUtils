WcfUtils
========

Utility methods related to WCF 


## Fault Safe Proxies

WCF is great, but the problem with channels and service references is that if they ever fault they become unusable and must be aborted and recreated.

FaultSafeProxyEmitter and FaultSafeServiceReferenceClientEmitter can dynamically create proxies around channels and automatically generated service reference clients that, in the case of exceptions, will abort and recreate the channel/client.


Copyright 2014 Entropa Software Ltd.


