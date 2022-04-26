// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

$(document).ready(function () {
    let descriptionBlock = $('#description');
    let reviewBlock = $('#reviews');

    descriptionBlock.show();
    reviewBlock.hide();

    let checkIsSelected = $("[name='Input.ToWhere']");
    checkIsSelected.each(function (i) {
        let v = { "toOfficeEkont": "ekont", "toOfficeSpeedy": "speedy", "toEkont": "homeEkont", "toSpeedy": "homeSpeedy" }
        if (checkIsSelected[i].checked) {
            let value = "#" + v[checkIsSelected[i].value]
            $("#city").show()
            $(value).show()
        }
    });

    changeCardNum()

    //check screen width for menu
    let srchInput = $('#main_search');
    let windowWidth = $(window).width();
    if (windowWidth < 1025) {
        let drop = $("[name = 'navDrop']").parent().children('div')
        drop.hide();

        srchInput.attr('size', 30);
        $('#footer-sm').show()
        $('#footer-lg').remove()
        //$('#upcomingSlider-sm').show()
        //$('#upcomingSlider-lg').remove()
        $('#pages-sm').show()
        $('#pages-lg').remove()
    } else {
        $('#footer-sm').remove()
        $('#footer-lg').show()
        //$('#upcomingSlider-sm').remove()
        //$('#upcomingSlider-lg').show()
        $('#pages-sm').remove()
        $('#pages-lg').show()
    }

    if (windowWidth < 768) {
        $('#upcomingSlider-sm').show()
        $('#upcomingSlider-lg').remove()
    } else {
        $('#upcomingSlider-sm').remove()
        $('#upcomingSlider-lg').show()
    }

    let orderCheckAddress = $("input:radio[name='ChooseAddress']")
    for (let i = 0; i < orderCheckAddress.length; i++) {
        let btn = orderCheckAddress[i].getAttribute("checked")
        if (btn === 'checked') {
            setOrderAddress(orderCheckAddress[i])
        }
    }

    // Add smooth scrolling to all links
    $("#check-reviews").on('click', function (event) {
        if (this.hash !== "") {
            event.preventDefault();
            var hash = this.hash;
            $('html, body').animate({
                scrollTop: $(hash).offset().top
            }, 800, function () {
                window.location.hash = hash;
            });
        }
    });

    //$(document).click(function (event) {
    //    var clickover = $(event.target);
    //    var _opened = $(".navbar-collapse").hasClass("navbar-collapse in");
    //    if (_opened === true && !clickover.hasClass("navbar-toggle")) {
    //        $("button.navbar-toggle").click();
    //    }
    //});
});

$(window).resize(function () {
    let srchInput = $('#main_search');
    let windowWidth = $(this).width();
    if (windowWidth < 1025) {
        srchInput.attr('size', 30);
        $('#footer-sm').show()
        $('#footer-lg').hide()
    }
    if (windowWidth >= 1025) {
        srchInput.attr('size', 60);
        $('#footer-sm').hide()
        $('#footer-lg').show()
    }

})

function changeCardNum() {
    let cookie = getCookie("cart");
    if (cookie === "") {
        setCookie("cart", "", 24);
    }

    //cart number
    let c = cookie.split(":")
    let productInCartCount = c.length
    if (c.length === 1 && c[0] === "") {
        //console.log('empty - ', c)
        $("[name='cartNum']").filter(function () {
            return $(this).text(0)
        })
        //$("#cartNum").text(0);
    } else {
        //console.log('full', c)
        $("[name='cartNum']").filter(function () {
            return $(this).text(productInCartCount)
        })
        //$("#cartNum").text(productInCartCount);
    }
}

//$("#btnDescription").click(onClickShowInfo);
//$("#btnReviews").click(onClickShowInfo);

function onClickShowInfo(e) {
    e.preventDefault();
    let btn = $(e.target);
    let btnName = btn.attr('name');
    console.log(btnName);

    let descriptionBlock = $('#description');
    let reviewBlock = $('#reviews');

    if (btnName === 'description') {
        descriptionBlock.show();
        reviewBlock.hide();
    }
    else if (btnName === 'reviews') {
        descriptionBlock.hide();
        reviewBlock.show();
    }
}


$("[name='Input.ToWhere']").click(onClickRadioAddress);

function onClickRadioAddress(e) {
    let radioBtn = $(e.target);
    let radioId = radioBtn.attr('value');

    switch (radioId) {
        case "toOfficeEkont":
            $("#city").show();
            $("#speedy").hide();
            $("#ekont").show();
            $("#homeEkont").hide();
            $("#homeSpeedy").hide();
            break;
        case "toOfficeSpeedy":
            $("#city").show();
            $("#speedy").show();
            $("#ekont").hide();
            $("#homeEkont").hide();
            $("#homeSpeedy").hide();
            break;
        case "toEkont":
            $("#city").show();
            $("#speedy").hide();
            $("#ekont").hide();
            $("#homeEkont").show();
            $("#homeSpeedy").hide();
            break;
        case "toSpeedy":
            $("#city").show();
            $("#ekont").hide();
            $("#speedy").hide();
            $("#homeEkont").hide();
            $("#homeSpeedy").show();
            break;
    }
}


