using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Web;
using System.Text;
using Microsoft.ServiceBus;

//This class is based on the reverse web proxy presented here:
//http://blogs.msdn.com/clemensv/archive/2009/04/05/net-services-march-2009-ctp-host-a-public-website-at-the-kitchen-table-or-from-a-coffee-shop-no-kidding.aspx
namespace Bracket.Hosting.Azure.ServiceBus
{
    public class RackServiceHost : IDisposable
    {
        readonly WebHttpRelayBinding _binding;
        readonly Uri _serviceUri;
        readonly string _serviceBasePath;
        readonly TransportClientEndpointBehavior _credentials;
        readonly MessageEncoder _encoder;
        IChannelListener<IReplyChannel> _listener;
        private Rack _rack;

        public RackServiceHost(string solutionName,string solutionPassword,string urlNamespace,bool useSsl)
            : this(
                ServiceBusEnvironment.CreateServiceUri("http" + (useSsl ? "s" : ""), solutionName, urlNamespace),
                ServiceBusHelper.CreateUsernamePasswordCredential(solutionName, solutionPassword))
        {
        }

        public RackServiceHost(Uri serviceUri, TransportClientEndpointBehavior credentials)
        {
            _serviceUri = serviceUri;

            _serviceBasePath = _serviceUri.PathAndQuery;
            _serviceBasePath.TrimEnd(new[] {'/'});
            
            ServicePointManager.DefaultConnectionLimit = 50;

            _binding = new WebHttpRelayBinding(EndToEndWebHttpSecurityMode.None, RelayClientAuthenticationType.None);
            _binding.MaxReceivedMessageSize = int.MaxValue;
            _binding.TransferMode = TransferMode.Streamed;
            _binding.AllowCookies = true;
            _binding.ReceiveTimeout = TimeSpan.MaxValue;
            _binding.ReaderQuotas.MaxArrayLength = int.MaxValue;
            _binding.ReaderQuotas.MaxStringContentLength = int.MaxValue;

            var encoderBindingElement = new WebMessageEncodingBindingElement();
            encoderBindingElement.ReaderQuotas.MaxArrayLength = int.MaxValue;
            encoderBindingElement.ReaderQuotas.MaxStringContentLength = int.MaxValue;
            encoderBindingElement.ContentTypeMapper = new RawContentTypeMapper();
            _encoder = encoderBindingElement.CreateMessageEncoderFactory().Encoder;

            _credentials = credentials;
        }

        public void Start(RubyEnvironment rubyEnvironment)
        {
            _rack = rubyEnvironment == null ? new Rack() : new Rack(rubyEnvironment);
            
            _listener = _binding.BuildChannelListener<IReplyChannel>(_serviceUri, _credentials);
            _listener.Open();
            _listener.BeginAcceptChannel(ChannelAccepted, _listener);
        }

        public void Stop()
        {
            if (_listener != null)
            {
                _listener.Close();
            }
        }

