using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace EstanzuelaEats.Api.Controllers
{
    using System;
    using System.IO;
    using System.Web.Http;
    using Common.Modelos;
    using Helpers;
    using Newtonsoft.Json.Linq;

    [RoutePrefix("api/Users")]
    public class UsersController : ApiController
    {
        public IHttpActionResult PostUser(UserRequest userRequest)
        {
            if (userRequest.ImageArray != null && userRequest.ImageArray.Length > 0)
            {
                var stream = new MemoryStream(userRequest.ImageArray);
                var guid = Guid.NewGuid().ToString();
                var file = $"{guid}.jpg";
                var folder = "~/Content/Usuarios";
                var fullPath = $"{folder}/{file}";
                var response = FilesHelper.UploadPhoto(stream, folder, file);

                if (response)
                {
                    userRequest.ImagePath = fullPath;
                }
            }

            var answer = UsersHelper.CreateUserASP(userRequest);
            if (answer.Logrado)
            {
                return Ok(answer);
            }

            return BadRequest(answer.Mensaje);
        }

        [HttpPost]
        [Authorize]
        [Route("GetUser")]
        public IHttpActionResult GetUser(JObject form)
        {
            try
            {
                var email = string.Empty;
                dynamic jsonObject = form;

                try
                {
                    email = jsonObject.Email.Value;
                }
                catch
                {
                    return BadRequest("Incorrect call.");
                }

                var user = UsersHelper.GetUserASP(email);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("LoginFacebook")]
        public IHttpActionResult LoginFacebook(FacebookResponse profile)
        {
            var user = UsersHelper.GetUserASP(profile.Id);
            if (user != null)
            {
                return Ok(true); // TODO: Pending update the user with new facebook data
            }

            var userRequest = new UserRequest
            {
                EMail = profile.Id,
                FirstName = profile.FirstName,
                ImagePath = profile.Picture.Data.Url,
                LastName = profile.LastName,
                Password = profile.Id,
            };

            var answer = UsersHelper.CreateUserASP(userRequest);
            return Ok(answer);
        }
    }
}
