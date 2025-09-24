using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace _2312607_NguyenNgocHan_NhapThongTinSV_ChuDe3
{
	 public class QLSinhVien
	 {
		  public List<SinhVien> DanhSach { get; set; } = new List<SinhVien>();

		  public void LoadFromText(string path)
		  {
				DanhSach.Clear();
				if (!File.Exists(path)) return;
				foreach (var line in File.ReadAllLines(path))
				{
					 if (!string.IsNullOrWhiteSpace(line))
						  DanhSach.Add(SinhVien.Parse(line));
				}
		  }
		  // Ghi toàn bộ danh sách sinh viên ra file .txt
		  public void SaveToText(string path)
		  {
				File.WriteAllLines(path, DanhSach.Select(sv => sv.ToString()));
		  }

		  // Đọc danh sách sinh viên từ file XML (students.xml)
		  public void LoadFromXml(string path)
		  {
				if (!File.Exists(path)) return;
				var ser = new XmlSerializer(typeof(List<SinhVien>));
				using (var fs = new FileStream(path, FileMode.Open))
				{
					 DanhSach = (List<SinhVien>)ser.Deserialize(fs);
				}
		  }

		  // Ghi danh sách sinh viên ra file XML (students.xml)
		  public void SaveToXml(string path)
		  {
				var ser = new XmlSerializer(typeof(List<SinhVien>));
				using (var fs = new FileStream(path, FileMode.Create))
				{
					 ser.Serialize(fs, DanhSach);
				}
		  }

		  // Đọc danh sách sinh viên từ file JSON (students.json)
		  public void LoadFromJson(string path)
		  {
				if (!File.Exists(path)) return;
				var json = File.ReadAllText(path);
				DanhSach = JsonConvert.DeserializeObject<List<SinhVien>>(json);
		  }

		  // Ghi danh sách sinh viên ra file JSON (students.json)
		  public void SaveToJson(string path)
		  {
				var json = JsonConvert.SerializeObject(DanhSach, Formatting.Indented);
				File.WriteAllText(path, json);
		  }

		  public void Add(SinhVien sv)
		  {
				DanhSach.Add(sv);
		  }
		  // Hàm thêm hoặc cập nhật theo MSSV
		  public void AddOrUpdate(SinhVien sv)
		  {
				var existing = DanhSach.FirstOrDefault(x => x.MSSV == sv.MSSV);
				if (existing != null)
				{
					 // update
					 existing.HoTenLot = sv.HoTenLot;
					 existing.Ten = sv.Ten;
					 existing.NgaySinh = sv.NgaySinh;
					 existing.Lop = sv.Lop;
					 existing.GioiTinh = sv.GioiTinh;
					 existing.CMND = sv.CMND;
					 existing.SoDT = sv.SoDT;
					 existing.DiaChi = sv.DiaChi;
					 existing.MonHocDK = sv.MonHocDK;
				}
				else
				{
					 DanhSach.Add(sv);
				}
		  }


		  //Xoá 1 hoặc nhiều sinh viên
		  public void RemoveByMSSV(IEnumerable<string> mssvList)
		  {
				DanhSach.RemoveAll(x => mssvList.Contains(x.MSSV));
		  }

		  public List<SinhVien> Search(string mssv, string ten, string lop)
		  {
				return DanhSach.Where(s =>
					 (string.IsNullOrEmpty(mssv) || s.MSSV.Contains(mssv)) &&
					 (string.IsNullOrEmpty(ten) ||
						 s.Ten.IndexOf(ten, StringComparison.OrdinalIgnoreCase) >= 0 ||
						 s.HoTenLot.IndexOf(ten, StringComparison.OrdinalIgnoreCase) >= 0) &&
					 (string.IsNullOrEmpty(lop) || s.Lop.Equals(lop, StringComparison.OrdinalIgnoreCase))
				).ToList();
		  }

	 }
}
