using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace _2312607_NguyenNgocHan_NhapThongTinSV_ChuDe3
{
	 [Serializable]
	 public class SinhVien
	 {
		  public string MSSV { get; set; }
		  public string HoTenLot { get; set; }
		  public string Ten { get; set; }
		  public DateTime NgaySinh { get; set; }
		  public string Lop { get; set; }
		  public string GioiTinh { get; set; }
		  public string CMND { get; set; }
		  public string SoDT { get; set; }
		  public string DiaChi { get; set; }
		  public List<string> MonHocDK { get; set; } = new List<string>();

		  public override string ToString()
		  {
				return $"{MSSV}|{HoTenLot}|{Ten}|{NgaySinh:dd/MM/yyyy}|{Lop}|{GioiTinh}|{CMND}|{SoDT}|{DiaChi}|{string.Join(",", MonHocDK)}";
		  }

		  public static SinhVien Parse(string line)
		  {
				var parts = line.Split('|');
				return new SinhVien
				{
					 MSSV = parts[0],
					 HoTenLot = parts[1],
					 Ten = parts[2],
					 NgaySinh = DateTime.ParseExact(parts[3], "dd/MM/yyyy", CultureInfo.InvariantCulture),
					 Lop = parts[4],
					 GioiTinh = parts[5],
					 CMND = parts[6],
					 SoDT = parts[7],
					 DiaChi = parts[8],
					 MonHocDK = parts.Length > 9 ? parts[9].Split(',').ToList() : new List<string>()
				};
		  }

	 }
}
