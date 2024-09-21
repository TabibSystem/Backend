using System.Text.Json.Serialization;

namespace TabibApp.Application.Dtos;

public class DayScheduleDto
{
    [JsonIgnore]
    public int Id { get; set; }
    public DayOfWeek DayOfWeek { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
}