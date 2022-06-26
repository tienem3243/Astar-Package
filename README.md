# Astar Package
Thuật giải A*

    Open: tập các trạng thái đã được sinh ra nhưng chưa được xét đến.
    Close: tập các trạng thái đã được xét đến.
    Cost(p, q): là khoảng cách giữa p, q.
    g(p): khoảng cách từ trạng thái đầu đến trạng thái hiện tại p.
    h(p): giá trị được lượng giá từ trạng thái hiện tại đến trạng thái đích.
    f(p) = g(p) + h(p)
        Bước 1:
            Open: = {s}
            Close: = {}
        Bước 2: while (Open !={})
            Chọn trạng thái (đỉnh) tốt nhất p trong Open (xóa p khỏi Open).
            Nếu p là trạng thái kết thúc thì thoát.
            Chuyển p qua Close và tạo ra các trạng thái kế tiếp q sau p.
                Nếu q đã có trong Open
                    Nếu g(q) > g(p) + Cost(p, q)
                        g(q) = g(p) + Cost(p, q)
                        f(q) = g(q) + h(q)
                        prev(q) = p (đỉnh cha của q là p)
                Nếu q chưa có trong Open
                    g(q) = g(p) + cost(p, q)
                    f(q) = g(q) + h(q)
                    prev(q) = p
                    Thêm q vào Open
                Nếu q có trong Close
                    Nếu g(q) > g(p) + Cost(p, q)
                        Bỏ q khỏi Close
                        Thêm q vào Open
        Bước 3: Không tìm được.
        Các class quan trọng
        Node: các điểm có thẻ hoặc không thể đi qua
        Grid: Lưới chứa các điểm
        Pathfinding áp dụng Astar để xử lí


