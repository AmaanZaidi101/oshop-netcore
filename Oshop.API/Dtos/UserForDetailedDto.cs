namespace Oshop.API.Dtos
{
    public class UserForDetailedDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Created { get; set; }
        public CartDto Cart { get; set; }
    }
}