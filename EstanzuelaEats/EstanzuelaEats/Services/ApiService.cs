
namespace EstanzuelaEats.Services
{
    using Common.Modelos;
    using Newtonsoft.Json;
    using Plugin.Connectivity;
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
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
    }
}
