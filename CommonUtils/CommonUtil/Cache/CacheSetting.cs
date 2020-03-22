namespace CommonUtil.Cache
{
    using System;

    public static class CacheSetting
    {
        #region Khac Hang

        public static class KhachHang
        {
            public const string Key = "KhachHang";
            public static readonly TimeSpan SlidingExpiration = TimeSpan.FromDays(30);
            public static readonly TimeSpan SlidingUpdate = TimeSpan.FromDays(5);
        }

        public static class HDKhachHang
        {
            public const string Key = "HDKhachHang";
            public static readonly TimeSpan SlidingExpiration = TimeSpan.FromDays(30);
            public static readonly TimeSpan SlidingUpdate = TimeSpan.FromDays(5);
        }
        #endregion

        #region System
        public static class DMPublic
        {
            public const string Key = "DMPublic";
            public static readonly TimeSpan SlidingExpiration = TimeSpan.FromDays(30);
            public static readonly TimeSpan SlidingUpdate = TimeSpan.FromDays(5);
        }

        public static class DMKhac
        {
            public const string Key = "DMKhac";
            public static readonly TimeSpan SlidingExpiration = TimeSpan.FromDays(30);
            public static readonly TimeSpan SlidingUpdate = TimeSpan.FromDays(30);
        }

        public static class SysConfig
        {
            public const string Key = "SysConfig";
            public static readonly TimeSpan SlidingExpiration = TimeSpan.FromDays(30);
            public static readonly TimeSpan SlidingUpdate = TimeSpan.FromDays(30);
        }
        public static class Menu
        {
            public const string Key = "Menu";
            public static readonly TimeSpan SlidingExpiration = TimeSpan.FromDays(90);
            public static readonly TimeSpan SlidingUpdate = TimeSpan.FromDays(30);
        }

        public static class TempCDG
        {
            public const string Key = "TempCDG";
            public static readonly TimeSpan SlidingExpiration = TimeSpan.FromDays(30);
            public static readonly TimeSpan SlidingUpdate = TimeSpan.FromDays(30);
        }

        public static class TempDatMua
        {
            public const string Key = "TempDatMua";
            public static readonly TimeSpan SlidingExpiration = TimeSpan.FromDays(30);
            public static readonly TimeSpan SlidingUpdate = TimeSpan.FromDays(30);
        }

        public static class TempKhachHang
        {
            public const string Key = "TempKhachHang";
            public static readonly TimeSpan SlidingExpiration = TimeSpan.FromDays(30);
            public static readonly TimeSpan SlidingUpdate = TimeSpan.FromDays(30);
        }

        public static class MailTripNubmer
        {
            public const string Key = "MailTripNubmer";
            public static readonly TimeSpan SlidingExpiration = TimeSpan.FromDays(30);
            public static readonly TimeSpan SlidingUpdate = TimeSpan.FromDays(30);
        }

        public static class NgayLamViec
        {
            public const string Key = "NgayLamViec";
            public static readonly TimeSpan SlidingExpiration = TimeSpan.FromDays(30);
            public static readonly TimeSpan SlidingUpdate = TimeSpan.FromDays(30);
        }

        public static class NgayNghi
        {
            public const string Key = "NgayNghi";
            public static readonly TimeSpan SlidingExpiration = TimeSpan.FromDays(30);
            public static readonly TimeSpan SlidingUpdate = TimeSpan.FromDays(30);
        }
        #endregion

        #region bao
        public static class ToaSoan
        {
            public const string Key = "ToaSoan";
            public static readonly TimeSpan SlidingExpiration = TimeSpan.FromDays(30);
            public static readonly TimeSpan SlidingUpdate = TimeSpan.FromDays(5);
        }

        public static class KHXB
        {
            public const string Key = "KHXB";
            public static readonly TimeSpan SlidingExpiration = TimeSpan.FromDays(30);
            public static readonly TimeSpan SlidingUpdate = TimeSpan.FromMinutes(30);
        }

        public static class TH_SoBao
        {
            public const string Key = "TH_SoBao";
            public static readonly TimeSpan SlidingExpiration = TimeSpan.FromDays(30);
            public static readonly TimeSpan SlidingUpdate = TimeSpan.FromMinutes(30);
        }

        public static class ThongTinBao
        {
            public const string Key = "ThongTinBao";
            public static readonly TimeSpan SlidingExpiration = TimeSpan.FromDays(30);
            public static readonly TimeSpan SlidingUpdate = TimeSpan.FromMinutes(5);
        }
        public static class BaoDiemTiepNhan
        {
            public const string Key = "BaoDiemTiepNhan";
            public static readonly TimeSpan SlidingExpiration = TimeSpan.FromDays(30);
            public static readonly TimeSpan SlidingUpdate = TimeSpan.FromDays(5);
        }
        #endregion

        #region Gia bao, cuoc phi, phan chia doanh thu
        public static class TyGia
        {
            public const string Key = "TyGia";
            public static readonly TimeSpan SlidingExpiration = TimeSpan.FromDays(30);
            public static readonly TimeSpan SlidingUpdate = TimeSpan.FromDays(5);
        }

        public static class DonViUyQuyen
        {
            public const string Key = "DonViUyQuyen";
            public static readonly TimeSpan SlidingExpiration = TimeSpan.FromDays(30);
            public static readonly TimeSpan SlidingUpdate = TimeSpan.FromDays(5);
        }

        public static class GiaBao
        {
            public const string Key = "GiaBao";
            public static readonly TimeSpan SlidingExpiration = TimeSpan.FromDays(30);
            public static readonly TimeSpan SlidingUpdate = TimeSpan.FromDays(5);
        }

        public static class GiaBaoTinh
        {
            public const string Key = "GiaBaoTinh";
            public static readonly TimeSpan SlidingExpiration = TimeSpan.FromDays(30);
            public static readonly TimeSpan SlidingUpdate = TimeSpan.FromDays(5);
        }

        public static class GiaBaoHuyen
        {
            public const string Key = "GiaBaoHuyen";
            public static readonly TimeSpan SlidingExpiration = TimeSpan.FromDays(30);
            public static readonly TimeSpan SlidingUpdate = TimeSpan.FromDays(5);
        }
        public static class CuocPhiPhatHanh
        {
            public const string Key = "CuocPhiPhatHanh";
            public static readonly TimeSpan SlidingExpiration = TimeSpan.FromDays(30);
            public static readonly TimeSpan SlidingUpdate = TimeSpan.FromDays(5);
        }

        public static class KhungTyLe
        {
            public const string Key = "KhungTyLe";
            public static readonly TimeSpan SlidingExpiration = TimeSpan.FromDays(30);
            public static readonly TimeSpan SlidingUpdate = TimeSpan.FromDays(5);
        }


        public static class TyLePCDT
        {
            public const string Key = "TyLePCDT";
            public static readonly TimeSpan SlidingExpiration = TimeSpan.FromDays(30);
            public static readonly TimeSpan SlidingUpdate = TimeSpan.FromDays(5);
        }
        #endregion

        #region Phan huong
        public static class UrlScriptProvince
        {
            public const string Key = "UrlScriptProvince";
            public static readonly TimeSpan SlidingExpiration = TimeSpan.FromDays(90);
            public static readonly TimeSpan SlidingUpdate = TimeSpan.FromDays(30);
        }

        public static class DeliveryRoute
        {
            public const string Key = "DeliveryRoute";
            public static readonly TimeSpan SlidingExpiration = TimeSpan.FromDays(90);
            public static readonly TimeSpan SlidingUpdate = TimeSpan.FromDays(30);
        }

        public static class DeliveryCommune
        {
            public const string Key = "DeliveryCommune";
            public static readonly TimeSpan SlidingExpiration = TimeSpan.FromDays(90);
            public static readonly TimeSpan SlidingUpdate = TimeSpan.FromDays(30);
        }

        public static class SysInfo
        {
            public const string Key = "SysInfo";
            public static readonly TimeSpan SlidingExpiration = TimeSpan.FromDays(30);
            public static readonly TimeSpan SlidingUpdate = TimeSpan.FromMinutes(5);
        }

        public static class DiemUyThac
        {
            public const string Key = "DiemUyThac";
            public static readonly TimeSpan SlidingExpiration = TimeSpan.FromDays(30);
            public static readonly TimeSpan SlidingUpdate = TimeSpan.FromDays(5);
        }

        public static class BaoKhuVuc
        {
            public const string Key = "BaoKhuVuc";
            public static readonly TimeSpan SlidingExpiration = TimeSpan.FromDays(30);
            public static readonly TimeSpan SlidingUpdate = TimeSpan.FromDays(5);
        }
        #endregion

        #region ke toan
        public static class TaiKhoan
        {
            public const string Key = "TaiKhoan";
            public static readonly TimeSpan SlidingExpiration = TimeSpan.FromDays(90);
            public static readonly TimeSpan SlidingUpdate = TimeSpan.FromDays(30);
        }
        public static class ConfigTaiKhoan
        {
            public const string Key = "ConfigTaiKhoan";
            public static readonly TimeSpan SlidingExpiration = TimeSpan.FromDays(90);
            public static readonly TimeSpan SlidingUpdate = TimeSpan.FromDays(30);
        }

        public static class ButToan
        {
            public const string Key = "ButToan";
            public static readonly TimeSpan SlidingExpiration = TimeSpan.FromDays(90);
            public static readonly TimeSpan SlidingUpdate = TimeSpan.FromDays(30);
        }

        public static class BieuThuc
        {
            public const string Key = "BieuThuc";
            public static readonly TimeSpan SlidingExpiration = TimeSpan.FromDays(30);
            public static readonly TimeSpan SlidingUpdate = TimeSpan.FromDays(5);
        }
        #endregion
    }
}