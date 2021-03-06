﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Reflection;
using System.Configuration;
using System.Transactions;

namespace WSSmartPhone
{
    class CBaoBao
    {
        CConnection _DAL = new CConnection("Data Source=server9;Initial Catalog=BaoBao;Persist Security Info=True;User ID=sa;Password=db9@tanhoa");

        public bool ThemKhachHang(string HoTen, string GioiTinh, string DienThoai, string BienSoXe, string MaPhong)
        {
            int ID = 0;
            if (int.Parse(_DAL.ExecuteQuery_DataTable("select COUNT(ID) from KhachHang").Rows[0][0].ToString()) == 0)
                ID = 1;
            else
                ID = int.Parse(_DAL.ExecuteQuery_DataTable("select MAX(ID)+1 from KhachHang").Rows[0][0].ToString());
            string sql = "insert into KhachHang(ID,HoTen,GioiTinh,DienThoai,BienSoXe,MaPhong,CreateDate)values(" + ID + ",N'" + HoTen + "'," + GioiTinh + ",'" + DienThoai + "','" + BienSoXe + "'," + MaPhong + ",GETDATE())";
            return _DAL.ExecuteNonQuery(sql);
        }

        public bool SuaKhachHang(string ID, string HoTen, string GioiTinh, string DienThoai, string BienSoXe, string MaPhong)
        {
            string sql = "update KhachHang set HoTen=N'" + HoTen + "',GioiTinh=" + GioiTinh + ",DienThoai='" + DienThoai + "',BienSoXe='" + BienSoXe + "',MaPhong=" + MaPhong + " where ID=" + ID;
            return _DAL.ExecuteNonQuery(sql);
        }

        public bool XoaKhachHang(string ID)
        {
            string sql = "delete KhachHang where ID=" + ID;
            return _DAL.ExecuteNonQuery(sql);
        }

        public DataTable GetDSKhachHang()
        {
            string sql = "select a.*,TenPhong=Name from KhachHang a left join Phong b on a.MaPhong=b.ID";
            DataTable dt = new DataTable();
            dt = _DAL.ExecuteQuery_DataTable(sql);
            dt.TableName = "MyTableName";
            return dt;
        }

        public bool SuaPhong(string ID, string Name, string GiaTien, string SoNKNuoc, string ChiSoDien, string ChiSoNuoc, string NgayThue, string Thue)
        {
            string[] date = NgayThue.Split('/');
            string sql = "update Phong set Name=N'" + Name + "',GiaTien=" + GiaTien + ",SoNKNuoc=" + SoNKNuoc + ",ChiSoDien=" + ChiSoDien + ",ChiSoNuoc=" + ChiSoNuoc + ",NgayThue='" + date[2] + "-" + date[1] + "-" + date[0] + "',Thue=" + Thue + " where ID=" + ID;
            return _DAL.ExecuteNonQuery(sql);
        }

        public DataTable GetDSPhong()
        {
            string sql = "select ID,Name,GiaTien,Thue,NgayThue=CONVERT(varchar(10), NgayThue, 103),ChiSoDienOld,ChiSoDien,SoNKNuoc,ChiSoNuocOld,ChiSoNuoc from Phong";
            DataTable dt = new DataTable();
            dt = _DAL.ExecuteQuery_DataTable(sql);
            dt.TableName = "MyTableName";
            return dt;
        }

        public bool SuaGiaDien(string ID, string Name, string GiaTien)
        {
            string sql = "update GiaDien set Name=N'" + Name + "',GiaTien=" + GiaTien + " where ID=" + ID;
            return _DAL.ExecuteNonQuery(sql);
        }

        public DataTable GetDSGiaDien()
        {
            string sql = "select * from GiaDien";
            DataTable dt = new DataTable();
            dt = _DAL.ExecuteQuery_DataTable(sql);
            dt.TableName = "MyTableName";
            return dt;
        }

        public bool SuaGiaNuoc(string ID, string Name, string GiaTien)
        {
            string sql = "update GiaNuoc set Name=N'" + Name + "',GiaTien=" + GiaTien + " where ID=" + ID;
            return _DAL.ExecuteNonQuery(sql);
        }

        public DataTable GetDSGiaNuoc()
        {
            string sql = "select * from GiaNuoc";
            DataTable dt = new DataTable();
            dt = _DAL.ExecuteQuery_DataTable(sql);
            dt.TableName = "MyTableName";
            return dt;
        }

