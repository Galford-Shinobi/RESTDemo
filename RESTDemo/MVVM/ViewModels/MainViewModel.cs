using RESTDemo.MVVM.Models;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Text.Json;
using System.Windows.Input;

namespace RESTDemo.MVVM.ViewModels
{
    public class MainViewModel
    {
        //HttpClient client;
        JsonSerializerOptions _serializerOptions;
        string baseUrl = "https://63d3f125a93a149755b68109.mockapi.io";
        private List<User> Users;

        public MainViewModel()
        {}
        public ICommand GetAllUsersCommand =>
              new Command(async () =>
              {
                  try
                  {
                      var urls = App.Current.Resources["UrlAPI"].ToString();
                      //https://63d3f125a93a149755b68109.mockapi.io/users
                      var url = $"{baseUrl}/users";
                      var client = new HttpClient
                      {
                          BaseAddress = new Uri(baseUrl),
                      };


                      var response =
                            await client.GetAsync(url);
                      if (response.IsSuccessStatusCode)
                      {
                          //var content = await response.Content.ReadAsStringAsync();
                          using (var responseStream =
                                await response.Content.ReadAsStreamAsync())
                          {
                              var data =
                               await JsonSerializer
                               .DeserializeAsync<List<User>>(responseStream, _serializerOptions);
                              Users = data;
                          }
                      }
                  }
                  catch (Exception ex)
                  {

                      throw;
                  }
                  
              });

        public ICommand GetSingleUserCommand =>
              new Command(async () =>
              {
                  var baseurls = App.Current.Resources["UrlAPI"].ToString();
                  var client = new HttpClient
                  {
                      BaseAddress = new Uri(baseurls),
                  };

                  //var url = $"/users/25";
                  var url = $"{"/users"}{"/25"}";
                  var response =
                        await client.GetStringAsync(url);
              });

        public ICommand AddUserCommand =>
               new Command(async () =>
               {
                   var urls = App.Current.Resources["UrlAPI"].ToString();
                   var client = new HttpClient
                   {
                       BaseAddress = new Uri(urls),
                   };
                   //var url = $"{baseUrl}/users";
                   var url = $"{"/users"}";
                   var user =
                   new User
                   {
                       createdAt = DateTime.Now,
                       name = "Item - Delted",
                       avatar = "https://fakeimg.pl/350x200/?text=MAUI",
                       latitude = "01.123654",
                       longitude = "-12.35687",
                   };
                   string json =
                        JsonSerializer.Serialize<User>(user, _serializerOptions);

                   StringContent content =
                   new StringContent(json, Encoding.UTF8, "application/json");

                   var response = await client.PostAsync(url, content);

               });

        public ICommand UpdateUserCommand =>
     new Command(async () =>
     {

         var user = Users.FirstOrDefault(x => x.id == "68");
         HttpClient client = new HttpClient
         {
             BaseAddress = new Uri(baseUrl),
         };
         //var url = $"{baseUrl}/users/1";
         var url = $"{"/users"}{"/68"}";
         user.name = "Item - Deleted";
        


         string json =
              JsonSerializer.Serialize<User>(user, _serializerOptions);

         StringContent content =
              new StringContent(json, Encoding.UTF8, "application/json");

         var response = await client.PutAsync(url, content);

     });

        public ICommand DeleteUserCommand =>
             new Command(async () =>
             {
                 HttpClient client = new HttpClient
                 {
                     BaseAddress = new Uri(baseUrl),
                 };

                 //var url = $"{baseUrl}/users/10";
                 var url = $"{"/users"}{"/68"}";
                 var response = await client.DeleteAsync(url);
             });

    }
}

