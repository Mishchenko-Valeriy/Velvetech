using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Velvetech
{
    public class RelationshipString
    {
        public int GroupID { get; set; }
        public int StudentID { get; set; }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class RelationshipController : ControllerBase
    {
        // POST api/<Relationship>
        /// <summary>
        /// Добавление студента в группу
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Relationship
        ///     {
        ///         "GroupID": 2,
        ///         "StudentID": "2"
        ///     }
        ///
        /// </remarks>
        [HttpPost]
        public List<ValidationResult> Post([FromBody] RelationshipString message)
        {
            Facade facade = new Facade(new Relationship());
            return facade.RelationshipSet(message);
        }

        // DELETE api/<Relationship>
        /// <summary>
        /// Удаление студента из группы
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     DELETE /Relationship
        ///     /1/1
        ///
        /// </remarks>
        [HttpDelete("{GroupId}/{StudentId}")]
        public List<ValidationResult> Delete(int GroupId, int StudentId)
        {
            Facade facade = new Facade(new Relationship());
            return facade.RelationshipDel(GroupId, StudentId);
        }
    }
}