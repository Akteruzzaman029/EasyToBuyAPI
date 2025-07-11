namespace Core.ModelDto.AspNetUsers
{
    public class AspNetUsersFilterDto
    {
        public string Name { get; set; } = string.Empty;
        public int Type { get; set; }
        public bool IsActive { get; set; }
    }
}
