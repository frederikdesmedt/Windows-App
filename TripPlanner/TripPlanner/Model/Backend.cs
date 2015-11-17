using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Authentication.Web.Provider;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TripPlanner.Model
{
    public class Backend
    {
        private Backend()
        {
            
        }

        private static Backend _backend = new Backend();
        public static Backend Local => _backend;

        private static readonly Uri ENDPOINT = new Uri("http://localhost:5579/");
        private const string GET = "Get";
        private const string POST = "Post";
        private const string PUT = "Put";

        private string _authenticationToken = "";
        private string _authenticationTokenType = "";
        
        public string Username { get; private set; }

        public async Task<BackendResponse> Login(string username, string password)
        {
            HttpWebRequest request = PopulateRequest(new Uri("/Token", UriKind.Relative), POST, "application/x-www-form-urlencoded");
            Task<Stream> outStream = request.GetRequestStreamAsync();
            FormUrlEncodedContent encodedContent = new FormUrlEncodedContent(new KeyValuePair<string, string>[]
            {
                new KeyValuePair<string, string>("username", username),
                new KeyValuePair<string, string>("password", password),
                new KeyValuePair<string, string>("grant_type", "password")
            });

            Task<string> serializedContentTask = encodedContent.ReadAsStringAsync();
            using (StreamWriter writer = new StreamWriter(await outStream))
            {
                await writer.WriteAsync(await serializedContentTask);
            }

            using (StreamReader response = new StreamReader((await request.GetResponseAsync()).GetResponseStream()))
            {
                string responseString = response.ReadToEnd();
                JObject responseObject = JObject.Parse(responseString);
                if (responseObject["access_token"] != null)
                {
                    _authenticationToken = responseObject["access_token"].ToString();
                    _authenticationTokenType = responseObject["token_type"].ToString();
                    Username = responseObject["userName"].ToString();
                    return BackendResponse.Ok;
                }
                else
                {
                    return BackendResponse.Unauthorized;
                }
            }
        }

        public void Logout()
        {
            _authenticationToken = "";
            _authenticationTokenType = "";
            Username = null;
        }

        public async Task<IEnumerable<Trip>> GetTrips()
        {
            if (_authenticationToken == null)
            {
                throw new InvalidOperationException("Not logged in");
            }

            HttpWebRequest request = PopulateRequest(new Uri("/api/Trip", UriKind.Relative), GET);
            using (StreamReader response = new StreamReader((await request.GetResponseAsync()).GetResponseStream()))
            {
                string responseString = response.ReadToEnd();
                var deserializedObject = JsonConvert.DeserializeObject<dynamic>(responseString);

                List<Trip> trips = new List<Trip>();
                foreach (var trip in deserializedObject)
                {
                    Trip legalTrip = new Trip()
                    {
                        Icon = trip.Icon,
                        Name = trip.Title
                    };

                    foreach (var item in trip.Items)
                    {
                        legalTrip.ItemList.Add(new Item
                        {
                            Name = item.Name,
                            Priority = item.Priority,
                            IsChecked = item.IsChecked
                        });
                    }

                    trips.Add(legalTrip);
                }

                return trips;
            }
        } 

        private HttpWebRequest PopulateRequest(Uri relativeUri, string method, string contentType = "application/json")
        {
            HttpWebRequest request = WebRequest.CreateHttp(new Uri(ENDPOINT, relativeUri));
            request.ContentType = contentType;
            request.Accept = "application/json";
            request.Method = method;

            if (!string.IsNullOrEmpty(_authenticationToken))
            {
                request.Headers["Authorization"] = (_authenticationTokenType ?? "Bearer") + " " + _authenticationToken;
            }

            return request;
        }

        public enum BackendResponse
        {
            Ok, Unauthorized, Error
        }
    }
}
