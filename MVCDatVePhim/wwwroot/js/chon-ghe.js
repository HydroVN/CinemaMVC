/**
 * chon-ghe.js — Interactive seat selection for CineViet
 */
(function () {
  'use strict';

  const GiaVe    = parseFloat(document.getElementById('giaVe')?.value || 0);
  const formEl   = document.getElementById('chonGheForm');
  const tongEl   = document.getElementById('tongTien');
  const soGheEl  = document.getElementById('soGheDaChon');
  const listEl   = document.getElementById('danhSachGhe');
  const btnEl    = document.getElementById('btnXacNhan');

  const selectedIds = new Set();

  function formatVND(n) {
    return n.toLocaleString('vi-VN', { style: 'currency', currency: 'VND' });
  }

  function getGiaGhe(seatEl) {
    return seatEl.dataset.loai === 'VIP' ? GiaVe * 1.5 : GiaVe;
  }

  function updateSummary() {
    let total = 0;
    selectedIds.forEach(id => {
      const el = document.querySelector(`.seat[data-id="${id}"]`);
      if (el) total += getGiaGhe(el);
    });

    const count = selectedIds.size;
    if (tongEl)   tongEl.textContent  = formatVND(total);
    if (soGheEl)  soGheEl.textContent = count;
    if (btnEl)    btnEl.disabled      = count === 0;
    if (listEl)   listEl.textContent  = count > 0
      ? [...selectedIds].map(id => {
          const el = document.querySelector(`.seat[data-id="${id}"]`);
          return el ? el.dataset.nhan : id;
        }).join(', ')
      : '—';

    // Sync hidden inputs
    formEl?.querySelectorAll('input[name="gheIds"]').forEach(i => i.remove());
    selectedIds.forEach(id => {
      const inp = document.createElement('input');
      inp.type  = 'hidden';
      inp.name  = 'gheIds';
      inp.value = id;
      formEl?.appendChild(inp);
    });
  }

  document.querySelectorAll('.seat').forEach(seat => {
    if (seat.classList.contains('seat-booked')) return;

    seat.addEventListener('click', () => {
      const id   = seat.dataset.id;
      const isVip = seat.dataset.loai === 'VIP';

      if (selectedIds.has(id)) {
        selectedIds.delete(id);
        seat.classList.remove('seat-selected');
        seat.classList.add(isVip ? 'seat-vip' : 'seat-available');
      } else {
        if (selectedIds.size >= 8) {
          showToast('Bạn chỉ có thể chọn tối đa 8 ghế mỗi lần đặt.');
          return;
        }
        selectedIds.add(id);
        seat.classList.remove('seat-available', 'seat-vip');
        seat.classList.add('seat-selected');
      }
      updateSummary();
    });
  });

  function showToast(msg) {
    const div = document.createElement('div');
    div.style.cssText = `
      position:fixed; bottom:2rem; right:2rem; z-index:9999;
      background:#e50914; color:#fff; padding:.75rem 1.25rem;
      border-radius:10px; font-size:.88rem; font-weight:600;
      box-shadow:0 4px 20px rgba(0,0,0,.4);
      animation: fadeInUp .3s ease;`;
    div.textContent = msg;
    document.body.appendChild(div);
    setTimeout(() => div.remove(), 3000);
  }

  // Init summary
  updateSummary();
})();
