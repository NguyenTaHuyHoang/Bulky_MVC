using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.Utility
{
    public static class SD
    {
        // Khách hàng sử dụng website
        public const string Role_Customer = "Customer";
        // Khách hàng thuộc công ty sẽ có thời hạn thanh toán trong 30 ngày (Net30)
        public const string Role_Company = "Company";
        // Quản trị viên có khả năng thực hiện tất cả các thao tác CRUD trên sp và quản lý nội dung khác
        public const string Role_Admin = "Admin";
        // Nhân viên có quyền truy cập để sửa đổi việc vận chuyển sp và các chi tiết khác
        public const string Role_Employee = "Employee";
    }
}
