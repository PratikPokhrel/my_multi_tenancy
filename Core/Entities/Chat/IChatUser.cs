

using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Chats
{
    public interface IChatUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        [Column(TypeName = "text")]
        public string ProfilePictureDataUrl { get; set; }
    }
}