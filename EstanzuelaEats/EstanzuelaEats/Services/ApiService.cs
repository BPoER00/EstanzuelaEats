
namespace EstanzuelaEats.Services
{
    using Common.Modelos;
    using Newtonsoft.Json;
    using Plugin.Connectivity;
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;

    public class ApiService
    {

        //hacemos el check si tenemos conexion a internet
        public async Task<Respuestas> CheckConnection()
        {
            //si no tenemos conexion a internet se le mostrara un mensaje diciendo el problema
            if (!CrossConnectivity.Current.IsConnected)
            {
                return new Respuestas
                {
                    Logrado = false,
                    Mensaje = EstanzuelaEats.Helpers.Languages.TurnOnInternet
                };
            }

            //si estamos conectados a una red y no tenemos internet el programa para comprobar hacer un ping a google y devuelve el resultado si 
            //fue posible o no el ping
            var isReachable = await CrossConnectivity.Current.IsRemoteReachable("google.com");

            //si el ping no fue exitoso se le mostrara un mensaje de error diciendo el error
            if (!isReachable)
            {
                return new Respuestas
                {
                    Logrado = false,
                    Mensaje = EstanzuelaEats.Helpers.Languages.NoInternet,
                };
            }

            //si hemos llegado hasta aca es porque la conexion a internet es exitosa y podemos continuar
            return new Respuestas
            {
                Logrado = true,
            };
        }

        public async Task<TokenResponse> GetToken(string urlBase, string username, string password)
        {
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                var response = await client.PostAsync("Token",
                    new StringContent(string.Format(
                    "grant_type=password&username={0}&password={1}",
                    username, password),
                    Encoding.UTF8, "application/x-www-form-urlencoded"));
                var resultJSON = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<TokenResponse>(
                    resultJSON);
                return result;
            }
            catch
            {
                return null;
            }
        }

        //metodo para hacer la conexion cono el servidor del backend que trae una lista de tipo T
       
        public async Task<Respuestas> GetList<T>(string UrlBase, string prefix, string controller)
        {
            //abrimos un try para evitar errores llamando aca una var llamada cliente y instanciandola con el metodo httpclient
            //se le pasa al cliente la url de el backend que tendra los datos
            //a los otros dos datos se les concatena en uno
            //se crea una variable response que es la que termina de unir todo para obtener los resultados
            //por ultimio se crea una variable answer para metodo de error por si este llega a fallar
            try
            {
                var Cliente = new HttpClient();
                Cliente.BaseAddress = new Uri(UrlBase);
                var url = $"{prefix}{controller}";
                var response = await Cliente.GetAsync(url);
                var answer = await response.Content.ReadAsStringAsync();

                //si responsa no cumple su funcion este retorna el error
                if (!response.IsSuccessStatusCode)
                {
                    return new Respuestas
                    {
                        Logrado = false,
                        Mensaje = answer
                    };
                }
                //si hemos llegado hasta aca es porque no dio error y toda la conexion esta realizada de forma correcta
                //deserealizamos un String a un Json y los datos se lo pasamos a una lista
                //luego retornamos que fue exitoso y nuestra lista
                var lista = JsonConvert.DeserializeObject<List<T>>(answer);
                return new Respuestas
                {
                    Logrado = true,
                    Resultado = lista
                };
            }
            catch (Exception e)
            {
                //si falla retornamos que no fue posible y mostramos el mensaje de error
                return new Respuestas
                {
                    Logrado = false,
                    Resultado = e.Message,

                };
            }
        }

