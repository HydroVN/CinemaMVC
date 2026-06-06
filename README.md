# CineViet — Hệ Thống Đặt Vé Xem Phim Trực Tuyến

Dự án website đặt vé xem phim trực tuyến được xây dựng bằng công nghệ **ASP.NET Core MVC** (.NET 10) kết hợp với **Entity Framework Core** và **SQL Server**. 

Giao diện ứng dụng được thiết kế theo phong cách tối (Dark Theme) hiện đại, sang trọng, mang lại trải nghiệm tối ưu cho cả khách hàng và quản trị viên.

---

## 🛠️ Công Nghệ Sử Dụng
- **Backend**: C# / ASP.NET Core MVC 10
- **Database**: Microsoft SQL Server / EF Core (Sử dụng Fluent API để ánh xạ database theo chuẩn đồ án)
- **Authentication**: ASP.NET Core Identity (Quản lý đăng ký, đăng nhập và phân quyền Admin/User)
- **Frontend**: HTML5, CSS3 (Giao diện Dark Theme sang trọng), Javascript (Xử lý sơ đồ chọn ghế trực quan)

---

## 📊 Sơ Đồ Cơ Sở Dữ Liệu (Database Schema)

Cơ sở dữ liệu của hệ thống được chuẩn hóa và thiết kế theo đúng yêu cầu chi tiết của đồ án, sử dụng Fluent API trong EF Core để kết nối các thuộc tính C# với các bảng và cột tương ứng trong SQL Server:

