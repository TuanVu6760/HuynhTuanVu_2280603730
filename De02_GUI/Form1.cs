using De02_DAL.Entities;
using System;
using System.Linq;
using System.Windows.Forms;

namespace De02_GUI
{
    public partial class frmSanPham : Form
    {
        private SanPhamService sanPhamService = new SanPhamService();

        public frmSanPham()
        {
            InitializeComponent();
        }

        private void frmSanPham_Load(object sender, EventArgs e)
        {
            LoadSanpham();
            LoadLoaiSP();
            SetButtonState(true); // Mặc định trạng thái ban đầu
        }

        // Hàm tải dữ liệu sản phẩm lên DataGridView
        private void LoadSanpham()
        {
            dgvSanPham.Rows.Clear();
            var sanPhams = sanPhamService.GetAllSanPhams();

            foreach (var sp in sanPhams)
            {
                int rowIndex = dgvSanPham.Rows.Add();
                DataGridViewRow row = dgvSanPham.Rows[rowIndex];

                row.Cells["colMaSP"].Value = sp.MaSP;
                row.Cells["colTenSP"].Value = sp.TenSP;
                row.Cells["colNgayNhap"].Value = sp.Ngaynhap.ToString(); // Định dạng ngày tháng
                row.Cells["colLoaiSP"].Value = sp.LoaiSP.TenLoai;
            }
        }

        // Hàm tải dữ liệu loại sản phẩm lên combobox
        private void LoadLoaiSP()
        {
            var loaiSPs = sanPhamService.GetAllLoaiSPs();
            cboLoaiSP.DataSource = loaiSPs;
            cboLoaiSP.DisplayMember = "TenLoai";
            cboLoaiSP.ValueMember = "MaLoai";
        }

        // Xóa các trường nhập liệu
        private void ClearControls()
        {
            txtMaSP.Clear();
            txtTenSP.Clear();
            dtNgayNhap.Value = DateTime.Now;
            cboLoaiSP.SelectedIndex = -1;
        }

        // Thiết lập trạng thái nút (Thêm, Sửa, Xóa, Lưu, Không Lưu)
        private void SetButtonState(bool isAdding)
        {
            btnThem.Enabled = isAdding;       // Khi thêm thì bật nút Thêm
            btnXoa.Enabled = isAdding;       // Khi đang thêm thì tắt nút Xóa
            btnSua.Enabled = isAdding;       // Khi đang thêm thì tắt nút Sửa
            btnLuu.Enabled = !isAdding;       // Khi đang thêm/sửa thì bật nút Lưu
            btnKhongluu.Enabled = !isAdding;  // Khi đang thêm/sửa thì bật nút Không Lưu
        }

        // Sự kiện khi nhấn nút Thêm
        private void btnThem_Click(object sender, EventArgs e)
        {
            ClearControls();
            SetButtonState(false); // Bật các nút Lưu và Không Lưu khi thêm
        }

        // Sự kiện khi nhấn nút Sửa
        private void btnSua_Click(object sender, EventArgs e)
        {
            var sanPham = new Sanpham
            {
                MaSP = txtMaSP.Text,
                TenSP = txtTenSP.Text,
                Ngaynhap = dtNgayNhap.Value,
                MaLoai = cboLoaiSP.SelectedValue.ToString()
            };
            sanPhamService.UpdateSanPham(sanPham); // Cập nhật sản phẩm
            LoadSanpham(); // Tải lại danh sách sản phẩm
            SetButtonState(false); // Bật các nút Lưu và Không Lưu sau khi sửa
        }