$('#terms-accept').click(function () {
    window.location.href = window.location.origin + "/terms"
})
//on btn order
//$(document).on('show.bs.modal', '#myModal', function (e) {
//    let button = $(e.relatedTarget)
//    let productId = button.data('id');
//    let cookieCart = getCookie('cart');
//    let cookieData = '';
//    if (cookieCart === '') {
//        cookieData = productId + '-1';
//    } else {
//        let cookieArray = cookieCart.split(':');
//        let isHaveThisProduct = false;
//        for (let i = 0; i < cookieArray.length; i++) {
//            let id = Number(cookieArray[i].split('-')[0]);
//            if (id === productId) {
//                isHaveThisProduct = true;
//                break;
//            }
//        }

//        if (!isHaveThisProduct) {
//            cookieData = cookieCart + ':' + productId + '-1';

//        } else {
//            cookieData = cookieCart;
//        }
//    }

//    setCookie('cart', cookieData, 1);
//})

function addProductToCard(e) {
    let button = $(e.target)
    let productId = button.data('id');
    let cookieCart = getCookie('cart');
    let cookieData = '';
    if (cookieCart === '') {
        cookieData = productId + '-1';
    } else {
        let cookieArray = cookieCart.split(':');
        let isHaveThisProduct = false;
        for (let i = 0; i < cookieArray.length; i++) {
            let id = Number(cookieArray[i].split('-')[0]);
            if (id === productId) {
                isHaveThisProduct = true;
                break;
            }
        }

        if (!isHaveThisProduct) {
            cookieData = cookieCart + ':' + productId + '-1';

        } else {
            cookieData = cookieCart;
        }
    }

    setCookie('cart', cookieData, 24);
}

$('[name="btn-order"]').click(sendInfoToModal)

function sendInfoToModal(e) {
    let modalTitle = $('#modal-title');
    let modalAutor = $('#modal-author');
    let modalPublisher = $('#modal-publisher')
    let modalPrice = $('#modal-price')
    let modalImage = $('#modal-image')

    let productTitle = $('#product-title')
    let productAuthor = $('#product-author')
    let productPublisher = $('#product-publisher')
    let productPrice = $('#product-price')
    let productImage = $('#product-image')

    modalTitle.text(productTitle.text())
    modalAutor.text(productAuthor.text())
    modalPublisher.text(productPublisher.text())
    modalPrice.text('')
    modalPrice.append(productPrice.html())
    modalImage.attr('src', productImage.attr('src'))

    addProductToCard(e)
    changeCardNum()
}

//btn-order-index
$('[name="btn-order-index"]').click(function (e) {
    e.preventDefault()
    let btn = $(e.target).closest("a")
    let id = btn.data("id")

    $.ajax({
        type: 'GET',
        url: '/Book/BookOrderDetails?id=' + id,
        success: function (result) {
            console.log(result)

            let image = result["imageName"]
            if (image === null) {
                image = "NoImage.png"
            }

            $('#modal-title').text(result["title"])
            $('#modal-author').text(result["author"])
            $('#modal-publisher').text(result["publisher"])
            $('#modal-price').text(result["priceWithDisc"])
            $('#modal-image').attr('src', "/images/" + image)
        }
    })

    addProductToCard(e)
    changeCardNum()
})

//Cookie get and set
function setCookie(cname, cvalue, exhours) {
    var d = new Date();
    d.setTime(d.getTime() + (exhours * 60 * 60 * 1000));
    var expires = "expires=" + d.toUTCString();
    document.cookie = cname + "=" + cvalue + ";" + expires + ";path=/";
}

function getCookie(cname) {
    var name = cname + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) == 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}

//btn increment, decrement
$("[name='btn_increment']").click(function myfunction(e) {
    let quantity = $(e.target).closest('td').find('input');
    if (quantity.val() >= 1 && quantity.val() < 100) {
        quantity.val(+quantity.val() + 1);
        let id = +quantity.attr('name');
        changeQuantityInCookie('cart', id, +quantity.val());
        location.reload();
    }
})

$("[name='btn_decrement']").click(function myfunction(e) {
    let quantity = $(e.target).closest('td').find('input');
    if (quantity.val() > 1 && quantity.val() <= 100) {
        quantity.val(+quantity.val() - 1);
        let id = +quantity.attr('name');
        changeQuantityInCookie('cart', id, +quantity.val());
        location.reload();
    }

})

