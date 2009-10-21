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
                    TransportClientCredentialType
                    .UserNamePassword
            };
            userNamePasswordServiceBusCredential.Credentials.UserName.UserName = userName;
            userNamePasswordServiceBusCredential.Credentials.UserName.Password = password;
            return userNamePasswordServiceBusCredential;
        }
    }
}
