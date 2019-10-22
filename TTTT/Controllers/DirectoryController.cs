using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TTTT.Model;
using TTTT.Services;

namespace TTTT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DirectoryController : ControllerBase
    {
        protected readonly IUserService userService;

        public DirectoryController(IUserService a_userService)
        {
            userService = a_userService;
        }
        // POST: api/Directory
        [HttpPost]
        public async Task<ContentResult> Post()
        {
            var users = await userService.ListAsync();
            return Content(OnGetDirectory(users), "text/xml", Encoding.UTF8);
        }

        protected string OnGetDirectory(IEnumerable<User> a_users)
        {
            string users = GetUsersSection(a_users);

            string first =
                  "<document type = \"freeswitch/xml\">" +
                  "\n<section name=\"directory\">" +
                  "\n<domain name=\"192.168.99.66\" > " +
                  "\n <params>" +
                  "\n     <param name=\"dial-string\" value=\"{^^:sip_invite_domain=${dialed_domain}:presence_id=${dialed_user}@${dialed_domain}}${sofia_contact(*/${dialed_user}@${dialed_domain})},${verto_contact(${dialed_user}@${dialed_domain})}\"/>" +
                  "\n     <param name =\"jsonrpc-allowed-methods\" value=\"verto\"/>" +
                  "\n </params>" +
                  "\n <variables>" +
                  "\n     <variable name =\"record_stereo\" value=\"true\"/>" +
                  "\n     <variable name =\"default_gateway\" value=\"$${default_provider}\"/>" +
                  "\n     <variable name =\"default_areacode\" value=\"$${default_areacode}\"/>" +
                  "\n     <variable name =\"transfer_fallback_extension\" value=\"operator\"/>" +
                  "\n </variables >" +
                  "\n <groups>" +
                  "\n     <group name=\"default\">" +
                  "\n	<users>";
            string rest =
                  "\n </users>" +
                  "\n    </group>" +
                  "\n    </groups>" +
                  "\n</domain>" +
                  "\n</section>" +
                  "\n</document>";

            return first + users + rest;
        }

        protected string GetUsersSection(IEnumerable<User> a_users)
        {
            const string userTemplate =
                "\n  <user id=\"@Number@\">" +
                "\n    <params>" +
                "\n      <param name=\"password\" value=\"@Password@\"/>" +
                "\n      <param name=\"vm-password\" value=\"@Number@\"/>" +
                "\n    </params>" +
                "\n    <variables>" +
                "\n      <variable name=\"toll_allow\" value=\"domestic,international,local\"/>" +
                "\n      <variable name=\"accountcode\" value=\"@Number@\"/>" +
                "\n      <variable name=\"user_context\" value=\"default\"/>" +
                "\n      <variable name=\"effective_caller_id_name\" value=\"Extension @Number@\"/>" +
                "\n      <variable name=\"effective_caller_id_number\" value=\"@Number@\"/>" +
                "\n      <variable name=\"outbound_caller_id_name\" value=\"FreeSWITCH\"/>" +
                "\n      <variable name=\"outbound_caller_id_number\" value=\"0000000000\"/>" +
                "\n      <variable name=\"callgroup\" value=\"techsupport\"/>" +
                "\n    </variables>" +
                "\n </user>";
            string result = "";
            var type = typeof(User);
            foreach (var user in a_users)
            {
                var properties = type.GetProperties();
                var currentTemplate = userTemplate;
                foreach (var property in properties)
                    currentTemplate = currentTemplate.Replace("@" + property.Name + "@", property.GetValue(user).ToString());
                result += currentTemplate;
            }
            return result;
        }
    }
}
