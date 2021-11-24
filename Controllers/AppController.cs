using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GeocacheAPI.Data;

namespace GeocacheAPI.Controllers
{
    public class AppController : ControllerBase
    {
        private readonly IGeocacheRepository _repository;

        public AppController(IGeocacheRepository repository)
        {
            _repository = repository;
        }
    }
}
