﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using WSTanHoa.Models;
using WSTanHoa.Providers;

namespace WSTanHoa.Controllers
{
    [RoutePrefix("api/TrungTamKhachHang")]
    public class apiTrungTamKhachHangController : ApiController
    {
        CConnection _cDAL_DHN = new CConnection(CConstantVariable.DHN);
        CConnection _cDAL_DocSo = new CConnection(CConstantVariable.DocSo);
        CConnection _cDAL_DocSo12 = new CConnection(CConstantVariable.DocSo12);
        CConnection _cDAL_GanMoi = new CConnection(CConstantVariable.GanMoi);
        CConnection _cDAL_ThuTien = new CConnection(CConstantVariable.ThuTien);
        CConnection _cDAL_KinhDoanh = new CConnection(CConstantVariable.KinhDoanh);
        CConnection _cDAL_TrungTam = new CConnection(CConstantVariable.TrungTamKhachHang);
        string _pass = "s@l@2019";

        /// <summary>
        /// Lấy thông tin khách hàng
        /// </summary>
        /// <param name="DanhBo"></param>
        /// <param name="checksum"></param>
        /// <returns></returns>
        [Route("getThongTinKhachHang")]
        public ThongTinKhachHang getThongTinKhachHang(string DanhBo, string checksum)
        {
            try
            {
                //if (CConstantVariable.getSHA256(DanhBo + _pass) != checksum)
                //{
                //    ErrorResponse error = new ErrorResponse(ErrorResponse.ErrorPassword, ErrorResponse.ErrorCodePassword);
                //    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.OK, error));
                //}
                DataTable dt = new DataTable();

                //lấy thông tin khách hàng
                string sql = "select DanhBo"
                             + ",HoTen"
                             + ",DiaChi=SoNha+' '+TenDuong+', P.'+(select TenPhuong from Phuong where MaPhuong=Phuong and MaQuan=Quan)+', Q.'+(select TenQuan from Quan where MaQuan=Quan)"
                             + ",DiaChiHoaDon=DiaChiHoaDon+', P.'+(select TenPhuong from Phuong where MaPhuong=Phuong and MaQuan=Quan)+', Q.'+(select TenQuan from Quan where MaQuan=Quan)"
                             + ",HopDong"
                             + ",DienThoai"
                             + ",MLT=LoTrinh"
                             + ",DinhMuc"
                             + ",DinhMucHN"
                             + ",GiaBieu"
                             + ",HieuDH"
                             + ",CoDH"
                             + ",Cap"
                             + ",SoThanDH"
                             + ",ViTriDHN"
                             + ",NgayThay"
                             + ",NgayKiemDinh"
                             + ",HieuLuc=convert(varchar(2),Ky)+'/'+convert(char(4),Nam)"
                             + " from TB_DULIEUKHACHHANG where DanhBo=" + DanhBo;
                dt.Merge(_cDAL_DHN.ExecuteQuery_DataTable(sql));
                //lấy thông tin khách hàng đã hủy
                if (dt == null || dt.Rows.Count == 0)
                {
                    sql = "select DanhBo"
                                 + ",HoTen"
                                 + ",DiaChi=SoNha+' '+TenDuong+', P.'+(select TenPhuong from Phuong where MaPhuong=Phuong and MaQuan=Quan)+', Q.'+(select TenQuan from Quan where MaQuan=Quan)"
                                 + ",DiaChiHoaDon=DiaChiHoaDon+', P.'+(select TenPhuong from Phuong where MaPhuong=Phuong and MaQuan=Quan)+', Q.'+(select TenQuan from Quan where MaQuan=Quan)"
                                 + ",HopDong"
                                 + ",DienThoai=''"
                                 + ",MLT=LoTrinh"
                                 + ",DinhMuc"
                                 + ",DinhMucHN"
                                 + ",GiaBieu"
                                 + ",HieuDH"
                                 + ",CoDH"
                                 + ",Cap"
                                 + ",SoThanDH"
                                 + ",ViTriDHN"
                                 + ",NgayThay"
                                 + ",NgayKiemDinh"
                                 + ",HieuLuc=convert(varchar(2),Ky)+'/'+convert(char(4),Nam)"
                                 + " from TB_DULIEUKHACHHANG_HUYDB where DanhBo=" + DanhBo;
                    dt.Merge(_cDAL_DHN.ExecuteQuery_DataTable(sql));
                }
                //
                if (dt != null && dt.Rows.Count > 0)
                {
                    ThongTinKhachHang en = new ThongTinKhachHang();
                    en.DanhBo = dt.Rows[0]["DanhBo"].ToString();
                    en.HoTen = dt.Rows[0]["HoTen"].ToString();
                    if (dt.Rows[0]["DiaChi"].ToString() == dt.Rows[0]["DiaChiHoaDon"].ToString())
                        en.DiaChi = dt.Rows[0]["DiaChi"].ToString();
                    else
                        en.DiaChi = dt.Rows[0]["DiaChi"].ToString() + " =>HD: " + dt.Rows[0]["DiaChiHoaDon"].ToString();
                    en.HopDong = dt.Rows[0]["HopDong"].ToString();
                    en.DienThoai = dt.Rows[0]["DienThoai"].ToString();
                    en.MLT = dt.Rows[0]["MLT"].ToString();
                    en.DinhMuc = dt.Rows[0]["DinhMuc"].ToString();
                    en.DinhMucHN = dt.Rows[0]["DinhMucHN"].ToString();
                    en.GiaBieu = dt.Rows[0]["GiaBieu"].ToString();
                    en.HieuDH = dt.Rows[0]["HieuDH"].ToString();
                    en.CoDH = dt.Rows[0]["CoDH"].ToString();
                    en.Cap = dt.Rows[0]["Cap"].ToString();
                    en.SoThanDH = dt.Rows[0]["SoThanDH"].ToString();
                    en.ViTriDHN = dt.Rows[0]["ViTriDHN"].ToString();
                    if (dt.Rows[0]["NgayThay"].ToString() != "")
                        en.NgayThay = DateTime.Parse(dt.Rows[0]["NgayThay"].ToString());
                    if (dt.Rows[0]["NgayKiemDinh"].ToString() != "")
                        en.NgayKiemDinh = DateTime.Parse(dt.Rows[0]["NgayKiemDinh"].ToString());
                    en.HieuLuc = dt.Rows[0]["HieuLuc"].ToString();
                    if ((int)_cDAL_DocSo12.ExecuteQuery_ReturnOneValue("select count(DanhBa) from KhachHang where DanhBa='" + dt.Rows[0]["DanhBo"].ToString() + "' and Gieng=1") == 1)
                        en.ThongTin = "Có sử dụng Giếng";
                    if (en.ThongTin != "")
                        en.ThongTin += " - ";
                    en.ThongTin += "<a target='_blank' href='http://113.161.88.180:1802/api/thuongvu/hosogoc/" + en.DanhBo + "'>Hồ sơ gốc</a>";
                    return en;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                ErrorResponse error = new ErrorResponse(ex.Message, ErrorResponse.ErrorCodeSQL);
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.OK, error));
            }
        }

        /// <summary>
        /// Lấy danh sách 12 kỳ đọc số gần nhất
        /// </summary>
        /// <param name="DanhBo"></param>
        /// <param name="checksum"></param>
        /// <returns></returns>
        //[Route("getGhiChiSo")]
        //public IList<GhiChiSo> getGhiChiSo(string DanhBo, string checksum)
        //{
        //    if (CConstantVariable.getSHA256(DanhBo + _pass) != checksum)
        //    {
        //        ErrorResponse error = new ErrorResponse(ErrorResponse.ErrorPassword, ErrorResponse.ErrorCodePassword);
        //        throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.OK, error));
        //    }
        //    DataTable dt = new DataTable();
        //    try
        //    {
        //        string sql = "select top(12) Ky=CONVERT(char(2),Ky)+'/'+CONVERT(char(4),Nam)"
        //                       //+ ",NgayDoc=CONVERT(char(10),DenNgay,103)"
        //                       + ",NgayDoc=DenNgay"
        //                       + ",CodeMoi"
        //                       + ",ChiSoCu=CSCu"
        //                       + ",ChiSoMoi=CSMoi"
        //                       + ",TieuThu=TieuThuMoi"
        //                       + " from DocSo"
        //                       + " where DanhBa=" + DanhBo
        //                       + " order by Nam desc,CAST(Ky as int) desc";
        //        dt = _cDAL_DocSo.ExecuteQuery_DataTable(sql);
        //        //
        //        if (dt != null && dt.Rows.Count > 0)
        //        {
        //            List<GhiChiSo> lst = new List<GhiChiSo>();
        //            foreach (DataRow item in dt.Rows)
        //            {
        //                GhiChiSo en = new GhiChiSo();
        //                en.Ky = item["Ky"].ToString();
        //                if (item["NgayDoc"].ToString() != "")
        //                    en.NgayDoc = DateTime.Parse(item["NgayDoc"].ToString());
        //                en.CodeMoi = item["CodeMoi"].ToString();
        //                en.ChiSoCu = item["ChiSoCu"].ToString();
        //                en.ChiSoMoi = item["ChiSoMoi"].ToString();
        //                en.TieuThu = item["TieuThu"].ToString();
        //                lst.Add(en);
        //            }
        //            return lst;
        //        }
        //        else
        //            return null;
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorResponse error = new ErrorResponse(ex.Message, ErrorResponse.ErrorCodeSQL);
        //        throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.OK, error));
        //    }
        //}

        /// <summary>
        /// Lấy danh sách thông tin ghi chú
        /// </summary>
        /// <param name="DanhBo"></param>
        /// <param name="checksum"></param>
        /// <returns></returns>
        //[Route("getGhiChu")]
        //public IList<GhiChu> getGhiChu(string DanhBo, string checksum)
        //{
        //    if (CConstantVariable.getSHA256(DanhBo + _pass) != checksum)
        //    {
        //        ErrorResponse error = new ErrorResponse(ErrorResponse.ErrorPassword, ErrorResponse.ErrorCodePassword);
        //        throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.OK, error));
        //    }
        //    DataTable dt = new DataTable();
        //    try
        //    {
        //        string sql = "select NoiDung,CreateDate from TB_GHICHU where DanhBo=" + DanhBo + " order by CreateDate desc";
        //        dt = _cDAL_DHN.ExecuteQuery_DataTable(sql);
        //        //
        //        if (dt != null && dt.Rows.Count > 0)
        //        {
        //            List<GhiChu> lst = new List<GhiChu>();
        //            foreach (DataRow item in dt.Rows)
        //            {
        //                GhiChu en = new GhiChu();
        //                en.NoiDung = item["NoiDung"].ToString();
        //                if (item["CreateDate"].ToString() != "")
        //                    en.CreateDate = DateTime.Parse(item["CreateDate"].ToString());
        //                lst.Add(en);
        //            }
        //            return lst;
        //        }
        //        else
        //            return null;
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorResponse error = new ErrorResponse(ex.Message, ErrorResponse.ErrorCodeSQL);
        //        throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.OK, error));
        //    }
        //}

        /// <summary>
        /// Lấy thông tin nhân viên đọc số, lịch ghi chỉ số nước, ghi chú
        /// </summary>
        /// <param name="DanhBo"></param>
        /// <param name="checksum"></param>
        /// <returns></returns>
        [Route("getDocSo")]
        public DocSo getDocSo(string DanhBo, string checksum)
        {
            try
            {
                if (CConstantVariable.getSHA256(DanhBo + _pass) != checksum)
                {
                    ErrorResponse error = new ErrorResponse(ErrorResponse.ErrorPassword, ErrorResponse.ErrorCodePassword);
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.OK, error));
                }
                DocSo en = new DocSo();
                DataTable dt = new DataTable();

