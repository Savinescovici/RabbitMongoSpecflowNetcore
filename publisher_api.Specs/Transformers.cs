using System;
using System.Collections.Generic;
using System.Text;
using publisher_api.Dto;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace publisher_api.Specs
{
    [Binding]
    public class Transformers
    {
        [StepArgumentTransformation("users with")]
        public IEnumerable<UserDto> BooksTransform(Table table)
        {
            return table.CreateSet<UserDto>();
        }
    }
}
