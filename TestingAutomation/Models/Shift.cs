using System;

namespace TestFramework.Models
{
    public class Shift
    {
        public Guid Id { get; set; }
        public int StoreId { get; set; }
        public int UserId { get; set; }
        public int TillId { get; set; }
    }
}
