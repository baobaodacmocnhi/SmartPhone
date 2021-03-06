﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WSTanHoa.Models
{
    public class HoaDon
    {
        public string HoTen { get; set; }
        public string DiaChi { get; set; }
        public int? MaHD { get; set; }
        public string SoHoaDon { get; set; }
        public string DanhBo { get; set; }
        public int? Nam { get; set; }
        public int? Ky { get; set; }
        public int GiaBan { get; set; }
        public int ThueGTGT { get; set; }
        public int PhiBVMT { get; set; }
        public int TongCong { get; set; }
        public int PhiMoNuoc { get; set; }
        public int TienDu { get; set; }
        //public DateTime? NgayGiaiTrach { get; set; }
        //public string KyHD { get; set; }

        public HoaDon()
        {
            HoTen = "";
            DiaChi = "";
            MaHD = null;
            SoHoaDon = "";
            DanhBo = "";
            Nam = null;
            Ky = null;
            GiaBan = 0;
            ThueGTGT = 0;
            PhiBVMT = 0;
            TongCong = 0;
            PhiMoNuoc = 0;
            TienDu = 0;
            //NgayGiaiTrach = null;
            //KyHD = "";
        }

    }
}