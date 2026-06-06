-- ============================================================
-- ĐỒ ÁN MÔN HỌC: HỆ THỐNG ĐẶT VÉ XEM PHIM TRỰC TUYẾN
-- Tên CSDL: MVCDatVePhimDB
-- Phiên bản: SQL Server
-- ============================================================

USE master;
GO

-- Tạo cơ sở dữ liệu mới
IF EXISTS (SELECT name FROM sys.databases WHERE name = N'MVCDatVePhimDB')
BEGIN
    ALTER DATABASE MVCDatVePhimDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE MVCDatVePhimDB;
END
GO

CREATE DATABASE MVCDatVePhimDB;
GO

USE MVCDatVePhimDB;
GO

-- ============================================================
-- 1. TẠO CÁC BẢNG HỆ THỐNG (IDENTITY)
-- ============================================================

CREATE TABLE AspNetRoles (
    Id NVARCHAR(450) PRIMARY KEY,
    Name NVARCHAR(256) NULL,
    NormalizedName NVARCHAR(256) NULL,
    ConcurrencyStamp NVARCHAR(MAX) NULL
);

CREATE TABLE AspNetUsers (
    Id NVARCHAR(450) PRIMARY KEY,
    HoTen NVARCHAR(256) NOT NULL,
    UserName NVARCHAR(256) NULL,
    NormalizedUserName NVARCHAR(256) NULL,
    Email NVARCHAR(256) NULL,
    NormalizedEmail NVARCHAR(256) NULL,
    EmailConfirmed BIT NOT NULL,
    PasswordHash NVARCHAR(MAX) NULL,
    SecurityStamp NVARCHAR(MAX) NULL,
    ConcurrencyStamp NVARCHAR(MAX) NULL,
    PhoneNumber NVARCHAR(MAX) NULL,
    PhoneNumberConfirmed BIT NOT NULL,
    TwoFactorEnabled BIT NOT NULL,
    LockoutEnd DATETIMEOFFSET NULL,
    LockoutEnabled BIT NOT NULL,
    AccessFailedCount INT NOT NULL
);

CREATE TABLE AspNetRoleClaims (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    RoleId NVARCHAR(450) FOREIGN KEY REFERENCES AspNetRoles(Id) ON DELETE CASCADE,
    ClaimType NVARCHAR(MAX) NULL,
    ClaimValue NVARCHAR(MAX) NULL
);

CREATE TABLE AspNetUserClaims (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UserId NVARCHAR(450) FOREIGN KEY REFERENCES AspNetUsers(Id) ON DELETE CASCADE,
    ClaimType NVARCHAR(MAX) NULL,
    ClaimValue NVARCHAR(MAX) NULL
);

CREATE TABLE AspNetUserLogins (
    LoginProvider NVARCHAR(450) NOT NULL,
    ProviderKey NVARCHAR(450) NOT NULL,
    ProviderDisplayName NVARCHAR(MAX) NULL,
    UserId NVARCHAR(450) FOREIGN KEY REFERENCES AspNetUsers(Id) ON DELETE CASCADE,
    PRIMARY KEY (LoginProvider, ProviderKey)
);

CREATE TABLE AspNetUserRoles (
    UserId NVARCHAR(450) FOREIGN KEY REFERENCES AspNetUsers(Id) ON DELETE CASCADE,
    RoleId NVARCHAR(450) FOREIGN KEY REFERENCES AspNetRoles(Id) ON DELETE CASCADE,
    PRIMARY KEY (UserId, RoleId)
);

CREATE TABLE AspNetUserTokens (
    UserId NVARCHAR(450) FOREIGN KEY REFERENCES AspNetUsers(Id) ON DELETE CASCADE,
    LoginProvider NVARCHAR(450) NOT NULL,
    Name NVARCHAR(450) NOT NULL,
    Value NVARCHAR(MAX) NULL,
    PRIMARY KEY (UserId, LoginProvider, Name)
);

-- ============================================================
-- 2. TẠO CÁC BẢNG NGHIỆP VỤ ĐẶT VÉ PHIM
-- ============================================================

-- Bảng Thể Loại Phim (Danh mục)
CREATE TABLE Genres (
    Id INT PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL
);

