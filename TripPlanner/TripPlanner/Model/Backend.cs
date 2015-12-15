using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Authentication.Web.Provider;
using Windows.Security.Credentials;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TripPlanner.ViewModel;

namespace TripPlanner.Model
{
    public class Backend
    {
        private Backend()
        {
            
        }

        private static Backend _backend = new Backend();
        public static Backend Azure => _backend;

        public static MapService MapService => new MapService();

        private static readonly Uri ENDPOINT = new Uri("http://tripmanager.azurewebsites.net/");
        private const string GET = "GET";
        private const string POST = "POST";
        private const string PUT = "PUT";
        private const string DELETE = "DELETE";

        private string _authenticationToken = "";
        private string _authenticationTokenType = "";
        
        public string Username { get; private set; }

        public async Task<RegistrationResponse> Register(UserViewModel user)
        {
            List<ValidationResult> results = new List<ValidationResult>();
            if (!Validator.TryValidateObject(user, new ValidationContext(user), results, true))
            {
                return new RegistrationResponse(results);
            }
            else
            {
                if (user.Password != user.RepeatPassword)
                {
                    results.Add(new ValidationResult("Passwords are not the same"));
                    return new RegistrationResponse(results);
                }

                HttpWebRequest request = PopulateRequest(new Uri("/api/Account/Register", UriKind.Relative), POST);
                Task<Stream> outStream = request.GetRequestStreamAsync();
                using (StreamWriter writer = new StreamWriter(await outStream))
                {
                    string userData = JsonConvert.SerializeObject(user);
                    await writer.WriteAsync(userData);
                }

                try
                {
                    using (StreamReader response = new StreamReader((await request.GetResponseAsync()).GetResponseStream()))
                    {
                        return new RegistrationResponse(response: BackendResponse.Ok);
                    }
                }
                catch (WebException e)
                {
                    return new RegistrationResponse(response: BackendResponse.BadRequest);
                }
            }
        }

        public async Task<BackendResponse> Login(string username, string password)
        {
            PasswordVault vault = new PasswordVault();
            vault.Add(new PasswordCredential("creds", username, password));
            
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

            try
            {
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
            catch (Exception)
            {
                return BackendResponse.Unauthorized;
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
                    int tripId = trip.Id;
                    Trip legalTrip = new Trip(tripId)
                    {
                        Icon = trip.Icon,
                        Name = trip.Title
                    };

                    foreach (var item in trip.Items)
                    {
                        int itemId = item.Id;
                        legalTrip.ItemList.Add(new Item(itemId)
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

        public Task SaveTripWithoutItems(TripViewModel tripViewModel)
        {
            return SaveTripWithoutItems(tripViewModel.Trip);
        }

        public async Task SaveTripWithoutItems(Trip trip)
        {
            HttpWebRequest request = PopulateRequest(new Uri("/api/Trip/Save/Trip/NoItems", UriKind.Relative), POST);
            Task<Stream> outStream = request.GetRequestStreamAsync();
            using (StreamWriter writer = new StreamWriter(await outStream))
            {
                Trip outTrip = new Trip
                {
                    Date = trip.Date, Icon = trip.Icon, Id = trip.Id, Name = trip.Name
                };

                await writer.WriteAsync(JsonConvert.SerializeObject(outTrip));
            }

            using (StreamReader response = new StreamReader((await request.GetResponseAsync()).GetResponseStream()))
            {
                if (trip.Id == 0)
                {
                    var responseData = await response.ReadToEndAsync();
                    trip.Id = int.Parse(responseData);
                }
            }
        }

        public Task SaveTrip(TripViewModel tripViewModel)
        {
            return SaveTrip(tripViewModel.Trip);
        }

        public async Task SaveTrip(Trip trip)
        {
            HttpWebRequest request = PopulateRequest(new Uri("/api/Trip/Save/Trip", UriKind.Relative), POST);
            Task<Stream> outStream = request.GetRequestStreamAsync();
            using (StreamWriter writer = new StreamWriter(await outStream))
            {
                await writer.WriteAsync(JsonConvert.SerializeObject(trip));
            }

            using (StreamReader response = new StreamReader((await request.GetResponseAsync()).GetResponseStream()))
            {
                if (trip.Id == 0)
                {
                    var responseData = await response.ReadToEndAsync();
                    trip.Id = int.Parse(responseData);
                }
            }
        }

        public async Task UpdateItemChecked(Item item)
        {
            int checkCode = item.IsChecked.HasValue && item.IsChecked.Value ? 1 : 0;
            HttpWebRequest request = PopulateRequest(new Uri($"api/Trip/Check/{item.Id}/{checkCode}", UriKind.Relative), POST);
            await request.GetResponseAsync();
        }

        public async Task SaveItem(Item item, Trip containingTrip)
        {
            HttpWebRequest request = PopulateRequest(new Uri($"/api/Trip/Save/{containingTrip.Id}/Item", UriKind.Relative), POST);
            Task<Stream> outStream = request.GetRequestStreamAsync();
            using (StreamWriter writer = new StreamWriter(await outStream))
            {
                string data = JsonConvert.SerializeObject(item);
                await writer.WriteAsync(data);
            }

            using (StreamReader response = new StreamReader((await request.GetResponseAsync()).GetResponseStream()))
            {
                if (item.Id == 0)
                {
                    var responseData = await response.ReadToEndAsync();
                    item.Id = int.Parse(responseData);
                }
            }
        }

        public async Task RemoveItem(Item item)
        {
            if (item.Id == 0)
            {
                return;
            }

            HttpWebRequest request = PopulateRequest(new Uri($"/api/Trip/Remove/Item/{item.Id}", UriKind.Relative), DELETE);
            await request.GetResponseAsync();
        }

        public async Task RemoveTrip(Trip trip)
        {
            if (trip.Id == 0)
            {
                return;
            }

            HttpWebRequest request = PopulateRequest(new Uri($"api/Trip/Remove/Trip/{trip.Id}", UriKind.Relative), DELETE);
            await request.GetResponseAsync();
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
            Ok, Unauthorized, Error, BadRequest
        }

        public class RegistrationResponse
        {
            public IEnumerable<ValidationResult> ValidationErrors { get; private set; }
            public BackendResponse BackendResponse { get; private set; }

            public RegistrationResponse(IEnumerable<ValidationResult> validationErrors = null, BackendResponse response = BackendResponse.Error)
            {
                ValidationErrors = validationErrors;
                BackendResponse = response;
            }
        }
    }
}
