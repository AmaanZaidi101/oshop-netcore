namespace Oshop.API.Dtos
{
    public class UserForRolesDto
    {
        public string UserName { get; set; }
        public string Id { get; set; }
        public string[] Roles { get; set; }
    }
}