-- Bảng Phim (Movies)
CREATE TABLE Movies (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Title NVARCHAR(250) NOT NULL,
    Description NVARCHAR(MAX) NULL,
    Genre_Id INT NOT NULL FOREIGN KEY REFERENCES Genres(Id),
    Duration INT NOT NULL,
    DanhGia FLOAT NOT NULL,
    Image_URL NVARCHAR(500) NULL,
    UrlTrailer NVARCHAR(500) NULL,
    Created_At DATETIME NOT NULL,
    Status BIT NOT NULL,
    DaoDien NVARCHAR(250) NULL,
    DienVien NVARCHAR(500) NULL,
    NgonNgu NVARCHAR(100) NULL
);

-- Bảng Rạp Chiếu
CREATE TABLE RapChieus (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    TenRap NVARCHAR(250) NOT NULL,
    DiaChi NVARCHAR(250) NULL,
    ThanhPho NVARCHAR(100) NULL,
    SoDienThoai NVARCHAR(50) NULL
);

-- Bảng Phòng Chiếu
CREATE TABLE PhongChieus (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    RapChieuId INT FOREIGN KEY REFERENCES RapChieus(Id) ON DELETE CASCADE,
    TenPhong NVARCHAR(100) NOT NULL,
    SoHang INT NOT NULL,
    SoCot INT NOT NULL
);

-- Bảng Ghế
CREATE TABLE Ghes (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    PhongChieuId INT FOREIGN KEY REFERENCES PhongChieus(Id) ON DELETE CASCADE,
    HangGhe CHAR(1) NOT NULL,
    SoGhe INT NOT NULL,
    LoaiGhe INT NOT NULL, -- 0: Thường, 1: VIP
    CONSTRAINT UQ_Ghe UNIQUE (PhongChieuId, HangGhe, SoGhe)
);

-- Bảng Suất Chiếu (Showtimes)
CREATE TABLE Showtimes (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Movie_Id INT FOREIGN KEY REFERENCES Movies(Id) ON DELETE CASCADE,
    PhongChieuId INT FOREIGN KEY REFERENCES PhongChieus(Id) ON DELETE CASCADE,
    Start_Time DATETIME NOT NULL,
    End_Time DATETIME NOT NULL,
    Ticket_Price DECIMAL(18,2) NOT NULL,
    DangHoatDong BIT NOT NULL
);

-- Bảng Đặt Vé (Bookings)
CREATE TABLE Bookings (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UserId NVARCHAR(450) FOREIGN KEY REFERENCES AspNetUsers(Id) ON DELETE NO ACTION,
    SuatChieuId INT FOREIGN KEY REFERENCES Showtimes(Id) ON DELETE CASCADE,
    Booking_Date DATETIME NOT NULL,
    Total_Price DECIMAL(18,2) NOT NULL,
    Status INT NOT NULL, -- 0: Chờ duyệt, 1: Đã xác nhận, 2: Đã hủy
    MaVe NVARCHAR(100) NOT NULL
);

-- Bảng Chi Tiết Đặt Vé (Booking_Items)
CREATE TABLE Booking_Items (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Booking_Id INT FOREIGN KEY REFERENCES Bookings(Id) ON DELETE CASCADE,
    Ghe_Id INT FOREIGN KEY REFERENCES Ghes(Id) ON DELETE NO ACTION,
    Price DECIMAL(18,2) NOT NULL
);
GO

-- ============================================================
-- 3. CHÈN DỮ LIỆU MẪU (SEED DATA)
-- ============================================================

-- Chèn Vai Trò (Roles)
INSERT INTO AspNetRoles (Id, Name, NormalizedName)
VALUES 
('role-admin', 'Admin', 'ADMIN'),
('role-user', 'User', 'USER');