        public bool ThemHoaDon(string MaPhong, int ChiSoDienNew, int ChiSoNuocNew)
        {
            try
            {
                using (var scope = new TransactionScope())
                {
                    bool flag = true;

                    int SoNKDien = (int)_DAL.ExecuteQuery_ReturnOneValue("select COUNT(ID) from KhachHang where MaPhong=" + MaPhong);
                    int ChiSoDienOld = (int)_DAL.ExecuteQuery_ReturnOneValue("select ChiSoDien from Phong where ID=" + MaPhong);
                    int TieuThuDien = ChiSoDienNew - ChiSoDienOld;
                    if (TieuThuDien <= 0)
                        return false;
                    string ChiTietDien = "";
                    int TienDien = TinhTienDien(SoNKDien, TieuThuDien, out ChiTietDien);

                    int DinhMucNuoc = (int)_DAL.ExecuteQuery_ReturnOneValue("select SoNKNuoc from Phong where ID=" + MaPhong) * 4;
                    int ChiSoNuocOld = (int)_DAL.ExecuteQuery_ReturnOneValue("select ChiSoNuoc from Phong where ID=" + MaPhong);
                    int TieuThuNuoc = ChiSoNuocNew - ChiSoNuocOld;
                    if (TieuThuNuoc <= 0)
                        return false;
                    string ChiTietNuoc = "";
                    int TienNuoc = TinhTienNuoc(DinhMucNuoc, TieuThuNuoc, out ChiTietNuoc);

                    int ID = 0;
                    if (int.Parse(_DAL.ExecuteQuery_DataTable("select COUNT(ID) from HoaDon").Rows[0][0].ToString()) == 0)
                        ID = 1;
                    else
                        ID = int.Parse(_DAL.ExecuteQuery_DataTable("select MAX(ID)+1 from HoaDon").Rows[0][0].ToString());
                    string sql = "insert into HoaDon(ID,MaPhong,ChiSoDienOld,ChiSoDienNew,TieuThuDien,TienDien,ChiTietDien,ChiSoNuocOld,ChiSoNuocNew,TieuThuNuoc,TienNuoc,ChiTietNuoc,CreateDate)values(" + ID + "," + MaPhong + ","
                               + ChiSoDienOld + "," + ChiSoDienNew + "," + TieuThuDien + "," + TienDien + ",N'" + ChiTietDien + "',"
                               + ChiSoNuocOld + "," + ChiSoNuocNew + "," + TieuThuNuoc + "," + TienNuoc + ",N'" + ChiTietNuoc + "',GETDATE())";

                    if (_DAL.ExecuteNonQuery(sql) == false)
                        flag = false;

                    if (_DAL.ExecuteNonQuery("update Phong set ChiSoDienOld=ChiSoDien,ChiSoDien=" + ChiSoDienNew + ",ChiSoNuocOld=ChiSoNuoc,ChiSoNuoc=" + ChiSoNuocNew + " where ID=" + MaPhong) == false)
                        flag = false;

                    if (flag == true)
                    {
                        scope.Complete();
                        return true;
                    }
                    else
                        return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool SuaHoaDon(string ID, int ChiSoDienNew, int ChiSoNuocNew)
        {
            try
            {
                using (var scope = new TransactionScope())
                {
                    bool flag = true;
                    int MaPhong = (int)_DAL.ExecuteQuery_ReturnOneValue("select MaPhong from HoaDon where ID=" + ID);

                    int SoNKDien = (int)_DAL.ExecuteQuery_ReturnOneValue("select COUNT(ID) from KhachHang where MaPhong=" + MaPhong);
                    int ChiSoDienOld = (int)_DAL.ExecuteQuery_ReturnOneValue("select ChiSoDienOld from Phong where ID=" + MaPhong);
                    int TieuThuDien = ChiSoDienNew - ChiSoDienOld;
                    if (TieuThuDien <= 0)
                        return false;
                    string ChiTietDien = "";
                    int TienDien = TinhTienDien(SoNKDien, TieuThuDien, out ChiTietDien);

                    int DinhMucNuoc = (int)_DAL.ExecuteQuery_ReturnOneValue("select SoNKNuoc from Phong where ID=" + MaPhong) * 4;
                    int ChiSoNuocOld = (int)_DAL.ExecuteQuery_ReturnOneValue("select ChiSoNuocOld from Phong where ID=" + MaPhong);
                    int TieuThuNuoc = ChiSoNuocNew - ChiSoNuocOld;
                    if (TieuThuNuoc <= 0)
                        return false;
                    string ChiTietNuoc = "";
                    int TienNuoc = TinhTienNuoc(DinhMucNuoc, TieuThuNuoc, out ChiTietNuoc);

                    string sql = "update HoaDon set ChiSoDienNew=" + ChiSoDienNew + ",TieuThuDien=" + TieuThuDien + ",TienDien=" + TienDien + ",ChiTietDien='" + ChiTietDien
                               + "',ChiSoNuocNew=" + ChiSoNuocNew + ",TieuThuNuoc=" + TieuThuNuoc + ",TienNuoc=" + TienNuoc + ",ChiTietNuoc='" + ChiTietNuoc + "' where ID=" + ID;

                    if (_DAL.ExecuteNonQuery(sql) == false)
                        flag = false;

                    if (_DAL.ExecuteNonQuery("update Phong set ChiSoDien=" + ChiSoDienNew + ",ChiSoNuoc=" + ChiSoNuocNew + " where ID=" + MaPhong) == false)
                        flag = false;

                    if (flag == true)
                    {
                        scope.Complete();
                        return true;
                    }
                    else
                        return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool XoaHoaDon(string ID)
        {
            try
            {
                using (var scope = new TransactionScope())
                {
                    bool flag = true;
                    int MaPhong = (int)_DAL.ExecuteQuery_ReturnOneValue("select MaPhong from HoaDon where ID=" + ID);

                    string sql = "update Phong set ChiSoDien=ChiSoDienOld,ChiSoDienOld=0,ChiSoNuoc=ChiSoNuocOld,ChiSoNuocOld=0 where ID=" + MaPhong;
                    if (_DAL.ExecuteNonQuery(sql) == false)
                        flag = false;

                    sql = "delete HoaDon where ID=" + ID;
                    if (_DAL.ExecuteNonQuery(sql) == false)
                        flag = false;

                    if (flag == true)
                    {
                        scope.Complete();
                        return true;
                    }
                    else
                        return false;
                }
            }
            catch (Exception)
            {
                return false;
            }

        }

        public DataTable GetDSHoaDon()
        {
            string sql = "select a.*,TenPhong=Name from HoaDon a left join Phong b on a.MaPhong=b.ID";
            DataTable dt = new DataTable();
            dt = _DAL.ExecuteQuery_DataTable(sql);
            dt.TableName = "MyTableName";
            return dt;
        }

        public DataTable GetDSHoaDon(string MaPhong)
        {
            string sql = "select a.*,TenPhong=Name from HoaDon a left join Phong b on a.MaPhong=b.ID where MaPhong=" + MaPhong;
            DataTable dt = new DataTable();
            dt = _DAL.ExecuteQuery_DataTable(sql);
            dt.TableName = "MyTableName";
            return dt;
        }

        public int TinhTienDien(int SoNKDien, int TieuThu, out string ChiTiet)
        {
            int B1 = (int)Math.Round(50 * ((double)SoNKDien / 4), 1);
            int B2 = (int)Math.Round(100 * ((double)SoNKDien / 4), 1);
            int B3 = (int)Math.Round(200 * ((double)SoNKDien / 4), 1);
            int B4 = (int)Math.Round(300 * ((double)SoNKDien / 4), 1);
            int B5 = (int)Math.Round(400 * ((double)SoNKDien / 4), 1);

            DataTable dtGiaDien = _DAL.ExecuteQuery_DataTable("select * from GiaDien");
            int TienDien = 0;
            ChiTiet = "";
            if (TieuThu <= B1)
            {
                TienDien = TieuThu * (int)dtGiaDien.Rows[0]["GiaTien"];
                ChiTiet = TieuThu + " x " + String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN"), "{0:#,##}", (int)dtGiaDien.Rows[0]["GiaTien"]);
            }
            else
                if (B1 < TieuThu && TieuThu <= B2)
                {
                    TienDien = (B1 * (int)dtGiaDien.Rows[0]["GiaTien"]) + ((TieuThu - B1) * (int)dtGiaDien.Rows[1]["GiaTien"]);
                    ChiTiet = B1 + " x " + String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN"), "{0:#,##}", (int)dtGiaDien.Rows[0]["GiaTien"]) + "\r\n"
                            + (TieuThu - B1).ToString() + " x " + String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN"), "{0:#,##}", (int)dtGiaDien.Rows[1]["GiaTien"]);
                }
                else
                    if (B2 < TieuThu && TieuThu <= B3)
                    {
                        TienDien = (B1 * (int)dtGiaDien.Rows[0]["GiaTien"]) + (B2 * (int)dtGiaDien.Rows[1]["GiaTien"]) + ((TieuThu - 100) * (int)dtGiaDien.Rows[2]["GiaTien"]);
                        ChiTiet = B1 + " x " + String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN"), "{0:#,##}", (int)dtGiaDien.Rows[0]["GiaTien"]) + "\r\n"
                                + B2 + " x " + String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN"), "{0:#,##}", (int)dtGiaDien.Rows[1]["GiaTien"]) + "\r\n"
                                + (TieuThu - B2).ToString() + " x " + String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN"), "{0:#,##}", (int)dtGiaDien.Rows[2]["GiaTien"]);
                    }
                    else
                        if (B3 < TieuThu && TieuThu <= B4)
                        {
                            TienDien = (B1 * (int)dtGiaDien.Rows[0]["GiaTien"]) + (B2 * (int)dtGiaDien.Rows[1]["GiaTien"]) + (B3 * (int)dtGiaDien.Rows[2]["GiaTien"]) + ((TieuThu - B3) * (int)dtGiaDien.Rows[3]["GiaTien"]);
                            ChiTiet = B1 + " x " + String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN"), "{0:#,##}", (int)dtGiaDien.Rows[0]["GiaTien"]) + "\r\n"
                                    + B2 + " x " + String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN"), "{0:#,##}", (int)dtGiaDien.Rows[1]["GiaTien"]) + "\r\n"
                                    + B3 + " x " + String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN"), "{0:#,##}", (int)dtGiaDien.Rows[2]["GiaTien"]) + "\r\n"
                                    + (TieuThu - B3).ToString() + " x " + String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN"), "{0:#,##}", (int)dtGiaDien.Rows[3]["GiaTien"]);
                        }
                        else
                            if (B3 < TieuThu && TieuThu <= B5)
                            {
                                TienDien = (B1 * (int)dtGiaDien.Rows[0]["GiaTien"]) + (B2 * (int)dtGiaDien.Rows[1]["GiaTien"]) + (B3 * (int)dtGiaDien.Rows[2]["GiaTien"]) + (B4 * (int)dtGiaDien.Rows[3]["GiaTien"]) + ((TieuThu - B4) * (int)dtGiaDien.Rows[4]["GiaTien"]);
                                ChiTiet = B1 + " x " + String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN"), "{0:#,##}", (int)dtGiaDien.Rows[0]["GiaTien"]) + "\r\n"
                                        + B2 + " x " + String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN"), "{0:#,##}", (int)dtGiaDien.Rows[1]["GiaTien"]) + "\r\n"
                                        + B3 + " x " + String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN"), "{0:#,##}", (int)dtGiaDien.Rows[2]["GiaTien"]) + "\r\n"
                                        + B4 + " x " + String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN"), "{0:#,##}", (int)dtGiaDien.Rows[3]["GiaTien"]) + "\r\n"
                                        + (TieuThu - B4).ToString() + " x " + String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN"), "{0:#,##}", (int)dtGiaDien.Rows[4]["GiaTien"]);
                            }
                            else
                                if (B4 < TieuThu)
                                {
                                    TienDien = (B1 * (int)dtGiaDien.Rows[0]["GiaTien"]) + (B2 * (int)dtGiaDien.Rows[1]["GiaTien"]) + (B3 * (int)dtGiaDien.Rows[2]["GiaTien"]) + (B4 * (int)dtGiaDien.Rows[3]["GiaTien"]) + (B5 * (int)dtGiaDien.Rows[4]["GiaTien"]) + ((TieuThu - B5) * (int)dtGiaDien.Rows[5]["GiaTien"]);
                                    ChiTiet = B1 + " x " + String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN"), "{0:#,##}", (int)dtGiaDien.Rows[0]["GiaTien"]) + "\r\n"
                                             + B2 + " x " + String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN"), "{0:#,##}", (int)dtGiaDien.Rows[1]["GiaTien"]) + "\r\n"
                                             + B3 + " x " + String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN"), "{0:#,##}", (int)dtGiaDien.Rows[2]["GiaTien"]) + "\r\n"
                                             + B4 + " x " + String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN"), "{0:#,##}", (int)dtGiaDien.Rows[3]["GiaTien"]) + "\r\n"
                                             + B5 + " x " + String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN"), "{0:#,##}", (int)dtGiaDien.Rows[4]["GiaTien"]) + "\r\n"
                                             + (TieuThu - B5).ToString() + " x " + String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN"), "{0:#,##}", (int)dtGiaDien.Rows[5]["GiaTien"]);
                                }
            
            TienDien = (int)(TienDien * 1.10);
            ChiTiet += "\r\nThuế 10% " + ((int)(TienDien * 0.10)).ToString();
            ///tỷ lệ hao hụt
            TienDien += (int)Math.Round(TienDien * double.Parse(dtGiaDien.Rows[6]["GiaTien"].ToString()) / 100);
            ChiTiet += "\r\nTỷ lệ hao hụt " + dtGiaDien.Rows[6]["GiaTien"].ToString() + "% " + (int)Math.Round(TienDien * double.Parse(dtGiaDien.Rows[6]["GiaTien"].ToString()) / 100);
            return TienDien;
        }

        public int TinhTienNuoc(int DinhMuc, int TieuThu, out string ChiTiet)
        {
            DataTable dtGiaNuoc = _DAL.ExecuteQuery_DataTable("select * from GiaNuoc");
            int TienNuoc = 0;
            if (TieuThu <= DinhMuc)
            {
                TienNuoc = (TieuThu * (int)dtGiaNuoc.Rows[0]["GiaTien"]);

                ChiTiet = TieuThu + " x " + String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN"), "{0:#,##}", (int)dtGiaNuoc.Rows[0]["GiaTien"]);
            }
            else
            {
                if (TieuThu - DinhMuc <= Math.Round((double)DinhMuc / 2))
                {
                    TienNuoc = (DinhMuc * (int)dtGiaNuoc.Rows[0]["GiaTien"])
                            + (TieuThu-(int)Math.Round((double)DinhMuc / 2) * (int)dtGiaNuoc.Rows[1]["GiaTien"]);

                    ChiTiet = DinhMuc + " x " + String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN"), "{0:#,##}", (int)dtGiaNuoc.Rows[0]["GiaTien"]) + "\r\n"
                               + (TieuThu-(int)Math.Round((double)DinhMuc / 2)) + " x " + String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN"), "{0:#,##}", (int)dtGiaNuoc.Rows[1]["GiaTien"]);
                }
                else
                {
                    TienNuoc = (DinhMuc * (int)dtGiaNuoc.Rows[0]["GiaTien"])
                            + ((int)Math.Round((double)DinhMuc / 2) * (int)dtGiaNuoc.Rows[1]["GiaTien"])
                            + ((TieuThu - DinhMuc - (int)Math.Round((double)DinhMuc / 2)) * (int)dtGiaNuoc.Rows[2]["GiaTien"]);

                    ChiTiet = DinhMuc + " x " + String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN"), "{0:#,##}", (int)dtGiaNuoc.Rows[0]["GiaTien"]) + "\r\n"
                               + (int)Math.Round((double)DinhMuc / 2) + " x " + String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN"), "{0:#,##}", (int)dtGiaNuoc.Rows[1]["GiaTien"]) + "\r\n"
                               + (TieuThu - DinhMuc - (int)Math.Round((double)DinhMuc / 2)) + " x " + String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN"), "{0:#,##}", (int)dtGiaNuoc.Rows[2]["GiaTien"]);
                }
            }

            TienNuoc = (int)(TienNuoc * 1.15);
            ChiTiet += "\r\nThuế 15% " + ((int)(TienNuoc * 0.15)).ToString();
            TienNuoc += (int)Math.Round(TienNuoc * double.Parse(dtGiaNuoc.Rows[3]["GiaTien"].ToString()) / 100);
            ChiTiet += "\r\nTỷ lệ hao hụt " + dtGiaNuoc.Rows[3]["GiaTien"].ToString() + "% " + (int)Math.Round(TienNuoc * double.Parse(dtGiaNuoc.Rows[3]["GiaTien"].ToString()) / 100);
            return TienNuoc;
        }

    }
}