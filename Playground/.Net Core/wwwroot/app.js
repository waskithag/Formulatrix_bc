const API_BASE = '/api';

let currentTab = 'products';
let editingId = null;
let cachedEmployees = [];
let cachedProducts = [];

// Initialize on page load
document.addEventListener('DOMContentLoaded', () => {
  initAuth();
  loadData();
});

// =================== AUTHENTICATION =================== //

function initAuth() {
  const token = localStorage.getItem('jwt_token');
  const user = localStorage.getItem('jwt_user');

  const authSection = document.getElementById('authSection');
  const loggedInSection = document.getElementById('loggedInSection');
  const userBadge = document.getElementById('userBadge');

  if (token) {
    authSection.style.display = 'none';
    loggedInSection.style.display = 'flex';
    userBadge.textContent = user ? `Logged in: ${user}` : 'Logged in';
  } else {
    authSection.style.display = 'flex';
    loggedInSection.style.display = 'none';
  }
}

async function handleLogin() {
  const usernameInput = document.getElementById('authUsername');
  const passwordInput = document.getElementById('authPassword');

  const username = usernameInput.value.trim();
  const password = passwordInput.value.trim();

  if (!username || !password) {
    showNotification('Please enter username and password.', 'error');
    return;
  }

  try {
    const response = await fetch(`${API_BASE}/auth/login`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ username, password })
    });

    const result = await response.json();
    if (response.ok && result.success && result.data) {
      localStorage.setItem('jwt_token', result.data.token);
      localStorage.setItem('jwt_user', result.data.username);
      initAuth();
      showNotification('Successfully logged in!', 'success');
    } else {
      showNotification(result.message || 'Login failed.', 'error');
    }
  } catch (err) {
    showNotification('Network error during login.', 'error');
    console.error(err);
  }
}

function handleLogout() {
  localStorage.removeItem('jwt_token');
  localStorage.removeItem('jwt_user');
  initAuth();
  showNotification('Logged out.', 'success');
}

function getAuthHeaders() {
  const token = localStorage.getItem('jwt_token');
  const headers = {
    'Content-Type': 'application/json'
  };
  if (token) {
    headers['Authorization'] = `Bearer ${token}`;
  }
  return headers;
}

// =================== NOTIFICATIONS =================== //

function showNotification(message, type = 'success') {
  const notif = document.getElementById('notification');
  notif.textContent = message;
  notif.className = type;
  notif.style.display = 'block';

  setTimeout(() => {
    notif.style.display = 'none';
  }, 4000);
}

// =================== TAB SWITCHING =================== //

function switchTab(tab) {
  currentTab = tab;
  editingId = null;

  document.querySelectorAll('.tab-btn').forEach(btn => btn.classList.remove('active'));
  document.getElementById(`tab${capitalize(tab)}`).classList.add('active');

  // Toggle field visibility
  document.getElementById('productFields').style.display = tab === 'products' ? 'block' : 'none';
  document.getElementById('employeeFields').style.display = tab === 'employees' ? 'block' : 'none';
  document.getElementById('salesFields').style.display = tab === 'sales' ? 'block' : 'none';

  resetForm();
  loadData();
}

function capitalize(str) {
  return str.charAt(0).toUpperCase() + str.slice(1);
}

// =================== DATA LOADING =================== //

