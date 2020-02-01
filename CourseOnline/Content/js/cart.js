jQuery(document).ready(function ($) {
    var shoppingCart = (function() {
        var cart = [];

        //ItemCart:
        function Item(id, name, price, author, image, count) {
            this.id = id;
            this.name = name;
            this.price = price;
            this.author = author;
            this.image = image;
            this.count = count;
        }
        //Save cart to session;
        function saveCart() {
            sessionStorage.setItem('shoppingCart', JSON.stringify(cart));
        }
        //Load cart:
        function loadCart() {
            cart = JSON.parse(sessionStorage.getItem('shoppingCart'));
        }
        if (sessionStorage.getItem('shoppingCart') != null) {
            loadCart();
        }
        //------------------------------
        //Them sua xoa cart:
        var obj = {};
        //Add:
        obj.addItemToCart = function(id, name, price, author, image, count) {
            for (var i in cart) {
                if (cart[i].id === id) {
                    cart[i].count ++;
                    saveCart();
                    return;
                }
            }
            var item = new Item(id, name, price, author, image, count);
            cart.push(item);
            saveCart();
        }
        //Add khi da set count:
        obj.addItemFromDetail = function (id, name, price, author, image, count) {
            for (var i in cart) {
                if (cart[i].id === id) {
                    cart[i].count += count;
                    saveCart();
                    return;
                }
            }
            var item = new Item(id, name, price, author, image, count);
            cart.push(item);
            saveCart();
        }
        //Remove:
        obj.removeItemFromCart = function(id) {
            for (var i in cart) {
                if (cart[i].id === id) {
                    cart[i].count--;
                    if (cart[i].count === 0) {
                        cart.splice(i,1);
                    }
                    break;
                }
            }
            saveCart();
        }
        //Remove item from cart:
        obj.removeAllItemFromCart = function(id) {
            for (var i in cart) {
                if (cart[i].id === id) {
                    cart.splice(i, 1);
                    break;
                }
            }
            saveCart();
        }
        //Clear cart:
        obj.clearCart = function() {
            cart = [];
            saveCart();
        }
        //Count cart:
        obj.totalCount = function() {
            var totalCount = 0;
            for (var i in cart) {
                totalCount += cart[i].count;
            }
            return totalCount;
        }
        //Set count item nhap tu input:
        obj.countFromInput = function(id, count) {
            for (var i in cart) {
                if (cart[i].id === id) {
                    cart[i].count += count;
                    break;
                }
            }
        };
        //Total cart:
        obj.totalCart = function() {
            var totalCart = 0;
            for (var i in cart) {
                totalCart += cart[i].price * cart[i].count;
            }
            return Number(totalCart.toFixed(2));
        }
        //List cart:
        obj.listCart = function () {
            var cartCopy = [];
            for (i in cart) {
                item = cart[i];
                itemCopy = {};
                for (p in item) {
                    itemCopy[p] = item[p];

                }
                itemCopy.total = Number(item.price * item.count).toFixed(2);
                cartCopy.push(itemCopy)
            }
            return cartCopy;
        }
        return obj;
    })();
    //-------------------------------------------------------------------
    //Do du lieu khi click add to cart:
    $('.add-to-cart').click(function () {
        var id = $(this).data('id');
        var name = $(this).data('name');
        var price = $(this).data('price');
        var author = $(this).data('author');
        var image = $(this).data('image');
        shoppingCart.addItemToCart(id, name, price, author, image, 1);
        displayCart();
    });

    function displayCart() {
        $('.total-count').html(shoppingCart.totalCount());
    }
    //Delete item:
    $('.show-cart').on('click', '.delete-item', function() {
        var id = $(this).data('id');
        console.log("id = "+id);
        shoppingCart.removeAllItemFromCart(id);
        displayCart();
    });

    $('#add-cart-detail').click(function() {
        var id = $(this).data('id');
        var name = $(this).data('name');
        var price = $(this).data('price');
        var image = $(this).data('image');
        var count = Number($('#counter-detail').val());
        shoppingCart.addItemFromDetail(id, name, price, image, count);
        displayCart();
    });
    function displayCartPage() {
        var cartArray = shoppingCart.listCart();
        var contentCart = "";
        for (var i in cartArray) {
            contentCart +=
                            `<tr class="woocommerce-cart-form__cart-item cart_item ">
                                <td class="product-remove delete-item-cart" data-id="${cartArray[i].id}" style="width: 8%;">
                                    <a class="remove" aria-label="Remove this item" style="cursor: pointer;">×</a>
                                </td>

                                <td class="product-thumbnail" style="width: 17%;"><a href="/Home/ProductDetail/${cartArray[i].id}"><img src="${cartArray[i].image}" class="attachment-woocommerce_thumbnail size-woocommerce_thumbnail wp-post-image" alt="" style="width: 150px;height:75px;"></a></td>

                                <td class="product-name" data-title="Product">
                                    <a href="/Home/ProductDetail/${cartArray[i].id}">${cartArray[i].name}</a>
                                    <p>${cartArray[i].author}</p>
                                </td>
                                <td class="product-price" data-title="Price">
                                    <span class="woocommerce-Price-amount amount"><span class="woocommerce-Price-currencySymbol">$</span>${cartArray[i].price}</span>
                                </td>
                            </tr>`;
        }
        $('.show-cart-page').html(contentCart);
        $('.total-cart-page').html("$ "+shoppingCart.totalCart());
    }
    function displayOrderCheckout() {
        var cartArray = shoppingCart.listCart();
        var contentCart = "";
        for (var i in cartArray) {
            contentCart +=
                                `<tr class="totals item-details">
                                    <input type="hidden" name="listCourseId[]" class="" value="${cartArray[i].id}"/>
                                    <th class="mark" scope="row">${cartArray[i].name}</th>
                                    <td class="amount"><span class="price">$${cartArray[i].price}</span></td>
                                </tr>`;
        }
        $('.content-checkout').html(contentCart);
        $('.total-checkout').html("$" + shoppingCart.totalCart());
        $('.input-amount').val(shoppingCart.totalCart());
    }
    function displayListHiddenInputCourseId() {
        var cartArray = shoppingCart.listCart();
        var contentCart = "";
        for (var i in cartArray) {
            contentCart +=
                `<input type="hidden" name="listCourseId[]" class="" value="${cartArray[i].id}"/>`;
        }
        $('.hidden-input-courseId').html(contentCart);
    }
    $('.show-cart-page').on('click', '.delete-item-cart', function () {
        var id = $(this).data('id');
        shoppingCart.removeAllItemFromCart(id);
        displayCart();
        displayCartPage();
        if (shoppingCart.totalCount == 0) {
            $('.no-item-cart').css({ "display": "block" });
            $('.items-cart').css({ "display": "none" });
        }
    });
    //Check item cart:
    if (shoppingCart.totalCount() > 0) {
        $('.no-item-cart').css({ "display": "none" });
        $('.items-cart').css({ "display": "block" });
    } else if (shoppingCart.totalCount() == 0){
        $('.no-item-cart').css({ "display": "block" });
        $('.items-cart').css({ "display": "none" });
    }
    //Neu vao trang Cart an nut gio hang, inner html:
    if (window.location.pathname == "/Home/Cart") {
        $('.wrap-icon-header').css({ "display": "none" });
        if (shoppingCart.listCart().length == 0) {
            $('.shop-cart-page').css({ "display": "none" });
            $('.coupon').css({ "display": "none" });
            $('.shopping-success').css({ "display": "block" });
        } else {
            displayCartPage();
        }
        

    }
    $(".link").each(function () {
        if (this.href == window.location.href) {
            $(this).css({ "color": "#6c7ae0" });
        }
    });
    $('.check-out').click(function() {
        shoppingCart.clearCart();
        displayCartPage();
    });

    displayCart();
    displayOrderCheckout();
    displayListHiddenInputCourseId();
});

    
