using System.ComponentModel.DataAnnotations;

namespace StatusesApi.Models.View.Responses;

public class CreateStatusResponse
{
    /// <summary>
    /// Id статуса
    /// </summary>
    [Required(ErrorMessage = "Id статуса обязателен.")]
    public Guid StatusId { get; set; }
}