-- Chèn Tài Khoản Mẫu (Mật khẩu mặc định: Admin@123)
-- Hash cho mật khẩu "Admin@123"
INSERT INTO AspNetUsers (Id, HoTen, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnabled, AccessFailedCount)
VALUES 
('user-admin', N'Quản Trị Viên', 'admin@cineviet.vn', 'ADMIN@CINEVIET.VN', 'admin@cineviet.vn', 'ADMIN@CINEVIET.VN', 1, 'AQAAAAIAAYagAAAAEKXvXRWaYkSBCVx3Nf7Y2i7t0G5RUdBuKrMxvGpXRY1GxJcpX4oCrOzX2Y7jQ==', NEWID(), NEWID(), '0901234567', 1, 0, 0, 0),
('user-member1', N'Nguyễn Văn Nam', 'nam@gmail.com', 'NAM@GMAIL.COM', 'nam@gmail.com', 'NAM@GMAIL.COM', 1, 'AQAAAAIAAYagAAAAEKXvXRWaYkSBCVx3Nf7Y2i7t0G5RUdBuKrMxvGpXRY1GxJcpX4oCrOzX2Y7jQ==', NEWID(), NEWID(), '0912345678', 1, 0, 0, 0);

-- Gán vai trò cho tài khoản
INSERT INTO AspNetUserRoles (UserId, RoleId)
VALUES 
('user-admin', 'role-admin'),
('user-member1', 'role-user');

-- Chèn Danh Mục Thể Loại
INSERT INTO Genres (Id, Name) VALUES
(0, N'Hành Động'),
(1, N'Tình Cảm'),
(2, N'Hài Hước'),
(3, N'Kinh Dị'),
(4, N'Khoa Học Viễn Tưởng'),
(5, N'Hoạt Hình'),
(6, N'Tâm Lý'),
(7, N'Khác');

-- Chèn Rạp Chiếu
INSERT INTO RapChieus (TenRap, DiaChi, ThanhPho, SoDienThoai)
VALUES 
(N'CineViet Quận 1', N'123 Nguyễn Huệ, Quận 1', N'Hồ Chí Minh', '0281111222'),
(N'CineViet Đống Đa', N'45 Chùa Bộc, Đống Đa', N'Hà Nội', '0243333444');

-- Chèn Phòng Chiếu
INSERT INTO PhongChieus (RapChieuId, TenPhong, SoHang, SoCot)
VALUES 
(1, N'Phòng 1 (Standard)', 4, 6),
(1, N'Phòng 2 (VIP)', 3, 5),
(2, N'Phòng 1 (Standard)', 4, 6);

-- Chèn Ghế Cho Phòng Chiếu (Chèn trực tiếp kiểu đơn giản)
-- Phòng 1: 4 hàng x 6 cột = 24 ghế
INSERT INTO Ghes (PhongChieuId, HangGhe, SoGhe, LoaiGhe) VALUES
(1, 'A', 1, 0), (1, 'A', 2, 0), (1, 'A', 3, 0), (1, 'A', 4, 0), (1, 'A', 5, 0), (1, 'A', 6, 0),
(1, 'B', 1, 0), (1, 'B', 2, 0), (1, 'B', 3, 0), (1, 'B', 4, 0), (1, 'B', 5, 0), (1, 'B', 6, 0),
(1, 'C', 1, 0), (1, 'C', 2, 0), (1, 'C', 3, 0), (1, 'C', 4, 0), (1, 'C', 5, 0), (1, 'C', 6, 0),
(1, 'D', 1, 1), (1, 'D', 2, 1), (1, 'D', 3, 1), (1, 'D', 4, 1), (1, 'D', 5, 1), (1, 'D', 6, 1);

-- Phòng 2: 3 hàng x 5 cột = 15 ghế
INSERT INTO Ghes (PhongChieuId, HangGhe, SoGhe, LoaiGhe) VALUES
(2, 'A', 1, 0), (2, 'A', 2, 0), (2, 'A', 3, 0), (2, 'A', 4, 0), (2, 'A', 5, 0),
(2, 'B', 1, 0), (2, 'B', 2, 0), (2, 'B', 3, 0), (2, 'B', 4, 0), (2, 'B', 5, 0),
(2, 'C', 1, 1), (2, 'C', 2, 1), (2, 'C', 3, 1), (2, 'C', 4, 1), (2, 'C', 5, 1);

