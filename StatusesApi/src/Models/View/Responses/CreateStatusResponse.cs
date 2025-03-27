using System.ComponentModel.DataAnnotations;

namespace StatusesApi.Models.View.Responses;

public class CreateStatusResponse
{
    [Required(ErrorMessage = "Id статуса обязателен.")]
    public Guid StatusId { get; set; }
}