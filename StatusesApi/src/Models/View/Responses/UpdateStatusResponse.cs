namespace StatusesApi.Models.View.Responses
{
    using System.ComponentModel.DataAnnotations;

    public class UpdateStatusResponse
    {
        /// <summary>
        /// Id статуса
        /// </summary>
        [Required(ErrorMessage = "Id статуса обязателен.")]
        public Guid StatusId { get; set; }
    }
} 