$(".q00").on('input', function (e) {
    let num = Number($(e.target).val())

    if (num > 0) {
        let id = +$(e.target).attr('name');
        changeQuantityInCookie('cart', id, +num)
        location.reload()
    }
})

function changeQuantityInCookie(cname, id, quantity) {
    let c = getCookie(cname);
    let newCookie = getCookie(cname);
    let allProductsWithQuantity = c.split(':')
    for (var i = 0; i < allProductsWithQuantity.length; i++) {
        let cId = +allProductsWithQuantity[i].split('-')[0];
        if (cId === id) {
            let newCookieElement = cId + '-' + quantity;
            newCookie = c.replace(allProductsWithQuantity[i], newCookieElement);
        }
    }
    setCookie('cart', newCookie, 24);
}

//btn remove from cart
$("[name='btn_remove_from_cart']").click(function myfunction(e) {
    e.preventDefault();
    let id = $(e.target).closest('a').attr('id');
    let c = getCookie('cart');
    let newCookie = getCookie('cart');
    let allProductsWithQuantity = c.split(':')
    for (var i = 0; i < allProductsWithQuantity.length; i++) {
        let cId = allProductsWithQuantity[i].split('-')[0];
        if (cId === id) {
            if (allProductsWithQuantity.length - 1 === i) {
                newCookie = c.replace(':' + allProductsWithQuantity[i], '');
                if (c.charAt(0) == ':') {
                    newCookie = c.substring(1);
                }
            } else {
                let produtRemove = allProductsWithQuantity[i] + ':';
                newCookie = c.replace(produtRemove, '');
            }
            if (allProductsWithQuantity.length == 1) {
                newCookie = c.replace(allProductsWithQuantity[i], '');
            }

        }
    }
    setCookie('cart', newCookie, 24);
    location.reload();
})

//radio check address no reg

$('input:radio[name="DeliveryOption"]').change(
    function () {
        changeDeliveryPriceAndAddIt(this)
    }
)

//radio check address
$('input:radio[name="ChooseAddress"]').change(
    function () {
        setOrderAddress(this)
    }
);

function setOrderAddress(e) {
    if ($(e).is(':checked') && $(e).val() == 'OfficeSpeedy') {
        getAddress('OfficeSpeedy')
        changeDeliveryPriceAndAddIt(e)
    } else if ($(e).is(':checked') && $(e).val() == 'OfficeEkont') {
        getAddress('OfficeEkont')
        changeDeliveryPriceAndAddIt(e)
    } else if ($(e).is(':checked') && $(e).val() == 'Ekont') {
        getAddress('Ekont')
        changeDeliveryPriceAndAddIt(e)
    } else if ($(e).is(':checked') && $(e).val() == 'Speedy') {
        getAddress('Speedy')
        changeDeliveryPriceAndAddIt(e)
    }
}

//ajax get address
function getAddress(addressPlace) {
    $.ajax({
        type: 'GET',
        url: '/Checkout/GetAddress?a=' + addressPlace,
        success: function (result) {
            if (result) {
                $('#myAddress option').each(function (i, value) {
                    if (i > 0) {
                        value.remove()
                    } else {
                        $(value).prop('selected', true)
                    }
                })

                for (var i = 0; i < result.length; i++) {
                    let id = result[i].id
                    let address = result[i].address
                    let city = result[i].city

                    $('#myAddressSel').append('<option value="' + id + '">' + address + ', ' + city + '</option')
                }
                let isNewAddr = bla()
                if (isNewAddr === 'newAddress') {
                    $('#myAddressSel').append('<option value="newAddress" id="newAddr" selected> Нов адрес </option')
                    $('#myAddress').prop('hidden', false)
                    $('#form-new-address').prop('hidden', false)
                    disableFormNewAddress(false)
                } else {
                    $('#myAddressSel').append('<option value="newAddress" id="newAddr"> Нов адрес </option')
                    $('#myAddress').prop('hidden', false)
                    $('#form-new-address').prop('hidden', true)
                    disableFormNewAddress(true)
                }

            } else {
                $('#myAddress').prop('hidden', true)
                $('#form-new-address').prop('hidden', false)
                disableFormNewAddress(false)
                console.log("ne")
            }
        }
    });
};

//Change Price
function changeDeliveryPriceAndAddIt(e) {
    let delivery = $(e).parent().children('label').children('span').text()
    let d = delivery
    if (isNaN(delivery)) {
        d = 0.00.toFixed(2)
    }
    $('#delivery-price').text(d)
    let currentPrice = $('#total-price').text()
    $('#total-with-delivery').text((parseFloat(d) + parseFloat(currentPrice)).toFixed(2))
}