-- Phòng 3: 4 hàng x 6 cột = 24 ghế
INSERT INTO Ghes (PhongChieuId, HangGhe, SoGhe, LoaiGhe) VALUES
(3, 'A', 1, 0), (3, 'A', 2, 0), (3, 'A', 3, 0), (3, 'A', 4, 0), (3, 'A', 5, 0), (3, 'A', 6, 0),
(3, 'B', 1, 0), (3, 'B', 2, 0), (3, 'B', 3, 0), (3, 'B', 4, 0), (3, 'B', 5, 0), (3, 'B', 6, 0),
(3, 'C', 1, 0), (3, 'C', 2, 0), (3, 'C', 3, 0), (3, 'C', 4, 0), (3, 'C', 5, 0), (3, 'C', 6, 0),
(3, 'D', 1, 1), (3, 'D', 2, 1), (3, 'D', 3, 1), (3, 'D', 4, 1), (3, 'D', 5, 1), (3, 'D', 6, 1);

-- Chèn Phim Mẫu vào Movies
INSERT INTO Movies (Title, Description, Genre_Id, Duration, DanhGia, Image_URL, UrlTrailer, Created_At, Status, DaoDien, DienVien, NgonNgu)
VALUES
(
    N'Avengers: Endgame',
    N'Các siêu anh hùng tập hợp để đánh bại Thanos và cứu vũ trụ.',
    0, 181, 8.4,
    'https://upload.wikimedia.org/wikipedia/en/0/0d/Avengers_Endgame_official_poster.jpg',
    'https://www.youtube.com/watch?v=TcMBFSGVi1c',
    '2026-05-01', 1,
    N'Anthony Russo, Joe Russo',
    N'Robert Downey Jr., Chris Evans, Mark Ruffalo',
    N'Tiếng Anh'
),
(
    N'Lật Mặt 7: Một Điều Ước',
    N'Bộ phim tâm lý tình cảm gia đình đầy xúc động của đạo diễn Lý Hải.',
    6, 128, 7.2,
    'https://upload.wikimedia.org/wikipedia/vi/b/b7/L%E1%BA%ADt_M%E1%BA%B7t_7_poster.jpg',
    'https://www.youtube.com/watch?v=b114_YlPspQ',
    '2026-05-15', 1,
    N'Lý Hải',
    N'Lý Hải, Minh Hà, Louis Nguyễn',
    N'Tiếng Việt'
),
(
    N'Inside Out 2',
    N'Riley bước vào tuổi dậy thì và làm quen với các cảm xúc mới như Lo Lắng.',
    5, 100, 7.8,
    'https://upload.wikimedia.org/wikipedia/en/thumb/9/9e/Inside_Out_2_poster.jpg/220px-Inside_Out_2_poster.jpg',
    'https://www.youtube.com/watch?v=LEjhY15eCx0',
    '2026-06-01', 1,
    N'Kelsey Mann',
    N'Amy Poehler, Maya Hawke',
    N'Tiếng Anh'
);

-- Chèn Suất Chiếu vào Showtimes
INSERT INTO Showtimes (Movie_Id, PhongChieuId, Start_Time, End_Time, Ticket_Price, DangHoatDong)
VALUES 
(1, 1, DATEADD(HOUR, 19, CAST(GETDATE() AS DATE)), DATEADD(HOUR, 22, CAST(GETDATE() AS DATE)), 90000, 1),
(2, 1, DATEADD(HOUR, 22, CAST(GETDATE() AS DATE)), DATEADD(HOUR, 24, CAST(GETDATE() AS DATE)), 80000, 1),
(3, 2, DATEADD(DAY, 1, DATEADD(HOUR, 18, CAST(GETDATE() AS DATE))), DATEADD(DAY, 1, DATEADD(HOUR, 20, CAST(GETDATE() AS DATE))), 100000, 1);

-- Chèn Vé Đã Đặt Mẫu vào Bookings
INSERT INTO Bookings (UserId, SuatChieuId, Booking_Date, Total_Price, Status, MaVe)
VALUES 
('user-member1', 1, GETDATE(), 180000, 1, 'VE20260606001');

-- Chèn Chi Tiết Vé Đã Đặt vào Booking_Items (Ghế A1 và A2)
INSERT INTO Booking_Items (Booking_Id, Ghe_Id, Price)
VALUES 
(1, 1, 90000), -- Ghế A1
(1, 2, 90000); -- Ghế A2
GO
