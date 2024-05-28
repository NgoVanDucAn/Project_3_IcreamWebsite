$('.buy-button').click(function() {
    var book_id = $(this).attr('book-id');
    var book_name = $(this).attr('book-name');
    var book_avatar = $(this).attr('book-avt');
    
    var quantity = $('#input-quantity').val();
    
    var order_item = {
        book_id: book_id,
        book_name: book_name,
        book_avatar: book_avatar,
        quantity: quantity
    };
    
    var orders = [];
    var stored_orders = localStorage.getItem('book_order');
    
    if (stored_orders === null) {
        localStorage.setItem('book_order', JSON.stringify([]));
    } else {
        orders = JSON.parse(stored_orders);
    }
    
    orders.push(order_item);
    localStorage.setItem('book_order', JSON.stringify(orders));
});