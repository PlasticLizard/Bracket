using Microsoft.ServiceBus;

namespace Bracket.Hosting.Azure.ServiceBus
{
    public static class ServiceBusHelper
    {
        public static TransportClientEndpointBehavior CreateUsernamePasswordCredential(string userName, string password)
        {
            var userNamePasswordServiceBusCredential = new TransportClientEndpointBehavior
            {
                CredentialType =
                    TransportClientCredentialType.SharedSecret
                    
            };
            userNamePasswordServiceBusCredential.Credentials.SharedSecret.IssuerName = userName;
            userNamePasswordServiceBusCredential.Credentials.SharedSecret.IssuerSecret = password;
            return userNamePasswordServiceBusCredential;
        }
    }
}
