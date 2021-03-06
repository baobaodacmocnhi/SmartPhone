﻿namespace WSTanHoa.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Zalo")]
    public partial class Zalo
    {
        [Key]
        [Column(Order = 0, TypeName = "numeric")]
        [Display(Name = "ID Zalo")]
        public decimal IDZalo { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(13, MinimumLength = 11, ErrorMessage = "Danh bộ gồm 11 ký tự")]
        [Required(ErrorMessage = "Vui lòng nhập danh bộ")]
        [Display(Name = "Danh bộ")]
        public string DanhBo { get; set; }

        [StringLength(500)]
        [Display(Name = "Họ tên")]
        public string HoTen { get; set; }

        [StringLength(500)]
        [Display(Name = "Địa chỉ")]
        public string DiaChi { get; set; }

        [StringLength(10)]
        //[Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        [Display(Name = "Điện Thoại")]
        public string DienThoai { get; set; }

        [Display(Name = "Ngày tạo")]
        public DateTime? CreateDate { get; set; }
    }

    public class ViewZalo
    {
        public IEnumerable<Zalo> vlstZalo { get; set; }
        public Zalo vZalo { get; set; }

        public ViewZalo()
        {
            vlstZalo = null;
            vZalo = null;
        }

        public ViewZalo(IEnumerable<Zalo> _vlstZalo, Zalo _vZalo)
        {
            vlstZalo = _vlstZalo;
            vZalo = _vZalo;
        }
    }
}
