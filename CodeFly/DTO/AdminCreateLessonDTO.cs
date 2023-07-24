namespace CodeFly.DTO;

public class AdminCreateLessonDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int ChapterId { get; set; }
    
    public string HTML { get; set; }
    public string Description { get; set; }
}