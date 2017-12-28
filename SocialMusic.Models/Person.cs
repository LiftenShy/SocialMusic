using SocialMusic.Models.EntityModels.BaseModels;

namespace SocialMusic.Models.EntityModels
{
    public class Person : BaseEntity
    {
        public string Name { get; set; }

        public int Age { get; set; }
    }
}
