function filterProducts() {
    const selectedCategories = Array.from(document.querySelectorAll('input[name="categoryFilter"]:checked'))
        .map(cb => cb.value);

    fetch('/Home/FilterByCategory', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
        },
        body: JSON.stringify(selectedCategories)
    })
        .then(response => response.text())
        .then(html => {
            document.getElementById('products_dev').innerHTML = html;
        })
        .catch(error => console.error('Error filtering products:', error));
}
function addToCart(productId, element) {
    fetch('/Home/AddToCart', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
        },
        body: JSON.stringify({ productId: productId })
    })
        .then(response => {
            if (response.ok) {
                updateCartUI();
            } else {
                alert('Failed to add to cart');
            }
        })
        .catch(err => console.error(err));
}
function updateCartUI() {
    fetch('/Home/GetCartSummary')
        .then(res => res.json())
        .then(data => {
            document.querySelector('.count_item').innerText = data.count;
            document.querySelector('.price_cart_Head').innerText = `$${data.total.toFixed(2)}`;
        });
}
function loadCartItems() {
    fetch('/Home/GetCartItems')
        .then(res => res.text())
        .then(html => {
            document.querySelector('.items_in_cart').innerHTML = html;
            updateCartUI(); // reuse this to update total
        })
        .catch(err => console.error("Failed to load cart items", err));
}

function open_cart() {
    const cart = document.querySelector('.cart');
    cart.classList.add('open');
    cart.classList.add('active');
    loadCartItems();
}
