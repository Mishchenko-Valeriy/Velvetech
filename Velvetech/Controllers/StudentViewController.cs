using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Velvetech
{
    public class StudentStringGet
    {
        public bool ItMan { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public string Patronymic { get; set; }
        public string Callsign { get; set; }
        public string Group { get; set; }
        public int Page { get; set; }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class StudentViewController : ControllerBase
    {
        // POST api/<Student>
        /// <summary>
        /// Выдать список студентов с возможностью фильтрации по полям (Пол, ФИО, уникальный идентификатор, название группы) 
        /// и ограничением по результату (пагинация)
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /Student
        ///     "itMan": true,
        ///     "surname": "ен",
        ///     "name": "",
        ///     "patronymic": "",
        ///     "callsign": "",
        ///     "group": "",
        ///     "page": 1
        ///     
        /// </remarks>
        [HttpPost]
        public IEnumerable<Student> POST([FromBody] StudentStringGet message)
        {
            Facade facade = new Facade(new Student());
            return facade.StudentGet(message);
        }
    }
}