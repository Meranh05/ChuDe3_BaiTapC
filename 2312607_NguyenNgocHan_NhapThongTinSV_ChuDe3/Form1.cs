using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace _2312607_NguyenNgocHan_NhapThongTinSV_ChuDe3
{
	 public partial class Form1 : Form
	 {
		  QLSinhVien ql = new QLSinhVien();
		  public List<SinhVien> DanhSach { get; set; } = new List<SinhVien>();
		  string fileTxt = "student.txt";
		  string fileXml = "student.xml";
		  string fileJson = "student.json";

		  public Form1()
		  {
				InitializeComponent();
		  }



		  private void Form1_Load(object sender, EventArgs e)
		  {				
				// Hiển thị lên ListView
				ChuyenTextSangXmlJson();
				HienThiDanhSach();

		  }

		  private void ChuyenTextSangXmlJson()
		  {
				string txtPath = Path.Combine(Application.StartupPath, "D:\\DLU\\Hoc Ky 5\\Phat Trien Ung Dung desktop\\ChuDe3_BaiTapC\\2312607_NguyenNgocHan_NhapThongTinSV_ChuDe3\\bin\\Debug\\student.txt");
				string xmlPath = Path.Combine(Application.StartupPath, "D:\\DLU\\Hoc Ky 5\\Phat Trien Ung Dung desktop\\ChuDe3_BaiTapC\\2312607_NguyenNgocHan_NhapThongTinSV_ChuDe3\\student.xml");
				string jsonPath = Path.Combine(Application.StartupPath, "D:\\DLU\\Hoc Ky 5\\Phat Trien Ung Dung desktop\\ChuDe3_BaiTapC\\2312607_NguyenNgocHan_NhapThongTinSV_ChuDe3\\student.json");

				// Đọc từ file txt
				ql.LoadFromText(txtPath);

				// Xuất ra xml/json
				ql.SaveToXml(xmlPath);
				ql.SaveToJson(jsonPath);
		  }


		  private SinhVien LayThongTinTuForm()
		  {
				return new SinhVien
				{
					 MSSV = mtbMSSV.Text,
					 HoTenLot = txtHoTenLot.Text,
					 Ten = txtTen.Text,
					 NgaySinh = dtpNgaySinh.Value,
					 Lop = cboLop.Text,
					 GioiTinh = rdNam.Checked ? "Nam" : "Nữ",
					 CMND = mtbCMND.Text,
					 SoDT = mtkSoDT.Text,
					 DiaChi = txtDiaChi.Text,
					 MonHocDK = clbMonHocDK.CheckedItems.Cast<string>().ToList()
				};
		  }

		  private void HienThiDanhSach()
		  {
				lvSinhVien.Items.Clear();
				foreach (var sv in ql.DanhSach)
				{
					 ListViewItem item = new ListViewItem(sv.MSSV);
					 item.SubItems.Add(sv.HoTenLot);
					 item.SubItems.Add(sv.Ten);
					 item.SubItems.Add(sv.NgaySinh.ToShortDateString());
					 item.SubItems.Add(sv.Lop);
					 item.SubItems.Add(sv.CMND);
					 item.SubItems.Add(sv.SoDT);
					 item.SubItems.Add(sv.DiaChi);
					 lvSinhVien.Items.Add(item);
				}
		  }


		 
		  private void btnThemMoi_Click(object sender, EventArgs e)
		  {
				// Kiểm tra bắt buộc
				if (new[] { mtbMSSV.Text, txtHoTenLot.Text, txtTen.Text, mtbCMND.Text, mtkSoDT.Text }
					 .Any(x => string.IsNullOrWhiteSpace(x)))
				{
					 MessageBox.Show("Vui lòng nhập đầy đủ thông tin (MSSV, Họ lót, Tên, CMND, SĐT).",
										  "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					 return;
				}

				// Validate MSSV vs class
				string mssv = mtbMSSV.Text.Trim();
				string err;
				if (!ValidateMSSVWithClass(mssv, cboLop.Text, out err))
				{
					 MessageBox.Show(err, "Lỗi MSSV", MessageBoxButtons.OK, MessageBoxIcon.Error);
					 return;
				}

				// Kiểm tra trùng MSSV (với thêm mới: không được trùng)
				if (ql.DanhSach.Any(s => s.MSSV == mssv))
				{
					 MessageBox.Show($"MSSV {mssv} đã tồn tại trong hệ thống. Vui lòng nhập MSSV khác hoặc nhấn 'Tự tạo MSSV'.",
										  "Trùng MSSV", MessageBoxButtons.OK, MessageBoxIcon.Information);
					 return;
				}

				// Kiểm tra CMND và SĐT (độ dài)
				if (!Regex.IsMatch(mtbCMND.Text.Trim(), @"^\d{9}$"))
				{
					 MessageBox.Show("Số CMND phải đúng 9 chữ số.", "Lỗi CMND", MessageBoxButtons.OK, MessageBoxIcon.Error);
					 return;
				}
				if (!Regex.IsMatch(mtkSoDT.Text.Trim().Replace(".", "").Replace("-", ""), @"^\d{10}$"))
				{
					 MessageBox.Show("Số điện thoại phải đúng 10 chữ số.", "Lỗi SĐT", MessageBoxButtons.OK, MessageBoxIcon.Error);
					 return;
				}

				// Lấy SV từ form và thêm
				var sv = LayThongTinTuForm();
				ql.Add(sv);
				ql.SaveToText(fileTxt);
				HienThiDanhSach();

				MessageBox.Show($"Đã thêm {sv.HoTenLot} {sv.Ten} (MSSV: {sv.MSSV}) thành công.",
									 "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
		  }


		  private void ResetForm()
		  {
				mtbMSSV.Text = "";
				txtHoTenLot.Text = "";
				txtTen.Text = "";
				dtpNgaySinh.Value = DateTime.Now;
				cboLop.SelectedIndex = -1;
				mtbCMND.Text = "";
				mtkSoDT.Text = "";
				txtDiaChi.Text = "";

				// reset giới tính
				rdNam.Checked = true;

				// reset CheckedListBox môn học
				for (int i = 0; i < clbMonHocDK.Items.Count; i++)
				{
					 clbMonHocDK.SetItemChecked(i, false);
				}
				MessageBox.Show("Đã làm mới !");
				// hiển thị lại danh sách sinh viên ở data
				HienThiDanhSach();
		  }

		  private void btnLamMoi_Click(object sender, EventArgs e)
		  {
				ResetForm();
		  }

		  private void btnCapNhat_Click(object sender, EventArgs e)
		  {
				try
				{
					 // Lấy dữ liệu từ các ô nhập
					 var sv = LayThongTinTuForm();
					 ql.AddOrUpdate(sv);

					 // Kiểm tra MSSV phải nhập
					 if (string.IsNullOrWhiteSpace(sv.MSSV))
					 {
						  MessageBox.Show("Bạn chưa nhập MSSV");
						  return;
					 }

					 // Tìm trong danh sách theo MSSV
					 var existing = ql.DanhSach.FirstOrDefault(x => x.MSSV == sv.MSSV);
					 if (existing == null)
					 {
						  MessageBox.Show("Không tìm thấy sinh viên có MSSV " + sv.MSSV);
						  return;
					 }

					 // Cập nhật các trường
					 existing.HoTenLot = sv.HoTenLot;
					 existing.Ten = sv.Ten;
					 existing.NgaySinh = sv.NgaySinh;
					 existing.Lop = sv.Lop;
					 existing.GioiTinh = sv.GioiTinh;
					 existing.CMND = sv.CMND;
					 existing.SoDT = sv.SoDT;
					 existing.DiaChi = sv.DiaChi;
					 existing.MonHocDK = sv.MonHocDK;

					 // Lưu xuống file
					 ql.SaveToText(fileTxt);

					 // Hiển thị lại danh sách
					 HienThiDanhSach();

					 MessageBox.Show("Đã cập nhật sinh viên có MSSV: " + sv.MSSV);
				}
				catch (Exception ex)
				{
					 MessageBox.Show(ex.Message);
				}
		  }


		  private void btnTimKiem_Click(object sender, EventArgs e)
		  {
				string mssv = mtbMSSV.Text.Trim();
				string ten = txtTen.Text.Trim();
				string lop = cboLop.Text.Trim();

				var kq = ql.Search(mssv, ten, lop);
				lvSinhVien.Items.Clear();
				foreach (var sv in kq)
				{
					 ListViewItem item = new ListViewItem(sv.MSSV);
					 item.SubItems.Add(sv.HoTenLot);
					 item.SubItems.Add(sv.Ten);
					 item.SubItems.Add(sv.NgaySinh.ToShortDateString());
					 item.SubItems.Add(sv.Lop);
					 item.SubItems.Add(sv.CMND);
					 item.SubItems.Add(sv.SoDT);
					 item.SubItems.Add(sv.DiaChi);
					 lvSinhVien.Items.Add(item);
				}
		  }


		  private void btnExit_Click(object sender, EventArgs e)
		  {
				if (MessageBox.Show("Bạn có chắc chắn thoát?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
				{
					 this.Close();
				}
		  }

		  private void lvSinhVien_SelectedIndexChanged(object sender, EventArgs e)
		  {
				if (lvSinhVien.SelectedItems.Count > 0)
				{
					 var item = lvSinhVien.SelectedItems[0];
					 mtbMSSV.Text = item.SubItems[0].Text;
					 txtHoTenLot.Text = item.SubItems[1].Text;
					 txtTen.Text = item.SubItems[2].Text;
					 dtpNgaySinh.Value = DateTime.Parse(item.SubItems[3].Text);
					 cboLop.Text = item.SubItems[4].Text;
					 mtbCMND.Text = item.SubItems[5].Text;
					 mtkSoDT.Text = item.SubItems[6].Text;
					 txtDiaChi.Text = item.SubItems[7].Text;

					 //Reset CheckedListBox trước khi tick mới
					 for (int i = 0; i < clbMonHocDK.Items.Count; i++)
					 {
						  clbMonHocDK.SetItemChecked(i, false);
					 }

					 // Lấy SinhVien từ danh sách QL theo MSSV
					 var sv = ql.DanhSach.FirstOrDefault(s => s.MSSV == mtbMSSV.Text);
					 if (sv != null)
					 {
						  foreach (string mon in sv.MonHocDK)
						  {
								// tick những môn có trong MonHocDK
								int index = clbMonHocDK.Items.IndexOf(mon);
								if (index >= 0)
									 clbMonHocDK.SetItemChecked(index, true);
						  }
					 }
				}
		  }


		  private void btnXoa_Click(object sender, EventArgs e)
		  {
				// Lấy MSSV của các sinh viên được tick
				var mssvList = lvSinhVien.CheckedItems
					 .Cast<ListViewItem>()
					 .Select(item => item.SubItems[0].Text)
					 .ToList();

				if (mssvList.Count == 0)
				{
					 MessageBox.Show("Vui lòng chọn ít nhất một sinh viên để xóa (tick vào ô vuông).");
					 return;
				}

				// Hỏi lại người dùng
				if (MessageBox.Show("Bạn có chắc chắn muốn xóa " + mssvList.Count + " sinh viên đã chọn?",
					 "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
				{
					 // Xóa trong danh sách
					 ql.RemoveByMSSV(mssvList);

					 // Lưu xuống file
					 ql.SaveToText(fileTxt);

					 // Hiển thị lại danh sách
					 HienThiDanhSach();

					 MessageBox.Show("Đã xóa " + mssvList.Count + " sinh viên.");
				}
		  }

		  /*Hàm lấy AA từ lớp
		   DateTime.Now.Year trả về năm hiện tại (ví dụ 2025).
		  .ToString().Substring(2,2) lấy 2 ký tự từ vị trí thứ 2 (index = 2) → "25"
			Kết quả: AA = "25"*/
		  private string GetYearCodeFromClass(string lop)
		  {
				return DateTime.Now.Year.ToString().Substring(2, 2);
		  }

		  /*Lấy mssvPrefix “AABB” = năm hiện tại (AA) + 10 (BB cố định)
		  Kiểm tra:
		  MSSV có 7 chữ số không (^\d{7}$)
		  Có bắt đầu bằng “AABB” không.
		  Nếu sai -> gán errorMessage để hiện thông báo.*/
		  private bool ValidateMSSVWithClass(string mssv, string lop, out string errorMessage)
		  {
				errorMessage = null;

				var aa = GetYearCodeFromClass(lop);
				string prefix = aa + "10";

				if (string.IsNullOrWhiteSpace(mssv) || mssv.Length != 7 || !Regex.IsMatch(mssv, @"^\d{7}$"))
				{
					 errorMessage = "MSSV phải gồm đúng 7 chữ số (AABBCCC).";
					 return false;
				}

				if (!mssv.StartsWith(prefix))
				{
					 errorMessage = $"MSSV phải bắt đầu bằng '{prefix}' (AA = {aa}, BB = 10). Ví dụ: {prefix}001";
					 return false;
				}

				return true;
		  }

		  /*
		  Lấy mssvPrefix “AABB” = năm hiện tại + 10.
		  Tìm tất cả MSSV đã có cùng mssvPrefix.
		  Lấy CCC (3 số cuối) hiện có → tìm số lớn nhất -> cộng 1
		  Nếu 0-999 số là 2700 số -> báo lỗi
		  Ghép lại mssvPrefix + CCC (với D3 = đủ 3 chữ số, thêm 0 nếu cần).
		   */
		  private string GenerateNextMSSVForClass(string lop)
		  {
				var aa = GetYearCodeFromClass(lop);
				string mssvPrefix = aa + "10"; // AA10
													// Lấy các MSSV hiện có cùng mssvPrefix
				var listSameMSSVPrefix = ql.DanhSach
											.Where(s => s.MSSV != null && s.MSSV.StartsWith(mssvPrefix))
											.Select(s => {
												 int v;
												 if (int.TryParse(s.MSSV.Substring(4, 3), out v)) return v;
												 return -1;
											})
											.Where(x => x >= 0)
											.ToList();

				int max = listSameMSSVPrefix.Any() ? listSameMSSVPrefix.Max() : -1;
				int next = max + 1; // next CCC
				if (next > 999) throw new Exception("Đã hết mã CCC cho mssvPrefix " + mssvPrefix);
				return mssvPrefix + next.ToString("D3"); // đảm bảo 3 chữ số
		  }
		  // Tạo mã số sinh viên tự động dựa theo năm học hiện tại vd: 2025= 2510000 + 1
		  private void btnTuTaoMSSV_Click(object sender, EventArgs e) 
		  {
				try
				{
					 if (string.IsNullOrWhiteSpace(cboLop.Text))
					 {
						  MessageBox.Show("Vui lòng chọn lớp trước khi tự tạo MSSV.", "Thiếu lớp", MessageBoxButtons.OK, MessageBoxIcon.Warning);
						  return;
					 }
					 string newMSSV = GenerateNextMSSVForClass(cboLop.Text);
					 mtbMSSV.Text = newMSSV;
					 MessageBox.Show($"MSSV đã được tạo: {newMSSV}", "Đã tạo MSSV", MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
				catch (Exception ex)
				{
					 MessageBox.Show("Không tạo được MSSV: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
		  }

	 }
}