        private void ChannelAccepted(IAsyncResult result)
        {
            try
            {
                IReplyChannel replyChannel = _listener.EndAcceptChannel(result);
                if (replyChannel != null)
                {
                    try
                    {
                        replyChannel.Open();
                        replyChannel.BeginReceiveRequest(RequestAccepted, replyChannel);
                    }
                    catch
                    {
                        replyChannel.Abort();
                    }

                    if (_listener.State == CommunicationState.Opened)
                    {
                        _listener.BeginAcceptChannel(ChannelAccepted, _listener);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e); 
                _listener.Abort();
                _listener = null;
            }
        }

        void RequestAccepted(IAsyncResult result)
        {
            var replyChannel = (IReplyChannel)result.AsyncState;
            
            try
            {
                RequestContext requestContext = replyChannel.EndReceiveRequest(result);
                if (requestContext != null)
                {
                    Uri requestUri = requestContext.RequestMessage.Properties.Via;

                    string relativePath = String.Empty;

                    if (requestUri.PathAndQuery.Length > _serviceBasePath.Length)
                        relativePath = requestUri.PathAndQuery.Substring(_serviceBasePath.Length);

                    relativePath.TrimEnd(new[] {'/'});

                    var rm = 
                        requestContext.RequestMessage.Properties[HttpRequestMessageProperty.Name] as HttpRequestMessageProperty;


                    var rackRequest = new RackRequest();
                    rackRequest.RequestMethod = rm.Method;
                    rackRequest.ApplicationPath = _serviceBasePath;
                    rackRequest.ResourcePath = relativePath;
                    rackRequest.QueryString = rm.QueryString;
                    rackRequest.ServerName = requestUri.Host;
                    rackRequest.UrlScheme = requestUri.Scheme.ToLower();
                    rackRequest.ServerProtocol = rm.Headers["SERVER_PROTOCOL"];
                    rackRequest.RequestPath = requestUri.PathAndQuery;
                    //target.RemoteAddress = ??
                    rackRequest.HttpVersion = "HTTP 1.1";
                    rackRequest.RequestUri = requestUri.ToString();
                    rackRequest.GatewayInterface = rm.Headers["GATEWAY_INTERFACE"];
                    rackRequest.ContentType = rm.Headers[HttpRequestHeader.ContentType];

                    int contentLength = 0;
                    if (!String.IsNullOrEmpty(rm.Headers[HttpRequestHeader.ContentLength]))
                        Int32.TryParse(rm.Headers[HttpRequestHeader.ContentLength], out contentLength);

                    rackRequest.ContentLength = contentLength;

                    foreach (string header in rm.Headers.Keys)
                    {
                        rackRequest.AddHeader(header, rm.Headers[header]);
                    }
                    
                    
                    if (contentLength > 0 && rm.SuppressEntityBody == false)
                    {
                        using(var message = new MemoryStream()) 
                        {
                            _encoder.WriteMessage(requestContext.RequestMessage, message);
                        
                            using (var reader = new StreamReader(message))
                            {
                                rackRequest.Body = reader.ReadToEnd();
                            }
                        }
                    }

                    var responseStream = new MemoryStream();
                    var rackResponse = new RackResponse(responseStream);

                    _rack.HandleRequest(rackRequest, rackResponse);

                    Message replyMessage = null;
                    var responseProperty = new HttpResponseMessageProperty();

                    if (rackResponse.OutputStream.Length > 0)
                    {
                        rackResponse.OutputStream.Position = 0;
                        Message responseMessage = _encoder.ReadMessage(rackResponse.OutputStream, 65536);
                        replyMessage = Message.CreateMessage(MessageVersion.None, "RESPONSE",
                                                             responseMessage.GetReaderAtBodyContents());
                    }
                    else
                    {
                        replyMessage = Message.CreateMessage(MessageVersion.None, "RESPONSE");
                        responseProperty.SuppressEntityBody = true;
                    }
                    
                    foreach (var header in rackResponse.Headers)
                        responseProperty.Headers.Add(header.Key, header.Value);
                    responseProperty.StatusCode = (HttpStatusCode) rackResponse.Status;
                    responseProperty.StatusDescription = ((HttpStatusCode) rackResponse.Status).ToString();
                    replyMessage.Properties.Add(WebBodyFormatMessageProperty.Name,
                                                new WebBodyFormatMessageProperty(WebContentFormat.Raw));
                    replyMessage.Properties.Add(HttpResponseMessageProperty.Name, responseProperty);
                    requestContext.BeginReply(replyMessage, DoneReplying, new object[] { rackResponse, requestContext, replyChannel });
              }
            }
            catch (Exception e)
            {
                replyChannel.Abort();
            }

        }

        public void Dispose()
        {
            Stop();
            _listener = null;
        }

        void DoneReplying(IAsyncResult result)
        {
            var response = (RackResponse) ((object[]) result.AsyncState)[0];
            var requestContext = (RequestContext)((object[])result.AsyncState)[1];
            var replyChannel = (IReplyChannel)((object[])result.AsyncState)[2];

            try
            {
                requestContext.EndReply(result);
            }
            catch (Exception e)
            {
                replyChannel.Abort();
            }
            finally
            {
                if (response != null)
                {
                    response.OutputStream.Close();
                }
        
                replyChannel.BeginReceiveRequest(RequestAccepted, replyChannel);
            }
        }

        class RawContentTypeMapper : WebContentTypeMapper
        {
            public override WebContentFormat GetMessageFormatForContentType(string contentType)
            {
                return WebContentFormat.Raw;
            }
        }
    }
}
