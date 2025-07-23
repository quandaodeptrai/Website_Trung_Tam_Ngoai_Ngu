using System.Collections.Generic;

namespace QuanLyTTNgoaiNgu.Models
{
    public class DebtViewModel
    {
        public List<DebtItemViewModel> Items { get; set; } = new();
        // sẽ dùng JS để tính tổng on-the-fly
    }
}

