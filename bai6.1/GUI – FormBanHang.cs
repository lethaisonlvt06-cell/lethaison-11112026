using System;
using System.Data;
using System.Windows.Forms;
using RetailSales.BUS;
using RetailSales.DAL;
using RetailSales.DTO;

namespace RetailSales.GUI
{
    public class FormBanHang : Form
    {
        SanPhamBUS bus = new SanPhamBUS();
        ThanhToanDAL thanhToan = new ThanhToanDAL();
        BaoCaoDAL baoCao = new BaoCaoDAL();

        DataGridView dgv = new DataGridView();
        TextBox txtMa = new TextBox();
        TextBox txtTen = new TextBox();
        TextBox txtGia = new TextBox();
        TextBox txtTon = new TextBox();
        TextBox txtTim = new TextBox();
        TextBox txtSoLuong = new TextBox();
        TextBox txtMaNV = new TextBox();
        TextBox txtMaKH = new TextBox();
        Label lblTong = new Label();

        DataTable gioHang = new DataTable();

        public FormBanHang()
        {
            Text = "QUẢN LÝ SIÊU THỊ MINI";
            Width = 1050;
            Height = 700;
            StartPosition = FormStartPosition.CenterScreen;

            TaoGiaoDien();
            TaoGioHang();
            LoadSanPham();
        }

        void TaoGiaoDien()
        {
            int y = 20;

            ThemLabel("Mã sản phẩm", 20, y);
            txtMa.SetBounds(130, y, 150, 25);
            Controls.Add(txtMa);

            ThemLabel("Tên sản phẩm", 320, y);
            txtTen.SetBounds(430, y, 180, 25);
            Controls.Add(txtTen);

            y += 40;
            ThemLabel("Giá bán", 20, y);
            txtGia.SetBounds(130, y, 150, 25);
            Controls.Add(txtGia);

            ThemLabel("Tồn kho", 320, y);
            txtTon.SetBounds(430, y, 180, 25);
            Controls.Add(txtTon);

            y += 45;
            ThemLabel("Tìm kiếm", 20, y);
            txtTim.SetBounds(130, y, 250, 25);
            Controls.Add(txtTim);

            TaoNut("Tìm", 400, y, TimKiem);
            TaoNut("Thêm SP", 500, y, ThemSP);
            TaoNut("Sửa SP", 600, y, SuaSP);
            TaoNut("Xóa SP", 700, y, XoaSP);

            dgv.SetBounds(20, 150, 980, 220);
            dgv.ReadOnly = true;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.CellClick += ChonSanPham;
            Controls.Add(dgv);

            y = 390;
            ThemLabel("Số lượng", 20, y);
            txtSoLuong.SetBounds(130, y, 100, 25);
            Controls.Add(txtSoLuong);

            TaoNut("Thêm vào giỏ", 250, y, ThemGioHang);

            ThemLabel("Mã nhân viên", 20, 440);
            txtMaNV.SetBounds(130, 440, 100, 25);
            Controls.Add(txtMaNV);

            ThemLabel("Mã khách hàng", 270, 440);
            txtMaKH.SetBounds(390, 440, 100, 25);
            Controls.Add(txtMaKH);

            DataGridView dgvGio = new DataGridView();
            dgvGio.SetBounds(20, 480, 650, 130);
            dgvGio.DataSource = gioHang;
            dgvGio.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            Controls.Add(dgvGio);

            lblTong.SetBounds(700, 490, 250, 30);
            Controls.Add(lblTong);

            TaoNut("THANH TOÁN", 700, 540, ThanhToan);
            TaoNut("Doanh thu hôm nay", 700, 580, BaoCaoNgay);
            TaoNut("Top 5 bán chạy", 850, 580, Top5);
        }

        void ThemLabel(string text, int x, int y)
        {
            Controls.Add(new Label
            {
                Text = text,
                Left = x,
                Top = y + 4,
                AutoSize = true
            });
        }

        void TaoNut(string text, int x, int y, Action action)
        {
            Button b = new Button();
            b.Text = text;
            b.SetBounds(x, y, 100, 30);
            b.Click += (s, e) => action();
            Controls.Add(b);
        }

