using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseApiController : ControllerBase
    {
        protected void LowerAddressAndConc(AddressDto address)
        {
            var properties = from p in typeof(AddressDto).GetProperties()
                         where p.PropertyType == typeof(string) &&
                               p.CanRead &&
                               p.CanWrite
                         select p;
        
            foreach (var property in properties)
            {
                var value = (string)property.GetValue(address, null);
                if (value != null)
                {   
                    value = value.ToLower().Trim();
                    property.SetValue(address, value, null);
                    if (property.Name != "FullAddress" && property.Name != "DisplayName")
                        address.FullAddress += value + " ";
                }
            }
        }
    }
    
}