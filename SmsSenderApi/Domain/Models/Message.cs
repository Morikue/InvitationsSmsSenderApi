using System;
using System.ComponentModel.DataAnnotations;

namespace SmsSenderApi.Domain.Models
{
    /// <summary>
    /// Модель сообщения
    /// </summary>
    public class Message
    {
        //При создании нового сообщения присваиваем дате создания дату и время момента создания объекта
        public Message() => DateOfCreation = DateTime.Now;

        /// <summary>
        /// Первичный ключ прирглашения
        /// </summary>
        [Key]
        public int MessageId { get; set; }

        /// <summary>
        /// Дата создания приглашения
        /// </summary>
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime DateOfCreation { get; set; }

        /// <summary>
        /// Номер телефона получателя
        /// </summary>
        [Required]
        [Phone]
        [StringLength(11)]
        public string RecipientPhoneNumber { get; set; }

        /// <summary>
        /// ApiId
        /// </summary>
        [Required]
        public int ApiId { get; set; }

        /// <summary>
        /// Текст сообщения
        /// </summary>
        [Required]
        [StringLength(160)]
        public string Text { get; set; }



    }
}
