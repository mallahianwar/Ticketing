//using Microsoft.Extensions.Configuration;
//using System.Threading.Tasks;
////using Twilio.Clients;
////using Twilio.Rest.Api.V2010.Account;
////using Twilio.Types;

//public class TwilioClient
//{
//    private readonly IConfiguration _Configure;
//    public void sendSMS(string from,string To , string Msg)
//    {
//        //_Configure = IConfiguration configuration;
//         //string accountSid = _Configure.GetValue<string>("Twilio:AccountSid");
//         //string authToken = _Configure.GetValue<string>("Twilio:AuthToken");
//         string accountSid = "AC0afe63f3ddff4d7f0d9b5ce7d7479381";
//         string authToken = "24387cd12d34883be7d6c16890bae235";

//        //TwilioClient.Init(accountSid, authToken);
//        var client = new TwilioRestClient(accountSid, authToken);

//        var message = MessageResource.Create(
//            to: new PhoneNumber(To),
//            from: new PhoneNumber(from),
//            body: Msg,
//            client: client);
//        var n = message;
//    }
//}