async function loadData() {
  const thead = document.getElementById('tableHead');
  const tbody = document.getElementById('tableBody');
  const tableTitle = document.getElementById('tableTitle');

  tbody.innerHTML = '<tr><td colspan="6" class="empty-state">Loading data...</td></tr>';

  try {
    if (currentTab === 'products') {
      tableTitle.textContent = 'Product Records';
      thead.innerHTML = `
        <tr>
          <th>ID</th>
          <th>Name</th>
          <th>Price</th>
          <th>Stock</th>
          <th style="width: 140px;">Actions</th>
        </tr>
      `;

      const res = await fetch(`${API_BASE}/products`);
      const result = await res.json();

      if (result.success && result.data && result.data.length > 0) {
        cachedProducts = result.data;
        tbody.innerHTML = result.data.map(p => `
          <tr>
            <td>${p.id}</td>
            <td><strong>${escapeHtml(p.name)}</strong></td>
            <td>$${Number(p.price).toFixed(2)}</td>
            <td>${p.stock}</td>
            <td class="table-actions">
              <button class="btn btn-secondary btn-sm" onclick="startEditProduct(${p.id})">Edit</button>
              <button class="btn btn-danger btn-sm" onclick="deleteItem('products', ${p.id})">Delete</button>
            </td>
          </tr>
        `).join('');
      } else {
        tbody.innerHTML = '<tr><td colspan="5" class="empty-state">No products found.</td></tr>';
      }
    } else if (currentTab === 'employees') {
      tableTitle.textContent = 'Employee Records';
      thead.innerHTML = `
        <tr>
          <th>ID</th>
          <th>Name</th>
          <th style="width: 140px;">Actions</th>
        </tr>
      `;

      const res = await fetch(`${API_BASE}/employees`);
      const result = await res.json();

      if (result.success && result.data && result.data.length > 0) {
        cachedEmployees = result.data;
        tbody.innerHTML = result.data.map(e => `
          <tr>
            <td>${e.id}</td>
            <td><strong>${escapeHtml(e.name)}</strong></td>
            <td class="table-actions">
              <button class="btn btn-secondary btn-sm" onclick="startEditEmployee(${e.id})">Edit</button>
              <button class="btn btn-danger btn-sm" onclick="deleteItem('employees', ${e.id})">Delete</button>
            </td>
          </tr>
        `).join('');
      } else {
        tbody.innerHTML = '<tr><td colspan="3" class="empty-state">No employees found.</td></tr>';
      }
    } else if (currentTab === 'sales') {
      tableTitle.textContent = 'Sales Records';
      thead.innerHTML = `
        <tr>
          <th>ID</th>
          <th>Employee</th>
          <th>Products Purchased</th>
          <th>Total Price</th>
          <th style="width: 140px;">Actions</th>
        </tr>
      `;

      // Also ensure employees and products are cached for sales dropdowns
      await refreshSalesFormData();

      const res = await fetch(`${API_BASE}/sales`);
      const result = await res.json();

      if (result.success && result.data && result.data.length > 0) {
        tbody.innerHTML = result.data.map(s => {
          const productList = (s.products && s.products.length > 0)
            ? s.products.map(p => `<span class="badge-tag">${escapeHtml(p.name)} ($${Number(p.price).toFixed(2)})</span>`).join(' ')
            : '<em>None</em>';

          return `
            <tr>
              <td>${s.id}</td>
              <td><strong>${escapeHtml(s.employeeName || ('Employee #' + s.employeeId))}</strong></td>
              <td>${productList}</td>
              <td>$${Number(s.totalPrice || 0).toFixed(2)}</td>
              <td class="table-actions">
                <button class="btn btn-secondary btn-sm" onclick="startEditSales(${s.id}, ${s.employeeId}, [${(s.products || []).map(p => p.id).join(',')}])">Edit</button>
                <button class="btn btn-danger btn-sm" onclick="deleteItem('sales', ${s.id})">Delete</button>
              </td>
            </tr>
          `;
        }).join('');
      } else {
        tbody.innerHTML = '<tr><td colspan="5" class="empty-state">No sales transactions found.</td></tr>';
      }
    }
  } catch (err) {
    tbody.innerHTML = '<tr><td colspan="6" class="empty-state" style="color:red;">Error loading data.</td></tr>';
    console.error(err);
  }
}

async function refreshSalesFormData() {
  try {
    const [empRes, prodRes] = await Promise.all([
      fetch(`${API_BASE}/employees`),
      fetch(`${API_BASE}/products`)
    ]);

    const empData = await empRes.json();
    const prodData = await prodRes.json();

    cachedEmployees = empData.data || [];
    cachedProducts = prodData.data || [];

    // Populate employee select dropdown
    const select = document.getElementById('salesEmployee');
    select.innerHTML = '<option value="">-- Select Employee --</option>' +
      cachedEmployees.map(e => `<option value="${e.id}">${escapeHtml(e.name)} (ID: ${e.id})</option>`).join('');

    // Populate products checkbox list
    const checkContainer = document.getElementById('salesProductCheckboxes');
    if (cachedProducts.length === 0) {
      checkContainer.innerHTML = '<small style="color: #94a3b8;">No products available to select.</small>';
    } else {
      checkContainer.innerHTML = cachedProducts.map(p => `
        <label class="checkbox-item">
          <input type="checkbox" name="salesProduct" value="${p.id}" id="prodCheck_${p.id}" />
          <span>${escapeHtml(p.name)} - $${Number(p.price).toFixed(2)}</span>
        </label>
      `).join('');
    }
  } catch (err) {
    console.error('Failed to populate sales form dependencies', err);
  }
}

// =================== FORM SUBMISSION & CRUD =================== //

