using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Velvetech
{
    public class GroupStringAdd
    {
        public string Name { get; set; }
    }
    public class GroupStringPut
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        // POST api/<Group>
        /// <summary>
        /// Создание новой группы
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Group
        ///     {
        ///        "Name": "Новая группа"
        ///     }
        ///
        /// </remarks>
        [HttpPost]
        public List<ValidationResult> Post([FromBody] GroupStringAdd message)
        {
            Facade facade = new Facade(new Group());
            return facade.GroupSet(message);
        }

        // PUT api/<Group>
        /// <summary>
        /// Редактирование группы
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /Group
        ///     {
        ///        "Id": 1,
        ///        "Name": "Обновленная группа"
        ///     }
        ///
        /// </remarks>
        [HttpPut]
        public List<ValidationResult> Put([FromBody] GroupStringPut message)
        {
            Facade facade = new Facade(new Group());
            return facade.GroupSet(message);
        }

        // DELETE api/<Group>
        /// <summary>
        /// Удаление группы
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     DELETE /Group
        ///     /1
        ///
        /// </remarks>
        [HttpDelete("{Id}")]
        public List<ValidationResult> Delete(int Id)
        {
            Facade facade = new Facade(new Group());
            return facade.GroupDel(Id);
        }

        // GET api/<Group>
        /// <summary>
        /// Выдать указануую группу
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /Group
        ///     /1
        ///
        /// </remarks>
        [HttpGet("{Id}")]
        public Group Get(int Id)
        {
            Facade facade = new Facade(new Group());
            return facade.GroupGet(Id);
        }

        // GET api/<Group>
        /// <summary>
        /// Выдать список всех групп
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /Group
        ///
        /// </remarks>
        [HttpGet]
        public IEnumerable<Group> Get()
        {
            Facade facade = new Facade(new Group());
            return facade.GroupGet();
        }

        // GET api/<Group>
        /// <summary>
        /// Выдать список групп по имени
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /Group
        ///     "Группа 1"
        ///
        /// </remarks>
        [Route("/{name?}")]
        [HttpGet]
        public IEnumerable<Group> Get(string name = "")
        {
            Facade facade = new Facade(new Group());
            return facade.GroupGet(name);
        }
    }
}