### 1. Bảng Danh Mục & Nghiệp Vụ
| Tên Bảng (C# Model) | Tên Bảng trong SQL | Các Cột và Kiểu Dữ Liệu chính |
| :--- | :--- | :--- |
| `Genre` (Enum) | **`Genres`** | `Id` (INT - PK), `Name` (NVARCHAR) |
| `Phim` | **`Movies`** | `Id` (INT - PK), `Title` (NVARCHAR), `Description` (NVARCHAR(MAX)), `Genre_Id` (INT - FK), `Duration` (INT), `DanhGia` (FLOAT), `Image_URL` (NVARCHAR), `UrlTrailer` (NVARCHAR), `Created_At` (DATETIME), `Status` (BIT), `DaoDien`, `DienVien`, `NgonNgu` |
| `RapChieu` | **`RapChieus`** | `Id` (INT - PK), `TenRap` (NVARCHAR), `DiaChi`, `ThanhPho`, `SoDienThoai` |
| `PhongChieu` | **`PhongChieus`** | `Id` (INT - PK), `RapChieuId` (INT - FK), `TenPhong`, `SoHang`, `SoCot` |
| `Ghe` | **`Ghes`** | `Id` (INT - PK), `PhongChieuId` (INT - FK), `HangGhe` (CHAR), `SoGhe` (INT), `LoaiGhe` (INT) |
| `SuatChieu` | **`Showtimes`** | `Id` (INT - PK), `Movie_Id` (INT - FK), `PhongChieuId` (INT - FK), `Start_Time` (DATETIME), `End_Time` (DATETIME), `Ticket_Price` (DECIMAL), `DangHoatDong` (BIT) |
| `DatVe` | **`Bookings`** | `Id` (INT - PK), `UserId` (NVARCHAR - FK), `SuatChieuId` (INT - FK), `Booking_Date` (DATETIME), `Total_Price` (DECIMAL), `Status` (INT), `MaVe` |
| `ChiTietDatVe` | **`Booking_Items`** | `Id` (INT - PK), `Booking_Id` (INT - FK), `Ghe_Id` (INT - FK), `Price` (DECIMAL) |

### 2. Các Bảng Hệ Thống (ASP.NET Core Identity)
- **`AspNetUsers`**: Lưu trữ thông tin tài khoản người dùng và quản trị viên (có thêm cột mở rộng `HoTen`).
- **`AspNetRoles`**: Quản lý các nhóm quyền truy cập (`Admin` và `User`).
- **`AspNetUserRoles`**: Bảng trung gian liên kết người dùng với vai trò tương ứng.

---

## 🚀 Hướng Dẫn Cài Đặt và Chạy Dự Án

### Bước 1: Tải mã nguồn về máy
Bạn có thể tải trực tiếp file zip từ GitHub hoặc sử dụng lệnh clone:
```bash
git clone https://github.com/username/MVCDatVePhim.git
cd MVCDatVePhim
```

### Bước 2: Setup Cơ Sở Dữ Liệu (SQL Server)
Có 2 cách để khởi tạo Database:
- **Cách 1 (Khuyên dùng cho Sinh Viên)**: 
  1. Mở **SQL Server Management Studio (SSMS)** hoặc **Azure Data Studio**.
  2. Mở file [MVCDatVePhim.sql](MVCDatVePhim.sql) ở thư mục gốc của dự án.
  3. Nhấn **Execute** (hoặc `F5`) để chạy toàn bộ mã SQL giúp tạo DB `MVCDatVePhimDB` và chèn đầy đủ dữ liệu mẫu có sẵn.
- **Cách 2 (Sử dụng EF Core Migrations)**:
  Ứng dụng đã được cấu hình tự động chạy `MigrateAsync` khi khởi động. Bạn chỉ cần sửa Connection String và chạy chương trình, EF Core sẽ tự động sinh database và chèn dữ liệu mẫu (Seed Data).

### Bước 3: Cấu hình Connection String
Mở file `appsettings.json` trong thư mục [MVCDatVePhim/appsettings.json](MVCDatVePhim/appsettings.json) và điều chỉnh connection string kết nối tới SQL Server của bạn:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=MVCDatVePhimDB;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
}
```
*(Thay `YOUR_SERVER_NAME` bằng tên SQL Server trên máy bạn, ví dụ: `localhost`, `.` hoặc `SQLEXPRESS`)*.

### Bước 4: Chạy ứng dụng bằng dòng lệnh (Terminal)
Di chuyển vào thư mục dự án chứa file `.csproj`:
```bash
cd MVCDatVePhim
```
Thực hiện khôi phục các thư viện NuGet và chạy ứng dụng:
```bash
dotnet restore
dotnet run --urls "https://localhost:5001;http://localhost:5000"
```

### Bước 5: Truy cập Website
Mở trình duyệt web và truy cập theo đường dẫn:
- Giao diện người dùng: [http://localhost:5000](http://localhost:5000) hoặc [https://localhost:5001](https://localhost:5001)
- Giao diện quản trị (Admin): [http://localhost:5000/Admin](http://localhost:5000/Admin) (yêu cầu đăng nhập tài khoản Admin)

---

## 🔐 Tài Khoản Kiểm Thử Mặc Định
Hệ thống đã được cài đặt sẵn dữ liệu tài khoản mẫu để thuận tiện cho việc chấm điểm:

| Vai trò | Email | Mật khẩu | Chức năng chính |
| :--- | :--- | :--- | :--- |
| **Quản trị (Admin)** | `admin@cineviet.vn` | `Admin@123` | Quản lý Phim, Suất chiếu, Rạp, Phòng chiếu, và xem danh sách Vé |
| **Người dùng (User)** | `nam@gmail.com` | `Admin@123` | Xem phim, lọc nâng cao, chọn suất chiếu, chọn ghế và đặt vé |

---

## 📂 Cấu Trúc Thư Mục Dự Án
```text
├── MVCDatVePhim/               # Thư mục chính dự án ASP.NET Core
│   ├── Areas/Admin/            # Phân vùng quản trị viên (Controllers, Views)
│   ├── Controllers/            # Bộ điều hướng giao diện public (Home, Phim, DatVe...)
│   ├── Models/                 # Lớp thực thể dữ liệu (Phim, Ghe, DatVe...)
│   ├── Services/               # Tầng nghiệp vụ xử lý logic (DatVeService, GheService...)
│   ├── ViewModels/             # Các lớp Model trung gian truyền dữ liệu cho View
│   ├── Views/                  # Giao diện người dùng (Razor Views)
│   ├── wwwroot/                # Tài nguyên tĩnh (CSS, JS, Hình ảnh)
│   └── Program.cs              # File khởi chạy ứng dụng
├── MVCDatVePhim.sql            # File script tạo database & chèn dữ liệu mẫu
└── README.md                   # Hướng dẫn sử dụng
```

---

## 📝 Các Bước Đưa Dự Án Lên GitHub (Dành cho Sinh Viên)
Nếu bạn muốn đẩy mã nguồn này lên kho lưu trữ GitHub cá nhân của mình, hãy thực hiện các bước sau:
1. Tạo một repository mới trên GitHub (không tích chọn *Add a README.md*).
2. Mở Terminal tại thư mục gốc của dự án trên máy tính và chạy các lệnh sau:
   ```bash
   git init
   git add .
   git commit -m "First commit - CineViet Movie Booking"
   git branch -M main
   git remote add origin LINK_REPOSITORY_CỦA_BẠN
   git push -u origin main
   ```
*(Lưu ý: Dự án đã được cấu hình sẵn file `.gitignore` để tự động bỏ qua các thư mục biên dịch trung gian như `bin/`, `obj/`, `.vs/` để giữ repository luôn sạch sẽ).*