async function handleFormSubmit(e) {
  e.preventDefault();

  const token = localStorage.getItem('jwt_token');
  if (!token) {
    showNotification('Please log in with JWT credentials (admin / admin123) to perform write operations.', 'error');
    return;
  }

  let endpoint = `${API_BASE}/${currentTab}`;
  let method = editingId ? 'PUT' : 'POST';
  if (editingId) {
    endpoint += `/${editingId}`;
  }

  let payload = {};

  if (currentTab === 'products') {
    const name = document.getElementById('productName').value.trim();
    const price = parseFloat(document.getElementById('productPrice').value);
    const stock = parseInt(document.getElementById('productStock').value, 10);

    if (!name) {
      showNotification('Product name is required.', 'error');
      return;
    }
    if (isNaN(price) || price < 0) {
      showNotification('Please enter a valid non-negative price.', 'error');
      return;
    }
    if (isNaN(stock) || stock < 0) {
      showNotification('Please enter a valid non-negative stock count.', 'error');
      return;
    }

    payload = { name, price, stock };
  } else if (currentTab === 'employees') {
    const name = document.getElementById('employeeName').value.trim();
    if (!name) {
      showNotification('Employee name is required.', 'error');
      return;
    }
    payload = { name };
  } else if (currentTab === 'sales') {
    const empId = parseInt(document.getElementById('salesEmployee').value, 10);
    if (!empId) {
      showNotification('Please select an employee.', 'error');
      return;
    }

    const selectedProductCheckboxes = document.querySelectorAll('input[name="salesProduct"]:checked');
    const productIds = Array.from(selectedProductCheckboxes).map(cb => parseInt(cb.value, 10));

    payload = { employeeId: empId, productIds };
  }

  try {
    const res = await fetch(endpoint, {
      method,
      headers: getAuthHeaders(),
      body: JSON.stringify(payload)
    });

    if (res.status === 401) {
      showNotification('Unauthorized! Please log in first.', 'error');
      return;
    }

    const result = await res.json();
    if (res.ok && result.success) {
      showNotification(result.message || 'Saved successfully!', 'success');
      resetForm();
      loadData();
    } else {
      showNotification(result.message || 'Error occurred while saving.', 'error');
    }
  } catch (err) {
    showNotification('Error communicating with API.', 'error');
    console.error(err);
  }
}

async function deleteItem(type, id) {
  const token = localStorage.getItem('jwt_token');
  if (!token) {
    showNotification('Please log in with JWT credentials to delete records.', 'error');
    return;
  }

  if (!confirm(`Are you sure you want to delete ${type.slice(0, -1)} #${id}?`)) {
    return;
  }

  try {
    const res = await fetch(`${API_BASE}/${type}/${id}`, {
      method: 'DELETE',
      headers: getAuthHeaders()
    });

    if (res.status === 401) {
      showNotification('Unauthorized! Please log in first.', 'error');
      return;
    }

    const result = await res.json();
    if (res.ok && result.success) {
      showNotification(result.message || 'Deleted successfully.', 'success');
      if (editingId === id) resetForm();
      loadData();
    } else {
      showNotification(result.message || 'Failed to delete record.', 'error');
    }
  } catch (err) {
    showNotification('Error communicating with API.', 'error');
    console.error(err);
  }
}

// =================== EDIT HELPERS =================== //

function startEditProduct(id) {
  const product = cachedProducts.find(p => p.id === id);
  if (!product) return;

  editingId = id;
  document.getElementById('editId').value = id;
  document.getElementById('formTitle').textContent = `Edit Product #${id}`;
  document.getElementById('submitBtn').textContent = 'Update Product';
  document.getElementById('cancelBtn').style.display = 'inline-block';

  document.getElementById('productName').value = product.name;
  document.getElementById('productPrice').value = product.price;
  document.getElementById('productStock').value = product.stock;
}

function startEditEmployee(id) {
  const employee = cachedEmployees.find(e => e.id === id);
  if (!employee) return;

  editingId = id;
  document.getElementById('editId').value = id;
  document.getElementById('formTitle').textContent = `Edit Employee #${id}`;
  document.getElementById('submitBtn').textContent = 'Update Employee';
  document.getElementById('cancelBtn').style.display = 'inline-block';

  document.getElementById('employeeName').value = employee.name;
}

function startEditSales(id, employeeId, productIds) {
  editingId = id;
  document.getElementById('editId').value = id;
  document.getElementById('formTitle').textContent = `Edit Sales Record #${id}`;
  document.getElementById('submitBtn').textContent = 'Update Sales';
  document.getElementById('cancelBtn').style.display = 'inline-block';

  document.getElementById('salesEmployee').value = employeeId;

  // Uncheck all first
  document.querySelectorAll('input[name="salesProduct"]').forEach(cb => cb.checked = false);

  // Check matching product checkboxes
  if (productIds && productIds.length > 0) {
    productIds.forEach(pId => {
      const cb = document.getElementById(`prodCheck_${pId}`);
      if (cb) cb.checked = true;
    });
  }
}

function resetForm() {
  editingId = null;
  document.getElementById('editId').value = '';
  document.getElementById('crudForm').reset();
  document.getElementById('cancelBtn').style.display = 'none';

  const singular = capitalize(currentTab.slice(0, -1));
  document.getElementById('formTitle').textContent = `Add ${singular}`;
  document.getElementById('submitBtn').textContent = `Save ${singular}`;

  // If sales, clear checked boxes
  document.querySelectorAll('input[name="salesProduct"]').forEach(cb => cb.checked = false);
}

function escapeHtml(str) {
  if (!str) return '';
  return String(str)
    .replace(/&/g, '&amp;')
    .replace(/</g, '&lt;')
    .replace(/>/g, '&gt;')
    .replace(/"/g, '&quot;')
    .replace(/'/g, '&#039;');
}