        public async Task<Respuestas> GetList<T>(string UrlBase, string prefix, string controller, string tokenType, string accessToken)
        {
            //abrimos un try para evitar errores llamando aca una var llamada cliente y instanciandola con el metodo httpclient
            //se le pasa al cliente la url de el backend que tendra los datos
            //a los otros dos datos se les concatena en uno
            //se crea una variable response que es la que termina de unir todo para obtener los resultados
            //por ultimio se crea una variable answer para metodo de error por si este llega a fallar
            try
            {
                var Cliente = new HttpClient();
                Cliente.BaseAddress = new Uri(UrlBase);
                Cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, accessToken);
                var url = $"{prefix}{controller}";
                var response = await Cliente.GetAsync(url);
                var answer = await response.Content.ReadAsStringAsync();

                //si responsa no cumple su funcion este retorna el error
                if (!response.IsSuccessStatusCode)
                {
                    return new Respuestas
                    {
                        Logrado = false,
                        Mensaje = answer
                    };
                }
                //si hemos llegado hasta aca es porque no dio error y toda la conexion esta realizada de forma correcta
                //deserealizamos un String a un Json y los datos se lo pasamos a una lista
                //luego retornamos que fue exitoso y nuestra lista
                var lista = JsonConvert.DeserializeObject<List<T>>(answer);
                return new Respuestas
                {
                    Logrado = true,
                    Resultado = lista
                };
            }
            catch (Exception e)
            {
                //si falla retornamos que no fue posible y mostramos el mensaje de error
                return new Respuestas
                {
                    Logrado = false,
                    Resultado = e.Message,

                };
            }
        }

        public async Task<Respuestas> GetList<T>(string UrlBase, string prefix, string controller, int id, string tokenType, string accessToken)
        {
            //abrimos un try para evitar errores llamando aca una var llamada cliente y instanciandola con el metodo httpclient
            //se le pasa al cliente la url de el backend que tendra los datos
            //a los otros dos datos se les concatena en uno
            //se crea una variable response que es la que termina de unir todo para obtener los resultados
            //por ultimio se crea una variable answer para metodo de error por si este llega a fallar
            try
            {
                var Cliente = new HttpClient();
                Cliente.BaseAddress = new Uri(UrlBase);
                Cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, accessToken);
                var url = $"{prefix}{controller}/{id}";
                var response = await Cliente.GetAsync(url);
                var answer = await response.Content.ReadAsStringAsync();

                //si responsa no cumple su funcion este retorna el error
                if (!response.IsSuccessStatusCode)
                {
                    return new Respuestas
                    {
                        Logrado = false,
                        Mensaje = answer
                    };
                }
                //si hemos llegado hasta aca es porque no dio error y toda la conexion esta realizada de forma correcta
                //deserealizamos un String a un Json y los datos se lo pasamos a una lista
                //luego retornamos que fue exitoso y nuestra lista
                var lista = JsonConvert.DeserializeObject<List<T>>(answer);
                return new Respuestas
                {
                    Logrado = true,
                    Resultado = lista
                };
            }
            catch (Exception e)
            {
                //si falla retornamos que no fue posible y mostramos el mensaje de error
                return new Respuestas
                {
                    Logrado = false,
                    Resultado = e.Message,

                };
            }
        }




        public async Task<Respuestas> Post<T>(string UrlBase, string prefix, string controller, T model)
        {
            //abrimos un try para evitar errores llamando aca una var llamada cliente y instanciandola con el metodo httpclient
            //se le pasa al cliente la url de el backend que tendra los datos
            //a los otros dos datos se les concatena en uno
            //se crea una variable response que es la que termina de unir todo para obtener los resultados
            //por ultimio se crea una variable answer para metodo de error por si este llega a fallar
            try
            {
                var request = JsonConvert.SerializeObject(model);
                var content = new StringContent(request, Encoding.UTF8, "application/json");

                var Cliente = new HttpClient();
                Cliente.BaseAddress = new Uri(UrlBase);
                var url = $"{prefix}{controller}";
                var response = await Cliente.PostAsync(url, content);
                var answer = await response.Content.ReadAsStringAsync();

                //si responsa no cumple su funcion este retorna el error
                if (!response.IsSuccessStatusCode)
                {
                    return new Respuestas
                    {
                        Logrado = false,
                        Mensaje = answer
                    };
                }
                //si hemos llegado hasta aca es porque no dio error y toda la conexion esta realizada de forma correcta
                //deserealizamos un String a un Json y los datos se lo pasamos a una lista
                //luego retornamos que fue exitoso y nuestra lista
                var Obj = JsonConvert.DeserializeObject<T>(answer);
                return new Respuestas
                {
                    Logrado = true,
                    Resultado = Obj
                };
            }
            catch (Exception e)
            {
                //si falla retornamos que no fue posible y mostramos el mensaje de error
                return new Respuestas
                {
                    Logrado = false,
                    Resultado = e.Message,

                };
            }
        }

        public async Task<Respuestas> Post<T>(string UrlBase, string prefix, string controller, T model, string tokenType, string accessToken)
        {
            //abrimos un try para evitar errores llamando aca una var llamada cliente y instanciandola con el metodo httpclient
            //se le pasa al cliente la url de el backend que tendra los datos
            //a los otros dos datos se les concatena en uno
            //se crea una variable response que es la que termina de unir todo para obtener los resultados
            //por ultimio se crea una variable answer para metodo de error por si este llega a fallar
            try
            {
                var request = JsonConvert.SerializeObject(model);
                var content = new StringContent(request, Encoding.UTF8, "application/json");

                var Cliente = new HttpClient();
                Cliente.BaseAddress = new Uri(UrlBase);
                Cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, accessToken);
                var url = $"{prefix}{controller}";
                var response = await Cliente.PostAsync(url, content);
                var answer = await response.Content.ReadAsStringAsync();

                //si responsa no cumple su funcion este retorna el error
                if (!response.IsSuccessStatusCode)
                {
                    return new Respuestas
                    {
                        Logrado = false,
                        Mensaje = answer
                    };
                }
                //si hemos llegado hasta aca es porque no dio error y toda la conexion esta realizada de forma correcta
                //deserealizamos un String a un Json y los datos se lo pasamos a una lista
                //luego retornamos que fue exitoso y nuestra lista
                var Obj = JsonConvert.DeserializeObject<T>(answer);
                return new Respuestas
                {
                    Logrado = true,
                    Resultado = Obj
                };
            }
            catch (Exception e)
            {
                //si falla retornamos que no fue posible y mostramos el mensaje de error
                return new Respuestas
                {
                    Logrado = false,
                    Resultado = e.Message,

                };
            }
        }



        public async Task<Respuestas> Delete(string UrlBase, string prefix, string controller, int Id)
        {
            //abrimos un try para evitar errores llamando aca una var llamada cliente y instanciandola con el metodo httpclient
            //se le pasa al cliente la url de el backend que tendra los datos
            //a los otros dos datos se les concatena en uno
            //se crea una variable response que es la que termina de unir todo para obtener los resultados
            //por ultimio se crea una variable answer para metodo de error por si este llega a fallar
            try
            {
                var Cliente = new HttpClient();
                Cliente.BaseAddress = new Uri(UrlBase);
                var url = $"{prefix}{controller}/{Id}";
                var response = await Cliente.DeleteAsync(url);
                var answer = await response.Content.ReadAsStringAsync();

                //si responsa no cumple su funcion este retorna el error
                if (!response.IsSuccessStatusCode)
                {
                    return new Respuestas
                    {
                        Logrado = false,
                        Mensaje = answer
                    };
                }

                return new Respuestas
                {
                    Logrado = true
                };
            }
            catch (Exception e)
            {
                //si falla retornamos que no fue posible y mostramos el mensaje de error
                return new Respuestas
                {
                    Logrado = false,
                    Resultado = e.Message,

                };
            }
        }

        public async Task<Respuestas> Delete(string UrlBase, string prefix, string controller, int Id, string tokenType, string accessToken)
        {
            //abrimos un try para evitar errores llamando aca una var llamada cliente y instanciandola con el metodo httpclient
            //se le pasa al cliente la url de el backend que tendra los datos
            //a los otros dos datos se les concatena en uno
            //se crea una variable response que es la que termina de unir todo para obtener los resultados
            //por ultimio se crea una variable answer para metodo de error por si este llega a fallar
            try
            {
                var Cliente = new HttpClient();
                Cliente.BaseAddress = new Uri(UrlBase);
                Cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, accessToken);
                var url = $"{prefix}{controller}/{Id}";
                var response = await Cliente.DeleteAsync(url);
                var answer = await response.Content.ReadAsStringAsync();

                //si responsa no cumple su funcion este retorna el error
                if (!response.IsSuccessStatusCode)
                {
                    return new Respuestas
                    {
                        Logrado = false,
                        Mensaje = answer
                    };
                }

                return new Respuestas
                {
                    Logrado = true
                };
            }
            catch (Exception e)
            {
                //si falla retornamos que no fue posible y mostramos el mensaje de error
                return new Respuestas
                {
                    Logrado = false,
                    Resultado = e.Message,

                };
            }
        }



        public async Task<Respuestas> Put<T>(string UrlBase, string prefix, string controller, T model, int Id)
        {
            //abrimos un try para evitar errores llamando aca una var llamada cliente y instanciandola con el metodo httpclient
            //se le pasa al cliente la url de el backend que tendra los datos
            //a los otros dos datos se les concatena en uno
            //se crea una variable response que es la que termina de unir todo para obtener los resultados
            //por ultimio se crea una variable answer para metodo de error por si este llega a fallar
            try
            {
                var request = JsonConvert.SerializeObject(model);
                var content = new StringContent(request, Encoding.UTF8, "application/json");

                var Cliente = new HttpClient();
                Cliente.BaseAddress = new Uri(UrlBase);
                var url = $"{prefix}{controller}/{Id}";
                var response = await Cliente.PutAsync(url, content);
                var answer = await response.Content.ReadAsStringAsync();

                //si responsa no cumple su funcion este retorna el error
                if (!response.IsSuccessStatusCode)
                {
                    return new Respuestas
                    {
                        Logrado = false,
                        Mensaje = answer
                    };
                }
                //si hemos llegado hasta aca es porque no dio error y toda la conexion esta realizada de forma correcta
                //deserealizamos un String a un Json y los datos se lo pasamos a una lista
                //luego retornamos que fue exitoso y nuestra lista
                var Obj = JsonConvert.DeserializeObject<T>(answer);
                return new Respuestas
                {
                    Logrado = true,
                    Resultado = Obj
                };
            }
            catch (Exception e)
            {
                //si falla retornamos que no fue posible y mostramos el mensaje de error
                return new Respuestas
                {
                    Logrado = false,
                    Resultado = e.Message,

                };
            }
        }

        public async Task<Respuestas> Put<T>(string UrlBase, string prefix, string controller, T model, int Id, string tokenType, string accessToken)
        {
            //abrimos un try para evitar errores llamando aca una var llamada cliente y instanciandola con el metodo httpclient
            //se le pasa al cliente la url de el backend que tendra los datos
            //a los otros dos datos se les concatena en uno
            //se crea una variable response que es la que termina de unir todo para obtener los resultados
            //por ultimio se crea una variable answer para metodo de error por si este llega a fallar
            try
            {
                var request = JsonConvert.SerializeObject(model);
                var content = new StringContent(request, Encoding.UTF8, "application/json");

                var Cliente = new HttpClient();
                Cliente.BaseAddress = new Uri(UrlBase);
                Cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, accessToken);
                var url = $"{prefix}{controller}/{Id}";
                var response = await Cliente.PutAsync(url, content);
                var answer = await response.Content.ReadAsStringAsync();

                //si responsa no cumple su funcion este retorna el error
                if (!response.IsSuccessStatusCode)
                {
                    return new Respuestas
                    {
                        Logrado = false,
                        Mensaje = answer
                    };
                }
                //si hemos llegado hasta aca es porque no dio error y toda la conexion esta realizada de forma correcta
                //deserealizamos un String a un Json y los datos se lo pasamos a una lista
                //luego retornamos que fue exitoso y nuestra lista
                var Obj = JsonConvert.DeserializeObject<T>(answer);
                return new Respuestas
                {
                    Logrado = true,
                    Resultado = Obj
                };
            }
            catch (Exception e)
            {
                //si falla retornamos que no fue posible y mostramos el mensaje de error
                return new Respuestas
                {
                    Logrado = false,
                    Resultado = e.Message,

                };
            }
        }



        public async Task<Respuestas> GetUser(string urlBase, string prefix, string controller, string email, string tokenType, string accessToken)
        {
            try
            {
                var getUserRequest = new GetUserRequest
                {
                    Email = email,
                };

                var request = JsonConvert.SerializeObject(getUserRequest);
                var content = new StringContent(request, Encoding.UTF8, "application/json");

                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, accessToken);
                var url = $"{prefix}{controller}";
                var response = await client.PostAsync(url, content);
                var answer = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    return new Respuestas
                    {
                        Logrado = false,
                        Mensaje = answer,
                    };
                }

                var user = JsonConvert.DeserializeObject<UsuariosASP>(answer);
                return new Respuestas
                {
                    Logrado = true,
                    Resultado = user,
                };
            }
            catch (Exception ex)
            {
                return new Respuestas
                {
                    Logrado = false,
                    Mensaje = ex.Message,
                };
            }
        }



        public async Task<FacebookResponse> GetFacebook(string accessToken)
        {
            var requestUrl = "https://graph.facebook.com/v2.8/me/?fields=name," +
                "picture.width(999),cover,age_range,devices,email,gender," +
                "is_verified,birthday,languages,work,website,religion," +
                "location,locale,link,first_name,last_name," +
                "hometown&access_token=" + accessToken;
            var httpClient = new HttpClient();
            var userJson = await httpClient.GetStringAsync(requestUrl);
            var facebookResponse =
                JsonConvert.DeserializeObject<FacebookResponse>(userJson);
            return facebookResponse;
        }

        public async Task<InstagramResponse> GetInstagram(string accessToken)
        {
            var client = new HttpClient();
            var userJson = await client.GetStringAsync(accessToken);
            var InstagramJson = JsonConvert.DeserializeObject<InstagramResponse>(userJson);
            return InstagramJson;
        }

        public async Task<TokenResponse> LoginTwitter(string urlBase, string servicePrefix, string controller, TwitterResponse profile)
        {
            try
            {
                var request = JsonConvert.SerializeObject(profile);
                var content = new StringContent(
                    request,
                    Encoding.UTF8,
                    "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                var url = $"{servicePrefix}{controller}";
                var response = await client.PostAsync(url, content);

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                var tokenResponse = await GetToken(
                    urlBase,
                    profile.IdStr,
                    profile.IdStr);
                return tokenResponse;
            }
            catch
            {
                return null;
            }
        }

        public async Task<TokenResponse> LoginInstagram(string urlBase, string servicePrefix, string controller, InstagramResponse profile)
        {
            try
            {
                var request = JsonConvert.SerializeObject(profile);
                var content = new StringContent(
                    request,
                    Encoding.UTF8,
                    "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                var url = $"{servicePrefix}{controller}";
                var response = await client.PostAsync(url, content);

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                var tokenResponse = await GetToken(
                    urlBase,
                    profile.UserData.Id,
                    profile.UserData.Id);
                return tokenResponse;
            }
            catch
            {
                return null;
            }
        }
         
        public async Task<TokenResponse> LoginFacebook(string urlBase, string servicePrefix, string controller, FacebookResponse profile)
        {
            try
            {
                var request = JsonConvert.SerializeObject(profile);
                var content = new StringContent(
                    request,
                    Encoding.UTF8,
                    "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                var url = $"{servicePrefix}{controller}";
                var response = await client.PostAsync(url, content);

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                var tokenResponse = await GetToken(
                    urlBase,
                    profile.Id,
                    profile.Id);
                return tokenResponse;
            }
            catch
            {
                return null;
            }
        }
    }
}