        // Sự kiện khi nhấn nút Xóa
        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc chắn muốn xóa?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                sanPhamService.DeleteSanPham(txtMaSP.Text); // Xóa sản phẩm
                LoadSanpham(); // Tải lại danh sách sản phẩm
                ClearControls(); // Xóa các trường nhập liệu
                SetButtonState(false); // Bật các nút Lưu và Không Lưu sau khi xóa
            }
        }

        // Sự kiện khi nhấn nút Lưu
        // Sự kiện khi nhấn nút Lưu
        // Sự kiện khi nhấn nút Lưu
        private void btnLuu_Click(object sender, EventArgs e)
        {
            // Kiểm tra xem các trường cần thiết có được điền đầy đủ không
            if (string.IsNullOrEmpty(txtMaSP.Text) || string.IsNullOrEmpty(txtTenSP.Text) || cboLoaiSP.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin sản phẩm.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // Dừng lại nếu thông tin không đầy đủ
            }

            var sanPham = new Sanpham
            {
                MaSP = txtMaSP.Text,
                TenSP = txtTenSP.Text,
                Ngaynhap = dtNgayNhap.Value,
                MaLoai = cboLoaiSP.SelectedValue.ToString()
            };

            // Kiểm tra xem sản phẩm có tồn tại không để xác định hành động
            var existingSanPham = sanPhamService.GetById(sanPham.MaSP);

            if (existingSanPham != null)
            {
                // Cập nhật sản phẩm nếu đã tồn tại
                sanPhamService.UpdateSanPham(sanPham);
                MessageBox.Show("Cập nhật sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                // Thêm sản phẩm nếu chưa tồn tại
                sanPhamService.AddSanPham(sanPham);
                MessageBox.Show("Thêm sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            // Tải lại danh sách sản phẩm
            LoadSanpham();

            // Giữ lại thông tin đã nhập để tiếp tục thêm sản phẩm mới
            // Không gọi ClearControls() ở đây

            // Thiết lập trạng thái nút
            SetButtonState(false); // Bật các nút Lưu và Không Lưu để tiếp tục nhập thêm sản phẩm
        }



        // Sự kiện khi nhấn nút Không Lưu
        private void btnKhongluu_Click(object sender, EventArgs e)
        {
            SetButtonState(true); // Trở về trạng thái ban đầu
            ClearControls(); // Xóa các trường nhập liệu
        }

        // Sự kiện khi nhấn nút Tìm kiếm
        private void btnTim_Click(object sender, EventArgs e)
        {
            dgvSanPham.Rows.Clear(); // Xóa các dòng cũ trong DataGridView
            var searchTerm = txtTim.Text.ToLower();

            // Lọc sản phẩm theo tên
            var sanPhams = sanPhamService.GetAllSanPhams()
                            .Where(sp => sp.TenSP.ToLower().Contains(searchTerm))
                            .ToList();

            // Thêm các kết quả tìm được vào DataGridView
            foreach (var sp in sanPhams)
            {
                int rowIndex = dgvSanPham.Rows.Add();
                DataGridViewRow row = dgvSanPham.Rows[rowIndex];

                row.Cells["colMaSP"].Value = sp.MaSP;
                row.Cells["colTenSP"].Value = sp.TenSP;
                row.Cells["colNgayNhap"].Value = sp.Ngaynhap.ToString(); // Định dạng ngày tháng
                row.Cells["colLoaiSP"].Value = sp.LoaiSP.TenLoai;
            }
        }

        private void dgvSanPham_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvSanPham.SelectedRows.Count > 0)
            {
                // Lấy dòng hiện tại được chọn
                var selectedRow = dgvSanPham.SelectedRows[0];

                // Đổ dữ liệu từ DataGridView lên các control
                txtMaSP.Text = selectedRow.Cells["colMaSP"].Value?.ToString();
                txtTenSP.Text = selectedRow.Cells["colTenSP"].Value?.ToString();
                dtNgayNhap.Value = DateTime.Parse(selectedRow.Cells["colNgayNhap"].Value?.ToString() ?? DateTime.Now.ToString());
                cboLoaiSP.Text = selectedRow.Cells["colLoaiSP"].Value?.ToString();

             
             
            }
            else
            {
                

         
                ClearControls();
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {

            if (MessageBox.Show("Bạn có chắc chắn muốn thoát?", "Xác nhận thoát", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void frmSanPham_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn thoát?", "Xác nhận thoát", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.No)
            {
                // Hủy quá trình đóng form nếu người dùng chọn "No"
                e.Cancel = true;
            }
        }
    }

}
