using De02_DAL.Entities;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Contexts;

public class SanPhamService
{
    private SanPhamModel context;

    public SanPhamService()
    {
        context = new SanPhamModel();
    }

    // Lấy tất cả sản phẩm, kèm theo thông tin loại sản phẩm
    public List<Sanpham> GetAllSanPhams()
    {
        return context.Sanphams.Include(sp => sp.LoaiSP).ToList();
    }

    // Lấy tất cả loại sản phẩm
    public List<LoaiSP> GetAllLoaiSPs()
    {
        return context.LoaiSPs.ToList();
    }
    public Sanpham GetById(string maSP)
    {
        return context.Sanphams.FirstOrDefault(sp => sp.MaSP == maSP);
    }

    // Thêm mới sản phẩm
    public void AddSanPham(Sanpham sanPham)
    {
        context.Sanphams.Add(sanPham);
        context.SaveChanges();
    }

    // Cập nhật thông tin sản phẩm
    public void UpdateSanPham(Sanpham sanPham)
    {
        var existingSanPham = context.Sanphams.Find(sanPham.MaSP); // Chỉnh lại để đúng tên bảng
        if (existingSanPham != null)
        {
            existingSanPham.TenSP = sanPham.TenSP;
            existingSanPham.Ngaynhap = sanPham.Ngaynhap;
            existingSanPham.MaLoai = sanPham.MaLoai;
            context.SaveChanges();
        }
    }

    // Xóa sản phẩm
    public void DeleteSanPham(string maSP)
    {
        var sanPham = context.Sanphams.Find(maSP); // Đồng nhất cách gọi bảng
        if (sanPham != null)
        {
            context.Sanphams.Remove(sanPham);
            context.SaveChanges();
        }
    }

    // Tìm kiếm sản phẩm theo tên
    public List<Sanpham> SearchSanPhams(string searchTerm)
    {
        return context.Sanphams
            .Where(sp => sp.TenSP.ToLower().Contains(searchTerm.ToLower())) // ToLower cho cả 2 bên
            .Include(sp => sp.LoaiSP)
            .ToList();
    }
}