        void TaoGioHang()
        {
            gioHang.Columns.Add("MaSP", typeof(string));
            gioHang.Columns.Add("TenSP", typeof(string));
            gioHang.Columns.Add("SoLuong", typeof(int));
            gioHang.Columns.Add("DonGia", typeof(decimal));
            gioHang.Columns.Add("ThanhTien", typeof(decimal));
        }

        void LoadSanPham()
        {
            dgv.DataSource = bus.GetAll();
        }

        void ChonSanPham(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow r = dgv.Rows[e.RowIndex];
            txtMa.Text = r.Cells["MaSP"].Value.ToString();
            txtTen.Text = r.Cells["TenSP"].Value.ToString();
            txtGia.Text = r.Cells["GiaBan"].Value.ToString();
            txtTon.Text = r.Cells["SoLuongTon"].Value.ToString();
        }

        SanPhamDTO LaySanPham()
        {
            return new SanPhamDTO
            {
                MaSP = txtMa.Text.Trim(),
                TenSP = txtTen.Text.Trim(),
                GiaBan = decimal.Parse(txtGia.Text),
                SoLuongTon = int.Parse(txtTon.Text)
            };
        }

        void ThemSP()
        {
            try
            {
                bus.Insert(LaySanPham());
                LoadSanPham();
                MessageBox.Show("Đã thêm sản phẩm!");
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        void SuaSP()
        {
            try
            {
                bus.Update(LaySanPham());
                LoadSanPham();
                MessageBox.Show("Đã sửa sản phẩm!");
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        void XoaSP()
        {
            try
            {
                bus.Delete(txtMa.Text.Trim());
                LoadSanPham();
                MessageBox.Show("Đã xóa sản phẩm!");
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        void TimKiem()
        {
            dgv.DataSource = bus.Search(txtTim.Text.Trim());
        }

        void ThemGioHang()
        {
            try
            {
                int sl = int.Parse(txtSoLuong.Text);
                decimal gia = decimal.Parse(txtGia.Text);

                if (sl <= 0) throw new Exception("Số lượng phải lớn hơn 0!");

                foreach (DataRow r in gioHang.Rows)
                {
                    if (r["MaSP"].ToString() == txtMa.Text)
                    {
                        int moi = Convert.ToInt32(r["SoLuong"]) + sl;
                        r["SoLuong"] = moi;
                        r["ThanhTien"] = moi * gia;
                        TinhTong();
                        return;
                    }
                }

                gioHang.Rows.Add(txtMa.Text, txtTen.Text, sl, gia, sl * gia);
                TinhTong();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        void TinhTong()
        {
            decimal tong = 0;
            foreach (DataRow r in gioHang.Rows)
                tong += Convert.ToDecimal(r["ThanhTien"]);

            lblTong.Text = "Tổng tiền: " + tong.ToString("N0") + " VNĐ";
        }

        void ThanhToan()
        {
            try
            {
                if (gioHang.Rows.Count == 0)
                    throw new Exception("Giỏ hàng đang trống!");

                int maNV = int.Parse(txtMaNV.Text);
                int? maKH = string.IsNullOrWhiteSpace(txtMaKH.Text)
                    ? (int?)null : int.Parse(txtMaKH.Text);

                int maHD = thanhToan.ThanhToan(maNV, maKH, gioHang);

                MessageBox.Show("Thanh toán thành công! Mã hóa đơn: " + maHD);
                gioHang.Rows.Clear();
                TinhTong();
                LoadSanPham();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        void BaoCaoNgay()
        {
            decimal tong = baoCao.DoanhThuNgay(DateTime.Today);
            MessageBox.Show("Doanh thu hôm nay: " + tong.ToString("N0") + " VNĐ");
        }

        void Top5()
        {
            Form f = new Form();
            f.Text = "Top 5 sản phẩm bán chạy";
            f.Width = 600;
            f.Height = 350;

            DataGridView g = new DataGridView();
            g.Dock = DockStyle.Fill;
            g.DataSource = baoCao.Top5();
            f.Controls.Add(g);
            f.ShowDialog();
        }
    }
}
