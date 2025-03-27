namespace StatusesApi.Models.View.Responses
{
    using System.ComponentModel.DataAnnotations;

    public class UpdateStatusResponse
    {
        [Required(ErrorMessage = "Id статуса обязателен.")]
        public Guid StatusId { get; set; }
    }
} 