//$('#adminMenu .nav-link').on('click', changeView);
//$(document).ready(stayOnCurentView);
//$('#searchBookBtn').click(searchProducts);

function changeView(e) {
    e.preventDefault();
    let button = $(e.target);

    switch (button.attr('name')) {
        case 'productCreate':
            showElement('productCreateDiv');
            break;
        case 'category':
            showElement('categoryDiv');
            break;
        case 'showAllCategories':
            showElement('showAllCategoriesDiv');
            break;
        case 'editProduct':
            showElement('EditProductDiv');
            break;
        case 'helpAndTerms':
            showElement('')
            break
    }
}

function showElement(nameOfElement) {
    let menuItems = $('[name="menuItem"]');

    for (var m of menuItems) {
        let menuItem = $(m);

        if (menuItem.attr('id') === nameOfElement) {
            menuItem.attr('class', 'showElement');
        } else {
            menuItem.attr('class', 'hideElement');
        }
    }
}

function alertBoxShow(tempSuccess, tempMessage) {
    if (tempSuccess === 'true') {
        let alertBoxDiv = `<div class="alert alert-success alert - dismissible fade show" role="alert">${tempMessage}<button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button></div>`;
        $("#alertBox").append(alertBoxDiv);
    }
}

function stayOnCurentView(e) {
    if (tempSearchProduct.length > 0) {
        showElement(tempSearchProduct);
    }
}

function searchProducts(e) {
    e.preventDefault();
    console.log($('#tableSearch tbody').remove());

    let uri = 'Admin/Home/SearchProduct'
    let productToSearch = $('#productToSearch').val();
    let searchOption = $('#searchOption').val();

    $.getJSON(uri + '/' + productToSearch + '/' + searchOption, function (json) {
        console.log(json.books[0]);
    });
}