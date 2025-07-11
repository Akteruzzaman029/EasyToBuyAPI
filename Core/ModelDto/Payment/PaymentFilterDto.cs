﻿namespace Core.ModelDto.Payment
{
    public class PaymentFilterDto
    {
        public string Name { get; set; } = string.Empty;
        public DateTime StartDate { get; set; } 
        public DateTime EndDate { get; set; }
        public string PackageName { get; set; } = string.Empty;
        public int Status { get; set; }
        public bool IsActive { get; set; }
    }
}