//new address option
$('#myAddress').change(function (e) {
    let target = $(e.target)
    if (target.val() === 'newAddress') {
        $('#form-new-address').prop('hidden', false)
        disableFormNewAddress(false)
    } else {
        $('#form-new-address').prop('hidden', true)
        disableFormNewAddress(true)
    }
})

//disable prop newAddress
function disableFormNewAddress(hide) {
    $('#form-new-address input').each(function (i, value) {
        $(value).prop('disabled', hide)
    })
}

//delete favorite product
$("[name='delete-favorite']").click(function (e) {
    e.preventDefault()
    let id = $(e.target).closest('a').attr('id');

    deleteFav(id)
})

//Ajax delete fav
function deleteFav(id) {
    $.ajax({
        type: 'GET',
        url: '/Favorite/DeleteFavorite?id=' + id,
        success: function (result) {
            console.log(result)
            //TODO: съобщение??
            location.reload()
        }
    })
}

//main search
//$('#main_search').on("input", mainSearchFunc)
//$('#main_search').on("click", mainSearchFunc)
//var timer = null;
//$('#main_search').keydown(function (е) {
//    $('#search_result').empty()
//    clearTimeout(timer);
//    timer = setTimeout(mainSearchFunc(е), 1000)
//});

function mainSearchFunc(e) {
    let keyword = $(e.target).val().trim();
    if (keyword === "") {
        $('#search_result').empty()
    }

    let windowWidth = $(document).width();
    if (windowWidth > 481) {
        if (keyword.length != 0) {
            getProducts(keyword)
        } else {
            $('#searchBox').hide();
            $('#search_result').empty()
        }
    }
}

function getProducts(keyword) {
    console.log(location.origin)
    let path = location.origin
    let decodeurl = decodeURIComponent(path + '/Search/tempSearchData?keyword=' + keyword)
    if (path !== "" || path !== " ") {
        decodeurl = decodeURIComponent(path + '/Search/tempSearchData?keyword=' + keyword)
    }

    $.ajax({
        type: 'GET',
        url: decodeurl,
        success: function (result) {
            //console.log(result)
            if (result.length > 0) {
                $('#search_result').empty().append(result);
                $('#searchBox').show();
            } else {
                $('#search_result').empty()
                $('#searchBox').hide();
            }
        }
    })
}

$(document).bind('DOMSubtreeModified', function () {
    $(".clickable-row").click(function () {
        window.location = $(this).data("href");
    });
});

$(document).mouseup(function (e) {
    let container = $("#searchBox");

    // if the target of the click isn't the container nor a descendant of the container
    if (!container.is(e.target) && container.has(e.target).length === 0) {
        container.hide();
    }

    //let sidebar = $("#sidebar")
    //let btn_sidebar = $("#btn-sidebar")

    //if (!sidebar.is(e.target) && sidebar.has(e.target).length === 0) {
    //    btn_sidebar.click()
    //}

    let sidebar = $("#sidebar");

    // if the target of the click isn't the container nor a descendant of the container
    if (!sidebar.is(e.target) && sidebar.has(e.target).length === 0) {
        sidebar.collapse('hide');
    }
});

$("[name='navDrop']").click(function (e) {
    let drop = $(e.target).parent().children('div')
    //console.log(drop.css('display') == 'block')
    let windowWidth = $(document).width();
    if (windowWidth < 1025) {
        if (drop.css('display') === "block") {
            drop.hide();
        } else {
            drop.show()
        }
    }
})

function alertBoxShow(tempSuccess, tempMessage) {
    if (tempSuccess === 'true') {
        let alertBoxDiv = `<div class="alert alert-success alert - dismissible fade show" role="alert">${tempMessage}<button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button></div>`;
        $("#message").append(alertBoxDiv);
    } else if (tempSuccess === 'false') {
        let alertBoxDiv = `<div class="alert alert-danger alert - dismissible fade show" role="alert">${tempMessage}<button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button></div>`;
        $("#message").append(alertBoxDiv);
    }
}

document.addEventListener("DOMContentLoaded", function () {
    var elements = document.getElementsByTagName("input");
    for (var i = 0; i < elements.length; i++) {
        elements[i].oninvalid = function (e) {
            e.target.setCustomValidity("");
            if (!e.target.validity.valid) {
                e.target.setCustomValidity("This field cannot be left blank");
            }
        };
        elements[i].oninput = function (e) {
            e.target.setCustomValidity("");
        };
    }
})

$('#addReview').click(function (e) {
    let inputs = $('#reviewForm :input')
    for (var i = 0; i < inputs.length; i++) {
        //console.log(inputs[i])
        if (!inputs[i].checkValidity()) {
            let input = $(inputs[i])
            if (input.prop('name') == 'stars') {
                input.parent().parent().parent().children('span').text('Моля оценете продукта')
            } else {
                input.parent().children('span').text('Полето е задължително')
            }
        }
    }

})
