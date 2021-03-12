using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Velvetech
{
    public class StudentStringAdd
    {
        public bool ItMan { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public string Patronymic { get; set; }
        public string Callsign { get; set; }
    }
    public class StudentStringPut
    {
        public int Id { get; set; }
        public bool ItMan { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public string Patronymic { get; set; }
        public string Callsign { get; set; }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        // POST api/<Student>
        /// <summary>
        /// Добавление нового студента
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Student
        ///     {
        ///         "ItMan": false,
        ///         "Surname": "Мищенко",
        ///         "Name": "Мария",
        ///         "Patronymic": "Валерьевна",
        ///         "Callsign": "Дакота"
        ///     }
        ///
        /// </remarks>
        [HttpPost]
        public List<ValidationResult> Post([FromBody] StudentStringAdd message)
        {
            Facade facade = new Facade(new Student());
            return facade.StudentSet(message);
        }

        // PUT api/<Student>
        /// <summary>
        /// Редактирование студента
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /Student
        ///     {
        ///         "Id": 1,
        ///         "ItMan": false,
        ///         "Surname": "Мищенко",
        ///         "Name": "Мария",
        ///         "Patronymic": "Валерьевна",
        ///         "Callsign": "Небраска"
        ///     }
        ///
        /// </remarks>
        [HttpPut]
        public List<ValidationResult> Put([FromBody] StudentStringPut message)
        {
            Facade facade = new Facade(new Student());
            return facade.StudentSet(message);
        }

        // DELETE api/<Student>
        /// <summary>
        /// Удаление студента
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     DELETE /Student
        ///     /1
        ///
        /// </remarks>
        [HttpDelete("{Id}")]
        public List<ValidationResult> Delete(int Id)
        {
            Facade facade = new Facade(new Student());
            return facade.StudentDel(Id);
        }
    }
}