                //lấy lịch ghi chỉ số
                string sql = "select top(12) Ky=CONVERT(char(2),Ky)+'/'+CONVERT(char(4),Nam)"
                               //+ ",NgayDoc=CONVERT(char(10),DenNgay,103)"
                               + ",NgayDoc=DenNgay"
                               + ",CodeMoi"
                               + ",ChiSoCu=CSCu"
                               + ",ChiSoMoi=CSMoi"
                               + ",TieuThu=TieuThuMoi"
                               + ",MLT=MLT2"
                               + " from DocSo"
                               + " where DanhBa=" + DanhBo
                               + " order by Nam desc,CAST(Ky as int) desc";
                dt = _cDAL_DocSo.ExecuteQuery_DataTable(sql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    en.NhanVien = _cDAL_DocSo.ExecuteQuery_ReturnOneValue("select N'Nhân viên ghi chỉ số: '+NhanVienID+' : '+DienThoai from MayDS where May=" + dt.Rows[0]["MLT"].ToString().Substring(2, 2)).ToString();
                    //string sql_Lich = "select top 1 NoiDung=N'Kỳ '+CONVERT(varchar(2),a.Ky)+'/'+CONVERT(varchar(4),a.Nam)+N' dự kiến sẽ được ghi chỉ số vào ngày '+CONVERT(varchar(10),b.NgayDoc,103) from Lich_DocSo a,Lich_DocSo_ChiTiet b,Lich_Dot c where a.ID=b.IDDocSo and c.ID=b.IDDot"
                    //                                    + " and((c.TB1_From <= " + dt.Rows[0]["MLT"].ToString() + " and c.TB1_To >= " + dt.Rows[0]["MLT"].ToString() + ")"
                    //                                    + " or (c.TB2_From <= " + dt.Rows[0]["MLT"].ToString() + " and c.TB2_To >= " + dt.Rows[0]["MLT"].ToString() + ")"
                    //                                    + " or (c.TP1_From <= " + dt.Rows[0]["MLT"].ToString() + " and c.TP1_To >= " + dt.Rows[0]["MLT"].ToString() + ")"
                    //                                    + " or (c.TP2_From <= " + dt.Rows[0]["MLT"].ToString() + " and c.TP2_To >= " + dt.Rows[0]["MLT"].ToString() + "))"
                    //                                    + " order by a.CreateDate desc";
                    en.NhanVien += " ; " + getLichDocSo_Func_String(DanhBo, dt.Rows[0]["MLT"].ToString());

                    foreach (DataRow item in dt.Rows)
                    {
                        GhiChiSo enCT = new GhiChiSo();
                        enCT.Ky = item["Ky"].ToString();
                        if (item["NgayDoc"].ToString() != "")
                            enCT.NgayDoc = DateTime.Parse(item["NgayDoc"].ToString());
                        enCT.CodeMoi = item["CodeMoi"].ToString();
                        enCT.ChiSoCu = item["ChiSoCu"].ToString();
                        enCT.ChiSoMoi = item["ChiSoMoi"].ToString();
                        enCT.TieuThu = item["TieuThu"].ToString();
                        en.lstGhiChiSo.Add(enCT);
                    }
                }
                //lấy ghi chú
                sql = "select NoiDung,CreateDate from TB_GHICHU where DanhBo=" + DanhBo + " order by CreateDate desc";
                dt = _cDAL_DHN.ExecuteQuery_DataTable(sql);
                //
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        GhiChu enCT = new GhiChu();
                        enCT.NoiDung = item["NoiDung"].ToString();
                        if (item["CreateDate"].ToString() != "")
                            enCT.CreateDate = DateTime.Parse(item["CreateDate"].ToString());
                        en.lstGhiChu.Add(enCT);
                    }
                }
                return en;
            }
            catch (Exception ex)
            {
                ErrorResponse error = new ErrorResponse(ex.Message, ErrorResponse.ErrorCodeSQL);
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.OK, error));
            }
        }

        /// <summary>
        /// Lấy lịch đọc số
        /// </summary>
        /// <param name="DanhBo"></param>
        /// <param name="checksum"></param>
        /// <returns></returns>
        [Route("getLichDocSo")]
        public ThongTinKhachHang getLichDocSo(string DanhBo, string checksum)
        {
            try
            {
                if (CConstantVariable.getSHA256(DanhBo + _pass) != checksum)
                {
                    ErrorResponse error = new ErrorResponse(ErrorResponse.ErrorPassword, ErrorResponse.ErrorCodePassword);
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.OK, error));
                }
                DataTable dt_ThongTin = _cDAL_ThuTien.ExecuteQuery_DataTable("select top 1 DanhBo=DANHBA,HoTen=TENKH,DiaChi=(SO+' '+DUONG),GiaBieu=GB,DinhMuc=DM,MLT=MALOTRINH from HOADON where DANHBA='" + DanhBo + "' order by ID_HOADON desc");
                if (dt_ThongTin != null && dt_ThongTin.Rows.Count > 0)
                {
                    ThongTinKhachHang en = new ThongTinKhachHang();
                    en.DanhBo = dt_ThongTin.Rows[0]["DanhBo"].ToString();
                    en.HoTen = dt_ThongTin.Rows[0]["HoTen"].ToString();
                    en.DiaChi = dt_ThongTin.Rows[0]["DiaChi"].ToString();
                    en.DinhMuc = dt_ThongTin.Rows[0]["DinhMuc"].ToString();
                    en.GiaBieu = dt_ThongTin.Rows[0]["GiaBieu"].ToString();

                    //string sql_Lich = "select top 1 NoiDung=N'Kỳ '+CONVERT(varchar(2),a.Ky)+'/'+CONVERT(varchar(4),a.Nam)+N' dự kiến sẽ được ghi chỉ số vào ngày '+CONVERT(varchar(10),b.NgayDoc,103) from Lich_DocSo a,Lich_DocSo_ChiTiet b,Lich_Dot c where a.ID=b.IDDocSo and c.ID=b.IDDot"
                    //                + " and((c.TB1_From <= " + dt_ThongTin.Rows[0]["MLT"].ToString() + " and c.TB1_To >= " + dt_ThongTin.Rows[0]["MLT"].ToString() + ")"
                    //                + " or (c.TB2_From <= " + dt_ThongTin.Rows[0]["MLT"].ToString() + " and c.TB2_To >= " + dt_ThongTin.Rows[0]["MLT"].ToString() + ")"
                    //                + " or (c.TP1_From <= " + dt_ThongTin.Rows[0]["MLT"].ToString() + " and c.TP1_To >= " + dt_ThongTin.Rows[0]["MLT"].ToString() + ")"
                    //                + " or (c.TP2_From <= " + dt_ThongTin.Rows[0]["MLT"].ToString() + " and c.TP2_To >= " + dt_ThongTin.Rows[0]["MLT"].ToString() + "))"
                    //                + " order by a.CreateDate desc";
                    string result_Lich = getLichDocSo_Func_String(DanhBo, dt_ThongTin.Rows[0]["MLT"].ToString());

                    string result_NhanVien = _cDAL_DocSo.ExecuteQuery_ReturnOneValue("select NhanVien=N'Nhân viên ghi chỉ số: '+NhanVienID+' : '+DienThoai from MayDS where May=" + dt_ThongTin.Rows[0]["MLT"].ToString().Substring(2, 2)).ToString();

                    en.ThongTin = result_NhanVien + " ; " + result_Lich;
                    return en;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                ErrorResponse error = new ErrorResponse(ex.Message, ErrorResponse.ErrorCodeSQL);
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.OK, error));
            }
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public DataTable getLichDocSo_Func_SQL(string DanhBo, string MLT)
        {
            string sql_Lich = "WITH docso AS(select top 2 KY, NAM from [SERVER8].[DocSoTH].[dbo].[DocSo] where DANHBA = '" + DanhBo + "' order by DocSoID desc)"
                                   + " select distinct NoiDung=N'Kỳ '+CONVERT(varchar(2),a.Ky)+'/'+CONVERT(varchar(4),a.Nam)+N' dự kiến sẽ được ghi chỉ số vào ngày '+CONVERT(varchar(10),b.NgayDoc,103)"
                                   + " ,NgayDoc,NgayChuyenListing"
                                   + " from Lich_DocSo a,Lich_DocSo_ChiTiet b,Lich_Dot c,docso where a.ID=b.IDDocSo and c.ID=b.IDDot and ((a.Nam>docso.Nam) or (a.Nam=docso.Nam and a.Ky>=docso.Ky))"
                                   + " and((c.TB1_From <= " + MLT + " and c.TB1_To >= " + MLT + ")"
                                   + " or (c.TB2_From <= " + MLT + " and c.TB2_To >= " + MLT + ")"
                                   + " or (c.TP1_From <= " + MLT + " and c.TP1_To >= " + MLT + ")"
                                   + " or (c.TP2_From <= " + MLT + " and c.TP2_To >= " + MLT + "))";
            //+ " order by a.CreateDate desc";
            return _cDAL_TrungTam.ExecuteQuery_DataTable(sql_Lich);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public string getLichDocSo_Func_String(string DanhBo, string MLT)
        {
            DataTable dt = getLichDocSo_Func_SQL(DanhBo, MLT);
            string result = "";
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow item in dt.Rows)
                    if (DateTime.Parse(item["NgayDoc"].ToString()).Date >= DateTime.Now.Date)
                    {
                        result = item["NoiDung"].ToString();
                        break;
                    }
                if (result == "")
                    result = dt.Rows[dt.Rows.Count - 1]["NoiDung"].ToString();
            }
            return result;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public DataRow getLichDocSo_Func_DataRow(string DanhBo, string MLT)
        {
            DataTable dt = getLichDocSo_Func_SQL(DanhBo, MLT);
            DataRow result = null;
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow item in dt.Rows)
                    if (DateTime.Parse(item["NgayDoc"].ToString()).Date >= DateTime.Now.Date)
                    {
                        result = item;
                        break;
                    }
                if (result == null)
                    result = dt.Rows[dt.Rows.Count - 1];
            }
            return result;
        }

        /// <summary>
        /// Lấy thông tin hóa đơn
        /// </summary>
        /// <param name="DanhBo"></param>
        /// <param name="checksum"></param>
        /// <returns></returns>
        //[Route("getHoaDon")]
        //public IList<HoaDonThuTien> getHoaDon(string DanhBo, string checksum)
        //{
        //    if (CConstantVariable.getSHA256(DanhBo + _pass) != checksum)
        //    {
        //        ErrorResponse error = new ErrorResponse(ErrorResponse.ErrorPassword, ErrorResponse.ErrorCodePassword);
        //        throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.OK, error));
        //    }
        //    DataTable dt = new DataTable();
        //    try
        //    {
        //        string sql = "select * from fnTimKiem('" + DanhBo + "','') order by MaHD desc";
        //        dt = _cDAL_ThuTien.ExecuteQuery_DataTable(sql);
        //        //
        //        if (dt != null && dt.Rows.Count > 0)
        //        {
        //            List<HoaDonThuTien> lst = new List<HoaDonThuTien>();
        //            foreach (DataRow item in dt.Rows)
        //            {
        //                HoaDonThuTien en = new HoaDonThuTien();
        //                en.GiaBieu = item["GiaBieu"].ToString();
        //                en.DinhMuc = item["DinhMuc"].ToString();
        //                en.SoHoaDon = item["SoHoaDon"].ToString();
        //                en.Ky = item["Ky"].ToString();
        //                en.TieuThu = item["TieuThu"].ToString();
        //                en.GiaBan = item["GiaBan"].ToString();
        //                en.ThueGTGT = item["ThueGTGT"].ToString();
        //                en.PhiBVMT = item["PhiBVMT"].ToString();
        //                en.TongCong = item["TongCong"].ToString();
        //                if (item["NgayGiaiTrach"].ToString() != "")
        //                    en.NgayGiaiTrach = DateTime.Parse(item["NgayGiaiTrach"].ToString());
        //                en.DangNgan = item["DangNgan"].ToString();
        //                en.HanhThu = item["HanhThu"].ToString();
        //                en.MaDN = item["MaDN"].ToString();
        //                if (item["NgayDN"].ToString() != "")
        //                    en.NgayDN = DateTime.Parse(item["NgayDN"].ToString());
        //                if (item["NgayMN"].ToString() != "")
        //                    en.NgayMN = DateTime.Parse(item["NgayMN"].ToString());
        //                lst.Add(en);
        //            }
        //            return lst;
        //        }
        //        else
        //            return null;
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorResponse error = new ErrorResponse(ex.Message, ErrorResponse.ErrorCodeSQL);
        //        throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.OK, error));
        //    }
        //}

        /// <summary>
        /// Lấy thông tin hóa đơn
        /// </summary>
        /// <param name="DanhBo"></param>
        /// <param name="checksum"></param>
        /// <returns></returns>
        [Route("getThuTien")]
        public ThuTien getThuTien(string DanhBo, string checksum)
        {
            try
            {
                if (CConstantVariable.getSHA256(DanhBo + _pass) != checksum)
                {
                    ErrorResponse error = new ErrorResponse(ErrorResponse.ErrorPassword, ErrorResponse.ErrorCodePassword);
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.OK, error));
                }
                ThuTien en = new ThuTien();
                DataTable dt = new DataTable();
                int TongNo = 0;

                string sql = "select * from fnTimKiem('" + DanhBo + "','') order by MaHD desc";
                dt = _cDAL_ThuTien.ExecuteQuery_DataTable(sql);
                //
                if (dt != null && dt.Rows.Count > 0)
                {
                    en.NhanVien = "Nhân viên thu tiền: " + dt.Rows[0]["NhanVien"].ToString();
                    //string sql_Lich = "select top 1 NoiDung=N'Kỳ '+CONVERT(varchar(2),a.Ky)+'/'+CONVERT(varchar(4),a.Nam)+N' dự kiến sẽ được thu tiền từ ngày '+CONVERT(varchar(10),b.NgayThuTien_From,103)+N' đến ngày '+CONVERT(varchar(10),b.NgayThuTien_To,103) from Lich_ThuTien a,Lich_ThuTien_ChiTiet b,Lich_Dot c where a.ID=b.IDThuTien and c.ID=b.IDDot"
                    //                                   + " and((c.TB1_From <= " + dt.Rows[0]["MLT"].ToString() + " and c.TB1_To >= " + dt.Rows[0]["MLT"].ToString() + ")"
                    //                                   + " or (c.TB2_From <= " + dt.Rows[0]["MLT"].ToString() + " and c.TB2_To >= " + dt.Rows[0]["MLT"].ToString() + ")"
                    //                                   + " or (c.TP1_From <= " + dt.Rows[0]["MLT"].ToString() + " and c.TP1_To >= " + dt.Rows[0]["MLT"].ToString() + ")"
                    //                                   + " or (c.TP2_From <= " + dt.Rows[0]["MLT"].ToString() + " and c.TP2_To >= " + dt.Rows[0]["MLT"].ToString() + "))"
                    //                                   + " order by a.CreateDate desc";
                    en.NhanVien += " ; " + getLichThuTien_Func_String(DanhBo, dt.Rows[0]["MLT"].ToString());

                    foreach (DataRow item in dt.Rows)
                    {
                        HoaDonThuTien enCT = new HoaDonThuTien();
                        enCT.GiaBieu = item["GiaBieu"].ToString();
                        enCT.DinhMuc = item["DinhMuc"].ToString();
                        enCT.DinhMucHN = item["DinhMucHN"].ToString();
                        enCT.SoHoaDon = item["SoHoaDon"].ToString();
                        enCT.Ky = item["Ky"].ToString();
                        enCT.TieuThu = item["TieuThu"].ToString();
                        enCT.GiaBan = item["GiaBan"].ToString();
                        enCT.ThueGTGT = item["ThueGTGT"].ToString();
                        enCT.PhiBVMT = item["PhiBVMT"].ToString();
                        enCT.TongCong = item["TongCong"].ToString();
                        if (item["NgayGiaiTrach"].ToString() != "")
                            if (item["DangNgan"].ToString() != "CNKĐ")
                                enCT.NgayGiaiTrach = DateTime.Parse(item["NgayGiaiTrach"].ToString());
                            else
                                TongNo += int.Parse(item["TongCong"].ToString());
                        else
                            TongNo += int.Parse(item["TongCong"].ToString());
                        enCT.DangNgan = item["DangNgan"].ToString();
                        enCT.HanhThu = item["HanhThu"].ToString();
                        enCT.MaDN = item["MaDN"].ToString();
                        if (item["NgayDN"].ToString() != "")
                            enCT.NgayDN = DateTime.Parse(item["NgayDN"].ToString());
                        if (item["NgayMN"].ToString() != "")
                            enCT.NgayMN = DateTime.Parse(item["NgayMN"].ToString());
                        enCT.DongNuoc2 = bool.Parse(item["DongNuoc2"].ToString());
                        enCT.LenhHuy = bool.Parse(item["LenhHuy"].ToString());
                        enCT.ToTrinh = bool.Parse(item["ToTrinh"].ToString());
                        en.lstHoaDon.Add(enCT);
                    }
                }
                en.ThongTin = "Hết nợ";

                if (TongNo > 0)
                    en.ThongTin = "Hiện còn nợ: " + String.Format(CultureInfo.CreateSpecificCulture("vi-VN"), "{0:#,##}", TongNo);

                int PhiMoNuoc = (int)_cDAL_ThuTien.ExecuteQuery_ReturnOneValue("select PhiMoNuoc=dbo.fnGetPhiMoNuoc(" + DanhBo + ")");
                if (PhiMoNuoc > 0)
                    en.ThongTin += " ; Phí mở nước: " + String.Format(CultureInfo.CreateSpecificCulture("vi-VN"), "{0:#,##}", PhiMoNuoc);

                dt = _cDAL_ThuTien.ExecuteQuery_DataTable("select * from fnThuHoChuaDangNgan(" + DanhBo + ")");
                if (dt != null && dt.Rows.Count > 0)
                {
                    en.ThongTin += " ; Đã Thu Hộ " + String.Format(CultureInfo.CreateSpecificCulture("vi-VN"), "{0:#,##}", int.Parse(dt.Rows[0]["TongCong"].ToString())) + " - Kỳ " + dt.Rows[0]["Kys"].ToString() + " - ngày " + dt.Rows[0]["CreateDate"].ToString() + " - qua " + dt.Rows[0]["TenDichVu"].ToString();
                }
                return en;
            }
            catch (Exception ex)
            {
                ErrorResponse error = new ErrorResponse(ex.Message, ErrorResponse.ErrorCodeSQL);
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.OK, error));
            }
        }

        /// <summary>
        /// Lấy lịch thu tiền
        /// </summary>
        /// <param name="DanhBo"></param>
        /// <param name="checksum"></param>
        /// <returns></returns>
        [Route("getLichThuTien")]
        public ThongTinKhachHang getLichThuTien(string DanhBo, string checksum)
        {
            try
            {
                if (CConstantVariable.getSHA256(DanhBo + _pass) != checksum)
                {
                    ErrorResponse error = new ErrorResponse(ErrorResponse.ErrorPassword, ErrorResponse.ErrorCodePassword);
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.OK, error));
                }
                DataTable dt_ThongTin = _cDAL_ThuTien.ExecuteQuery_DataTable("select top 1 DanhBo=DANHBA,HoTen=TENKH,DiaChi=(SO+' '+DUONG),GiaBieu=GB,DinhMuc=DM,MLT=MALOTRINH from HOADON where DANHBA='" + DanhBo + "' order by ID_HOADON desc");
                if (dt_ThongTin != null && dt_ThongTin.Rows.Count > 0)
                {
                    ThongTinKhachHang en = new ThongTinKhachHang();
                    en.DanhBo = dt_ThongTin.Rows[0]["DanhBo"].ToString();
                    en.HoTen = dt_ThongTin.Rows[0]["HoTen"].ToString();
                    en.DiaChi = dt_ThongTin.Rows[0]["DiaChi"].ToString();
                    en.DinhMuc = dt_ThongTin.Rows[0]["DinhMuc"].ToString();
                    en.GiaBieu = dt_ThongTin.Rows[0]["GiaBieu"].ToString();

                    //string sql_Lich = "select top 1 NoiDung=N'Kỳ '+CONVERT(varchar(2),a.Ky)+'/'+CONVERT(varchar(4),a.Nam)+N' dự kiến sẽ được thu tiền từ ngày '+CONVERT(varchar(10),b.NgayThuTien_From,103)+N' đến ngày '+CONVERT(varchar(10),b.NgayThuTien_To,103) from Lich_ThuTien a,Lich_ThuTien_ChiTiet b,Lich_Dot c where a.ID=b.IDThuTien and c.ID=b.IDDot"
                    //            + " and((c.TB1_From <= " + dt_ThongTin.Rows[0]["MLT"].ToString() + " and c.TB1_To >= " + dt_ThongTin.Rows[0]["MLT"].ToString() + ")"
                    //            + " or (c.TB2_From <= " + dt_ThongTin.Rows[0]["MLT"].ToString() + " and c.TB2_To >= " + dt_ThongTin.Rows[0]["MLT"].ToString() + ")"
                    //            + " or (c.TP1_From <= " + dt_ThongTin.Rows[0]["MLT"].ToString() + " and c.TP1_To >= " + dt_ThongTin.Rows[0]["MLT"].ToString() + ")"
                    //            + " or (c.TP2_From <= " + dt_ThongTin.Rows[0]["MLT"].ToString() + " and c.TP2_To >= " + dt_ThongTin.Rows[0]["MLT"].ToString() + "))"
                    //            + " order by a.CreateDate desc";
                    string result_Lich = getLichThuTien_Func_String(DanhBo, dt_ThongTin.Rows[0]["MLT"].ToString());

                    string result_NhanVien = _cDAL_ThuTien.ExecuteQuery_ReturnOneValue("select top 1 NhanVien=N'Nhân viên thu tiền: '+HoTen+' : '+DienThoai from HOADON a,TT_NguoiDung b where DANHBA='" + dt_ThongTin.Rows[0]["DanhBo"].ToString() + "' and a.MaNV_HanhThu=b.MaND order by ID_HOADON desc").ToString();

                    en.ThongTin = result_NhanVien + " ; " + result_Lich;
                    return en;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                ErrorResponse error = new ErrorResponse(ex.Message, ErrorResponse.ErrorCodeSQL);
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.OK, error));
            }
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public DataTable getLichThuTien_Func_SQL(string DanhBo, string MLT)
        {
            string sql_Lich = "WITH hoadon AS (select top 2 KY,NAM from [SERVER9].[HOADON_TA].[dbo].[HOADON] where DANHBA='" + DanhBo + "' order by ID_HOADON desc)"
                                   + " select distinct NoiDung=N'Kỳ '+CONVERT(varchar(2),a.Ky)+'/'+CONVERT(varchar(4),a.Nam)+N' dự kiến sẽ được thu tiền từ ngày '+CONVERT(varchar(10),b.NgayThuTien_From,103)+N' đến ngày '+CONVERT(varchar(10),b.NgayThuTien_To,103)"
                                   + " ,NgayThuTien_From,NgayThuTien_To"
                                   + " from Lich_ThuTien a,Lich_ThuTien_ChiTiet b,Lich_Dot c,hoadon where a.ID=b.IDThuTien and c.ID=b.IDDot and ((a.Nam>hoadon.Nam) or (a.Nam=hoadon.Nam and a.Ky>=hoadon.Ky))"
                                   + " and((c.TB1_From <= " + MLT + " and c.TB1_To >= " + MLT + ")"
                                   + " or (c.TB2_From <= " + MLT + " and c.TB2_To >= " + MLT + ")"
                                   + " or (c.TP1_From <= " + MLT + " and c.TP1_To >= " + MLT + ")"
                                   + " or (c.TP2_From <= " + MLT + " and c.TP2_To >= " + MLT + "))";
            //+ " order by a.CreateDate desc";
            return _cDAL_TrungTam.ExecuteQuery_DataTable(sql_Lich);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public string getLichThuTien_Func_String(string DanhBo, string MLT)
        {
            DataTable dt = getLichThuTien_Func_SQL(DanhBo, MLT);
            string result = "";
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow item in dt.Rows)
                    if (DateTime.Parse(item["NgayThuTien_To"].ToString()).Date >= DateTime.Now.Date)
                    {
                        result = item["NoiDung"].ToString();
                        break;
                    }
                if (result == "")
                    result = dt.Rows[dt.Rows.Count - 1]["NoiDung"].ToString();
            }
            return result;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public DataRow getLichThuTien_Func_DataRow(string DanhBo, string MLT)
        {
            DataTable dt = getLichThuTien_Func_SQL(DanhBo, MLT);
            DataRow result = null;
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow item in dt.Rows)
                    if (DateTime.Parse(item["NgayThuTien_To"].ToString()).Date >= DateTime.Now.Date)
                    {
                        result = item;
                        break;
                    }
                if (result == null)
                    result = dt.Rows[dt.Rows.Count - 1];
            }
            return result;
        }

        //sẽ update getdontumoi

        /// <summary>
        /// Lấy thông tin đơn phòng kinh doanh
        /// </summary>
        /// <param name="DanhBo"></param>
        /// <param name="checksum"></param>
        /// <returns></returns>
        [Route("getDonKinhDoanh")]
        public DonKinhDoanh getDonKinhDoanh(string DanhBo, string checksum)
        {
            if (CConstantVariable.getSHA256(DanhBo + _pass) != checksum)
            {
                ErrorResponse error = new ErrorResponse(ErrorResponse.ErrorPassword, ErrorResponse.ErrorCodePassword);
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.OK, error));
            }
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            try
            {
                dt = _cDAL_KinhDoanh.ExecuteQuery_DataTable("exec spTimKiemByBanhBo_DonTu '" + DanhBo + "'");
                ds = _cDAL_KinhDoanh.ExecuteQuery_DataSet("exec spTimKiemByBanhBo_DonTuChiTiet '" + DanhBo + "'");
                //
                if (dt != null && dt.Rows.Count > 0)
                {
                    DonKinhDoanh enKD = new DonKinhDoanh();
                    enKD.ThongTin = _cDAL_KinhDoanh.ExecuteQuery_ReturnOneValue("select dbo.fnCheckTinhTrangCanKhachHangLienHe('" + DanhBo + "')").ToString();

                    foreach (DataRow item in dt.Rows)
                        if (enKD.lstDonTu.Any(itemA => itemA.MaDon == item["MaDon"].ToString()) == false)
                        {
                            DonTu en = new DonTu();
                            en.MaDon = item["MaDon"].ToString();
                            en.TenLD = item["TenLD"].ToString();
                            if (item["CreateDate"].ToString() != "")
                                en.CreateDate = DateTime.Parse(item["CreateDate"].ToString());
                            en.DanhBo = item["DanhBo"].ToString();
                            en.HoTen = item["HoTen"].ToString();
                            en.DiaChi = item["DiaChi"].ToString();
                            en.GiaBieu = item["GiaBieu"].ToString();
                            en.DinhMuc = item["DinhMuc"].ToString();
                            en.DinhMucHN = item["DinhMucHN"].ToString();
                            en.NoiDung = item["NoiDung"].ToString();

                            //thêm chi tiết
                            for (int i = 0; i < ds.Tables.Count; i++)
                                if (ds.Tables[i].Rows.Count > 0)
                                {
                                    switch (ds.Tables[i].Rows[0][0].ToString())
                                    {
                                        case "KTXM":
                                            if (ds.Tables[i].Select("MaDon = '" + en.MaDon + "'").Count() > 0)
                                            {
                                                DataRow[] dr = ds.Tables[i].Select("MaDon = '" + en.MaDon + "'");

                                                for (int j = 0; j < dr.Count(); j++)
                                                {
                                                    KTXM enCT = new KTXM();
                                                    enCT.MaDon = dr[j]["MaDon"].ToString();
                                                    enCT.TabSTT = dr[j]["TabSTT"].ToString();
                                                    enCT.TabName = dr[j]["TabName"].ToString();
                                                    if (dr[j]["NgayKTXM"].ToString() != "")
                                                        enCT.NgayKTXM = DateTime.Parse(dr[j]["NgayKTXM"].ToString());
                                                    enCT.DanhBo = dr[j]["DanhBo"].ToString();
                                                    enCT.HoTen = dr[j]["HoTen"].ToString();
                                                    enCT.DiaChi = dr[j]["DiaChi"].ToString();
                                                    enCT.NoiDungKiemTra = dr[j]["NoiDungKiemTra"].ToString();
                                                    enCT.NoiDungDongTien = dr[j]["NoiDungDongTien"].ToString();
                                                    if (dr[j]["NgayDongTien"].ToString() != "")
                                                        enCT.NgayDongTien = DateTime.Parse(dr[j]["NgayDongTien"].ToString());
                                                    enCT.SoTienDongTien = dr[j]["SoTienDongTien"].ToString();
                                                    enCT.CreateBy = dr[j]["CreateBy"].ToString();

                                                    en.lstKTXM.Add(enCT);
                                                }
                                            }
                                            break;
                                        case "BamChi":
                                            if (ds.Tables[i].Select("MaDon = '" + en.MaDon + "'").Count() > 0)
                                            {
                                                DataRow[] dr = ds.Tables[i].Select("MaDon = '" + en.MaDon + "'");

                                                for (int j = 0; j < dr.Count(); j++)
                                                {
                                                    BamChi enCT = new BamChi();
                                                    enCT.MaDon = dr[j]["MaDon"].ToString();
                                                    enCT.TabSTT = dr[j]["TabSTT"].ToString();
                                                    enCT.TabName = dr[j]["TabName"].ToString();
                                                    if (dr[j]["NgayBC"].ToString() != "")
                                                        enCT.NgayBC = DateTime.Parse(dr[j]["NgayBC"].ToString());
                                                    enCT.DanhBo = dr[j]["DanhBo"].ToString();
                                                    enCT.HoTen = dr[j]["HoTen"].ToString();
                                                    enCT.DiaChi = dr[j]["DiaChi"].ToString();
                                                    enCT.TrangThaiBC = dr[j]["TrangThaiBC"].ToString();
                                                    enCT.TheoYeuCau = dr[j]["TheoYeuCau"].ToString();
                                                    enCT.CreateBy = dr[j]["CreateBy"].ToString();

                                                    en.lstBamChi.Add(enCT);
                                                }
                                            }
                                            break;
                                        case "DongNuoc":
                                            if (ds.Tables[i].Select("MaDon = '" + en.MaDon + "'").Count() > 0)
                                            {
                                                DataRow[] dr = ds.Tables[i].Select("MaDon = '" + en.MaDon + "'");

                                                for (int j = 0; j < dr.Count(); j++)
                                                {
                                                    DongNuoc enCT = new DongNuoc();
                                                    enCT.MaDon = dr[j]["MaDon"].ToString();
                                                    enCT.TabSTT = dr[j]["TabSTT"].ToString();
                                                    enCT.TabName = dr[j]["TabName"].ToString();
                                                    if (dr[j]["NgayDN"].ToString() != "")
                                                        enCT.NgayDN = DateTime.Parse(dr[j]["NgayDN"].ToString());
                                                    if (dr[j]["NgayMN"].ToString() != "")
                                                        enCT.NgayMN = DateTime.Parse(dr[j]["NgayMN"].ToString());
                                                    enCT.DanhBo = dr[j]["DanhBo"].ToString();
                                                    enCT.HoTen = dr[j]["HoTen"].ToString();
                                                    enCT.DiaChi = dr[j]["DiaChi"].ToString();
                                                    enCT.CreateBy = dr[j]["CreateBy"].ToString();

                                                    en.lstDongNuoc.Add(enCT);
                                                }
                                            }
                                            break;
                                        case "DCBD":
                                            if (ds.Tables[i].Select("MaDon = '" + en.MaDon + "'").Count() > 0)
                                            {
                                                DataRow[] dr = ds.Tables[i].Select("MaDon = '" + en.MaDon + "'");

                                                for (int j = 0; j < dr.Count(); j++)
                                                {
                                                    DCBD enCT = new DCBD();
                                                    enCT.MaDon = dr[j]["MaDon"].ToString();
                                                    enCT.TabSTT = dr[j]["TabSTT"].ToString();
                                                    enCT.TabName = dr[j]["TabName"].ToString();
                                                    if (dr[j]["CreateDate"].ToString() != "")
                                                        enCT.CreateDate = DateTime.Parse(dr[j]["CreateDate"].ToString());
                                                    enCT.DanhBo = dr[j]["DanhBo"].ToString();
                                                    enCT.HoTen = dr[j]["HoTen"].ToString();
                                                    enCT.DiaChi = dr[j]["DiaChi"].ToString();
                                                    enCT.GiaBieu = dr[j]["GiaBieu"].ToString();
                                                    enCT.GiaBieu_BD = dr[j]["GiaBieu_BD"].ToString();
                                                    enCT.DinhMuc = dr[j]["DinhMuc"].ToString();
                                                    enCT.DinhMuc_BD = dr[j]["DinhMuc_BD"].ToString();
                                                    enCT.DinhMucHN = dr[j]["DinhMucHN"].ToString();
                                                    enCT.DinhMucHN_BD = dr[j]["DinhMucHN_BD"].ToString();
                                                    enCT.HoTen_BD = dr[j]["HoTen_BD"].ToString();
                                                    enCT.DiaChi_BD = dr[j]["DiaChi_BD"].ToString();
                                                    enCT.ThongTin = dr[j]["ThongTin"].ToString() + "; Hiệu lực: " + dr[j]["HieuLucKy"].ToString();
                                                    enCT.CreateBy = dr[j]["CreateBy"].ToString();

                                                    en.lstDCBD.Add(enCT);
                                                }
                                            }
                                            break;
                                        case "DCHD":
                                            if (ds.Tables[i].Select("MaDon = '" + en.MaDon + "'").Count() > 0)
                                            {
                                                DataRow[] dr = ds.Tables[i].Select("MaDon = '" + en.MaDon + "'");

                                                for (int j = 0; j < dr.Count(); j++)
                                                {
                                                    DCHD enCT = new DCHD();
                                                    enCT.MaDon = dr[j]["MaDon"].ToString();
                                                    enCT.TabSTT = dr[j]["TabSTT"].ToString();
                                                    enCT.TabName = dr[j]["TabName"].ToString();
                                                    if (dr[j]["CreateDate"].ToString() != "")
                                                        enCT.CreateDate = DateTime.Parse(dr[j]["CreateDate"].ToString());
                                                    enCT.DanhBo = dr[j]["DanhBo"].ToString();
                                                    enCT.HoTen = dr[j]["HoTen"].ToString();
                                                    enCT.DiaChi = dr[j]["DiaChi"].ToString();
                                                    enCT.GiaBieu = dr[j]["GiaBieu"].ToString();
                                                    enCT.GiaBieu_BD = dr[j]["GiaBieu_BD"].ToString();
                                                    enCT.DinhMuc = dr[j]["DinhMuc"].ToString();
                                                    enCT.DinhMuc_BD = dr[j]["DinhMuc_BD"].ToString();
                                                    enCT.DinhMucHN = dr[j]["DinhMucHN"].ToString();
                                                    enCT.DinhMucHN_BD = dr[j]["DinhMucHN_BD"].ToString();
                                                    enCT.TieuThu = dr[j]["TieuThu"].ToString();
                                                    enCT.TieuThu_BD = dr[j]["TieuThu_BD"].ToString();
                                                    enCT.TongCong_Start = dr[j]["TongCong_Start"].ToString();
                                                    enCT.TongCong_End = dr[j]["TongCong_End"].ToString();
                                                    enCT.TongCong_BD = dr[j]["TongCong_BD"].ToString();
                                                    enCT.TangGiam = dr[j]["TangGiam"].ToString();
                                                    enCT.CreateBy = dr[j]["CreateBy"].ToString();

                                                    en.lstDCHD.Add(enCT);
                                                }
                                            }
                                            break;
                                        case "CHDB":
                                            if (ds.Tables[i].Select("MaDon = '" + en.MaDon + "'").Count() > 0)
                                            {
                                                DataRow[] dr = ds.Tables[i].Select("MaDon = '" + en.MaDon + "'");

                                                for (int j = 0; j < dr.Count(); j++)
                                                {
                                                    CHDB enCT = new CHDB();
                                                    enCT.MaDon = dr[j]["MaDon"].ToString();
                                                    enCT.TabSTT = dr[j]["TabSTT"].ToString();
                                                    enCT.TabName = dr[j]["TabName"].ToString();
                                                    if (dr[j]["CreateDate"].ToString() != "")
                                                        enCT.CreateDate = DateTime.Parse(dr[j]["CreateDate"].ToString());
                                                    enCT.DanhBo = dr[j]["DanhBo"].ToString();
                                                    enCT.HoTen = dr[j]["HoTen"].ToString();
                                                    enCT.DiaChi = dr[j]["DiaChi"].ToString();
                                                    enCT.LoaiCat = dr[j]["LoaiCat"].ToString();
                                                    enCT.LyDo = dr[j]["LyDo"].ToString();
                                                    enCT.GhiChuLyDo = dr[j]["GhiChuLyDo"].ToString();
                                                    enCT.DaLapPhieu = bool.Parse(dr[j]["DaLapPhieu"].ToString());
                                                    enCT.SoPhieu = dr[j]["SoPhieu"].ToString();
                                                    if (dr[j]["NgayLapPhieu"].ToString() != "")
                                                        enCT.NgayLapPhieu = DateTime.Parse(dr[j]["NgayLapPhieu"].ToString());
                                                    enCT.CreateBy = dr[j]["CreateBy"].ToString();

                                                    en.lstCHDB.Add(enCT);
                                                }
                                            }
                                            break;
                                        case "PhieuCHDB":
                                            if (ds.Tables[i].Select("MaDon = '" + en.MaDon + "'").Count() > 0)
                                            {
                                                DataRow[] dr = ds.Tables[i].Select("MaDon = '" + en.MaDon + "'");

                                                for (int j = 0; j < dr.Count(); j++)
                                                {
                                                    PhieuCHDB enCT = new PhieuCHDB();
                                                    enCT.MaDon = dr[j]["MaDon"].ToString();
                                                    enCT.TabSTT = dr[j]["TabSTT"].ToString();
                                                    enCT.TabName = dr[j]["TabName"].ToString();
                                                    if (dr[j]["CreateDate"].ToString() != "")
                                                        enCT.CreateDate = DateTime.Parse(dr[j]["CreateDate"].ToString());
                                                    enCT.DanhBo = dr[j]["DanhBo"].ToString();
                                                    enCT.HoTen = dr[j]["HoTen"].ToString();
                                                    enCT.DiaChi = dr[j]["DiaChi"].ToString();
                                                    enCT.LyDo = dr[j]["LyDo"].ToString();
                                                    enCT.GhiChuLyDo = dr[j]["GhiChuLyDo"].ToString();
                                                    enCT.HieuLucKy = dr[j]["HieuLucKy"].ToString();
                                                    enCT.CreateBy = dr[j]["CreateBy"].ToString();

                                                    en.lstPhieuCHDB.Add(enCT);
                                                }
                                            }
                                            break;
                                        case "TTTL":
                                            if (ds.Tables[i].Select("MaDon = '" + en.MaDon + "'").Count() > 0)
                                            {
                                                DataRow[] dr = ds.Tables[i].Select("MaDon = '" + en.MaDon + "'");

                                                for (int j = 0; j < dr.Count(); j++)
                                                {
                                                    TTL enCT = new TTL();
                                                    enCT.MaDon = dr[j]["MaDon"].ToString();
                                                    enCT.TabSTT = dr[j]["TabSTT"].ToString();
                                                    enCT.TabName = dr[j]["TabName"].ToString();
                                                    if (dr[j]["CreateDate"].ToString() != "")
                                                        enCT.CreateDate = DateTime.Parse(dr[j]["CreateDate"].ToString());
                                                    enCT.DanhBo = dr[j]["DanhBo"].ToString();
                                                    enCT.HoTen = dr[j]["HoTen"].ToString();
                                                    enCT.DiaChi = dr[j]["DiaChi"].ToString();
                                                    enCT.VeViec = dr[j]["VeViec"].ToString();
                                                    enCT.NoiDung = dr[j]["NoiDung"].ToString();
                                                    enCT.NoiNhan = dr[j]["NoiNhan"].ToString();
                                                    enCT.CreateBy = dr[j]["CreateBy"].ToString();

                                                    en.lstTTL.Add(enCT);
                                                }
                                            }
                                            break;
                                        case "GianLan":
                                            if (ds.Tables[i].Select("MaDon = '" + en.MaDon + "'").Count() > 0)
                                            {
                                                DataRow[] dr = ds.Tables[i].Select("MaDon = '" + en.MaDon + "'");

                                                for (int j = 0; j < dr.Count(); j++)
                                                {
                                                    GianLan enCT = new GianLan();
                                                    enCT.MaDon = dr[j]["MaDon"].ToString();
                                                    enCT.TabSTT = dr[j]["TabSTT"].ToString();
                                                    enCT.TabName = dr[j]["TabName"].ToString();
                                                    if (dr[j]["CreateDate"].ToString() != "")
                                                        enCT.CreateDate = DateTime.Parse(dr[j]["CreateDate"].ToString());
                                                    enCT.DanhBo = dr[j]["DanhBo"].ToString();
                                                    enCT.HoTen = dr[j]["HoTen"].ToString();
                                                    enCT.DiaChi = dr[j]["DiaChi"].ToString();
                                                    enCT.NoiDungViPham = dr[j]["NoiDungViPham"].ToString();
                                                    enCT.TinhTrang = dr[j]["TinhTrang"].ToString();
                                                    enCT.ThanhToan1 = bool.Parse(dr[j]["ThanhToan1"].ToString());
                                                    enCT.ThanhToan2 = bool.Parse(dr[j]["ThanhToan2"].ToString());
                                                    enCT.ThanhToan3 = bool.Parse(dr[j]["ThanhToan3"].ToString());
                                                    enCT.XepDon = bool.Parse(dr[j]["XepDon"].ToString());
                                                    enCT.CreateBy = dr[j]["CreateBy"].ToString();

                                                    en.lstGianLan.Add(enCT);
                                                }
                                            }
                                            break;
                                        case "TruyThu":
                                            if (ds.Tables[i].Select("MaDon = '" + en.MaDon + "'").Count() > 0)
                                            {
                                                DataRow[] dr = ds.Tables[i].Select("MaDon = '" + en.MaDon + "'");

                                                for (int j = 0; j < dr.Count(); j++)
                                                {
                                                    TruyThu enCT = new TruyThu();
                                                    enCT.MaDon = dr[j]["MaDon"].ToString();
                                                    enCT.TabSTT = dr[j]["TabSTT"].ToString();
                                                    enCT.TabName = dr[j]["TabName"].ToString();
                                                    if (dr[j]["CreateDate"].ToString() != "")
                                                        enCT.CreateDate = DateTime.Parse(dr[j]["CreateDate"].ToString());
                                                    enCT.DanhBo = dr[j]["DanhBo"].ToString();
                                                    enCT.HoTen = dr[j]["HoTen"].ToString();
                                                    enCT.DiaChi = dr[j]["DiaChi"].ToString();
                                                    enCT.NoiDung = dr[j]["NoiDung"].ToString();
                                                    enCT.TongTien = dr[j]["TongTien"].ToString();
                                                    enCT.Tongm3BinhQuan = dr[j]["Tongm3BinhQuan"].ToString();
                                                    enCT.TinhTrang = dr[j]["TinhTrang"].ToString();
                                                    enCT.CreateBy = dr[j]["CreateBy"].ToString();

                                                    en.lstTruyThu.Add(enCT);
                                                }
                                            }
                                            break;
                                        case "ToTrinh":
                                            if (ds.Tables[i].Select("MaDon = '" + en.MaDon + "'").Count() > 0)
                                            {
                                                DataRow[] dr = ds.Tables[i].Select("MaDon = '" + en.MaDon + "'");

                                                for (int j = 0; j < dr.Count(); j++)
                                                {
                                                    ToTrinh enCT = new ToTrinh();
                                                    enCT.MaDon = dr[j]["MaDon"].ToString();
                                                    enCT.TabSTT = dr[j]["TabSTT"].ToString();
                                                    enCT.TabName = dr[j]["TabName"].ToString();
                                                    if (dr[j]["CreateDate"].ToString() != "")
                                                        enCT.CreateDate = DateTime.Parse(dr[j]["CreateDate"].ToString());
                                                    enCT.DanhBo = dr[j]["DanhBo"].ToString();
                                                    enCT.HoTen = dr[j]["HoTen"].ToString();
                                                    enCT.DiaChi = dr[j]["DiaChi"].ToString();
                                                    enCT.VeViec = dr[j]["VeViec"].ToString();
                                                    enCT.NoiDung = dr[j]["NoiDung"].ToString();
                                                    enCT.CreateBy = dr[j]["CreateBy"].ToString();

                                                    en.lstToTrinh.Add(enCT);
                                                }
                                            }
                                            break;
                                        case "ThuMoi":
                                            if (ds.Tables[i].Select("MaDon = '" + en.MaDon + "'").Count() > 0)
                                            {
                                                DataRow[] dr = ds.Tables[i].Select("MaDon = '" + en.MaDon + "'");

                                                for (int j = 0; j < dr.Count(); j++)
                                                {
                                                    ThuMoi enCT = new ThuMoi();
                                                    enCT.MaDon = dr[j]["MaDon"].ToString();
                                                    enCT.TabSTT = dr[j]["TabSTT"].ToString();
                                                    enCT.TabName = dr[j]["TabName"].ToString();
                                                    if (dr[j]["CreateDate"].ToString() != "")
                                                        enCT.CreateDate = DateTime.Parse(dr[j]["CreateDate"].ToString());
                                                    enCT.DanhBo = dr[j]["DanhBo"].ToString();
                                                    enCT.HoTen = dr[j]["HoTen"].ToString();
                                                    enCT.DiaChi = dr[j]["DiaChi"].ToString();
                                                    enCT.Lan = dr[j]["Lan"].ToString();
                                                    enCT.VeViec = dr[j]["VeViec"].ToString();
                                                    enCT.CreateBy = dr[j]["CreateBy"].ToString();

                                                    en.lstThuMoi.Add(enCT);
                                                }
                                            }
                                            break;
                                        case "TienTrinh":
                                            if (ds.Tables[i].Select("MaDon = '" + en.MaDon + "'").Count() > 0)
                                            {
                                                DataRow[] dr = ds.Tables[i].Select("MaDon = '" + en.MaDon + "'");

                                                for (int j = 0; j < dr.Count(); j++)
                                                {
                                                    TienTrinh enCT = new TienTrinh();
                                                    enCT.MaDon = dr[j]["MaDon"].ToString();
                                                    enCT.TabSTT = dr[j]["TabSTT"].ToString();
                                                    enCT.TabName = dr[j]["TabName"].ToString();
                                                    if (dr[j]["NgayChuyen"].ToString() != "")
                                                        enCT.NgayChuyen = DateTime.Parse(dr[j]["NgayChuyen"].ToString());
                                                    enCT.NoiChuyen = dr[j]["NoiChuyen"].ToString();
                                                    enCT.NoiNhan = dr[j]["NoiNhan"].ToString();
                                                    enCT.KTXM = dr[j]["KTXM"].ToString();
                                                    enCT.NoiDung = dr[j]["NoiDung"].ToString();

                                                    en.lstTienTrinh.Add(enCT);
                                                }
                                            }
                                            break;
                                    }
                                }

                            enKD.lstDonTu.Add(en);
                        }

                    return enKD;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                ErrorResponse error = new ErrorResponse(ex.Message, ErrorResponse.ErrorCodeSQL);
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.OK, error));
            }
        }

        //hàm getDonKhachHang cũ

        ///// <summary>
        ///// Lấy thông tin đơn phòng kinh doanh
        ///// </summary>
        ///// <param name="DanhBo"></param>
        ///// <param name="checksum"></param>
        ///// <returns></returns>
        //[Route("getDonKinhDoanh")]
        //public IList<DonTu> getDonKinhDoanh(string DanhBo, string checksum)
        //{
        //    try
        //    {
        //        if (CConstantVariable.getSHA256(DanhBo + _pass) != checksum)
        //        {
        //            ErrorResponse error = new ErrorResponse(ErrorResponse.ErrorPassword, ErrorResponse.ErrorCodePassword);
        //            throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.OK, error));
        //        }
        //        DataTable dt = new DataTable();
        //        DataSet ds = new DataSet();

        //        dt = _cDAL_KinhDoanh.ExecuteQuery_DataTable("exec spTimKiemByBanhBo_DonTu '" + DanhBo + "'");
        //        ds = _cDAL_KinhDoanh.ExecuteQuery_DataSet("exec spTimKiemByBanhBo_DonTuChiTiet '" + DanhBo + "'");
        //        //
        //        if (dt != null && dt.Rows.Count > 0)
        //        {
        //            List<DonTu> lst = new List<DonTu>();
        //            foreach (DataRow item in dt.Rows)
        //                if (lst.Any(itemA => itemA.MaDon == item["MaDon"].ToString()) == false)
        //                {
        //                    DonTu en = new DonTu();
        //                    en.MaDon = item["MaDon"].ToString();
        //                    en.TenLD = item["TenLD"].ToString();
        //                    if (item["CreateDate"].ToString() != "")
        //                        en.CreateDate = DateTime.Parse(item["CreateDate"].ToString());
        //                    en.DanhBo = item["DanhBo"].ToString();
        //                    en.HoTen = item["HoTen"].ToString();
        //                    en.DiaChi = item["DiaChi"].ToString();
        //                    en.GiaBieu = item["GiaBieu"].ToString();
        //                    en.DinhMuc = item["DinhMuc"].ToString();
        //                    en.NoiDung = item["NoiDung"].ToString();

        //                    //thêm chi tiết
        //                    for (int i = 0; i < ds.Tables.Count; i++)
        //                        if (ds.Tables[i].Rows.Count > 0)
        //                        {
        //                            switch (ds.Tables[i].Rows[0][0].ToString())
        //                            {
        //                                case "KTXM":
        //                                    if (ds.Tables[i].Select("MaDon = '" + en.MaDon + "'").Count() > 0)
        //                                    {
        //                                        DataRow[] dr = ds.Tables[i].Select("MaDon = '" + en.MaDon + "'");

        //                                        for (int j = 0; j < dr.Count(); j++)
        //                                        {
        //                                            KTXM enCT = new KTXM();
        //                                            enCT.MaDon = dr[j]["MaDon"].ToString();
        //                                            enCT.TabSTT = dr[j]["TabSTT"].ToString();
        //                                            enCT.TabName = dr[j]["TabName"].ToString();
        //                                            if (dr[j]["NgayKTXM"].ToString() != "")
        //                                                enCT.NgayKTXM = DateTime.Parse(dr[j]["NgayKTXM"].ToString());
        //                                            enCT.DanhBo = dr[j]["DanhBo"].ToString();
        //                                            enCT.HoTen = dr[j]["HoTen"].ToString();
        //                                            enCT.DiaChi = dr[j]["DiaChi"].ToString();
        //                                            enCT.NoiDungKiemTra = dr[j]["NoiDungKiemTra"].ToString();
        //                                            enCT.NoiDungDongTien = dr[j]["NoiDungDongTien"].ToString();
        //                                            if (dr[j]["NgayDongTien"].ToString() != "")
        //                                                enCT.NgayDongTien = DateTime.Parse(dr[j]["NgayDongTien"].ToString());
        //                                            enCT.SoTienDongTien = dr[j]["SoTienDongTien"].ToString();
        //                                            enCT.CreateBy = dr[j]["CreateBy"].ToString();

        //                                            en.lstKTXM.Add(enCT);
        //                                        }
        //                                    }
        //                                    break;
        //                                case "BamChi":
        //                                    if (ds.Tables[i].Select("MaDon = '" + en.MaDon + "'").Count() > 0)
        //                                    {
        //                                        DataRow[] dr = ds.Tables[i].Select("MaDon = '" + en.MaDon + "'");

        //                                        for (int j = 0; j < dr.Count(); j++)
        //                                        {
        //                                            BamChi enCT = new BamChi();
        //                                            enCT.MaDon = dr[j]["MaDon"].ToString();
        //                                            enCT.TabSTT = dr[j]["TabSTT"].ToString();
        //                                            enCT.TabName = dr[j]["TabName"].ToString();
        //                                            if (dr[j]["NgayBC"].ToString() != "")
        //                                                enCT.NgayBC = DateTime.Parse(dr[j]["NgayBC"].ToString());
        //                                            enCT.DanhBo = dr[j]["DanhBo"].ToString();
        //                                            enCT.HoTen = dr[j]["HoTen"].ToString();
        //                                            enCT.DiaChi = dr[j]["DiaChi"].ToString();
        //                                            enCT.TrangThaiBC = dr[j]["TrangThaiBC"].ToString();
        //                                            enCT.TheoYeuCau = dr[j]["TheoYeuCau"].ToString();
        //                                            enCT.CreateBy = dr[j]["CreateBy"].ToString();

        //                                            en.lstBamChi.Add(enCT);
        //                                        }
        //                                    }
        //                                    break;
        //                                case "DongNuoc":
        //                                    if (ds.Tables[i].Select("MaDon = '" + en.MaDon + "'").Count() > 0)
        //                                    {
        //                                        DataRow[] dr = ds.Tables[i].Select("MaDon = '" + en.MaDon + "'");

        //                                        for (int j = 0; j < dr.Count(); j++)
        //                                        {
        //                                            DongNuoc enCT = new DongNuoc();
        //                                            enCT.MaDon = dr[j]["MaDon"].ToString();
        //                                            enCT.TabSTT = dr[j]["TabSTT"].ToString();
        //                                            enCT.TabName = dr[j]["TabName"].ToString();
        //                                            if (dr[j]["NgayDN"].ToString() != "")
        //                                                enCT.NgayDN = DateTime.Parse(dr[j]["NgayDN"].ToString());
        //                                            if (dr[j]["NgayMN"].ToString() != "")
        //                                                enCT.NgayMN = DateTime.Parse(dr[j]["NgayMN"].ToString());
        //                                            enCT.DanhBo = dr[j]["DanhBo"].ToString();
        //                                            enCT.HoTen = dr[j]["HoTen"].ToString();
        //                                            enCT.DiaChi = dr[j]["DiaChi"].ToString();
        //                                            enCT.CreateBy = dr[j]["CreateBy"].ToString();

        //                                            en.lstDongNuoc.Add(enCT);
        //                                        }
        //                                    }
        //                                    break;
        //                                case "DCBD":
        //                                    if (ds.Tables[i].Select("MaDon = '" + en.MaDon + "'").Count() > 0)
        //                                    {
        //                                        DataRow[] dr = ds.Tables[i].Select("MaDon = '" + en.MaDon + "'");

        //                                        for (int j = 0; j < dr.Count(); j++)
        //                                        {
        //                                            DCBD enCT = new DCBD();
        //                                            enCT.MaDon = dr[j]["MaDon"].ToString();
        //                                            enCT.TabSTT = dr[j]["TabSTT"].ToString();
        //                                            enCT.TabName = dr[j]["TabName"].ToString();
        //                                            if (dr[j]["CreateDate"].ToString() != "")
        //                                                enCT.CreateDate = DateTime.Parse(dr[j]["CreateDate"].ToString());
        //                                            enCT.DanhBo = dr[j]["DanhBo"].ToString();
        //                                            enCT.HoTen = dr[j]["HoTen"].ToString();
        //                                            enCT.DiaChi = dr[j]["DiaChi"].ToString();
        //                                            enCT.GiaBieu = dr[j]["GiaBieu"].ToString();
        //                                            enCT.GiaBieu_BD = dr[j]["GiaBieu_BD"].ToString();
        //                                            enCT.DinhMuc = dr[j]["DinhMuc"].ToString();
        //                                            enCT.DinhMuc_BD = dr[j]["DinhMuc_BD"].ToString();
        //                                            enCT.HoTen_BD = dr[j]["HoTen_BD"].ToString();
        //                                            enCT.DiaChi_BD = dr[j]["DiaChi_BD"].ToString();
        //                                            enCT.ThongTin = dr[j]["ThongTin"].ToString() + "; Hiệu lực: " + dr[j]["HieuLucKy"].ToString();
        //                                            enCT.CreateBy = dr[j]["CreateBy"].ToString();

        //                                            en.lstDCBD.Add(enCT);
        //                                        }
        //                                    }
        //                                    break;
        //                                case "DCHD":
        //                                    if (ds.Tables[i].Select("MaDon = '" + en.MaDon + "'").Count() > 0)
        //                                    {
        //                                        DataRow[] dr = ds.Tables[i].Select("MaDon = '" + en.MaDon + "'");

        //                                        for (int j = 0; j < dr.Count(); j++)
        //                                        {
        //                                            DCHD enCT = new DCHD();
        //                                            enCT.MaDon = dr[j]["MaDon"].ToString();
        //                                            enCT.TabSTT = dr[j]["TabSTT"].ToString();
        //                                            enCT.TabName = dr[j]["TabName"].ToString();
        //                                            if (dr[j]["CreateDate"].ToString() != "")
        //                                                enCT.CreateDate = DateTime.Parse(dr[j]["CreateDate"].ToString());
        //                                            enCT.DanhBo = dr[j]["DanhBo"].ToString();
        //                                            enCT.HoTen = dr[j]["HoTen"].ToString();
        //                                            enCT.DiaChi = dr[j]["DiaChi"].ToString();
        //                                            enCT.GiaBieu = dr[j]["GiaBieu"].ToString();
        //                                            enCT.GiaBieu_BD = dr[j]["GiaBieu_BD"].ToString();
        //                                            enCT.DinhMuc = dr[j]["DinhMuc"].ToString();
        //                                            enCT.DinhMuc_BD = dr[j]["DinhMuc_BD"].ToString();
        //                                            enCT.TieuThu = dr[j]["TieuThu"].ToString();
        //                                            enCT.TieuThu_BD = dr[j]["TieuThu_BD"].ToString();
        //                                            enCT.TongCong_Start = dr[j]["TongCong_Start"].ToString();
        //                                            enCT.TongCong_End = dr[j]["TongCong_End"].ToString();
        //                                            enCT.TongCong_BD = dr[j]["TongCong_BD"].ToString();
        //                                            enCT.TangGiam = dr[j]["TangGiam"].ToString();
        //                                            enCT.CreateBy = dr[j]["CreateBy"].ToString();

        //                                            en.lstDCHD.Add(enCT);
        //                                        }
        //                                    }
        //                                    break;
        //                                case "CHDB":
        //                                    if (ds.Tables[i].Select("MaDon = '" + en.MaDon + "'").Count() > 0)
        //                                    {
        //                                        DataRow[] dr = ds.Tables[i].Select("MaDon = '" + en.MaDon + "'");

        //                                        for (int j = 0; j < dr.Count(); j++)
        //                                        {
        //                                            CHDB enCT = new CHDB();
        //                                            enCT.MaDon = dr[j]["MaDon"].ToString();
        //                                            enCT.TabSTT = dr[j]["TabSTT"].ToString();
        //                                            enCT.TabName = dr[j]["TabName"].ToString();
        //                                            if (dr[j]["CreateDate"].ToString() != "")
        //                                                enCT.CreateDate = DateTime.Parse(dr[j]["CreateDate"].ToString());
        //                                            enCT.DanhBo = dr[j]["DanhBo"].ToString();
        //                                            enCT.HoTen = dr[j]["HoTen"].ToString();
        //                                            enCT.DiaChi = dr[j]["DiaChi"].ToString();
        //                                            enCT.LoaiCat = dr[j]["LoaiCat"].ToString();
        //                                            enCT.LyDo = dr[j]["LyDo"].ToString();
        //                                            enCT.GhiChuLyDo = dr[j]["GhiChuLyDo"].ToString();
        //                                            enCT.DaLapPhieu = bool.Parse(dr[j]["DaLapPhieu"].ToString());
        //                                            enCT.SoPhieu = dr[j]["SoPhieu"].ToString();
        //                                            if (dr[j]["NgayLapPhieu"].ToString() != "")
        //                                                enCT.NgayLapPhieu = DateTime.Parse(dr[j]["NgayLapPhieu"].ToString());
        //                                            enCT.CreateBy = dr[j]["CreateBy"].ToString();

        //                                            en.lstCHDB.Add(enCT);
        //                                        }
        //                                    }
        //                                    break;
        //                                case "PhieuCHDB":
        //                                    if (ds.Tables[i].Select("MaDon = '" + en.MaDon + "'").Count() > 0)
        //                                    {
        //                                        DataRow[] dr = ds.Tables[i].Select("MaDon = '" + en.MaDon + "'");

        //                                        for (int j = 0; j < dr.Count(); j++)
        //                                        {
        //                                            PhieuCHDB enCT = new PhieuCHDB();
        //                                            enCT.MaDon = dr[j]["MaDon"].ToString();
        //                                            enCT.TabSTT = dr[j]["TabSTT"].ToString();
        //                                            enCT.TabName = dr[j]["TabName"].ToString();
        //                                            if (dr[j]["CreateDate"].ToString() != "")
        //                                                enCT.CreateDate = DateTime.Parse(dr[j]["CreateDate"].ToString());
        //                                            enCT.DanhBo = dr[j]["DanhBo"].ToString();
        //                                            enCT.HoTen = dr[j]["HoTen"].ToString();
        //                                            enCT.DiaChi = dr[j]["DiaChi"].ToString();
        //                                            enCT.LyDo = dr[j]["LyDo"].ToString();
        //                                            enCT.GhiChuLyDo = dr[j]["GhiChuLyDo"].ToString();
        //                                            enCT.HieuLucKy = dr[j]["HieuLucKy"].ToString();
        //                                            enCT.CreateBy = dr[j]["CreateBy"].ToString();

        //                                            en.lstPhieuCHDB.Add(enCT);
        //                                        }
        //                                    }
        //                                    break;
        //                                case "TTTL":
        //                                    if (ds.Tables[i].Select("MaDon = '" + en.MaDon + "'").Count() > 0)
        //                                    {
        //                                        DataRow[] dr = ds.Tables[i].Select("MaDon = '" + en.MaDon + "'");

        //                                        for (int j = 0; j < dr.Count(); j++)
        //                                        {
        //                                            TTL enCT = new TTL();
        //                                            enCT.MaDon = dr[j]["MaDon"].ToString();
        //                                            enCT.TabSTT = dr[j]["TabSTT"].ToString();
        //                                            enCT.TabName = dr[j]["TabName"].ToString();
        //                                            if (dr[j]["CreateDate"].ToString() != "")
        //                                                enCT.CreateDate = DateTime.Parse(dr[j]["CreateDate"].ToString());
        //                                            enCT.DanhBo = dr[j]["DanhBo"].ToString();
        //                                            enCT.HoTen = dr[j]["HoTen"].ToString();
        //                                            enCT.DiaChi = dr[j]["DiaChi"].ToString();
        //                                            enCT.VeViec = dr[j]["VeViec"].ToString();
        //                                            enCT.NoiDung = dr[j]["NoiDung"].ToString();
        //                                            enCT.NoiNhan = dr[j]["NoiNhan"].ToString();
        //                                            enCT.CreateBy = dr[j]["CreateBy"].ToString();

        //                                            en.lstTTL.Add(enCT);
        //                                        }
        //                                    }
        //                                    break;
        //                                case "GianLan":
        //                                    if (ds.Tables[i].Select("MaDon = '" + en.MaDon + "'").Count() > 0)
        //                                    {
        //                                        DataRow[] dr = ds.Tables[i].Select("MaDon = '" + en.MaDon + "'");

        //                                        for (int j = 0; j < dr.Count(); j++)
        //                                        {
        //                                            GianLan enCT = new GianLan();
        //                                            enCT.MaDon = dr[j]["MaDon"].ToString();
        //                                            enCT.TabSTT = dr[j]["TabSTT"].ToString();
        //                                            enCT.TabName = dr[j]["TabName"].ToString();
        //                                            if (dr[j]["CreateDate"].ToString() != "")
        //                                                enCT.CreateDate = DateTime.Parse(dr[j]["CreateDate"].ToString());
        //                                            enCT.DanhBo = dr[j]["DanhBo"].ToString();
        //                                            enCT.HoTen = dr[j]["HoTen"].ToString();
        //                                            enCT.DiaChi = dr[j]["DiaChi"].ToString();
        //                                            enCT.NoiDungViPham = dr[j]["NoiDungViPham"].ToString();
        //                                            enCT.TinhTrang = dr[j]["TinhTrang"].ToString();
        //                                            enCT.ThanhToan1 = bool.Parse(dr[j]["ThanhToan1"].ToString());
        //                                            enCT.ThanhToan2 = bool.Parse(dr[j]["ThanhToan2"].ToString());
        //                                            enCT.ThanhToan3 = bool.Parse(dr[j]["ThanhToan3"].ToString());
        //                                            enCT.XepDon = bool.Parse(dr[j]["XepDon"].ToString());
        //                                            enCT.CreateBy = dr[j]["CreateBy"].ToString();

        //                                            en.lstGianLan.Add(enCT);
        //                                        }
        //                                    }
        //                                    break;
        //                                case "TruyThu":
        //                                    if (ds.Tables[i].Select("MaDon = '" + en.MaDon + "'").Count() > 0)
        //                                    {
        //                                        DataRow[] dr = ds.Tables[i].Select("MaDon = '" + en.MaDon + "'");

        //                                        for (int j = 0; j < dr.Count(); j++)
        //                                        {
        //                                            TruyThu enCT = new TruyThu();
        //                                            enCT.MaDon = dr[j]["MaDon"].ToString();
        //                                            enCT.TabSTT = dr[j]["TabSTT"].ToString();
        //                                            enCT.TabName = dr[j]["TabName"].ToString();
        //                                            if (dr[j]["CreateDate"].ToString() != "")
        //                                                enCT.CreateDate = DateTime.Parse(dr[j]["CreateDate"].ToString());
        //                                            enCT.DanhBo = dr[j]["DanhBo"].ToString();
        //                                            enCT.HoTen = dr[j]["HoTen"].ToString();
        //                                            enCT.DiaChi = dr[j]["DiaChi"].ToString();
        //                                            enCT.NoiDung = dr[j]["NoiDung"].ToString();
        //                                            enCT.TongTien = dr[j]["TongTien"].ToString();
        //                                            enCT.Tongm3BinhQuan = dr[j]["Tongm3BinhQuan"].ToString();
        //                                            enCT.TinhTrang = dr[j]["TinhTrang"].ToString();
        //                                            enCT.CreateBy = dr[j]["CreateBy"].ToString();

        //                                            en.lstTruyThu.Add(enCT);
        //                                        }
        //                                    }
        //                                    break;
        //                                case "ToTrinh":
        //                                    if (ds.Tables[i].Select("MaDon = '" + en.MaDon + "'").Count() > 0)
        //                                    {
        //                                        DataRow[] dr = ds.Tables[i].Select("MaDon = '" + en.MaDon + "'");

        //                                        for (int j = 0; j < dr.Count(); j++)
        //                                        {
        //                                            ToTrinh enCT = new ToTrinh();
        //                                            enCT.MaDon = dr[j]["MaDon"].ToString();
        //                                            enCT.TabSTT = dr[j]["TabSTT"].ToString();
        //                                            enCT.TabName = dr[j]["TabName"].ToString();
        //                                            if (dr[j]["CreateDate"].ToString() != "")
        //                                                enCT.CreateDate = DateTime.Parse(dr[j]["CreateDate"].ToString());
        //                                            enCT.DanhBo = dr[j]["DanhBo"].ToString();
        //                                            enCT.HoTen = dr[j]["HoTen"].ToString();
        //                                            enCT.DiaChi = dr[j]["DiaChi"].ToString();
        //                                            enCT.VeViec = dr[j]["VeViec"].ToString();
        //                                            enCT.NoiDung = dr[j]["NoiDung"].ToString();
        //                                            enCT.CreateBy = dr[j]["CreateBy"].ToString();

        //                                            en.lstToTrinh.Add(enCT);
        //                                        }
        //                                    }
        //                                    break;
        //                                case "ThuMoi":
        //                                    if (ds.Tables[i].Select("MaDon = '" + en.MaDon + "'").Count() > 0)
        //                                    {
        //                                        DataRow[] dr = ds.Tables[i].Select("MaDon = '" + en.MaDon + "'");

        //                                        for (int j = 0; j < dr.Count(); j++)
        //                                        {
        //                                            ThuMoi enCT = new ThuMoi();
        //                                            enCT.MaDon = dr[j]["MaDon"].ToString();
        //                                            enCT.TabSTT = dr[j]["TabSTT"].ToString();
        //                                            enCT.TabName = dr[j]["TabName"].ToString();
        //                                            if (dr[j]["CreateDate"].ToString() != "")
        //                                                enCT.CreateDate = DateTime.Parse(dr[j]["CreateDate"].ToString());
        //                                            enCT.DanhBo = dr[j]["DanhBo"].ToString();
        //                                            enCT.HoTen = dr[j]["HoTen"].ToString();
        //                                            enCT.DiaChi = dr[j]["DiaChi"].ToString();
        //                                            enCT.Lan = dr[j]["Lan"].ToString();
        //                                            enCT.VeViec = dr[j]["VeViec"].ToString();
        //                                            enCT.CreateBy = dr[j]["CreateBy"].ToString();

        //                                            en.lstThuMoi.Add(enCT);
        //                                        }
        //                                    }
        //                                    break;
        //                                case "TienTrinh":
        //                                    if (ds.Tables[i].Select("MaDon = '" + en.MaDon + "'").Count() > 0)
        //                                    {
        //                                        DataRow[] dr = ds.Tables[i].Select("MaDon = '" + en.MaDon + "'");

        //                                        for (int j = 0; j < dr.Count(); j++)
        //                                        {
        //                                            TienTrinh enCT = new TienTrinh();
        //                                            enCT.MaDon = dr[j]["MaDon"].ToString();
        //                                            enCT.TabSTT = dr[j]["TabSTT"].ToString();
        //                                            enCT.TabName = dr[j]["TabName"].ToString();
        //                                            if (dr[j]["NgayChuyen"].ToString() != "")
        //                                                enCT.NgayChuyen = DateTime.Parse(dr[j]["NgayChuyen"].ToString());
        //                                            enCT.NoiChuyen = dr[j]["NoiChuyen"].ToString();
        //                                            enCT.NoiNhan = dr[j]["NoiNhan"].ToString();
        //                                            enCT.KTXM = dr[j]["KTXM"].ToString();
        //                                            enCT.NoiDung = dr[j]["NoiDung"].ToString();

        //                                            en.lstTienTrinh.Add(enCT);
        //                                        }
        //                                    }
        //                                    break;
        //                            }
        //                        }

        //                    lst.Add(en);
        //                }

        //            return lst;
        //        }
        //        else
        //            return null;
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorResponse error = new ErrorResponse(ex.Message, ErrorResponse.ErrorCodeSQL);
        //        throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.OK, error));
        //    }
        //}

        /// <summary>
        /// Lấy danh sách file đã scan
        /// </summary>
        /// <param name="DanhBo"></param>
        /// <param name="checksum"></param>
        /// <returns></returns>
        [Route("getHoSoScan")]
        public IList<HoSoScan> getHoSoScan(string DanhBo, string checksum)
        {
            try
            {
                //if (CConstantVariable.getSHA256(DanhBo + _pass) != checksum)
                //{
                //    ErrorResponse error = new ErrorResponse(ErrorResponse.ErrorPassword, ErrorResponse.ErrorCodePassword);
                //    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.OK, error));
                //}
                DataTable dt = _cDAL_KinhDoanh.ExecuteQuery_DataTable("select * from fnGetHoSoScan('" + DanhBo + "') order by CreateDate desc");
                if (dt != null && dt.Rows.Count > 0)
                {
                    List<HoSoScan> lst = new List<HoSoScan>();
                    foreach (DataRow item in dt.Rows)
                        if (lst.Any(itemA => itemA.ID == int.Parse(item["ID"].ToString())) == false)
                        {
                            HoSoScan en = new HoSoScan();
                            en.ID = int.Parse(item["ID"].ToString());
                            if (item["CreateDate"].ToString() != "")
                                en.CreateDate = DateTime.Parse(item["CreateDate"].ToString());
                            en.LoaiVanBan = item["LoaiVanBan"].ToString();
                            HoSoScan_File enF = new HoSoScan_File();
                            enF.File = (byte[])item["Hinh"];
                            en.lstFile.Add(enF);
                            lst.Add(en);
                        }
                        else
                        {
                            HoSoScan_File enF = new HoSoScan_File();
                            enF.File = (byte[])item["Hinh"];
                            lst.SingleOrDefault(itemA => itemA.ID == int.Parse(item["ID"].ToString())).lstFile.Add(enF);
                        }
                    return lst;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                ErrorResponse error = new ErrorResponse(ex.Message, ErrorResponse.ErrorCodeSQL);
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.OK, error));
            }
        }

        /// <summary>
        /// Tìm kiếm thông tin khách hàng bằng danh bộ
        /// </summary>
        /// <param name="DanhBo"></param>
        /// <param name="checksum"></param>
        /// <returns></returns>
        //[Route("searchThongTinKhachHang")]
        //[HttpGet]
        //public IList<ThongTinKhachHang> searchThongTinKhachHang(string DanhBo, string checksum)
        //{
        //    try
        //    {
        //        if (CConstantVariable.getSHA256(DanhBo + _pass) != checksum)
        //        {
        //            ErrorResponse error = new ErrorResponse(ErrorResponse.ErrorPassword, ErrorResponse.ErrorCodePassword);
        //            throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.OK, error));
        //        }
        //        DataTable dt = _cDAL_ThuTien.ExecuteQuery_DataTable("select * from fnTimKiem('" + DanhBo + "','') order by MaHD desc");
        //        if (dt != null && dt.Rows.Count > 0)
        //        {
        //            List<ThongTinKhachHang> lst = new List<ThongTinKhachHang>();
        //            foreach (DataRow item in dt.Rows)
        //            {
        //                ThongTinKhachHang en = new ThongTinKhachHang();
        //                en.DanhBo = dt.Rows[0]["DanhBo"].ToString();
        //                en.HoTen = dt.Rows[0]["HoTen"].ToString();
        //                en.DiaChi = dt.Rows[0]["DiaChi"].ToString();

        //                lst.Add(en);
        //            }

        //            return lst;
        //        }
        //        else
        //            return null;
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorResponse error = new ErrorResponse(ex.Message, ErrorResponse.ErrorCodeSQL);
        //        throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.OK, error));
        //    }
        //}

        /// <summary>
        /// Tìm kiếm thông tin khách hàng bằng họ tên, số nhà, tên đường
        /// </summary>
        /// <param name="HoTen"></param>
        /// <param name="SoNha"></param>
        /// <param name="TenDuong"></param>
        /// <param name="checksum"></param>
        /// <returns></returns>
        [Route("searchThongTinKhachHang")]
        [HttpGet]
        public IList<ThongTinKhachHang> searchThongTinKhachHang(string HoTen, string SoNha, string TenDuong, string checksum)
        {
            try
            {
                string checkHoTen = "", checkSoNha = "", checkTenDuong = "";

                if (HoTen != null)
                    checkHoTen = HoTen;
                if (SoNha != null)
                    checkSoNha = SoNha;
                if (TenDuong != null)
                    checkTenDuong = TenDuong;
                if (CConstantVariable.getSHA256(checkHoTen + checkSoNha + checkTenDuong + _pass) != checksum)
                {
                    ErrorResponse error = new ErrorResponse(ErrorResponse.ErrorPassword, ErrorResponse.ErrorCodePassword);
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.OK, error));
                }
                DataTable dt = new DataTable();

                List<ThongTinKhachHang> lst = new List<ThongTinKhachHang>();
                dt = _cDAL_ThuTien.ExecuteQuery_DataTable("select * from fnTimKiemTTKH('" + checkHoTen + "','" + checkSoNha + "','" + checkTenDuong + "')");
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        ThongTinKhachHang en = new ThongTinKhachHang();
                        en.DanhBo = item["DanhBo"].ToString();
                        en.HoTen = item["HoTen"].ToString();
                        if (item["DiaChi"].ToString() == item["DiaChiHoaDon"].ToString())
                            en.DiaChi = item["DiaChi"].ToString();
                        else
                            en.DiaChi = item["DiaChi"].ToString() + " =>HD: " + item["DiaChiHoaDon"].ToString();

                        lst.Add(en);
                    }
                }
                //kiếm danh bộ hủy
                //dt = _cDAL_DHN.ExecuteQuery_DataTable("select DANHBO,HOTEN,DiaChi=SONHA+' '+TENDUONG from TB_DULIEUKHACHHANG_HUYDB where HOTEN like N'%" + checkHoTen + "%' and ((SONHA like N'%" + checkSoNha + "%' and TENDUONG like N'%" + checkTenDuong + "%') or (SONHA+' '+TENDUONG like N'%" + checkSoNha + " " + checkTenDuong + "%'))");
                //if (dt != null && dt.Rows.Count > 0)
                //{
                //    foreach (DataRow item in dt.Rows)
                //    {
                //        ThongTinKhachHang en = new ThongTinKhachHang();
                //        en.DanhBo = item["DanhBo"].ToString();
                //        en.HoTen = item["HoTen"].ToString();
                //        en.DiaChi = item["DiaChi"].ToString();

                //        lst.Add(en);
                //    }
                //}
                if (lst.Count > 0)
                    return lst;
                else
                    return null;
            }
            catch (Exception ex)
            {
                ErrorResponse error = new ErrorResponse(ex.Message, ErrorResponse.ErrorCodeSQL);
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.OK, error));
            }
        }

        /// <summary>
        /// Tìm kiếm danh bộ bằng số điện thoại
        /// </summary>
        /// <param name="DienThoai"></param>
        /// <param name="checksum"></param>
        /// <returns></returns>
        [Route("searchDanhBoByDienThoai")]
        [HttpGet]
        public IList<ThongTinKhachHang> searchDanhBoByDienThoai(string DienThoai, string checksum)
        {
            try
            {
                if (CConstantVariable.getSHA256(DienThoai + _pass) != checksum)
                {
                    ErrorResponse error = new ErrorResponse(ErrorResponse.ErrorPassword, ErrorResponse.ErrorCodePassword);
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.OK, error));
                }
                DataTable dt = new DataTable();

                dt = _cDAL_KinhDoanh.ExecuteQuery_DataTable("select distinct DanhBo from DonDienThoai where DanhBo!='' and DienThoai like '%" + DienThoai + "%'");
                dt.Merge(_cDAL_DocSo.ExecuteQuery_DataTable("select DanhBo=DanhBa from KhachHang where SDT like '%" + DienThoai + "%'"));
                if (dt != null && dt.Rows.Count > 0)
                {
                    List<ThongTinKhachHang> lst = new List<ThongTinKhachHang>();
                    foreach (DataRow item in dt.Rows)
                    {
                        ThongTinKhachHang en = new ThongTinKhachHang();
                        en.DanhBo = item["DanhBo"].ToString();

                        lst.Add(en);
                    }

                    return lst;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                ErrorResponse error = new ErrorResponse(ex.Message, ErrorResponse.ErrorCodeSQL);
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.OK, error));
            }
        }

        /// <summary>
        /// Lấy thông tin tiến trình xử lý hồ sơ gắn mới
        /// </summary>
        /// <param name="SoHoSo"></param>
        /// <param name="checksum"></param>
        /// <returns></returns>
        [Route("getHoSoGanMoi")]
        public HoSoGanMoi getHoSoGanMoi(string SoHoSo, string checksum)
        {
            try
            {
                if (CConstantVariable.getSHA256(SoHoSo + _pass) != checksum)
                {
                    ErrorResponse error = new ErrorResponse(ErrorResponse.ErrorPassword, ErrorResponse.ErrorCodePassword);
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.OK, error));
                }
                HoSoGanMoi en = new HoSoGanMoi();
                DataTable dt = new DataTable();

                string sql = "SELECT  biennhan.SHS, biennhan.HOTEN,(SONHA + '  ' + DUONG + ',  P.' + p.TENPHUONG + ',  Q.' + q.TENQUAN) as 'DIACHI',"
                            + " biennhan.NGAYNHAN AS 'CreateDate',lhs.TENLOAI as 'LOAIHS'"
                            + " FROM QUAN q,PHUONG p, BIENNHANDON biennhan, LOAI_HOSO lhs"
                            + " WHERE biennhan.QUAN = q.MAQUAN AND q.MAQUAN = p.MAQUAN  AND biennhan.PHUONG = p.MAPHUONG AND lhs.MALOAI = biennhan.LOAIDON"
                            + " AND biennhan.SHS = '" + SoHoSo + "'";

                dt = _cDAL_GanMoi.ExecuteQuery_DataTable(sql);
                //
                if (dt != null && dt.Rows.Count > 0)
                {
                    en.SoHoSo = dt.Rows[0]["SHS"].ToString();
                    en.LoaiHoSo = dt.Rows[0]["LOAIHS"].ToString();
                    en.HoTen = dt.Rows[0]["HOTEN"].ToString();
                    en.DiaChi = dt.Rows[0]["DIACHI"].ToString();
                    if (dt.Rows[0]["CreateDate"].ToString() != "")
                        en.CreateDate = DateTime.Parse(dt.Rows[0]["CreateDate"].ToString());

                    string sqlCT = "select NgayChuyenThietKe=NGAYCHUYEN_HOSO,TRONGAITHIETKE,NOIDUNGTRONGAI,NGAYHOANTATTK=(select NGAYHOANTATTK from TOTHIETKE where SOHOSO=donkh.SOHOSO),"
                                  + " TaiXet=(select GHICHUTR from TMP_TAIXET where MAHOSO=donkh.SOHOSO),"
                                  + " NgayXinPhepDaoDuong = (select NGAYLAP from KH_XINPHEPDAODUONG where MADOT = (select MADOTDD from KH_HOSOKHACHHANG where SHS = donkh.SHS)),"
                                  + " NgayCoPhepDaoDuong = (select NGAYCOPHEP from KH_XINPHEPDAODUONG where MADOT = (select MADOTDD from KH_HOSOKHACHHANG where SHS = donkh.SHS)),"
                                  + " NgayThiCong = (select NGAYTHICONG from KH_HOSOKHACHHANG where SHS = donkh.SHS)"
                                  + " from DON_KHACHHANG donkh where SHS = '" + SoHoSo + "'";
                    DataTable dtCT = _cDAL_GanMoi.ExecuteQuery_DataTable(sqlCT);

                    GhiChu enCT = new GhiChu();
                    enCT.NoiDung = "Ngày Chuyển Thiết Kế";
                    if (dtCT.Rows[0]["NgayChuyenThietKe"].ToString() != "")
                        enCT.CreateDate = DateTime.Parse(dtCT.Rows[0]["NgayChuyenThietKe"].ToString());
                    en.lstGhiChu.Add(enCT);

                    if (dtCT.Rows[0]["TRONGAITHIETKE"].ToString() != "" && bool.Parse(dtCT.Rows[0]["TRONGAITHIETKE"].ToString()) == true)
                    {
                        enCT = new GhiChu();
                        enCT.NoiDung = "Hồ sơ trở ngại thiết kế; " + dtCT.Rows[0]["NOIDUNGTRONGAI"].ToString() + "; Tái xét: " + dtCT.Rows[0]["TaiXet"].ToString();
                        if (dtCT.Rows[0]["NGAYHOANTATTK"].ToString() != "")
                            enCT.CreateDate = DateTime.Parse(dtCT.Rows[0]["NGAYHOANTATTK"].ToString());
                        en.lstGhiChu.Add(enCT);
                    }

                    enCT = new GhiChu();
                    enCT.NoiDung = "Ngày Xin Phép Đào Đường";
                    if (dtCT.Rows[0]["NgayXinPhepDaoDuong"].ToString() != "")
                        enCT.CreateDate = DateTime.Parse(dtCT.Rows[0]["NgayXinPhepDaoDuong"].ToString());
                    en.lstGhiChu.Add(enCT);

                    enCT = new GhiChu();
                    enCT.NoiDung = "Ngày Có Phép Đào Đường";
                    if (dtCT.Rows[0]["NgayCoPhepDaoDuong"].ToString() != "")
                        enCT.CreateDate = DateTime.Parse(dtCT.Rows[0]["NgayCoPhepDaoDuong"].ToString());
                    en.lstGhiChu.Add(enCT);

                    enCT = new GhiChu();
                    enCT.NoiDung = "Ngày Thi Công";
                    if (dtCT.Rows[0]["NgayThiCong"].ToString() != "")
                        enCT.CreateDate = DateTime.Parse(dtCT.Rows[0]["NgayThiCong"].ToString());
                    en.lstGhiChu.Add(enCT);

                    //enCT.SoHoaDon = item["SoHoaDon"].ToString();
                    //enCT.Ky = item["Ky"].ToString();
                    //enCT.TieuThu = item["TieuThu"].ToString();
                    //enCT.GiaBan = item["GiaBan"].ToString();
                    //enCT.ThueGTGT = item["ThueGTGT"].ToString();
                    //enCT.PhiBVMT = item["PhiBVMT"].ToString();
                    //enCT.TongCong = item["TongCong"].ToString();
                    //if (item["NgayGiaiTrach"].ToString() != "")
                    //    enCT.NgayGiaiTrach = DateTime.Parse(item["NgayGiaiTrach"].ToString());
                    //enCT.DangNgan = item["DangNgan"].ToString();
                    //enCT.HanhThu = item["HanhThu"].ToString();
                    //enCT.MaDN = item["MaDN"].ToString();
                    //if (item["NgayDN"].ToString() != "")
                    //    enCT.NgayDN = DateTime.Parse(item["NgayDN"].ToString());
                    //if (item["NgayMN"].ToString() != "")
                    //    enCT.NgayMN = DateTime.Parse(item["NgayMN"].ToString());
                    //enCT.DongNuoc2 = bool.Parse(item["DongNuoc2"].ToString());
                    //enCT.LenhHuy = bool.Parse(item["LenhHuy"].ToString());
                    //enCT.ToTrinh = bool.Parse(item["ToTrinh"].ToString());
                    //en.lstHoaDon.Add(enCT);
                }
                return en;
            }
            catch (Exception ex)
            {
                ErrorResponse error = new ErrorResponse(ex.Message, ErrorResponse.ErrorCodeSQL);
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.OK, error));
            }
        }



    }

}