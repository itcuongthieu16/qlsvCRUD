using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace qlsvCRUD
{
    public class SinhVien
    {
        public int MaSV { get; set; }
        public string TenSV { get; set; }
        public string GioiTinh { get; set; }
        public string Email { get; set; }
        public string DiaChi { get; set; }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            List<SinhVien> danhSachSinhVien = new List<SinhVien>();

            SqlConnection sqlConnection;
            string connectionString = @"Data Source=Cuong\SQLEXPRESS;Initial Catalog=qlsvDb;Integrated Security=True";

            sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();

            while (true)
            {
                Console.WriteLine("1. Them sinh vien");
                Console.WriteLine("2. Sua sinh vien theo ma sinh vien");
                Console.WriteLine("3. Xoa sinh vien theo ma sinh vien");
                Console.WriteLine("4. Hien thi danh sach sinh vien");
                Console.WriteLine("0. Thoat");


                Console.WriteLine("Vui long nhap lua chon: ");
                int luaChon = int.Parse(Console.ReadLine());

                switch (luaChon)
                {
                    case 1:
                        themSinhVien(danhSachSinhVien, sqlConnection);
                        break;
                    case 2:
                        suaSinhVien(danhSachSinhVien, sqlConnection);
                        break;
                    case 3:
                        xoaSinhVien(danhSachSinhVien, sqlConnection); 
                        break;
                    case 4:
                        hienThiDanhSachSinhVien(danhSachSinhVien, sqlConnection);
                        break;
                    default:
                        Console.WriteLine("Lua chon khong hop le. Vui long chon lai.");
                        break;
                }
            }

            sqlConnection.Close();

            Console.ReadKey();
        }

        public static void themSinhVien(List<SinhVien> danhSachSinhVien, SqlConnection sqlConnection)
        {
            SinhVien sv = new SinhVien();

            Console.WriteLine("Nhap ma sinh vien: ");
            sv.MaSV = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Nhap ten sinh vien: ");
            sv.TenSV = Console.ReadLine();

            Console.WriteLine("Nhap gioi tinh: ");
            sv.GioiTinh = Console.ReadLine();

            Console.WriteLine("Nhap email: ");
            sv.Email = Console.ReadLine();

            Console.WriteLine("Nhap dia chi: ");
            sv.DiaChi = Console.ReadLine();

            string insertQuery = "INSERT INTO dbo.Info(MaSV, TenSV, GioiTinh, Email, DiaChi) VALUES (@MaSV, @TenSV, @GioiTinh, @Email, @DiaChi)";
            SqlCommand insertCommand = new SqlCommand(insertQuery, sqlConnection);
            insertCommand.Parameters.AddWithValue("@MaSV", sv.MaSV);
            insertCommand.Parameters.AddWithValue("@TenSV", sv.TenSV);
            insertCommand.Parameters.AddWithValue("@GioiTinh", sv.GioiTinh);
            insertCommand.Parameters.AddWithValue("@Email", sv.Email);
            insertCommand.Parameters.AddWithValue("@DiaChi", sv.DiaChi);
            insertCommand.ExecuteNonQuery();
        }

        public static void xoaSinhVien(List<SinhVien> danhSachSinhVien, SqlConnection sqlConnection)
        {
            Console.WriteLine("Nhap ma sinh vien can xoa: ");
            int maSV = int.Parse(Console.ReadLine());

            string deleteQuery = "DELETE FROM dbo.Info WHERE MaSV = @maSV";

            SqlCommand deleteCommand = new SqlCommand(deleteQuery, sqlConnection);
            deleteCommand.Parameters.AddWithValue("@maSV", maSV);
            int result = deleteCommand.ExecuteNonQuery();

            if (result > 0)
            {
                SinhVien deleteSV = danhSachSinhVien.Find(index => index.MaSV == maSV);
                danhSachSinhVien.Remove(deleteSV);
                Console.WriteLine("Xoa sinh vien thanh cong");
            }
            else
            {
                Console.WriteLine($"Khong tim thay sinh vien co ma sinh vien: {maSV}");
            }
        }

        public static void suaSinhVien(List<SinhVien> danhSachSinhVien, SqlConnection sqlConnection)
        {
            Console.WriteLine("Nhap ma sinh vien can sua: ");
            int maSV;

            while (!int.TryParse(Console.ReadLine(), out maSV))
            {
                Console.WriteLine("Ma sinh vien la so nguyen. Vui long nhap lai!");
            }
            SinhVien svCanSua = danhSachSinhVien.Find(index => index.MaSV == maSV);
            if (svCanSua == null)
            {
                Console.WriteLine($"Khong tim thay sinh vien co ma {maSV}");
                return;
            }

            Console.WriteLine("Nhap ten sinh vien: ");
            svCanSua.TenSV = Console.ReadLine();

            Console.WriteLine("Nhap gioi tinh (Nam/Nu/Khac): ");
            svCanSua.GioiTinh = Console.ReadLine();

            Console.WriteLine("Nhap email: ");
            svCanSua.Email = Console.ReadLine();

            Console.WriteLine("Nhap dia chi: ");
            svCanSua.DiaChi = Console.ReadLine();

            string updateQuery = "UPDATE dbo.Info SET TenSV=@TenSV, GioiTinh=@GioiTinh, Email=@Email, DiaChi=@DiaChi WHERE MaSV=@MaSV";

            SqlCommand updateCommand = new SqlCommand(updateQuery, sqlConnection);
            updateCommand.Parameters.AddWithValue("@MaSV", svCanSua.MaSV);
            updateCommand.Parameters.AddWithValue("@TenSV", svCanSua.TenSV);
            updateCommand.Parameters.AddWithValue("@GioiTinh", svCanSua.GioiTinh);
            updateCommand.Parameters.AddWithValue("@Email", svCanSua.Email);
            updateCommand.Parameters.AddWithValue("@DiaChi", svCanSua.DiaChi);
            updateCommand.ExecuteNonQuery();

            Console.WriteLine("Sua sinh vien thanh cong!");

        }

     

        public static void hienThiDanhSachSinhVien(List<SinhVien> danhSachSinhVien, SqlConnection sqlConnection)
        {
            SqlCommand selectCommand = new SqlCommand("SELECT * FROM dbo.Info", sqlConnection);
            SqlDataReader reader = selectCommand.ExecuteReader();

            Console.WriteLine("Danh sach sinh vien:");

            while (reader.Read())
            {
                SinhVien sv = new SinhVien();
                sv.MaSV = (int)reader["MaSV"];
                sv.TenSV = (string)reader["TenSV"];
                sv.GioiTinh = (string)reader["GioiTinh"];
                sv.Email = (string)reader["email"];
                sv.DiaChi = (string)reader["DiaChi"];

                danhSachSinhVien.Add(sv);
                Console.WriteLine($"Ma sinh vien: {sv.MaSV}");
                Console.WriteLine($"Ten sinh vien: {sv.TenSV}");
                Console.WriteLine($"Gioi tinh: {sv.GioiTinh}");
                Console.WriteLine($"Email: {sv.Email}");
                Console.WriteLine($"Dia chi: {sv.DiaChi}");
            }

            reader.Close();



            //foreach (SinhVien sv in danhSachSinhVien)
            //{
            //Console.WriteLine($"Ma sinh vien: {sv.MaSV}");
            //Console.WriteLine($"Ten sinh vien: {sv.TenSV}");
            //Console.WriteLine($"Gioi tinh: {sv.GioiTinh}");
            //Console.WriteLine($"Email: {sv.Email}");
            //Console.WriteLine($"Dia chi: {sv.DiaChi}");
            //}
        }
    }
}
